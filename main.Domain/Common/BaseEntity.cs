using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Common
{
    internal class BaseEntity : IEntityId, IEntiteDate
    {
        public long id { get; set; }
        public DateTime DateCreate { get ; set; }
        public DateTime DateUpdate { get ; set; }
    }
}
