# comma-separated-aggregations

Small **.NET 10** practice repository: parsing **comma-separated integers**, computing **per-line aggregates**, and reading input from **`TextReader`** / **files** (sync + async). Includes **xUnit** tests.

## Requirements

- **.NET SDK 10.x** (`TargetFramework` is `net10.0`)

Check:

```powershell
dotnet --version
```

## Build & test

From the repo root:

```powershell
dotnet build
dotnet test
```

## Run the CLI

From the repo root:

```powershell
dotnet run --project .\src\CsharpPhase1.Cli\CsharpPhase1.Cli.csproj -- --demo
```

Sum all comma-separated integers across all lines in a file:

```powershell
dotnet run --project .\src\CsharpPhase1.Cli\CsharpPhase1.Cli.csproj -- .\path\to\input.txt
```

### Configuration (`appsettings.json`)

The CLI reads `src/CsharpPhase1.Cli/appsettings.json` (copied next to the built executable).

- **`Cli:DefaultInputFile`**: if set, you can run the CLI **with no arguments** and it will read this path.
- **`Cli:Verbose`**: if `true`, prints the resolved input file path to stderr.

### Input format

- Each line is a comma-separated list of integers, e.g. `1, 2, 3`
- Empty / whitespace-only lines sum to **0** (same rules as `ParsingBasics`)

### CLI exit codes

- **0**: success
- **1**: missing args / usage
- **2**: file/path/access errors
- **3**: invalid number format in file

## Project layout

- `src/CsharpPhase1`: library code (`Week1` exercises)
- `src/CsharpPhase1.Cli`: tiny console runner
- `tests/CsharpPhase1.Tests`: xUnit tests

## Notes

This repo is intentionally **domain-neutral** (no “business story”), focused on fundamentals: parsing, aggregation, I/O, async/cancellation, and tests.
