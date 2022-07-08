using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter;
internal record Token
{
    public TokenType Type { get; init; }
    public string Lexme { get; init; } = string.Empty;
    public object? Literal { get; init; }
    public int Line { get; init; }

    public override string ToString()
    {
        return $"{Type} {Lexme} {Literal}";
    }
}
