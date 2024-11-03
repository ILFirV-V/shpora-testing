using Basic.Task.WordsStatistics.WordsStatistics;
using FluentAssertions;
using NUnit.Framework;

namespace Basic.Task.WordsStatistics;

// Документация по FluentAssertions с примерами : https://github.com/fluentassertions/fluentassertions/wiki

[TestFixture]
public class WordsStatisticsTests
{
    private IWordsStatistics wordsStatistics;

    [SetUp]
    public void SetUp()
    {
        wordsStatistics = CreateStatistics();
    }

    public virtual IWordsStatistics CreateStatistics()
    {
        // меняется на разные реализации при запуске exe
        return new WordsStatisticsImpl();
    }


    [Test]
    public void GetStatistics_IsEmpty_AfterCreation()
    {
        wordsStatistics.GetStatistics().Should().BeEmpty();
    }

    [Test]
    public void GetStatistics_ContainsItem_AfterAddition()
    {
        wordsStatistics.AddWord("abc");
        
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
    }

    [Test]
    public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.AddWord("def");
        
        wordsStatistics.GetStatistics().Should().HaveCount(2);
    }
    
    [Test]
    [TestCaseSource(nameof(TenNumbersBoundaryValues))]
    [TestCaseSource(nameof(TenCharsBoundaryValues))]
    [TestCaseSource(nameof(OneBoundaryValues))]
    [TestCaseSource(nameof(LetterCaseValues))]
    [TestCaseSource(nameof(SpecialCharactersValues))]
    [TestCaseSource(nameof(RussianValues))]
    public void AddWord_Should_ContainsWord_AfterAddOneWord(string word)
    {
        wordsStatistics.AddWord(word);
        
        wordsStatistics.GetStatistics().Should().HaveCount(1);
    }
    
    static string[] TenNumbersBoundaryValues = [ "123456789", "1234567891", "12345678912" ];
    static string[] TenCharsBoundaryValues = [ "aaaaaaaaa", "aaaaaaaaaa", "aaaaaaaaaaa" ];
    static string[] RussianValues = [ "Привет", "Hello" ];
    static string[] OneBoundaryValues = [ "1", "a" ];
    static string[] LetterCaseValues = [ "A", "a" ];
    static string[] SpecialCharactersValues = [ "$", "!", "@", "#", "%", "^", "&", "*", 
        "(", ")", "-", "=", "+", "/", "\\", "'" ];

    [Test]
    [TestCaseSource(nameof(EmptyValues))]
    public void AddWord_Should_NotContainsWord_AfterAddEmptyOrWhiteSpaceWord(string word)
    {
        wordsStatistics.AddWord(word);
        wordsStatistics.GetStatistics().Should().HaveCount(0);
    }
    
    static string[] EmptyValues = [ " ", string.Empty, "   ", "         ", "          ", "           "];
    
    [Test]
    public void AddWord_Should_WithException_AfterAddNull()
    {
        Assert.That(() => wordsStatistics.AddWord(null), Throws.ArgumentNullException);
    }
    
    [Test]
    [TestCaseSource(nameof(DifferentWords))]
    public void GetStatistics_Should_ResultCountEqualsCountWords_AfterAddMoreDifferentWord(IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            wordsStatistics.AddWord(word);
        }
        
        wordsStatistics.GetStatistics().Should().HaveCount(words.Count());
    }
    
    static IEnumerable<IEnumerable<string>> DifferentWords = 
        [ 
            new List<string>() {"123456789", "1234567891" },
            new List<string>() {"123456789", "1234567891" },
        ];
    
    [Test]
    [TestCaseSource(nameof(SimilarWords))]
    public void GetStatistics_Should_ContainsWord_AfterAddMoreWord(IEnumerable<string> words, int resultCount)
    {
        foreach (var word in words)
        {
            wordsStatistics.AddWord(word);
        }
        
        wordsStatistics.GetStatistics().Should().HaveCount(resultCount);
    }
    
    static IEnumerable<object[]> SimilarWords = 
    [
        [new List<string>() {"A", "a", "A", "A", "A" }, 1],
        [new List<string>() {"A", "A", "A" }, 1],
        [new List<string>() {"AaaAaaaaaa", "AAAAAAAAAAA", "aaaaaaaaaaa" }, 1],
    ];
    
    [Test]
    public void GetStatistics_Should_Order_AfterAddMoreWord()
    {
        var words = new List<string>()
        {
            "qwe", "qwe", "ewq"
        };
        foreach (var word in words)
        {
            wordsStatistics.AddWord(word);
        }
        var result = new List<WordCount>()
        {
            new WordCount("qwe", 2),
            new WordCount("ewq", 1),
        };
        wordsStatistics.GetStatistics().Should().BeEquivalentTo(result);
    }
    
    public void GetStatistics_Should_Order_AfterAddMoreWord2()
    {
        var words = new List<string>()
        {
            "qwe", "qwe", "ewq"
        };
        foreach (var word in words)
        {
            wordsStatistics.AddWord(word);
        }
        var result = new List<WordCount>()
        {
            new WordCount("qwe", 2),
            new WordCount("ewq", 1),
        };
        wordsStatistics.GetStatistics().Should().BeEquivalentTo(result);
    }
}
