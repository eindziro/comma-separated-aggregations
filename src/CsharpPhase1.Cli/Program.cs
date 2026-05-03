using CsharpPhase1.Week1;


if (args.Length == 0)
{
    Console.WriteLine("Usage: CsharpPhase1.Cli <file> | --demo");
    Environment.ExitCode = 1;
    return;
}
if (args[0] == "--demo")
{
    Console.WriteLine("CsharpPhase1 — фаза 1 (консоль для быстрых проверок).");
    var sample = "1, 2, 3";
    Console.WriteLine($"Пример: сумма \"{sample}\" = {ParsingBasics.SumCommaSeparatedIntegers(sample)}");
    Console.WriteLine($"         min = {ParsingBasics.MinCommaSeparatedIntegers(sample)}, max = {ParsingBasics.MaxCommaSeparatedIntegers(sample)}");
    Console.WriteLine("Тесты: из корня решения выполните  dotnet test");
    Environment.ExitCode = 0;
    return;
}

string path = args[0];

try
{
    using var reader = File.OpenText(path);
    var totalSum = CommaSeparatedLines.TotalSumFromAllLines(reader);
    Console.WriteLine($"Total sum: {totalSum}");
    Environment.ExitCode = 0;
}
catch (FileNotFoundException)
{
    Console.WriteLine($"File not found: {path}");
    Environment.ExitCode = 2;
}
catch (DirectoryNotFoundException)
{
    Console.WriteLine($"Directory Not found: {path}");
    Environment.ExitCode = 2;
}
catch (UnauthorizedAccessException)
{
    Console.Error.WriteLine($"No access to file: {path}");
    Environment.ExitCode = 2;
}
catch (FormatException e)
{
    Console.Error.WriteLine($"Invalid format: {path} {e.Message}");
    Environment.ExitCode = 3;
}





