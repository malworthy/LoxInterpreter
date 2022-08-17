using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LoxInterpreter.Exceptions;

namespace LoxInterpreter;

public class Lox
{
    static bool hadError = false;
    private static readonly Interpreter interpreter = new(new Output());

    public static int Start(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: lox [script]");
        }
        else if (args.Length == 1)
        {
            return RunFile(args[0]);
        }
        else
        {
            RunPrompt();
        }
        return 0;
    }

    private static int RunFile(string path)
    {
        var fileContents = File.ReadAllText(path);
        Run(fileContents);

        if (hadError) return 65;

        return 0;
    }

    private static void RunPrompt()
    {
        while(true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                return;
            Run(line);
            hadError = false;
        }
    }

    internal static void RuntimeError(RuntimeException error)
    {
        Lox.Report(error.Token.Line, "", error.Message);
    }

    private static void Run(string source)
    {
        try
        {
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();
            var parser = new Parser(tokens);
            var statements = parser.Parse();

            if (hadError) return;

            var resolver = new Resolver(interpreter);
            resolver.Resolve(statements);

            if (hadError) return;

            // temp code
            interpreter.Interpret(statements);
        }
        catch (Exception)
        {
            //throw;
        }
        
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    public static void Error(Token token, String message)
    {
        if (token.Type == TokenType.EOF)
        {
            Report(token.Line, " at end", message);
        }
        else
        {
            Report(token.Line, $" at '{token.Lexme}'", message);
        }
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error{where}: {message}");
        hadError = true;
    }
}
