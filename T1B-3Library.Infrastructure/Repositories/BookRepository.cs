using AtelieDaTransformacao.Domain.Entities;
using AtelieDaTransformacao.Domain.Interfaces;
using AtelieDaTransformacao.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AtelieDaTransformacao.Infrastructure.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository //•	Define a classe que implementa a interface IProductCategoryRepository — fornece operações de persistência para ProductCategory.
    {
        private readonly AtelieDaTransformacaoDbContext _context;//•	Campo somente leitura que guarda a instância do DbContext usada para acesso aos dados.

        public ProductCategoryRepository(AtelieDaTransformacaoDbContext context) //•	Construtor que recebe o DbContext via injeção de dependência.
        {
            _context = context;//•	Atribui a instância de DbContext ao campo local para uso nos métodos.
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()//•	Assinatura do método assíncrono que retorna todas as categorias de produto.
        {
            return await _context.ProductCategories //•	Inicia a query sobre o conjunto ProductCategories do contexto.
                .Include(c => c.Product) //•	Tenta incluir (eager-load) a propriedade de navegação Product para evitar lazy loading. Observação: confirmar se a propriedade do ProductCategory é Product ou Products (plural); se o nome estiver incorreto, o Include falhará em tempo de execução/compilação.
                .ToListAsync(); //•	Executa a query de forma assíncrona e materializa o resultado em uma lista.
        }

        public async Task<ProductCategory?> GetByIdAsync(int id)//•	Método assíncrono que retorna uma categoria por id ou null se não existir.
        {
            return await _context.ProductCategories //•	Inicia a query sobre ProductCategories.
                .Include(c => c.Product) //•	Mesma inclusão de navegação (ver observar nome da propriedade como acima).
                .FirstOrDefaultAsync(c => c.Id == id);//•	Executa a query e retorna a primeira entidade que corresponde ao id, ou null.
        }

        public async Task AddAsync(ProductCategory category)//•	Método assíncrono para adicionar uma nova categoria ao banco.
        {
            await _context.ProductCategories.AddAsync(category);//•	Adiciona a entidade ao DbSet de forma assíncrona (marca para inserção).
            await _context.SaveChangesAsync();//• Salva as alterações no banco de dados de forma assíncrona.
        }

        public async Task UpdateAsync(ProductCategory category)//•	Método assíncrono para atualizar uma categoria existente.
        {
            _context.ProductCategories.Update(category);//•	Marca a entidade como modificada no DbContext.
            await _context.SaveChangesAsync();//• Salva as alterações no banco de dados de forma assíncrona.
        }

        public async Task DeleteAsync(int id)//•	Método assíncrono para remover uma categoria por id.
        {
            var category = await _context.ProductCategories.FindAsync(id);//•	Procura a entidade pelo id usando FindAsync (busca por chave primária, pode usar cache do contexto).

            if (category != null)//•	Verifica se a entidade foi encontrada antes de tentar remover.
            {
                _context.ProductCategories.Remove(category);//•	Marca a entidade para remoção.
                await _context.SaveChangesAsync();//• Salva as alterações no banco de dados de forma assíncrona.
            }
        }

        public async Task<int> CountAsync()//•	Método assíncrono que retorna a contagem de categorias armazenadas.
        {
            return await _context.ProductCategories.CountAsync();//•	Executa a contagem de registros no banco de forma assíncrona e retorna o resultado.
        }
    }
}