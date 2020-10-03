using NerdStore.Core.DomainObjects;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 500);

            pedido.AdicionarItem(pedidoItem);

            Assert.Equal(2500, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Categoria", "Vendas - Pedido")]
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
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 100);

            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem));
        }

        [Fact(DisplayName = "Adicionar Item Pedido existente acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExixtenteSomaUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 1, 100);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM, 100);
            pedido.AdicionarItem(pedidoItem);

            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemNaoExistenteNaLista_DeveRetornarException()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemAtualizado = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 100);

            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Valido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 100);
            var novaQuantidade = pedidoItemAtualizado.Quantidade;

            pedido.AtualizarItem(pedidoItemAtualizado);

            Assert.Equal(novaQuantidade, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);

        }

        [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            var pedidoItemExistente2 = new PedidoItem(produtoId, "Produto Teste", 5, 50);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 10, 50);
            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario +
                              pedidoItemAtualizado.Quantidade * pedidoItemAtualizado.ValorUnitario;

            pedido.AtualizarItem(pedidoItemAtualizado);

            Assert.Equal(totalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Atualizar Item Pedido Quantidade acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(produtoId, "Produto Xpto", 2, 200);
            pedido.AdicionarItem(pedidoItemExistente1);
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Xpto", Pedido.MAX_UNIDADES_ITEM + 1, 200);

            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Remover Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemRemover = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);

            Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItemRemover));
        }

        [Fact(DisplayName = "Remover Item Pedido Deve Calcular Valor Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExistente_DeveAtualizarValorTotal()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 5, 50);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);
            var totalPedido = pedidoItem2.Quantidade * pedidoItem2.ValorUnitario;

            pedido.RemoverItem(pedidoItem1);

            Assert.Equal(totalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Válido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("Promo-15-Reais", null, 15, TipoDescontoVoucher.Valor,
                1, DateTime.Now.AddDays(15), true, false);

            var result = pedido.AplicarVoucher(voucher);

           Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar Voucher Inválido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherInvalido_DeveRetornarComErros()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var voucher = new Voucher("Promo-15-Reais", null, 15, TipoDescontoVoucher.Valor,
                1, DateTime.Now.AddDays(-1), true, true);

            var result = pedido.AplicarVoucher(voucher);

            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher tipo valor desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Abcd", 8, 30);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);
            var voucher = new Voucher("Promo-50-Reais", null,50, TipoDescontoVoucher.Valor,
                1,DateTime.Now.AddDays(10), true, false);
            var valorComDesconto = pedido.ValorTotal - voucher.ValorDesconto;

            pedido.AplicarVoucher(voucher);

            Assert.Equal(valorComDesconto, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher tipo percentual desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoPercentualDesconto_DeveDescontarDoValorTotal()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Abcd", 8, 30);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);
            var voucher = new Voucher("Promo-50-Reais", 30, null, TipoDescontoVoucher.Porcentagem,
                1, DateTime.Now.AddDays(10), true, false);
            var valorDesconto = (pedido.ValorTotal * voucher.PercentualDesconto) / 100;
            var valorTotalComDesconto = pedido.ValorTotal - valorDesconto;

            pedido.AplicarVoucher(voucher);

            Assert.Equal(valorTotalComDesconto, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher desconto excede valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_DescontoExcedeValorTotalPedido_PedidoDeveTerValorZero()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            pedido.AdicionarItem(pedidoItem1);
            var voucher = new Voucher("Promo-500-OFF", null, 500, TipoDescontoVoucher.Valor,
                1, DateTime.Now.AddDays(10), true, false);
              
            pedido.AplicarVoucher(voucher);
           
            Assert.Equal(0, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher recalcular desconto na modificação do pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_ModificarItensPedido_DeveCalcularDescontoValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            pedido.AdicionarItem(pedidoItem1);
            var voucher = new Voucher("Promo-50-Reais", null, 50, TipoDescontoVoucher.Valor,
                1, DateTime.Now.AddDays(10), true, false);
            pedido.AplicarVoucher(voucher);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Abcd", 8, 30);

            // Act
            pedido.AdicionarItem(pedidoItem2);

            // Assert
            var totalEsperado = pedido.PedidoItems.Sum(i => i.Quantidade * i.ValorUnitario) - voucher.ValorDesconto;
            Assert.Equal(totalEsperado, pedido.ValorTotal);
        }
    }
}