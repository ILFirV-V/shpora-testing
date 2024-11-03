using FluentAssertions;
using NUnit.Framework;
using TDD.Task;

namespace TDD;

public class GameTests
{
    [Test]
    public void HaveZeroScore_BeforeAnyRolls()
    {
        new Game()
            .GetScore()
            .Should().Be(0);
    }

    [Test]
    [TestCaseSource(nameof(pins))]
    public void GetScore_Should_SimpleRoll(int pinsCount)
    {
        var game = new Game();
        
        game.Roll(pinsCount);
        
        game.GetScore().Should().Be(pinsCount);
    }

    private static int[] pins = [0, 5, 9];
    
    [Test]
    [TestCaseSource(nameof(pinsSource))]
    public void GetScore_Should_Equals(IEnumerable<int> pins, int result)
    {
        var game = new Game();

        foreach (var pin in pins)
        {
            game.Roll(pin);
        }
        
        game.GetScore().Should().Be(result);
    }

    private static IEnumerable<TestCaseData> pinsSource =
        new List<TestCaseData>()
        {
            new TestCaseData(new List<int>() {1, 2, 3, 4}, 10)
                .SetName("SimpleTest"),
            new TestCaseData(new List<int>() {1, 9, 5, 4}, 24)
                .SetName("WithOneSpare"),
            new TestCaseData(new List<int>() {1, 9, 5, 5, 8}, 41)
                .SetName("WithTwoSpare"),
            new TestCaseData(new List<int>() {0, 10, 5, 5, 8}, 41)
                .SetName("WithSpareInTwoPins"),
            new TestCaseData(new List<int>() {10, 1}, 12)
                .SetName("WithStrikeAndOnePins"),
            new TestCaseData(new List<int>() {10, 1, 2}, 16)
                .SetName("WithStrikeAndTwoPins"),
        };
}