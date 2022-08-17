using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Expressions;
public class AstPrinter : IVisitor<string>
{
    public string Print(Expr expr)
    {
        return expr.Accept(this);
    }
    public string Visit(Assign expr)
    {
        throw new NotImplementedException();
    }

    public string Visit(Binary expr)
    {
        return Parenthesize(expr.Operator.Lexeme, expr.Left, expr.Right);
    }

    public string Visit(Grouping expr)
    {
        return Parenthesize("group", expr.Expression);
    }

    public string Visit(Literal expr)
    {
        return expr.Value?.ToString() ?? "nil";
    }

    public string Visit(Unary expr)
    {
        return Parenthesize(expr.Operator.Lexeme, expr.Right);
    }

    public string Visit(Variable expr)
    {
        throw new NotImplementedException();
    }

    public string Visit(Logical expr)
    {
        throw new NotImplementedException();
    }

    public string Visit(Call expr)
    {
        throw new NotImplementedException();
    }

    private String Parenthesize(String name, params Expr[] exprs)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("(").Append(name);
        foreach (Expr expr in exprs)
        {
            builder.Append(" ");
            builder.Append(expr.Accept(this));
        }
        builder.Append(")");

        return builder.ToString();
    }
}
