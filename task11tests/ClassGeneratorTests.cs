using Xunit;
using task11;

namespace task11tests
{
    public class CalculatorFactoryTests
    {
        [Fact]
        public void AddOperation_ReturnsCorrectResult()
        {
            var calc = ClassGenerator.CreateCalculator();
            Assert.Equal(5, calc.Add(2, 3));
        }

        [Fact]
        public void MinusOperation_ReturnsCorrectResult()
        {
            var calc = ClassGenerator.CreateCalculator();
            Assert.Equal(-1, calc.Minus(2, 3));
        }

        [Fact]
        public void MultiplyOperation_ReturnsCorrectResult()
        {
            var calc = ClassGenerator.CreateCalculator();
            Assert.Equal(6, calc.Mul(2, 3));
        }

        [Fact]
        public void DivideOperation_ReturnsCorrectResult()
        {
            var calc = ClassGenerator.CreateCalculator();
            Assert.Equal(2, calc.Div(6, 3));
        }

        [Fact]
        public void DivideOperation_ThrowsDivideByZeroException()
        {
            var calc = ClassGenerator.CreateCalculator();
            Assert.Throws<DivideByZeroException>(() => calc.Div(1, 0));
        }
    }
}

