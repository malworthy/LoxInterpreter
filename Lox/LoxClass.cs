namespace LoxInterpreter;

internal class LoxClass : ICallable
{
    private readonly string name;
    private readonly Dictionary<string, LoxFunction> methods;

    public LoxClass(string name, Dictionary<string, LoxFunction> methods)
    {
        this.name = name;
        this.methods = methods;
    }

    public int Arity => 0;

    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        var instance = new LoxInstance(this);

        return instance;
    }

    public override string ToString() => name;

    internal LoxFunction? FindMethod(string name)
    {
        if(methods.ContainsKey(name))
            return methods[name];

        return null;
    }
}