using LoxInterpreter.Exceptions;

namespace LoxInterpreter;

internal class LoxInstance
{
    private LoxClass loxClass;
    private readonly Dictionary<string, object> fields = new();

    public LoxInstance(LoxClass loxClass)
    {
        this.loxClass = loxClass;
    }

    public override string ToString() => $"{loxClass} instance";

    internal object? Get(Token name)
    {
        if (fields.ContainsKey(name.Lexeme))
            return fields[name.Lexeme];

        throw new RuntimeException(name, $"Undefined property {name.Lexeme} in {loxClass}");

    }
}