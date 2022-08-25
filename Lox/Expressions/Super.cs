namespace LoxInterpreter.Expressions;

public class Super : Expr
{
    public Token Keyword { get; init; }
    public Token Method { get; init; }

    public Super(Token keyword, Token method)
    {
        this.Keyword = keyword;
        this.Method = method;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}