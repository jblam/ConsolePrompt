using JBlam.ConsolePrompt;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
int i = 17_345;

Out.WriteLine($"Plain {i} / format {i:X2} / group-sep {i:##,#} / spacing: {i,8} / spacing-format: {i,8:X2}");
Out.WriteLine($"Plain {i:Red} / format {i:Magenta,X2} / group-sep {i:Blue,##,#} / spacing: {i,8:Cyan} / spacing-format: {i,8:Green,X2}");
Out.WriteLine($"Thank {"you":Green} for {"your":Cyan} attention.");
Console.WriteLine();
