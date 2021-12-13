using AdventOfCode.Days;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests.Days;


public class Day13Tests
{
    private ITestOutputHelper output;
    private readonly ISolution _sut = new Day13();
    
    private readonly string[] _testData =
    {
        "6,10",
        "0,14",
        "9,10",
        "0,3",
        "10,4",
        "4,11",
        "6,0",
        "6,12",
        "4,1",
        "0,13",
        "10,12",
        "3,4",
        "3,0",
        "8,4",
        "1,10",
        "2,14",
        "8,10",
        "9,0",
        "",
        "fold along y=7",
        "fold along x=5"
    };


    public Day13Tests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
    {
        Action act = () => _sut.PartOne(_sut.Input());

        act.Should().NotThrow<NotImplementedException>();
    }
    
    [Fact]
    public void PartOne_WhenCalled_ReturnsCorrectTestAnswer()
    {
        var actual = _sut.PartOne(_testData);

        actual.Should().Be("17");
    }


    [Fact]
    public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
    {
        Action act = () => _sut.PartTwo(_sut.Input());

        act.Should().NotThrow<NotImplementedException>();
    }
    
    [Fact]
    public void PartTwo_WhenCalled_ReturnsCorrectTestAnswer()
    {
        var actual = _sut.PartTwo(_testData);

        actual.Should().Be(@"
▓▓▓▓▓
▓   ▓
▓   ▓
▓   ▓
▓▓▓▓▓
");
    }
}