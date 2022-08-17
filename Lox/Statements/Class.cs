using LoxInterpreter;
using LoxInterpreter.Statements;

namespace LoxInterpreter.Statements;

public class Class : Stmt
{
    public Token Name { get; init; }
    public List<Function> Methods { get; init; }

    public Class(Token name, List<Function> methods)
    {
        this.Name = name;
        this.Methods = methods;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}