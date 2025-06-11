using NUnit.Framework;

namespace Prima.Tests
{
    [TestFixture]
    public class Prima_GetNextPrimaShould
    {
        private Prima _prima;

        [SetUp]
        public void SetUp()
        {
            _prima = new Prima();
        }

        [Test]
        public void GetNextPrima_InputIs1_Return2()
        {
            int result = _prima.GetNextPrima(1);
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void GetNextPrima_InputIs2_Return3()
        {
            int result = _prima.GetNextPrima(2);
            Assert.That(result, Is.EqualTo(3));
        }

        [TestCase(3, 5)]
        [TestCase(5, 7)]
        [TestCase(7, 11)]
        [TestCase(11, 13)]
        [TestCase(13, 17)]
        [TestCase(17, 19)]
        [TestCase(19, 23)]
        public void GetNextPrima_InputIsKnownPrima_ReturnNextPrima(int input, int expected)
        {
            int result = _prima.GetNextPrima(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(4, 5)]
        [TestCase(6, 7)]
        [TestCase(8, 11)]
        [TestCase(9, 11)]
        [TestCase(10, 11)]
        public void GetNextPrima_InputIsComposite_ReturnNextPrima(int input, int expected)
        {
            int result = _prima.GetNextPrima(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(0)]
        public void GetNextPrima_InputIsNegativeOrZero_Return2(int input)
        {
            int result = _prima.GetNextPrima(input);
            Assert.That(result, Is.EqualTo(2));
        }
    }
}