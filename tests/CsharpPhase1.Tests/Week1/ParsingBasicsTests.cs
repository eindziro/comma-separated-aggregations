using CsharpPhase1.Week1;

namespace CsharpPhase1.Tests.Week1;

public class ParsingBasicsTests
{
    [Fact]
    public void Sum_empty_or_whitespace_is_zero()
    {
        Assert.Equal(0, ParsingBasics.SumCommaSeparatedIntegers(""));
        Assert.Equal(0, ParsingBasics.SumCommaSeparatedIntegers("   "));
    }

    [Fact]
    public void Sum_single_number()
    {
        Assert.Equal(42, ParsingBasics.SumCommaSeparatedIntegers("42"));
    }

    [Fact]
    public void Sum_several_with_spaces()
    {
        Assert.Equal(6, ParsingBasics.SumCommaSeparatedIntegers("1, 2, 3"));
    }

    [Fact]
    public void Sum_negative_numbers()
    {
        Assert.Equal(-1, ParsingBasics.SumCommaSeparatedIntegers("2,-3"));
    }

    [Fact]
    public void Sum_invalid_token_throws_FormatException()
    {
        Assert.Throws<FormatException>(() => ParsingBasics.SumCommaSeparatedIntegers("1,x,3"));
    }

    [Fact]
    public void Sum_null_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ParsingBasics.SumCommaSeparatedIntegers(null!));
    }

    [Fact]
    public void TrySum_returns_false_on_invalid()
    {
        Assert.False(ParsingBasics.TrySumCommaSeparatedIntegers("1,a", out var sum));
        Assert.Equal(0, sum);
    }

    [Fact]
    public void TrySum_returns_true_and_value_on_valid()
    {
        Assert.True(ParsingBasics.TrySumCommaSeparatedIntegers("10, 5", out var sum));
        Assert.Equal(15, sum);
    }

    [Fact]
    public void Min_Max_single_number()
    {
        Assert.Equal(-7, ParsingBasics.MinCommaSeparatedIntegers("-7"));
        Assert.Equal(-7, ParsingBasics.MaxCommaSeparatedIntegers("-7"));
    }

    [Fact]
    public void Min_Max_several_with_spaces()
    {
        Assert.Equal(1, ParsingBasics.MinCommaSeparatedIntegers("3, 1, 2"));
        Assert.Equal(3, ParsingBasics.MaxCommaSeparatedIntegers("3, 1, 2"));
    }

    [Fact]
    public void Min_Max_negatives()
    {
        Assert.Equal(-5, ParsingBasics.MinCommaSeparatedIntegers("-1, -5, 0"));
        Assert.Equal(0, ParsingBasics.MaxCommaSeparatedIntegers("-1, -5, 0"));
    }

    [Fact]
    public void Min_Max_empty_or_whitespace_throws_InvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => ParsingBasics.MinCommaSeparatedIntegers(""));
        Assert.Throws<InvalidOperationException>(() => ParsingBasics.MaxCommaSeparatedIntegers("   "));
    }

    [Fact]
    public void Min_Max_only_commas_throws_InvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => ParsingBasics.MinCommaSeparatedIntegers(" , , "));
    }

    [Fact]
    public void Min_Max_invalid_token_throws_FormatException()
    {
        Assert.Throws<FormatException>(() => ParsingBasics.MinCommaSeparatedIntegers("1,x"));
        Assert.Throws<FormatException>(() => ParsingBasics.MaxCommaSeparatedIntegers("1,x"));
    }

    [Fact]
    public void Min_Max_null_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ParsingBasics.MinCommaSeparatedIntegers(null!));
        Assert.Throws<ArgumentNullException>(() => ParsingBasics.MaxCommaSeparatedIntegers(null!));
    }

    [Fact]
    public void Average_single_number(){
        Assert.Equal(42,ParsingBasics.AverageCommaSeparatedIntegers("42"));
    }

    [Fact]
    public void Average_with_float_result(){
        Assert.Equal(3.5, ParsingBasics.AverageCommaSeparatedIntegers("3, 4"));
    }

    [Fact]
    public void Average_empty_or_whitespace_throws_InvalidOperationException(){
        Assert.Throws<InvalidOperationException>(()=>ParsingBasics.AverageCommaSeparatedIntegers(""));
        Assert.Throws<InvalidOperationException>(()=>ParsingBasics.AverageCommaSeparatedIntegers("   "));
        Assert.Throws<InvalidOperationException>(()=>ParsingBasics.AverageCommaSeparatedIntegers(" , , "));
    }

    [Fact]
    public void Average_throws_FormatException_on_invalid_token(){
        Assert.Throws<FormatException>(()=>ParsingBasics.AverageCommaSeparatedIntegers("1,x"));
    }

    [Fact]
    public void Average_null_throws_ArgumentNullException(){
        Assert.Throws<ArgumentNullException>(()=>ParsingBasics.AverageCommaSeparatedIntegers(null!));
    }
}
