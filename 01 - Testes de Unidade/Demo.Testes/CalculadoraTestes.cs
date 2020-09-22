using Xunit;

namespace Demo.Testes
{
    public class CalculadoraTestes
    {
        [Fact]
        public void Calculadora_Somar_RetornarValorSoma()
        {
            // Arrange
            var calculadora = new Calculadora();

            // Act
            var resultado = calculadora.Somar(4, 2);

            // Assert
            Assert.Equal(6, resultado);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 5, 7)]
        [InlineData(10, 52, 62)]
        public void Calculadora_Somar_RetornarValoresSomaCorretos(double valor1, double valor2, double totalEsperado)
        {
            // Arrange
            var calculadora = new Calculadora();

            // Act
            var resultado = calculadora.Somar(valor1, valor2);

            // Assert
            Assert.Equal(totalEsperado, resultado);
        }
    }
}