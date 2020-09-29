using Xunit;

namespace Demo.Testes
{
    public class AssertingCollectionsTests
    {
        [Fact]
        public void Funcionario_Habilidades_NaoDevePossuirHabilidadesVazias()
        {
            // Assert & Act
            var funcionario = FuncionarioFactory.Criar("Wellington", 10000);

            //Assert
            Assert.All(funcionario.Habilidades, habilidade => Assert.False(string.IsNullOrWhiteSpace(habilidade)));
        }

        [Fact]
        public void Funcionario_Habilidades_JuniorDevePossuirHabilidadeBasica()
        {
            // Assert & Act
            var funcionario = FuncionarioFactory.Criar("Wellington", 1000);

            //Assert
            Assert.Contains("OOP", funcionario.Habilidades);
        }

        [Fact]
        public void Funcionario_Habilidades_JuniorNaoDevePossuirHabilidadeAvancada()
        {
            // Assert & Act
            var funcionario = FuncionarioFactory.Criar("Wellington", 1000);

            //Assert
            Assert.DoesNotContain("Microservices", funcionario.Habilidades);
        }

        [Fact]
        public void Funcionario_Habilidades_SeniorDevePossuirTodasHabilidades()
        {
            // Assert & Act
            var funcionario = FuncionarioFactory.Criar("Wellington", 15000);

            var habilidadesBasicas = new[]
            {
                "Lógica de Programação",
                "OOP",
                "Testes",
                "Microservices"
            };

            //Assert
            Assert.Equal(habilidadesBasicas, funcionario.Habilidades);
        }
    }
}