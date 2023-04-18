using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorizer.DbModel
{
    public class TelegramCode : BaseEntity
    {
        public string Code { get; set; }
    }
}
