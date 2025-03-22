using Dapper;
using DapperComPostgresql.Models;
using Npgsql;
using System.Data;

namespace DapperComPostgresql.Repository
{
    public class ItemRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ItemRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("postgres");
        }

        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            using (var connection = Connection)
            {
                string query = "SELECT * FROM Items";

                var items = await connection.QueryAsync<Item>(query);
                return items.OrderBy(item => item.Id); ;
            }
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            using (var connection = Connection)
            {
                string query = "SELECT * FROM Items WHERE Id=@id";

                var items = await connection.QuerySingleOrDefaultAsync<Item>(query, new {Id = id});
                return items;
            }
        }

        public async Task<int> AddItemAsync(Item item)
        {
            using (var connection = Connection)
            {
                string query = "INSERT INTO Items (NOME, PRECO) VALUES (@NOME, @PRECO) RETURNING Id";

                int id = await connection.ExecuteScalarAsync<int>(query, item);
                return id;
            }
        }

        public async Task<int> UpdateItemAsync(Item item)
        {
            using (var connection = Connection)
            {
                string query = "UPDATE Items SET Nome=@Nome, Preco=@Preco WHERE Id=@id";

                return await connection.ExecuteAsync(query, item);
            }
        }

        public async Task<int> DeleteItemAsync(int id)
        {
            using (var connection = Connection)
            {
                string query = "DELETE FROM Items WHERE Id=@id";

                return await Connection.ExecuteAsync(query, new {Id = id});
            }
        }
    }
}
