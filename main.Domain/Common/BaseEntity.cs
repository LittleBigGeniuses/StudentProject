using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Common
{
    public class BaseEntity : IEntityId, IEntityDate
    {
        public long Id { get; set; }
        public DateTime DateCreate { get ; set; }
        public DateTime DateUpdate { get ; set; }
    }
}
