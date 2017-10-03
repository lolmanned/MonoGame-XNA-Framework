using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using SpaceInvaders.GameData;
using SpaceInvaders.Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpaceInvaders.Sprites.Bullet;

namespace SpaceInvaders.Sprites
{
    public class FightingEntity : LivingEntity
    {
        private readonly ShootingMachine r_ShootingMachine;

        public FightingEntity(string i_AssetName, Game i_Game, int i_InitialSouls, int i_MaxBulletsAllowed, eLevelEntitiesPoints i_LifeWorth) : base(i_AssetName, i_Game, i_InitialSouls, i_LifeWorth)
        {
            r_ShootingMachine = new ShootingMachine(i_Game, i_MaxBulletsAllowed, this);
        }

        #region class methods
        protected void ShootBullet()
        {
            r_ShootingMachine.Shoot(this);
        }
        #endregion class methods
    }
}
