using System;
using metodo;
using model;
namespace LojaDeRoupas
{
    

    public class Promocao
    {
        public List<Produto> Produtos { get; set; }
        public CategoriaProduto? Categoria { get; set; }
        public TipoDesconto TipoDesconto { get; set; }
        public decimal ValorDesconto { get; set; }

        public decimal CalcularDesconto(List<Produto> ListaProdutos)
        {
            decimal totalDesconto = 0;

            if (TipoDesconto == TipoDesconto.Porcentagem)
            {
                if (Categoria != null)
                {
                    var produtos = ObterProdutosPorCategoria((CategoriaProduto)Categoria, ListaProdutos);
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
                    var produtos = ObterProdutosPorCategoria((CategoriaProduto)Categoria, ListaProdutos);
                    totalDesconto = produtos.Sum(p => p.Preco) > ValorDesconto ? ValorDesconto : produtos.Sum(p => p.Preco);
                }
                else if (Produtos != null)
                {
                    totalDesconto = Produtos.Sum(p => p.Preco) > ValorDesconto ? ValorDesconto : Produtos.Sum(p => p.Preco);
                }
            }

            return totalDesconto;
        }

        private List<Produto> ObterProdutosPorCategoria(CategoriaProduto categoria, List<Produto> ListaProdutos)
        {
            return ListaProdutos.Where(p => p.Categoria == categoria).ToList();
        }
    }

