using System;
using System.Text.RegularExpressions;
using metodo;
using LojaDeRoupas;


namespace metodo
{

    // Enumerador para representar as categorias de produtos
    

        // Método para calcular o valor total do desconto a ser aplicado
        public decimal CalcularDesconto()
        {
            decimal totalDesconto = 0;

            if (TipoDesconto == TipoDesconto.Porcentagem)
            {
                if (Categoria != null)
                {
                    var produtos = ObterProdutosPorCategoria((CategoriaProduto)Categoria);
                    foreach (var produto in produtos)
                    {
                        totalDesconto += produto.Preco * ValorDesconto / 100;
                    }
                }
                else if (Produtos != null)
                {
                    foreach (var produto in Produtos)
                    {
                        totalDesconto += produto.Preco * ValorDesconto / 100;
                    }
                }
            }
            else if (TipoDesconto == TipoDesconto.ValorFixo)
            {
                if (Categoria != null)
                {
                    var produtos = ObterProdutosPorCategoria((CategoriaProduto)Categoria);
                    totalDesconto = produtos.Sum(p => p.Preco) > ValorDesconto ? ValorDesconto : produtos.Sum(p => p.Preco);
                }
                else if (Produtos != null)
                {
                    totalDesconto = Produtos.Sum(p => p.Preco) > ValorDesconto ? ValorDesconto : Produtos.Sum(p => p.Preco);
                }
            }

            return totalDesconto;
        }

        // Método auxiliar para obter os produtos de uma categoria
        private List<Produto> ObterProdutosPorCategoria(CategoriaProduto categoria)
        {
            return Produtos.Where(p => p.Categoria == categoria).ToList();
        }
    }

    // Enumerador para representar os tipos de desconto
    public enum TipoDesconto
    {
        Porcentagem,
        ValorFixo
    }

    // Classe para representar o carrinho de compras
    public class CarrinhoDeCompras
    {
        public List<Produto> Produtos { get; set; }
        public List<Promocao> Promocoes { get; set; }


        // Método para calcular o valor total dos produtos e descontos aplicados
        public decimal CalcularTotal()
        {
            decimal total = 0;

            if (Produtos != null)
            {
                total = Produtos.Sum(p => p.Preco);
            }

            if (Promocoes != null)
            {
                foreach (var promocao in Promocoes)
                {
                    decimal desconto = promocao.CalcularDesconto();
                    if (desconto > total)
                    {
                        total = 0;
                    }
                    else
                    {
                        total -= desconto;
                    }
                }
            }

            return total;
        }
    }
}