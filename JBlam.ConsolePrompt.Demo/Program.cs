using JBlam.ConsolePrompt;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
int i = 17_345;
Out.WriteLine($"Plain: [{i}]");
Out.WriteLine($"Colour only: [{i:Red}]");
Out.WriteLine($"Format only: [{i:X2}]");
Out.WriteLine($"Format and colour: [{i:Magenta,X2}]");
Out.WriteLine($"With group separator: [{i:##,#}]");
Out.WriteLine($"With group separator and colour: [{i:Blue,##,#}]");
Out.WriteLine($"Spacing only: [{i,8}]");
Out.WriteLine($"Spacing and format: [{i,8:X2}]");
Out.WriteLine($"Colour and spacing: [{i,8:Cyan}]");
Out.WriteLine($"Colour and spacing and format: [{i,8:Green,X2}]");
Out.WriteLine($"Thank {"you":Green} for {"your":Cyan} attention.");
Console.WriteLine();