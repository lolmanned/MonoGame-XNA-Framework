using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Managers
{
    /// <summary>
    /// A class which assits with managing different screens during one game. such as: menu screens, play screens, etc.
    /// </summary>
    public class ScreensManager : GameService, IScreensManager
    {
        //private readonly Stack<GameScreen> 

        public ScreensManager(Game i_Game) : base(i_Game)
        {
        }

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(IScreensManager), this);
        }

        #region IScreensManager interface implementation
        public void PopScreen()
        {
            throw new NotImplementedException();
        }

        public void PushScreen()
        {
            throw new NotImplementedException();
        }

        public void SetCurrentScreen()
        {
            throw new NotImplementedException();
        }
        #endregion IScreensManager interface implementation
    }
}
