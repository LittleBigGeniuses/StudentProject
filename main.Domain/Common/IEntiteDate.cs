using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Common
{
    public interface IEntiteDate
    {
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
