using NerdStore.BDD.Tests.Config;
using NerdStore.BDD.Tests.Usuario;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Pedido
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class Pedido_AdicionarItemAoCarrinhoSteps
    {
        private readonly AutomacaoWebTestsFixture _testsFixture;
        private readonly PedidoTela _pedidoTela;
        private readonly LoginUsuarioTela _loginUsuarioTela;

        private string _urlProduto;

        public Pedido_AdicionarItemAoCarrinhoSteps(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _pedidoTela = new PedidoTela(testsFixture.BrowserHelper);
            _loginUsuarioTela = new LoginUsuarioTela(testsFixture.BrowserHelper);
        }

        [Given(@"O usuário esteja logado")]
        public void DadoOUsuarioEstejaLogado()
        {
            // Arrange
            var usuario = new Usuario.Usuario
            {
                Email = "natalia@ti.com",
                Senha = "Abc!123"
            };
            _testsFixture.Usuario = usuario;

            // Act
            var login = _loginUsuarioTela.Login(usuario);

            // Assert
            Assert.True(login);
        }

        [Given(@"Que um produto esteja na vitrine")]
        public void DadoQueUmProdutoEstejaNaVitrine()
        {
            // Arrange
            _pedidoTela.AcessarVitrineDeProdutos();

            // Act
            _pedidoTela.ObterDetalhesDoProduto();
            _urlProduto = _pedidoTela.ObterUrl();

            // Assert
            Assert.True(_pedidoTela.ValidarProdutoDisponivel());
        }

        [Given(@"Esteja disponível no estoque")]
        public void DadoEstejaDisponivelNoEstoque()
        {
            // Assert
            Assert.True(_pedidoTela.ObterQuantidadeNoEstoque() > 0);
        }

        [Given(@"Não tenha nenhum produto adicionado ao carrinho")]
        public void DadoNaoTenhaNenhumProdutoAdicionadoAoCarrinho()
        {
            // Act
            _pedidoTela.NavegarParaCarrinhoDeCompras();
            _pedidoTela.ZerarCarrinhoDeCompras();

            // Assert
            Assert.Equal(0, _pedidoTela.ObterValorTotalCarrinho());

            _pedidoTela.NavegarParaUrl(_urlProduto);
        }


        [Given(@"O mesmo produto já tenha sido adicionado ao carrinho anteriormente")]
        public void DadoOMesmoProdutoJaTenhaSidoAdicionadoAoCarrinhoAnteriormente()
        {
            // Act
            _pedidoTela.NavegarParaCarrinhoDeCompras();
            _pedidoTela.ZerarCarrinhoDeCompras();
            _pedidoTela.AcessarVitrineDeProdutos();
            _pedidoTela.ObterDetalhesDoProduto();
            _pedidoTela.ClicarEmComprarAgora();

            // Assert
            Assert.True(_pedidoTela.ValidarSeEstaNoCarrinhoDeCompras());

            _pedidoTela.VoltarNavegacao();
        }

        [When(@"O usuário adicionar uma unidade ao carrinho")]
        public void QuandoOUsuarioAdicionarUmaUnidadeAoCarrinho()
        {
            // Act
            _pedidoTela.ClicarEmComprarAgora();
        }

        [When(@"O usuário adicionar uma item acima da quantidade máxima permitida")]
        public void QuandoOUsuarioAdicionarUmaItemAcimaDaQuantidadeMaximaPermitida()
        {
            // Arrange
            _pedidoTela.ClicarAdicionarQuantidadeItens(Vendas.Domain.Pedido.MAX_UNIDADES_ITEM + 1);

            // Act
            _pedidoTela.ClicarEmComprarAgora();
        }

        [Then(@"O usuário será redireciondo ao resumo da compra")]
        public void EntaoOUsuarioSeraRedireciondoAoResumoDaCompra()
        {
            // Assert
            Assert.True(_pedidoTela.ValidarSeEstaNoCarrinhoDeCompras());
        }

        [Then(@"O valor total do pedido será exatamente o valor do item adicionado")]
        public void EntaoOValorTotalDoPedidoSeraExatamenteOValorDoItemAdicionado()
        {
            // Arrange
            var valorUnitario = _pedidoTela.ObterValorUnitarioProdutoCarrinho();
            var valorCarrinho = _pedidoTela.ObterValorTotalCarrinho();

            // Assert
            Assert.Equal(valorUnitario, valorCarrinho);
        }

        [Then(@"Receberá uma mensagem de erro mencionando que foi ultrapassada a quantidade limite")]
        public void EntaoReceberaUmaMensagemDeErroMencionandoQueFoiUltrapassadaAQuantidadeLimite()
        {
            // Arrange
            var mensagem = _pedidoTela.ObterMensagemDeErroProduto();

            // Assert
            Assert.Contains($"A quantidade máxima de um item é {Vendas.Domain.Pedido.MAX_UNIDADES_ITEM}", mensagem);
        }

        [Then(@"A quantidade de itens daquele produto terá sido acrescida em uma unidade a mais")]
        public void EntaoAQuantidadeDeItensDaqueleProdutoTeraSidoAcrescidaEmUmaUnidadeAMais()
        {
            // Assert
            Assert.True(_pedidoTela.ObterQuantidadeDeItensPrimeiroProdutoCarrinho() == 2);
        }

        [Then(@"O valor total do pedido será a multiplicação da quantidade de itens pelo valor unitário")]
        public void EntaoOValorTotalDoPedidoSeraAMultiplicacaoDaQuantidadeDeItensPeloValorUnitario()
        {
            // Arrange
            var valorUnitario = _pedidoTela.ObterValorUnitarioProdutoCarrinho();
            var valorCarrinho = _pedidoTela.ObterValorTotalCarrinho();
            var quantidadeUnidades = _pedidoTela.ObterQuantidadeDeItensPrimeiroProdutoCarrinho();

            // Assert
            Assert.Equal(valorUnitario * quantidadeUnidades, valorCarrinho);
        }

        [When(@"O usuário adicionar a quantidade máxima permitida ao carrinho")]
        public void QuandoOUsuarioAdicionarAQuantidadeMaximaPermitidaAoCarrinho()
        {
            // Arrange
            _pedidoTela.ClicarAdicionarQuantidadeItens(Vendas.Domain.Pedido.MAX_UNIDADES_ITEM);

            // Act
            _pedidoTela.ClicarEmComprarAgora();
        }
    }
}
