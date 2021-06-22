using System;
using System.Threading.Tasks;
using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IOptions<DiscountDatabaseOptions> _options;

        public DiscountRepository(IOptions<DiscountDatabaseOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<Coupon?> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_options.Value.ConnectionString);
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new {ProductName = productName});
            
            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_options.Value.ConnectionString);
            var affected =
                await connection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                    new {coupon.ProductName, coupon.Description, coupon.Amount});
            
            return affected != 0;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_options.Value.ConnectionString);
            var affected =
                await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id",
                    new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

            return affected != 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_options.Value.ConnectionString);
            var affected =
                await connection.ExecuteAsync
                ("DELETE FROM Coupon WHERE ProductName=@ProductName",
                    new { ProductName = productName });

            return affected != 0;
        }
    }
}