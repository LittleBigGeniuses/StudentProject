using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Common
{
    public interface IEntityDate
    {
        public DateTime DateCreate { get; }
        public DateTime DateUpdate { get; set; }
    }
}
