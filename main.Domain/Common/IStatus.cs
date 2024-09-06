using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Common
{
    public interface IStatus
    {
        void Approve(long emlpoyerId, string feedback);
        void Reject(long emlpoyerId, string feedback);
        void Restart(long emlpoyerId);
    }
}
