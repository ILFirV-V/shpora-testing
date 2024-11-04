using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    private Person actualTsar;
    private Person expectedTsar;

    [SetUp]
    public void SetUp()
    {
        actualTsar = TsarRegistry.GetCurrentTsar();
        expectedTsar = GetExpectedTsar();
    }

    private Person GetExpectedTsar()
    {
        return new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));
    }
    
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        // Это решение лучше, чем CheckCurrentTsar_WithCustomEquality по следующим причинам: 
        // 1) Нет зависимостей от конкретных имен свойств и полей,
        // нужно лишь менять GetExpectedTsar() при изменении класса по необходимости
        // 2) Более лаконичное решение за счет Fluent Assertions
        // 3) Легко настраивать под нужные условия проверки
        // (например если добавится другой класс и там нужно исключить какое-то определенное свойство)
        // 4) У этого решения есть подробное описание почему упал тест
        // 5) Есть защита от бесконечной рекурсии, Fluent Assertions по умолчанию выполняет рекурсию до 10 уровней,
        // но это можно настраивать
        // 6) Используется подготовка данных к тесту в SetUp
        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options.Excluding((IMemberInfo x) => x.Name == nameof(Person.Id)));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        // 1) Нужно актуализировать метод AreEqual при изменениях
        // 2) Нет детальной информации почему упал тест
        // 3) Трудно читать и можно запутаться в условии проверки
        // 4) Возможно переполнение из-за рекурсии 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
