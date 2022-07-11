using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Test;
public class InterpreterTests
{
    [Fact]
    public void Test()
    {
        var scanner = new Scanner("1+1");
        var tokens = scanner.ScanTokens();
        var parser = new Parser(tokens);
        var expr = parser.ParseExpression();
        var result = new Interpreter().Interpret(expr);
        Assert.Equal("2", result);
    }
}
