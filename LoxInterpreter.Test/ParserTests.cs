using LoxInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Test;
public class ParserTests
{
    [Theory]
    [InlineData("(1+1)/5 + 2 * 6.5", "(+ (/ (group (+ 1 1)) 5) (* 2 6.5))")]
    [InlineData("1==2", "(== 1 2)")]
    [InlineData("!(-1==-2)", "(! (group (== (- 1) (- 2))))")]
    [InlineData("-2", "(- 2)")]
    [InlineData("!true", "(! True)")]
    public void TestParseExpression(string source, string expected)
    {
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new Parser(tokens);
        var expr = parser.ParseExpression();
        var result = new AstPrinter().Print(expr);

        Assert.Equal(expected, result);
    }
}
