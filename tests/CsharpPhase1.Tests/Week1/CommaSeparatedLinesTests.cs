using System.IO;
using System.Text;
using System.Threading;
using CsharpPhase1.Week1;

namespace CsharpPhase1.Tests.Week1;

public class CommaSeparatedLinesTests
{
    [Fact]
    public void TotalSumFromFile_multiple_lines()
    {
        var path = Path.GetTempFileName();

        try
        {
            File.WriteAllLines(path, new[] { "1, 2", "10" });
            Assert.Equal(13, CommaSeparatedLines.TotalSumFromFile(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void TotalSumFromFile_empty_file_is_zero()
    {
        var path = Path.GetTempFileName();

        try
        {
            File.WriteAllText(path, "");
            Assert.Equal(0, CommaSeparatedLines.TotalSumFromFile(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void EnumerateLineSums_multiple_lines_and_empty_line_is_zero()
    {
        using var reader = new StringReader("1,2\n\n3, 4");
        Assert.Equal(new long[] { 3, 0, 7 }, CommaSeparatedLines.EnumerateLineSums(reader).ToArray());
    }

    [Fact]
    public void TotalSum_from_sample()
    {
        using var reader = new StringReader("10\n1,1,1");
        Assert.Equal(13, CommaSeparatedLines.TotalSumFromAllLines(reader));
    }

    [Fact]
    public void TotalSum_empty_input_is_zero()
    {
        using var reader = new StringReader("");
        Assert.Equal(0, CommaSeparatedLines.TotalSumFromAllLines(reader));
    }

    [Fact]
    public void EnumerateLineSums_invalid_second_line_throws_FormatException_when_advanced()
    {
        using var reader = new StringReader("1,2\noops");
        using var e = CommaSeparatedLines.EnumerateLineSums(reader).GetEnumerator();
        Assert.True(e.MoveNext());
        Assert.Equal(3, e.Current);
        Assert.Throws<FormatException>(() => e.MoveNext());
    }

    [Fact]
    public void EnumerateLineSums_null_reader_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => CommaSeparatedLines.EnumerateLineSums(null!).ToArray());
    }

    [Fact]
    public void TotalSum_null_reader_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => CommaSeparatedLines.TotalSumFromAllLines(null!));
    }

    [Fact]
    public void EnumerateLineSums_works_with_UTF8_file_via_StreamReader()
    {
        var bytes = Encoding.UTF8.GetBytes("2,2\r\n3");
        using var stream = new MemoryStream(bytes);
        using var reader = new StreamReader(stream, Encoding.UTF8);
        Assert.Equal(new long[] { 4, 3 }, CommaSeparatedLines.EnumerateLineSums(reader).ToArray());
    }

    [Fact]
    public async Task TotalSumFromFileAsync_multiple_lines()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(path, new[] { "1, 2", "10" });
            Assert.Equal(13, await CommaSeparatedLines.TotalSumFromFileAsync(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task TotalSumFromFileAsync_throws_when_cancelled()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(path, new[] { "1", "2", "3" });
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => CommaSeparatedLines.TotalSumFromFileAsync(path, cts.Token));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task TotalSumFromAllLinesAsync_sample_via_StringReader()
    {
        using var reader = new StringReader("10\n1,1,1");
        Assert.Equal(13, await CommaSeparatedLines.TotalSumFromAllLinesAsync(reader));
    }

    [Fact]
    public async Task TotalSumFromAllLinesAsync_throws_when_cancelled()
    {
        using var reader = new StringReader("1\n2\n3");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => CommaSeparatedLines.TotalSumFromAllLinesAsync(reader, cts.Token));
    }
}
