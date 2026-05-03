using CsharpPhase1.Week1;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var cliOptions = configuration.GetSection("Cli").Get<CliOptions>() ?? new CliOptions();

if (args.Length == 0)
{
    if (string.IsNullOrWhiteSpace(cliOptions.DefaultInputFile))
    {
        Console.WriteLine("Usage: CsharpPhase1.Cli <file> | --demo");
        Console.WriteLine("Tip: set Cli:DefaultInputFile in appsettings.json to run without arguments.");
        Environment.ExitCode = 1;
        return;
    }
}
if (args.Length > 0 && args[0] == "--demo")
{
    Console.WriteLine("CsharpPhase1 — фаза 1 (консоль для быстрых проверок).");
    var sample = "1, 2, 3";
    Console.WriteLine($"Пример: сумма \"{sample}\" = {ParsingBasics.SumCommaSeparatedIntegers(sample)}");
    Console.WriteLine($"         min = {ParsingBasics.MinCommaSeparatedIntegers(sample)}, max = {ParsingBasics.MaxCommaSeparatedIntegers(sample)}");
    Console.WriteLine("Тесты: из корня решения выполните  dotnet test");
    Environment.ExitCode = 0;
    return;
}

var path = args.Length > 0 ? args[0] : cliOptions.DefaultInputFile;
if (string.IsNullOrWhiteSpace(path))
{
    Console.WriteLine("Usage: CsharpPhase1.Cli <file> | --demo");
    Console.WriteLine("Tip: set Cli:DefaultInputFile in appsettings.json to run without arguments.");
    Environment.ExitCode = 1;
    return;
}

if (cliOptions.Verbose)
    Console.Error.WriteLine($"Resolved input file: {path}");

try
{
    var totalSum = await CommaSeparatedLines.TotalSumFromFileAsync(path);
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

file class CliOptions
{
    public string? DefaultInputFile { get; set; }
    public bool Verbose { get; set; }
}





