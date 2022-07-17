namespace LoxInterpreter.Statements;
public class Function : Stmt
{
    public Token Name { get; init; }
    public List<Token> Parameters { get; init; }
    public IEnumerable<Stmt> Body { get; init; }

    public Function(Token name, List<Token> parameters, IEnumerable<Stmt> body)
    {
        this.Name = name;
        this.Parameters = parameters;
        this.Body = body;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}