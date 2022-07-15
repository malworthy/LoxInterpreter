using LoxInterpreter.Expressions;
using Xunit;

namespace LoxInterpreter.Test;

public class ScannerTests
{
    [Fact]
    public void TestCreateTokens()
    {
        var source = "var x=123; var s = \"this is a string\"; x=(1+1.0)*20-5/600.56;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        Assert.Equal(25, tokens.Count);

        var parser = new Parser(tokens);
      
    }

    [Fact]
    public void TestExpressionTreePrint()
    {
        var expression = new Binary
        {
            Left = new Unary 
            { 
                Operator = new Token { Type = TokenType.MINUS, Lexme = "-" }, 
                Right = new Literal(123)
            },
            Operator = new Token { Type = TokenType.STAR, Lexme = "*" },
            Right = new Grouping { Expression = new Literal(45.67) }
        };

        var result = new AstPrinter().Print(expression);
        Assert.Equal("(* (- 123) (group 45.67))", result);
    }
}