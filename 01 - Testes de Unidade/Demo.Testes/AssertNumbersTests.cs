using Xunit;

namespace Demo.Testes
{
    public class AssertNumbersTests
    {
        [Fact]
        public void Calculadora_Somar_DeveSerIgual()
        {
            var calculadora = new Calculadora();

            var resultado = calculadora.Somar(1, 2);

            Assert.Equal(3, resultado);
        }

        [Fact]
        public void Calculadora_Somar_NaoDeveSerIgual()
        {
            var calculadora = new Calculadora();

            var resultado = calculadora.Somar(1.18, 2.25464654);

            Assert.NotEqual(3.3, resultado, 1);
        }
    }
}