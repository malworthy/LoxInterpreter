namespace LoxInterpreter.Expressions;

public class Call : Expr
{
    public Expr Callee { get; init; }
    public Token Paran { get; init; }
    public List<Expr> Arguments { get; init; }

    public Call(Expr callee, Token paran, List<Expr> arguments)
    {
        this.Callee = callee;
        this.Paran = paran;
        this.Arguments = arguments;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}