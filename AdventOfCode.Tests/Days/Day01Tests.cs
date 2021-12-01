using System;
using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days
{
    
    public class Day01Tests
    {
        private readonly ISolution _sut = new Day01();
        private readonly string[] _testData = {
            "199",
            "200",
            "208",
            "210",
            "200",
            "207",
            "240",
            "269",
            "260",
            "263"
        };

        [Fact]
        public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartOne(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
        
        [Fact]
        public void PartOne_WhenCalled_ReturnsCorrectTestAnswer()
        {
            var actual =  _sut.PartOne(_testData);

            actual.Should().Be("7");
        }
        
        [Fact]
        public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
        {
            Action act = () =>  _sut.PartTwo(_sut.Input());
            
            act.Should().NotThrow<NotImplementedException>();
        }
        
                
        [Fact]
        public void PartTwo_WhenCalled_ReturnsCorrectTestAnswer()
        {
            var actual =  _sut.PartTwo(_testData);

            actual.Should().Be("5");
        }
    }
}