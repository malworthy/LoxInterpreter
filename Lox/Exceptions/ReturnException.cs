using System.Runtime.Serialization;

namespace LoxInterpreter.Exceptions;
[Serializable]
internal class ReturnException : Exception
{
    public ReturnException(object? value)
    {
        this.Value = value;
    }

    public object? Value { get; private init; }
}