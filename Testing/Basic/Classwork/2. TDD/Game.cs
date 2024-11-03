using System.Drawing;

namespace TDD.Task;

public class Game
{
    private IList<Frame> frames = new List<Frame>();
    private const int maxPins = 10;
    
    public void Roll(int pins)
    {
        if (pins < 0 || pins > maxPins)
        {
            throw new ArgumentException("Invalid pin count.");
        }
        if (!frames.Any())
        {
            frames.Add(new Frame());
        }
        
        CalculateBonus(pins);
        
        var currentFrame = frames.Last();
        if (currentFrame.IsEnd)
        {
            currentFrame = new Frame();
            frames.Add(currentFrame);
        }
        currentFrame.AddRoll(pins);
    }

    public int GetScore()
    {
        return frames.Sum(frame => frame.GetFrameScore());
    }
    
    private void CalculateBonus(int pins)
    {
        var currentFrame = frames.Last();
        var lastFrame = frames.Count > 1 ? frames[^2] : null;

        if (currentFrame.WithSpareBonus() || currentFrame.WithStrikeBonus())
        {
            currentFrame.AddBonus(pins);
        }

        if (lastFrame != null && lastFrame.WithStrikeBonus())
        {
            lastFrame.AddBonus(pins);
        }
    }
}

class Frame
{
    public int? FirstRoll { get; private set; }
    public int? SecondRoll { get; private set; }
    public IList<int> Bonus { get; } = [];

    public bool IsEnd => SecondRoll != null || FirstRoll == 10;

    public bool WithSpareBonus()
    {
        return FirstRoll + SecondRoll == 10;
    }

    public bool WithStrikeBonus()
    {
        return FirstRoll == 10;
    }

    public void AddBonus(int bonusValue)
    {
        Bonus.Add(bonusValue);
    }

    public void AddRoll(int pinCount)
    {
        if (FirstRoll == null)
        {
            if (pinCount == 10)
            {
                SecondRoll = 0;
            }

            FirstRoll = pinCount;
        }
        else if (SecondRoll == null)
        {
            SecondRoll = pinCount;
        }
    }

    public int GetFrameScore()
    {
        return (FirstRoll ?? 0) + (SecondRoll ?? 0) + Bonus.Sum();
    }
}