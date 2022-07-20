using LoxInterpreter.Expressions;

namespace LoxInterpreter.Statements;
public class Return : Stmt
{
    public Token Keyword { get; init; }
    public Expr? Value { get; init; }

    public Return(Token keyword, Expr? value)
    {
        this.Keyword = keyword;
        this.Value = value;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}