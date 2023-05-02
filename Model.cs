using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using metodo;

namespace model
{
    public enum CategoriaProduto
    {
        Camiseta,
        Calca,
        Sapato,
        Bolsa,
        Acessorio
    }

    public enum TipoDesconto
    {
        Porcentagem,
        ValorFixo
    }

    public abstract class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public CategoriaProduto Categoria { get; set; }
    }

    public abstract class Acessorio : Produto
    {
        public string Tamanho { get; set; }
        public string Cor { get; set; }
    }

    public class Sapato : Produto
    {
        public string Tipo { get; set; }
    }

    public class Bolsa : Produto
    {
        public string Material { get; set; }
    }

    public class Roupa : Produto
    {
    }

    

    public class Promocao
    {
        public TipoDesconto Tipo { get; set; }
        public decimal Valor { get; set; }
        public CategoriaProduto CategoriaProduto { get; set; }
    }

    

    public class ItemCarrinho
    {
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal CalcularSubtotal()
        {
            decimal subtotal = Produto.Preco * Quantidade;
            return subtotal;
        }
    }
    
}
  
