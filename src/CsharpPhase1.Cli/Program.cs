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
        Console.WriteLine("Usage: CsharpPhase1.Cli <file> | --demo | --sum-url <url>");
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

if (args.Length > 0 && args[0] == "--sum-url")
{
    if (args.Length < 2)
    {
        Console.WriteLine("Usage: CsharpPhase1.Cli --sum-url <url>");
        Environment.ExitCode = 1;
        return;
    }

    var url = args[1];

    // Таймаут на всю операцию (и скачивание, и парсинг).
    using var cts = new CancellationTokenSource();
    var timeoutMs = Math.Clamp(cliOptions.HttpTimeoutMs, 1, 300_000);
    cts.CancelAfter(TimeSpan.FromMilliseconds(timeoutMs));

    try
    {
        using var http = new HttpClient();
        http.Timeout = TimeSpan.FromMilliseconds(timeoutMs);

        // Скачиваем ответ по URL с учётом отмены/таймаута.
        using var response = await http.GetAsync(url, cts.Token);

        // Если сервер вернул 404/500 и т.п. — это не “валидный ввод”, выдаём ошибку.
        if (!response.IsSuccessStatusCode)
        {
            Console.Error.WriteLine($"HTTP error: {(int)response.StatusCode} {response.ReasonPhrase}");
            Environment.ExitCode = 4;
            return;
        }

        // Тело ответа как текст.
        var content = await response.Content.ReadAsStringAsync(cts.Token);

        //обертка строки в поток для парсинга
        using var reader = new StringReader(content);
        var totalSum = await CommaSeparatedLines.TotalSumFromAllLinesAsync(reader, cts.Token);
        Console.WriteLine($"Total sum: {totalSum}");
        Environment.ExitCode = 0;
        return;
    }
    catch (OperationCanceledException)
    {
        Console.Error.WriteLine("Request cancelled (timeout or Ctrl+C).");
        Environment.ExitCode = 5;
        return;
    }
    catch (HttpRequestException e)
    {
        Console.Error.WriteLine($"HTTP request failed: {e.Message}");
        Environment.ExitCode = 4;
        return;
    }
    catch (FormatException e)
    {
        Console.Error.WriteLine($"Invalid format from URL: {url} {e.Message}");
        Environment.ExitCode = 3;
        return;
    }
}

var path = args.Length > 0 ? args[0] : cliOptions.DefaultInputFile;
if (string.IsNullOrWhiteSpace(path))
{
    Console.WriteLine("Usage: CsharpPhase1.Cli <file> | --demo | --sum-url <url>");
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
    public int HttpTimeoutMs { get; set; } = 10_000;
}





