using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Configurations
{
    public class RedisSettings
    {
        public static readonly string RedisSettingsSectionName = "RedisSettings";

        public string? ConnectionString { get; set; }

    }
}
