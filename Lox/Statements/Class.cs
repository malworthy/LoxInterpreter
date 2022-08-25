using LoxInterpreter;
using LoxInterpreter.Statements;

namespace LoxInterpreter.Statements;

public class Class : Stmt
{
    public Token Name { get; init; }
    public List<Function> Methods { get; init; }

    public Expressions.Variable Superclass { get; init; }

    public Class(Token name, Expressions.Variable superclass, List<Function> methods)
    {
        this.Name = name;
        this.Methods = methods;
        Superclass = superclass;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}