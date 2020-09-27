using System.Linq;
using System.Threading;
using Features.Clientes;
using MediatR;
using Moq;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClienteBogusCollection))]
    public class ClienteServiceTests
    {
        readonly ClienteTestsBogusFixture _clienteTestsBogusFixture;

        public ClienteServiceTests(ClienteTestsBogusFixture clienteTestsBogusFixture)
        {
            _clienteTestsBogusFixture = clienteTestsBogusFixture;
        }

        [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
        [Trait("Categoria", "Cliente Service Mock Tests")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            var cliente = _clienteTestsBogusFixture.GerarClienteValido();
            var clienteRepo = new Mock<IClienteRepository>();
            var mediatr = new Mock<IMediator>();
            var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);

            clienteService.Adicionar(cliente);

            clienteRepo.Verify(r => r.Adicionar(cliente), Times.Once);
            mediatr.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Cliente com Falha")]
        [Trait("Categoria", "Cliente Service Mock Tests")]
        public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
        {
            var cliente = _clienteTestsBogusFixture.GerarClienteInvalido();
            var clienteRepo = new Mock<IClienteRepository>();
            var mediatr = new Mock<IMediator>();
            var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);

            clienteService.Adicionar(cliente);

            clienteRepo.Verify(r => r.Adicionar(cliente), Times.Never);
            mediatr.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Obter Clientes Ativos")]
        [Trait("Categoria", "Cliente Service Mock Tests")]
        public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
        {
            var clienteRepo = new Mock<IClienteRepository>();
            var mediatr = new Mock<IMediator>();
            clienteRepo.Setup(c => c.ObterTodos())
                .Returns(_clienteTestsBogusFixture.ObterClientesVariados());
            var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);

            var clientes = clienteService.ObterTodosAtivos();

            clienteRepo.Verify(r => r.ObterTodos(), Times.Once);
            Assert.True(clientes.Any());
            Assert.False(clientes.Count(c => !c.Ativo) > 0);
        }
    }
}