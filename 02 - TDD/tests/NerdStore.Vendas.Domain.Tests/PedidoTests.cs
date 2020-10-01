using NerdStore.Core.DomainObjects;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Pedido Tests")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 500);

            pedido.AdicionarItem(pedidoItem);

            Assert.Equal(2500, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Categoria", "Pedido Tests")]
        public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadesSomarValores()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 5, 500);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 1, 500);

            pedido.AdicionarItem(pedidoItem2);

            Assert.Equal(3000, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItems.Count);
            Assert.Equal(6, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);

        }

        [Fact(DisplayName = "Adicionar Item Pedido acima do permitido")]
        [Trait("Categoria", "Pedido Tests")]
        public void AdicionarItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 100);

            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem));
        }
    }
}