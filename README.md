# JBlam.ConsolePrompt

A small .NET library for doing neat things in consoles.

**PRERELEASE**: I may decide to rename the parent object.

## Features

- print in colours to the console using C# template string syntax:
  ```csharp
  Out.WriteLine($"The value is {value:Green}.");
  ```

## Possible future features

- a 1-liner for prompting user input, like
  ```csharp
  // prints "What is your name: " and returns the user input
  var name = Out.PromptLine("What is your name");
  ```

## Licensing

This is not free software. See [the license](LICENSE.txt); but basically, don't be an arse and you can do whatever.

Note that "being an arse" does include working for arseholes. So, don't do that.

