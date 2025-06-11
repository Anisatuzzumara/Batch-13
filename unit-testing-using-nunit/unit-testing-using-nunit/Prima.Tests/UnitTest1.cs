namespace Prima.Tests;

public class Tests
{
    private Prima _prima;

    [SetUp]
    public void Setup()
    {
        _prima = new Prima();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test]
    public void IsPrima_BasicTest()
    {
        int number = 7;
        bool result = _prima.IsPrima(number);
        Assert.That(result, Is.True);
    }
}