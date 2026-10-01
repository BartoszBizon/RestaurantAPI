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
        public void CalculateFinalPrice_ForVip_CallsDiscountServiceOnce()
        {
            // Arrange
            var mock = new Mock<IDiscountService>();
            mock.Setup(x => x.GetDiscount("VIP")).Returns(0.2m);
            var calculator = new PriceCalculator(mock.Object);

            // Act
            calculator.CalculateFinalPrice(100, "VIP");

            // Assert
            mock.Verify(x => x.GetDiscount("VIP"), Times.Once);
        }

        [Theory]
        [InlineData("VIP", 0.2, "VIP", 80)]
        [InlineData("VIP", 0.2, "Regular", 100)]
        [InlineData("Student", 0.1, "Student", 90)]
        public void CalculateFinalPrice_ForCustomerType_ReturnsDiscountedPrice(
            string configuredCustomerType, decimal configuredDiscount, string customerType, decimal expected)
        {
            // Arrange
            var mock = new Mock<IDiscountService>();
            mock.Setup(x => x.GetDiscount(configuredCustomerType)).Returns(configuredDiscount);
            var calculator = new PriceCalculator(mock.Object);

            // Act
            var result = calculator.CalculateFinalPrice(100, customerType);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
