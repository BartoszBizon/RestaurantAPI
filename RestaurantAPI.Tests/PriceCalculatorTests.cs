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
        public void GetAmountOfInvoke()
        {
            var mock = new Mock<IDiscountService>();
            mock.Setup(x => x.GetDiscount("VIP")).Returns(0.2m);
            var calculator = new PriceCalculator(mock.Object);
            calculator.CalculateFinalPrice(100, "VIP");
            mock.Verify(x => x.GetDiscount("VIP"), Times.Once);
        }

        [Theory]
        [InlineData("VIP", 0.2, 80)]
        [InlineData("Regular", 0.2, 100)]
        [InlineData("Student", 0.1, 90)]
        public void CalculateFinalPrice_ReturnsDiscountedPrice(string customerType, decimal vipDiscount, decimal expected)
        {
            var mock = new Mock<IDiscountService>();
            mock.Setup(x => x.GetDiscount("VIP")).Returns(vipDiscount);
            var calculator = new PriceCalculator(mock.Object);
            var result = calculator.CalculateFinalPrice(100, customerType);
            Assert.Equal(expected, result);
        }
    }
}
