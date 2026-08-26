using Moq;

namespace RestaurantAPI.Tests
{
    public interface IDiscountService
    {
        decimal GetDiscount(string customerType);
    }

    public class PriceCalculator
    {
        private readonly IDiscountService _discountService;

        public PriceCalculator(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public decimal CalculateFinalPrice(decimal basePrice, string customerType)
        {
            var discount = _discountService.GetDiscount(customerType);
            return basePrice - (basePrice * discount);
        }
    }

    public class PriceCalculatorTests
    {
        [Fact]
        public void TestVipDiscount()
        {
            var mock = new Mock<IDiscountService>();
            mock.Setup(x => x.GetDiscount("VIP")).Returns(0.2m);

            var calculator = new PriceCalculator(mock.Object);
            var result = calculator.CalculateFinalPrice(100, "VIP");

            Assert.Equal(80, result);
        }

        [Fact]
        public void TestUnconfuredMock()
        {
            var mock = new Mock<IDiscountService>();
            mock.Setup(x => x.GetDiscount("VIP")).Returns(0.2m);
            var calculator = new PriceCalculator(mock.Object);
            var result = calculator.CalculateFinalPrice(100, "Regular");
            Assert.Equal(100, result);
        }
        [Fact]
        public void GetAmountOfInvoke()
        {
            var mock = new Mock<IDiscountService>();
            mock.Setup(x => x.GetDiscount("VIP")).Returns(0.2m);
            var calculator = new PriceCalculator(mock.Object);
            calculator.CalculateFinalPrice(100, "VIP");
            mock.Verify(x => x.GetDiscount("VIP"), Times.Once);
        }
    }
}
