using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ServiceInterfaces
{
    public interface IScreensManager
    {
        void SetCurrentScreen();
        void PushScreen();
        void PopScreen();
    }
}
