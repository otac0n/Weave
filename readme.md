Weave
=====

Weave is a text layout engine for .NET that is all about attention to detail.  Weave handles the tricky work of making your rendered text beautiful.

Getting Started
---------------

The easiest way to get a copy of Weave is to install the [Weave NuGet package](http://nuget.org/packages/Pegasus) in Visual Studio.

    PM> Install-Package Weave

Due to a limitation in Visual Studio, you will need to reload your project for the 'WeaveTemplate' build action to be recognized.

Once you have the package installed, files in your project marked as 'WeaveTemplate' in the properties window will be compiled to their respective `.weave.cs` template classes before every build.  These template classes will be automatically included in compilation.

For help with template syntax, see [the syntax guide wiki entry](https://github.com/otac0n/Weave/wiki/Syntax-Guide)

Example
-------

    @namespace MyProject
    @methodname RenderFizzBuzz
    @model IEnumerable<int>

    {{each i in model}}
        {{if i % 3 == 0 && i % 5 == 0}}
            {{: i }} FizzBuzz
        {{elif i % 3 == 0}}
            {{: i }} Fizz
        {{elif i % 5 == 0}}
            {{: i }} Buzz
        {{else}}
            {{: i }}
        {{/if}}
    {{/each}}
