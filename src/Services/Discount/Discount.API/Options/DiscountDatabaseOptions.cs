using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Options
{
    public class DiscountDatabaseOptions
    {
        public const string DiscountDatabase = "DiscountDatabase";
        public string ConnectionString { get; set; }
    }
}
