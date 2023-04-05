using Memorizer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Services
{
    public interface ICurrentUserService
    {
        User GetCurrentUser();
        Task SetCurrentUser(string id);
    }
}
