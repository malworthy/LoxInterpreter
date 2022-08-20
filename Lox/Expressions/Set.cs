namespace LoxInterpreter.Expressions;

public class Set : Expr
{
    public Expr Object { get; init; }
    public Token Name { get; init; }
    public Expr Value { get; init; }

    public Set(Expr @object, Token name, Expr value)
    {
        this.Object = @object;
        this.Name = name;
        this.Value = value;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}