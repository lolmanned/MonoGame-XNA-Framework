using Infrastructure.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ServiceInterfaces
{
    public interface IScoreManager
    {
        void AddObjectToMonitor(ICollidable i_ScoringSprite);
        void RemoveObjectFromMonitor(ICollidable i_ScoringSprite);
        int Points(ICollidable i_ScoringSprite);
    }
}
