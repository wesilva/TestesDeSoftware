using Xunit;

namespace Demo.Testes
{
    public class AssertingObjectTypesTests
    {
        [Fact]
        public void FuncionarioFactory_Criar_DeveRetornarTipoFuncionario()
        {
            // Arrange & Act
            var funcionario = FuncionarioFactory.Criar("Wellington", 10000);

            // Assert
            Assert.IsType<Funcionario>(funcionario);
        }

        [Fact]
        public void FuncionarioFactory_Criar_DeveRetornarTipoDerivadoPessoa()
        {
            var funcionario = FuncionarioFactory.Criar("Wellington", 10000);

            Assert.IsAssignableFrom<Pessoa>(funcionario);
        }
    }
}