using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;

namespace RestaurantAPI.Tests
{
    public interface INameGetter
    {
        string GetName(string name);
    }

    public class MockingBasicsTests
    {

        [Fact]
        public void MockForConfiguredInput_ReturnsConfiguredValue()
        {
            var mock = new Mock<INameGetter>();
            mock.Setup(x => x.GetName("Bartosz")).Returns("Cześć Bartosz");

            var result = mock.Object.GetName("Bartosz");
            Assert.Equal("Cześć Bartosz", result);
        }

        [Fact]
        public void Mock_ForNotConfiguredInput_ReturnsDefaultValue()
        {
            var mock = new Mock<INameGetter>();
            mock.Setup(x => x.GetName("Bartosz")).Returns("Cześć, Bartosz");
            var result = mock.Object.GetName("Inna osoba");
            Assert.Null(result);
        }


        [Fact]
        public void Mock_Verify_ChecksThatMethodWasCalled()
        {
            // ARRANGE
            var mock = new Mock<INameGetter>();

            // ACT
            mock.Object.GetName("Bartosz");

            // ASSERT
            // Verify sprawdza nie WYNIK, tylko FAKT wywołania - "czy Greet
            // zostało wywołane dokładnie 1 raz, z tym argumentem?"
            // Przydatne, gdy metoda nic nie zwraca (void), ale chcesz
            // sprawdzić, że jakaś zależność została użyta.
            mock.Verify(g => g.GetName("Bartosz"), Times.Once);
        }
    }
}