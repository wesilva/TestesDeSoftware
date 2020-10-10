using NerdStore.Core.DomainObjects;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        private readonly Guid _produtoId;
        private readonly Pedido _pedido;

        public PedidoTests()
        {
            _produtoId = Guid.NewGuid();
            _pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        }

        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 500);

            _pedido.AdicionarItem(pedidoItem);

            Assert.Equal(2500, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadesSomarValores()
        {
            var pedidoItem = new PedidoItem(_produtoId, "Produto Teste", 5, 500);
            _pedido.AdicionarItem(pedidoItem);
            var pedidoItem2 = new PedidoItem(_produtoId, "Produto Teste", 1, 500);

            _pedido.AdicionarItem(pedidoItem2);

            Assert.Equal(3000, _pedido.ValorTotal);
            Assert.Equal(1, _pedido.PedidoItems.Count);
            Assert.Equal(6, _pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == _produtoId).Quantidade);
        }

        [Fact(DisplayName = "Adicionar Item Pedido acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
        {
            var pedidoItem = new PedidoItem(_produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 100);

            Assert.Throws<DomainException>(() => _pedido.AdicionarItem(pedidoItem));
        }

        [Fact(DisplayName = "Adicionar Item Pedido existente acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExixtenteSomaUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            var pedidoItem = new PedidoItem(_produtoId, "Produto Teste", 1, 100);
            var pedidoItem2 = new PedidoItem(_produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM, 100);

            _pedido.AdicionarItem(pedidoItem);

            Assert.Throws<DomainException>(() => _pedido.AdicionarItem(pedidoItem2));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemNaoExistenteNaLista_DeveRetornarException()
        {
            var pedidoItemAtualizado = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 100);

            Assert.Throws<DomainException>(() => _pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Valido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            var pedidoItem = new PedidoItem(_produtoId, "Produto Teste", 2, 100);
            _pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = new PedidoItem(_produtoId, "Produto Teste", 5, 100);
            var novaQuantidade = pedidoItemAtualizado.Quantidade;

            _pedido.AtualizarItem(pedidoItemAtualizado);

            Assert.Equal(novaQuantidade, _pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == _produtoId).Quantidade);

        }

        [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            var pedidoItemExistente2 = new PedidoItem(_produtoId, "Produto Teste", 5, 50);
            _pedido.AdicionarItem(pedidoItemExistente1);
            _pedido.AdicionarItem(pedidoItemExistente2);
            var pedidoItemAtualizado = new PedidoItem(_produtoId, "Produto Teste", 10, 50);
            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario +
                              pedidoItemAtualizado.Quantidade * pedidoItemAtualizado.ValorUnitario;

            _pedido.AtualizarItem(pedidoItemAtualizado);

            Assert.Equal(totalPedido, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Atualizar Item Pedido Quantidade acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            var pedidoItemExistente1 = new PedidoItem(_produtoId, "Produto Xpto", 2, 200);
            _pedido.AdicionarItem(pedidoItemExistente1);
            var pedidoItemAtualizado = new PedidoItem(_produtoId, "Produto Xpto", Pedido.MAX_UNIDADES_ITEM + 1, 200);

            Assert.Throws<DomainException>(() => _pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Remover Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
        {
            var pedidoItemRemover = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);

            Assert.Throws<DomainException>(() => _pedido.RemoverItem(pedidoItemRemover));
        }

        [Fact(DisplayName = "Remover Item Pedido Deve Calcular Valor Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExistente_DeveAtualizarValorTotal()
        {
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            var pedidoItem2 = new PedidoItem(_produtoId, "Produto Teste", 5, 50);
            _pedido.AdicionarItem(pedidoItem1);
            _pedido.AdicionarItem(pedidoItem2);
            var totalPedido = pedidoItem2.Quantidade * pedidoItem2.ValorUnitario;

            _pedido.RemoverItem(pedidoItem1);

            Assert.Equal(totalPedido, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Válido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {
            var voucher = new Voucher("Promo-15-Reais", null, 15, TipoDescontoVoucher.Valor,
                1, DateTime.Now.AddDays(15), true, false);

            var result = _pedido.AplicarVoucher(voucher);

           Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar Voucher Inválido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherInvalido_DeveRetornarComErros()
        {
            var voucher = new Voucher("Promo-15-Reais", null, 15, TipoDescontoVoucher.Valor,
                1, DateTime.Now.AddDays(-1), true, true);

            var result = _pedido.AplicarVoucher(voucher);

            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher tipo valor desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
        {
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Abcd", 8, 30);
            _pedido.AdicionarItem(pedidoItem1);
            _pedido.AdicionarItem(pedidoItem2);
            var voucher = new Voucher("Promo-50-Reais", null,50, TipoDescontoVoucher.Valor,
                1,DateTime.Now.AddDays(10), true, false);
            var valorComDesconto = _pedido.ValorTotal - voucher.ValorDesconto;

            _pedido.AplicarVoucher(voucher);

            Assert.Equal(valorComDesconto, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher tipo percentual desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoPercentualDesconto_DeveDescontarDoValorTotal()
        {
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Abcd", 8, 30);
            _pedido.AdicionarItem(pedidoItem1);
            _pedido.AdicionarItem(pedidoItem2);
            var voucher = new Voucher("Promo-50-Reais", 30, null, TipoDescontoVoucher.Porcentagem,
                1, DateTime.Now.AddDays(10), true, false);
            var valorDesconto = (_pedido.ValorTotal * voucher.PercentualDesconto) / 100;
            var valorTotalComDesconto = _pedido.ValorTotal - valorDesconto;

            _pedido.AplicarVoucher(voucher);

            Assert.Equal(valorTotalComDesconto, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher desconto excede valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_DescontoExcedeValorTotalPedido_PedidoDeveTerValorZero()
        {
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            _pedido.AdicionarItem(pedidoItem1);
            var voucher = new Voucher("Promo-500-OFF", null, 500, TipoDescontoVoucher.Valor,
                1, DateTime.Now.AddDays(10), true, false);
              
            _pedido.AplicarVoucher(voucher);
           
            Assert.Equal(0, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher recalcular desconto na modificação do pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_ModificarItensPedido_DeveCalcularDescontoValorTotal()
        {
            // Arrange
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 200);
            _pedido.AdicionarItem(pedidoItem1);
            var voucher = new Voucher("Promo-50-Reais", null, 50, TipoDescontoVoucher.Valor,
                1, DateTime.Now.AddDays(10), true, false);
            _pedido.AplicarVoucher(voucher);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Abcd", 8, 30);

            // Act
            _pedido.AdicionarItem(pedidoItem2);

            // Assert
            var totalEsperado = _pedido.PedidoItems.Sum(i => i.Quantidade * i.ValorUnitario) - voucher.ValorDesconto;
            Assert.Equal(totalEsperado, _pedido.ValorTotal);
        }
    }
}