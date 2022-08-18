namespace LoxInterpreter.Expressions;

public class Get : Expr
{
    public Expr Object { get; init; }
    public Token Name { get; init; }

    public Get(Expr expr, Token name)
    {
        this.Object = expr;
        this.Name = name;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}