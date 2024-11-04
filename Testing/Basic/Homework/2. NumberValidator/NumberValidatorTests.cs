using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    private NumberValidator baseValidNumberValidator;
    
    [SetUp]
    public void SetUp()
    {
        baseValidNumberValidator = new NumberValidator(5);
    }
    
    [Test]
    [TestCaseSource(nameof(notPositivePrecisionTestCases))]
    public void Constructor_ShouldWithException_WhenNotPositivePrecision(int precision)
    {
        var action = () => new NumberValidator(precision);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }
    
    private static IEnumerable<TestCaseData> notPositivePrecisionTestCases =
        [
            new TestCaseData(-1)
                .SetName("negativePrecision"),
            new TestCaseData(0)
                .SetName("zeroPrecision"),
        ];
    
    [Test]
    [TestCaseSource(nameof(badScaleTestCases))]
    public void Constructor_ShouldWithException_WhenBadScale(int precision, int scale)
    {
        var action = () => new NumberValidator(precision, scale);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }
    
    private static IEnumerable<TestCaseData> badScaleTestCases =
    [
        new TestCaseData(1, -1)
            .SetName("negativeScale"),
        new TestCaseData(1, 2)
            .SetName("scaleGreaterThanPrecision"),
        new TestCaseData(1, 1)
            .SetName("scaleEqualPrecision"),
    ];
    
    [Test]
    [TestCaseSource(nameof(validParametersTestCases))]
    public void Constructor_ShouldNotException_WhenValidParameters(int precision, int scale, bool onlyPositive)
    {
        var action = () => new NumberValidator(precision, scale);

        action.Should()
            .NotThrow<ArgumentException>();
    }
    
    private static IEnumerable<TestCaseData> validParametersTestCases =
    [
        new TestCaseData(5, 0, false)
            .SetName("zeroScaleAndPositivePrecision"),
        new TestCaseData(5, 0, true)
            .SetName("zeroScalePositivePrecisionOnlyPositive"),
        new TestCaseData(5, 1, false)
            .SetName("positiveScaleAndPositivePrecision"),
    ];
    
    [Test]
    [TestCaseSource(nameof(badValueParametersTestCases))]
    public void IsValidNumber_ShouldFalse_WhenBadValueParameter(string value)
    {
        var isValid = baseValidNumberValidator.IsValidNumber(value);
        
        isValid.Should().BeFalse();
    }
    
    private static IEnumerable<TestCaseData> badValueParametersTestCases =
    [
        new TestCaseData(null)
            .SetName("nullValue"),
        new TestCaseData(string.Empty)
            .SetName("emptyValue"),
        new TestCaseData(" ")
            .SetName("whitespaceValue"),
    ];
    
    [Test]
    [TestCaseSource(nameof(baseInvalidValueParametersTestCases))]
    public void IsValidNumber_ShouldFalse_WhenBaseInvalidFormat(string value)
    {
        var isValid = baseValidNumberValidator.IsValidNumber(value);
        
        isValid.Should().BeFalse();
    }

    private static IEnumerable<TestCaseData> baseInvalidValueParametersTestCases =
    [
        new TestCaseData("abc")
            .SetName("notNumber"),
        new TestCaseData("123a")
            .SetName("numberWithLetter"),
        new TestCaseData("a.sd")
            .SetName("lettersAndPoint"),
        new TestCaseData("123привет")
            .SetName("numberWithRussianLetters"),
        new TestCaseData("0.")
            .SetName("unfinishedFractionalNumber"),
        new TestCaseData(".")
            .SetName("onlyPointWithoutDigits"),
        new TestCaseData(",")
            .SetName("onlyPointWithoutDigits"),
        new TestCaseData(".1")
            .SetName("withoutNumberBeforePoint"),
        new TestCaseData("1.1.0")
            .SetName("manyPoint"),
        new TestCaseData("++1")
            .SetName("manyPlusSymbolsBeforeNumber"),
        new TestCaseData("--1")
            .SetName("manyMinusSymbolsBeforeNumber"),
        new TestCaseData("#@1")
            .SetName("manyInvalidSymbolsBeforeNumber"),
        new TestCaseData("12 34")
            .SetName("numberWithWhiteSpace"),
        new TestCaseData("12!34")
            .SetName("numberWithSymbol"),
    ];
    
    [Test]
    [TestCaseSource(nameof(manyIntegerPartNumberInvalidValueParametersTestCases))]
    [TestCaseSource(nameof(manyFractionalPartNumberInvalidValueParametersTestCases))]
    public void IsValidNumber_ShouldFalse_WhenManyDigitsInvalidFormat(string value)
    {
        var numberValidator = new NumberValidator(3, 1);
        
        var isValid = numberValidator.IsValidNumber(value);
        
        isValid.Should().BeFalse();
    }

    private static IEnumerable<TestCaseData> manyIntegerPartNumberInvalidValueParametersTestCases =
    [
        new TestCaseData("9999")
            .SetName("manyDigits"),
        new TestCaseData("-123")
            .SetName("manyDigitsCountInNegativeNumber"),
        new TestCaseData("+123")
            .SetName("manyDigitsWithPlus"),
    ];
    
    private static IEnumerable<TestCaseData> manyFractionalPartNumberInvalidValueParametersTestCases =
    [
        new TestCaseData("-12.3")
            .SetName("manyNegativeNumberWithPoint"),
        new TestCaseData("0.34")
            .SetName("tooManyDigitsBeforeDecimalPoint"),
        new TestCaseData("123.4")
            .SetName("manyDigitsWithPoint"),
    ];
    
    [Test]
    [TestCaseSource(nameof(negativeValueTestCases))]
    public void IsValidNumber_ShouldFalse_WhenOnlyPositiveNumberValidator(string value)
    {
        var numberValidator = new NumberValidator(3, 1, true);
        
        var isValid = numberValidator.IsValidNumber(value);
        
        isValid.Should().BeFalse();
    }
    
    [Test]
    [TestCaseSource(nameof(negativeValueTestCases))]
    [TestCaseSource(nameof(positiveValueTestCases))]
    public void IsValidNumber_ShouldTrue_WhenNotOnlyPositiveNumberValidator(string value)
    {
        var numberValidator = new NumberValidator(3, 1, false);
        
        var isValid = numberValidator.IsValidNumber(value);
        
        isValid.Should().BeTrue();
    }
    
    private static IEnumerable<TestCaseData> negativeValueTestCases =
    [
        new TestCaseData("-12")
            .SetName("NegativeNumber"),
        new TestCaseData("-1.2")
            .SetName("negativeNumberWithDecimalPoint"),
    ];
    
    private static IEnumerable<TestCaseData> positiveValueTestCases =
    [
        new TestCaseData("12")
            .SetName("PositiveNumber"),
        new TestCaseData("1.2")
            .SetName("PositiveNumberWithPoint"),
    ];
    
    [Test]
    [TestCaseSource(nameof(validIntegerPartNumberTestCases))]
    [TestCaseSource(nameof(validFractionalPartNumberTestCases))]
    public void IsValidNumber_ShouldTrue_WhenValidValue(string value)
    {
        var numberValidator = new NumberValidator(3, 2);
        
        var isValid = numberValidator.IsValidNumber(value);
        
        isValid.Should().BeTrue();
    }
    
    private static IEnumerable<TestCaseData> validIntegerPartNumberTestCases =
    [
        new TestCaseData("0")
            .SetName("oneDigit"),
        new TestCaseData("999")
            .SetName("maxNumber"),
        new TestCaseData("-99")
            .SetName("minNumber"),
    ];
    
    private static IEnumerable<TestCaseData> validFractionalPartNumberTestCases =
    [
        new TestCaseData("0.00")
            .SetName("onlyZeroFractionalNumber"),
        new TestCaseData("0,00")
            .SetName("numberWithCommaSeparator"),
        new TestCaseData("-0.0")
            .SetName("onlyZeroFractionalNegativeNumber"),
        new TestCaseData("-1.0")
            .SetName("negativeNumber"),
        new TestCaseData("9.99")
            .SetName("maxFractionalNumber"),
        new TestCaseData("-9.9")
            .SetName("minFractionalNumber"),
    ];
}