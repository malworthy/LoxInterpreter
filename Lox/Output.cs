using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter;

public interface IOutput
{
    void Print(string message);
    void Print(object? message);
}
public class Output : IOutput
{
    public void Print(string message)
    {      
        Console.WriteLine(message);
    }

    public void Print(object? message)
    {
        Console.WriteLine(message);
    }
}
