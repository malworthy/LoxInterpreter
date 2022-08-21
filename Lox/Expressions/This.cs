namespace LoxInterpreter.Expressions;

public class This : Expr
{
    public Token Keyword { get; init; }

    public This(Token token)
    {
        this.Keyword = token;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}