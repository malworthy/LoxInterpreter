# LoxInterpreter

A c# impementation of the jlox interpeter from https://craftinginterpreters.com/

This is a pretty straight port from Java to c#.  I've change naming and code formatting to match c# standards as much as possible.
I didn't use any code generation to create classes, as Visual Studio does a pretty good job of creating boiler plate classes.
I've also included a basic set of unit tests, mainly just using code snippets from the book.  

Additions to the basic jlox interpreter:

- implemented '++' and '--' so you can do "for(int i; i<10; i++)" (implementation still a wip)
- added the ability to compare strings
