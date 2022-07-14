using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Test;
internal class TestOutput : IOutput
{
    private readonly StringBuilder output = new();

    public string Text { get => output.ToString(); }
    public void Print(string message)
    {
        output.AppendLine(message);
    }

    public void Print(object? message)
    {
        output.AppendLine(message?.ToString());
    }
}
