using NerdStore.BDD.Tests.Config;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.Pedido
{
    [Binding]
    public class Pedido_AdicionarItemAoCarrinhoSteps
    {
        [Given(@"Que um produto esteja na vitrine")]
        public void DadoQueUmProdutoEstejaNaVitrine()
        {
            // Arrange
            var browser= new SeleniumHelper(Browser.Chrome, new ConfigurationHelper(), false);
            browser.IrParaUrl("https://desenvolvedor.io");
            browser.ClicarLinkPorTexto("Entrar");
            browser.PreencherTextBoxPorId("Email", "abc@teste.com");
            browser.PreencherTextBoxPorId("Password", "123456");

            // Act

            // Assert
        }
        
        [Given(@"Esteja disponível no estoque")]
        public void DadoEstejaDisponivelNoEstoque()
        {
            // Arrange

            // Act

            // Assert
        }

        [Given(@"O usuário esteja logado")]
        public void DadoOUsuarioEstejaLogado()
        {
            // Arrange

            // Act

            // Assert
        }

        [Given(@"O mesmo produto já tenha sido adicionado ao carrinho anteriormente")]
        public void DadoOMesmoProdutoJaTenhaSidoAdicionadoAoCarrinhoAnteriormente()
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"O usuário adicionar uma unidade ao carrinho")]
        public void QuandoOUsuarioAdicionarUmaUnidadeAoCarrinho()
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"O usuário adicionar uma item acima da quantidade máxima permitida")]
        public void QuandoOUsuarioAdicionarUmaItemAcimaDaQuantidadeMaximaPermitida()
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"O usuário adicionar a quantidade máxima permitida ao carrinho")]
        public void QuandoOUsuarioAdicionarAQuantidadeMaximaPermitidaAoCarrinho()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"O usuário será redireciondo ao resumo da compra")]
        public void EntaoOUsuarioSeraRedireciondoAoResumoDaCompra()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"O valor total do pedido será exatamente o valor do item adicionado")]
        public void EntaoOValorTotalDoPedidoSeraExatamenteOValorDoItemAdicionado()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"Receberá uma mensagem de erro mencionando que foi ultrapassada a quantidade limite")]
        public void EntaoReceberaUmaMensagemDeErroMencionandoQueFoiUltrapassadaAQuantidadeLimite()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"A quantidade de itens daquele produto terá sido acrescida em uma unidade a mais")]
        public void EntaoAQuantidadeDeItensDaqueleProdutoTeraSidoAcrescidaEmUmaUnidadeAMais()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"O valor total do pedido será a multiplicação da quantidade de itens pelo valor unitário")]
        public void EntaoOValorTotalDoPedidoSeraAMultiplicacaoDaQuantidadeDeItensPeloValorUnitario()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
