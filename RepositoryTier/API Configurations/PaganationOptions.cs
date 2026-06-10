using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.API_Configurations
{
    public class PaganationOptions
    {
        public int Page { get; set; }
        public int TinyPageSize { get; set; }
        public int BigPageSize { get; set; }
    }
}
