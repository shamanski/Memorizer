using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class UserAuth : BaseEntity
    {
        public int UserId { get; set; }
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
    }
}