    public class CarrinhoDeCompras
    {
        public List<Produto> Produtos { get; set; }
        public List<Promocao> Promocoes { get; set; }

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
                    total -= promocao.CalcularDesconto(Produtos);
                }
            }

            return total;
        }
    }

    class Program
    {
        static List<Produto> Produtos = new List<Produto>();
        static List<Promocao> Promocoes = new List<Promocao>();
        static CarrinhoDeCompras Carrinho = new CarrinhoDeCompras();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Selecione uma opção:");
                Console.WriteLine("1 - Cadastrar categoria de produto");
                Console.WriteLine("2 - Cadastrar produto");
                Console.WriteLine("3 - Cadastrar promoção");
                Console.WriteLine("4 - Adicionar produto ao carrinho");
                Console.WriteLine("5 - Calcular total do carrinho");
                Console.WriteLine("0 - Sair");

                int opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        CadastrarCategoriaProduto();
                        break;
                    case 2:
                        CadastrarProduto();
                        break;
                    case 3:
                        CadastrarPromocao();
                        break;
                    case 4:
                        AdicionarProdutoAoCarrinho();
                        break;
                    case 5:
                        CalcularTotalDoCarrinho();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void CadastrarCategoriaProduto()
        {
            Console.WriteLine("Digite o nome da categoria:");
            string nome = Console.ReadLine();
            CategoriaProduto categoria;
            if (Enum.TryParse<CategoriaProduto>(nome, out categoria))
            {
                Console.WriteLine("Categoria cadastrada com sucesso!");
            }
            else
            {
                Console.WriteLine("Categoria inválida!");
            }
        }

        static void CadastrarProduto()
        {
            Console.WriteLine("Digite o nome do produto:");
            string nome = Console.ReadLine();

            Console.WriteLine("Digite o preço do produto:");
            decimal preco = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Digite a categoria do produto:");
            string categoriaStr = Console.ReadLine();
            CategoriaProduto categoria;
            if (!Enum.TryParse<CategoriaProduto>(categoriaStr, out categoria))
            {
                Console.WriteLine("Categoria inválida!");
                return;
            }

            Produto produto;
            switch (categoria)
            {
                case CategoriaProduto.Camiseta:
                case CategoriaProduto.Calca:
                    produto = new Roupa()
                    {
                        Nome = nome,
                        Preco = preco,
                        Categoria = categoria
                    };
                    break;
                case CategoriaProduto.Sapato:
                    Console.WriteLine("Digite o tipo do sapato:");
                    string tipo = Console.ReadLine();
                    produto = new Sapato()
                    {
                        Nome = nome,
                        Preco = preco,
                        Categoria = categoria,
                        Tipo = tipo
                    };
                    break;
                case CategoriaProduto.Bolsa:
                    Console.WriteLine("Digite o material da bolsa:");
                    string material = Console.ReadLine();
                    produto = new Bolsa()
                    {
                        Nome = nome,
                        Preco = preco,
                        Categoria = categoria,
                        Material = material
                    };
                    break;
                case CategoriaProduto.Acessorio:
                    Console.WriteLine("Digite o tamanho do acessório:");
                    string tamanho = Console.ReadLine();
                    Console.WriteLine("Digite a cor do acessório:");
                    string cor = Console.ReadLine();
                    produto = new Acessorio()
                    {
                        Nome = nome,
                        Preco = preco,
                        Categoria = categoria,
                        Tamanho = tamanho,
                        Cor = cor
                    };
                    break;
                default:
                    Console.WriteLine("Categoria inválida!");
                    return;
            }

            Produtos.Add(produto);
            Console.WriteLine("Produto cadastrado com sucesso!");
        }

        static void CadastrarPromocao()
        {
            Console.WriteLine("Digite o tipo de desconto da promoção:");
            string tipoStr = Console.ReadLine();
            TipoDesconto tipo;
            if (!Enum.TryParse<TipoDesconto>(tipoStr, out tipo))
            {
                Console.WriteLine("Tipo de desconto inválido!");
                return;
            }
            Console.WriteLine("Digite o valor do desconto:");
            decimal valor = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Digite a categoria do produto para a promoção:");
            string categoriaStr = Console.ReadLine();
            CategoriaProduto categoria;
            if (!Enum.TryParse<CategoriaProduto>(categoriaStr, out categoria))
            {
                Console.WriteLine("Categoria inválida!");
                return;
            }

            Promocao promocao = new Promocao()
            {
                Tipo = tipo,
                Valor = valor,
                CategoriaProduto = categoria
            };

            Promocoes.Add(promocao);
            Console.WriteLine("Promoção cadastrada com sucesso!");
        }

        static void AdicionarProdutoAoCarrinho()
        {
            Console.WriteLine("Digite o nome do produto:");
            string nome = Console.ReadLine();
            Produto produto = Produtos.FirstOrDefault(p => p.Nome == nome);
            if (produto == null)
            {
                Console.WriteLine("Produto não encontrado!");
                return;
            }

            Console.WriteLine("Digite a quantidade:");
            int quantidade = int.Parse(Console.ReadLine());

            ItemCarrinho itemCarrinho = new ItemCarrinho()
            {
                Produto = produto,
                Quantidade = quantidade
            };

            Carrinho.AddItem(itemCarrinho);
            Console.WriteLine("Produto adicionado ao carrinho!");
        }

        static void CalcularTotalDoCarrinho()
        {
            decimal total = Carrinho.CalcularTotal();
            Console.WriteLine($"Total do carrinho: R$ {total:F2}");
        }

    }



    public static class Carrinho
    {
        private static List<ItemCarrinho> itens = new List<ItemCarrinho>();
        public static void AddItem(ItemCarrinho item)
        {
            itens.Add(item);
        }

        public static decimal CalcularTotal()
        {
            decimal total = 0;
            foreach (var item in itens)
            {
                total += item.CalcularSubtotal();
            }
            return total;
        }
    }

    public static class Produtos
    {
        private static List<Produto> produtos = new List<Produto>();
        public static void Add(Produto produto)
        {
            produtos.Add(produto);
        }

        public static List<Produto> GetProdutos()
        {
            return produtos;
        }

        public static Produto FindProduto(string nome)
        {
            return produtos.FirstOrDefault(p => p.Nome == nome);
        }

        public static List<Produto> FindProdutosByCategoria(CategoriaProduto categoria)
        {
            return produtos.Where(p => p.Categoria == categoria).ToList();
        }

    }

    public static class Promocoes
    {
        private static List<Promocao> promocoes = new List<Promocao>();


        public static void Add(Promocao promocao)
        {
            promocoes.Add(promocao);
        }

        public static List<Promocao> GetPromocoes()
        {
            return promocoes;
        }

        public static List<Promocao> FindPromocoesByCategoria(CategoriaProduto categoria)
        {
            return promocoes.Where(p => p.CategoriaProduto == categoria).ToList();
        }

        public static List<Promocao> FindPromocoesByTipo(TipoDesconto tipo)
        {
            return promocoes.Where(p => p.Tipo == tipo).ToList();
        }

        public static List<Promocao> FindPromocoesByCategoriaETipo(CategoriaProduto categoria, TipoDesconto tipo)
        {
            return promocoes.Where(p => p.CategoriaProduto == categoria && p.Tipo == tipo).ToList();
        }

    }
}