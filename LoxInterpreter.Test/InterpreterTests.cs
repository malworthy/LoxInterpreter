﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Test;
public class InterpreterTests
{
    [Fact]
    public void Test()
    {
        var scanner = new Scanner("print 1+1;");
        var tokens = scanner.ScanTokens();
        var parser = new Parser(tokens);
        var statements = parser.Parse();
        var output = new TestOutput();
        new Interpreter(output).Interpret(statements);
        Assert.Equal("2", output.Text.Trim());
    }

    [Fact]
    public void TestBlockScope()
    {
        var a = Assembly.GetExecutingAssembly();
        var names = a.GetManifestResourceNames();
        var s = a.GetManifestResourceStream("LoxInterpreter.Test.TestPrograms.BlockScope.txt");
        using var sr = new StreamReader(s);
        var lines = sr.ReadToEnd();

        var scanner = new Scanner(lines);
        var tokens = scanner.ScanTokens();
        var parser = new Parser(tokens);
        var statements = parser.Parse();
        var output = new TestOutput();
        new Interpreter(output).Interpret(statements);
        //Assert.Equal("2", output.Text.Trim());
    }

    [Fact]
    public void TestConditionalExecution()
    {
        var prog = GetFileContents("ConditionalExecution.txt");
        var result = InterpreteGetOutout(prog);
        var expected = GetFileContents("ConditionalExecutionER.txt");
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("While.txt","WhileER.txt")]
    [InlineData("For.txt", "WhileER.txt")]
    public void TestWhile(string programFile, string expectedResultFile)
    {
        var prog = GetFileContents(programFile);
        var result = InterpreteGetOutout(prog);
        var expected = GetFileContents(expectedResultFile);
        Assert.Equal(expected, result);
    }

    private string GetFileContents(string filename)
    {
        var a = Assembly.GetExecutingAssembly();
        var s = a.GetManifestResourceStream($"LoxInterpreter.Test.TestPrograms.{filename}");
        using var sr = new StreamReader(s);
        return sr.ReadToEnd();
    }

    private string InterpreteGetOutout(string program)
    {
        var scanner = new Scanner(program);
        var tokens = scanner.ScanTokens();
        var parser = new Parser(tokens);
        var statements = parser.Parse();
        var output = new TestOutput();
        new Interpreter(output).Interpret(statements);
        return output.Text;
    }

}
