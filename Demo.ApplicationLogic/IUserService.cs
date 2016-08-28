using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationLogic
{
    public interface IUserService
    {
        bool CanSignIn(string userName, string password);
    }
}
