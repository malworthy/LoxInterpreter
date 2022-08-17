using LoxInterpreter.Expressions;
using LoxInterpreter.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter;
public class Resolver : Expressions.IVisitor<bool>, Statements.IVisitor<bool>
{
    private readonly Interpreter interpreter;
    private readonly Stack<Dictionary<string, bool>> scopes = new();
    private FunctionType currentFunction = FunctionType.None;

    private enum FunctionType
    {
        None,
        Function
    }

    public Resolver(Interpreter interpreter) 
    {
        this.interpreter = interpreter;
    }

    public bool Visit(Assign expr)
    {
        Resolve(expr.Value);
        ResolveLocal(expr, expr.Name);

        return true;
    }

    public bool Visit(Binary expr)
    {
        Resolve(expr.Left);
        Resolve(expr.Right);

        return true;
    }

    public bool Visit(Grouping expr)
    {
        Resolve(expr.Expression);

        return true;
    }

    public bool Visit(Literal expr)
    {
        return true;
    }

    public bool Visit(Unary expr)
    {
        Resolve(expr.Right);

        return true;
    }

    private void ResolveLocal(Expr expr, Token name)
    {
        int i = 0;
        foreach(var scope in scopes)
        {
            if(scope.ContainsKey(name.Lexeme))
            {
                interpreter.Resolve(expr, i);
                return;
            }
            i++;
        }
    }

    public bool Visit(Logical expr)
    {
        Resolve(expr.Left);
        Resolve(expr.Right);

        return true;
    }

    public bool Visit(Call expr)
    {
        Resolve(expr.Callee);

        foreach (var arg in expr.Arguments)
            Resolve(arg);

        return true;
    }

    public bool Visit(Expression stmt)
    {
        Resolve(stmt.expression);

        return true;
    }

    public bool Visit(Print stmt)
    {
        Resolve(stmt.expression);

        return true;
    }

    public bool Visit(Var stmt)
    {
        Declare(stmt.Name);
        if (stmt.Initializer != null)
            Resolve(stmt.Initializer);
        Define(stmt.Name);
        return true;
    }

    private void Define(Token name)
    {
        if (!scopes.Any()) return;
        var scope = scopes.Peek();
        scope[name.Lexeme] = true;
    }

    private void Declare(Token name)
    {
        if (!scopes.Any()) return;
        var scope = scopes.Peek();
        if (scope.ContainsKey(name.Lexeme))
            Lox.Error(name, "A variable already exists in this scope.");

        scope[name.Lexeme] = false;
    }

    public bool Visit(Block stmt)
    {
        BeginScope();
        Resolve(stmt.Statements);
        EndScope();
        return false;
    }

    private void EndScope()
    {
        scopes.Pop();
    }

    private void BeginScope()
    {
        scopes.Push(new Dictionary<string, bool>());
    }

    public void Resolve(IEnumerable<Stmt> statements)
    {
        foreach (var stmt in statements)
            Resolve(stmt);
    }

    private void Resolve(Stmt stmt) => stmt.Accept(this);

    private void Resolve(Expr expr) => expr.Accept(this);

    public bool Visit(If stmt)
    {
        Resolve(stmt.Condition);
        Resolve(stmt.ThenBranch);
        if(stmt.ElseBranch != null)
            Resolve(stmt.ElseBranch);

        return true;
    }

    public bool Visit(While stmt)
    {
        Resolve(stmt.Condition);
        Resolve(stmt.Body);

        return true;
    }

    public bool Visit(Function stmt)
    {
        Declare(stmt.Name);
        Define(stmt.Name);
        ResolveFunction(stmt, FunctionType.Function);

        return true;
    }

    public bool Visit(Variable expr)
    {
        if (scopes.Any() 
            && scopes.Peek().ContainsKey(expr.Name.Lexeme) 
            && scopes.Peek()[expr.Name.Lexeme] == false)
            Lox.Error(expr.Name, "Can't read local variable in its own initializer.");
        
        //throw new ApplicationException("WFT!!");
        ResolveLocal(expr, expr.Name);

        return true;
    }

    private void ResolveFunction(Function function, FunctionType type)
    {
        var enclosingFunction = currentFunction;
        currentFunction = type;

        BeginScope();
        foreach(var param in function.Parameters)
        {
            Declare(param);
            Define(param);
        }
        Resolve(function.Body);
        EndScope();
        currentFunction = enclosingFunction;
    }

    public bool Visit(Return stmt)
    {
        if (currentFunction == FunctionType.None)
            Lox.Error(stmt.Keyword, "Can't return from top level code");

        if (stmt.Value != null)
            Resolve(stmt.Value);

        return true;
    }

    public bool Visit(Class stmt)
    {
        Declare(stmt.Name);
        Define(stmt.Name);

        return true;
    }
}
