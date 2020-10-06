using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using Xunit;
namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {
        [Fact(DisplayName = "Adicionar Item Novo Pedido com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
           var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), 
               Guid.NewGuid(), "Novo Produto", 2, 150);
           var mocker = new AutoMocker();
           var pedidoHandler = mocker.CreateInstance<PedidoCommandHandler>();

           var result = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

           Assert.True(result);
           mocker.GetMock<IPedidoRepository>().Verify(r => r.Adicionar(It.IsAny<Pedido>()), Times.Once);
           mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }
    }
}