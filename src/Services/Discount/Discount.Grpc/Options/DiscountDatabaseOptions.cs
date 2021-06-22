using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Options
{
    public class DiscountDatabaseOptions
    {
        public const string DatabaseSettings = "DatabaseSettings";
        public string ConnectionString { get; set; }
    }
}
