using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days;


public class Day16Tests
{
    private readonly ISolution _sut = new Day16();
    
    private readonly string[] _testData =
    {
        "D2FE28"
    };


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

        actual.Should().Be("6");
    }

    [Theory]
    [InlineData("EE00D40C823060", "14")]
    [InlineData("8A004A801A8002F478", "16")]
    [InlineData("620080001611562C8802118E34", "12")]
    [InlineData("C0015000016115A2E0802F182340", "23")]
    [InlineData("A0016C880162017C3686B18A3D4780", "31")]
    public void PartOne_WhenCalled_ReturnsCorrectVersionSum(string transmission, string versionSum)
    {
        var actual = _sut.PartOne(new []{ transmission });

        actual.Should().Be(versionSum);
    }
    [Fact]
    public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
    {
        Action act = () => _sut.PartTwo(_sut.Input());

        act.Should().NotThrow<NotImplementedException>();
    }
    
    [Theory]
    [InlineData("C200B40A82", "3")]
    [InlineData("04005AC33890", "54")]
    [InlineData("880086C3E88112", "7")]
    [InlineData("CE00C43D881120", "9")]
    [InlineData("D8005AC2A8F0", "1")]   
    [InlineData("F600BC2D8F", "0")]
    [InlineData("9C005AC2F8F0", "0")]
    [InlineData("9C0141080250320F1802104A08", "1")]
    public void PartTwo_WhenCalled_ReturnsCorrectTestAnswer(string transmission, string value)
    {
        var actual = _sut.PartTwo(new []{ transmission });

        actual.Should().Be(value);
    }
}