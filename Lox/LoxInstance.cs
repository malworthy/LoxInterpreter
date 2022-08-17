namespace LoxInterpreter;

internal class LoxInstance
{
    private LoxClass loxClass;

    public LoxInstance(LoxClass loxClass)
    {
        this.loxClass = loxClass;
    }

    public override string ToString() => $"{loxClass} instance";
}