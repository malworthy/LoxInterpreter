using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Exceptions;
public  class RuntimeException : Exception
{
    public Token Token { get; init; }    
    public RuntimeException(Token token, string message) : base(message)
    {
       Token = token;
    }
}
