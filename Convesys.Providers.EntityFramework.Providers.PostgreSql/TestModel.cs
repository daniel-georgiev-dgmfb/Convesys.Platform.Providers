using Pirina.Kernel.Data;
using Pirina.Kernel.Data.Tenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pirina.Providers.EntityFramework.Providers.PostgreSql
{
    public class TestModel : BaseTenantModel
    {
        public string Name { get; set; }
        public uint Age { get; set; }
    }
}
