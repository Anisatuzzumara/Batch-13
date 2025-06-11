using NUnit.Framework;

namespace Prima.Tests
{
    [TestFixture]
    public class Prima_FindPrimaUpToShould
    {
        private Prima _prima;

        [SetUp]
        public void SetUp()
        {
            _prima = new Prima();
        }

        [Test]
        public void FindPrimaUpTo_InputIs2_ReturnArray2()
        {
            int[] result = _prima.FindPrimaUpTo(2);
            int[] expected = { 2 };
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FindPrimaUpTo_InputIs10_ReturnCorrectPrima()
        {
            int[] result = _prima.FindPrimaUpTo(10);
            int[] expected = { 2, 3, 5, 7 };
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FindPrimaUpTo_InputIs20_ReturnCorrectPrima()
        {
            int[] result = _prima.FindPrimaUpTo(20);
            int[] expected = { 2, 3, 5, 7, 11, 13, 17, 19 };
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(0)]
        [TestCase(1)]
        public void FindPrimaUpTo_InputIsInvalidRange_ReturnEmptyArray(int limit)
        {
            int[] result = _prima.FindPrimaUpTo(limit);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void FindPrimaUpTo_InputIs100_ReturnCorrectCount()
        {
            int[] result = _prima.FindPrimaUpTo(100);
            Assert.That(result.Length, Is.EqualTo(25), "There should be 25 primes up to 100");
        }
    }
}