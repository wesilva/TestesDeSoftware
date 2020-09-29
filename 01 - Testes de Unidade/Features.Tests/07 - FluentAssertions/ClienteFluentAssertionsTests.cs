using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Features.Tests
{
    [Collection(nameof(ClienteAutoMockerColletion))]
    public class ClienteFluentAssertionsTests
    {
        private readonly ClienteTestsAutoMockerFixture _clienteTestsAutoMockerFixture;
        private readonly ITestOutputHelper _outputHelper;

        public ClienteFluentAssertionsTests(ClienteTestsAutoMockerFixture clienteTestsAutoMockerFixture,
            ITestOutputHelper outputHelper)
        {
            _clienteTestsAutoMockerFixture = clienteTestsAutoMockerFixture;
            _outputHelper = outputHelper;
        }

        [Fact(DisplayName = "Novo Cliente Válido")]
        [Trait("Categoria", "Cliente Fluent Assertion Testes")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            var cliente = _clienteTestsAutoMockerFixture.GerarClienteValido();

            var result = cliente.EhValido();

            //Assert.True(result);
            //Assert.Equal(0, cliente.ValidationResult.Errors.Count);

            result.Should().BeTrue();
            cliente.ValidationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Novo Cliente Inválido")]
        [Trait("Categoria", "Cliente Fluent Assertion Testes")]
        public void Cliente_NovoCliente_DeveEstarInvalido()
        {
            var cliente = _clienteTestsAutoMockerFixture.GerarClienteInvalido();

            var result = cliente.EhValido();

            //Assert.False(result);
            //Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);

            result.Should().BeFalse();
            cliente.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(1, "Deve possuir erros de validação");
            _outputHelper.WriteLine($"Foram encontrados {cliente.ValidationResult.Errors.Count} erros nesta validação");
        }
    }
}