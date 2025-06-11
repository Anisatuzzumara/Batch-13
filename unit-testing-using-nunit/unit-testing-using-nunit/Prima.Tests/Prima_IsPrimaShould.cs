using NUnit.Framework;

namespace Prima.Tests
{
    [TestFixture]
    public class Prima_IsPrimaShould
    {
        private Prima _prima;

        [SetUp]
        public void SetUp()
        {
            _prima = new Prima();
        }

        [Test]
        public void IsPrima_InputIs1_ReturnFalse()
        {
            bool result = _prima.IsPrima(1);
            Assert.That(result, Is.False, "1 should not be considered prime");
        }

        [Test]
        public void IsPrima_InputIs2_ReturnTrue()
        {
            bool result = _prima.IsPrima(2);
            Assert.That(result, Is.True, "2 should be considered prime");
        }

        [TestCase(3)]
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(13)]
        [TestCase(17)]
        [TestCase(19)]
        [TestCase(23)]
        [TestCase(97)]
        [TestCase(101)]
        public void IsPrima_InputIsKnownPrima_ReturnTrue(int candidate)
        {
            bool result = _prima.IsPrima(candidate);
            Assert.That(result, Is.True, $"{candidate} should be considered prime");
        }

        [TestCase(4)]
        [TestCase(6)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(12)]
        [TestCase(15)]
        [TestCase(21)]
        [TestCase(25)]
        [TestCase(100)]
        public void IsPrima_InputIsKnownComposite_ReturnFalse(int candidate)
        {
            bool result = _prima.IsPrima(candidate);
            Assert.That(result, Is.False, $"{candidate} should not be considered prime");
        }

        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-100)]
        public void IsPrima_InputIsNegative_ReturnFalse(int candidate)
        {
            bool result = _prima.IsPrima(candidate);
            Assert.That(result, Is.False, "Negative numbers should not be considered prime");
        }

        [Test]
        public void IsPrima_InputIs0_ReturnFalse()
        {
            bool result = _prima.IsPrima(0);
            Assert.That(result, Is.False, "0 should not be considered prime");
        }
    }
}