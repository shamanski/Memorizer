using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorizer.DbModel
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; protected set; }
    }
}
