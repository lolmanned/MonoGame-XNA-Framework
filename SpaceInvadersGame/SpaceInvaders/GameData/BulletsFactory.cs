using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using SpaceInvaders.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpaceInvaders.Sprites.Bullet;

namespace SpaceInvaders.GameData
{
    public static class BulletsFactory
    {
        public static Bullet CreateBullet(Game i_Game, FightingEntity i_WeaponHolder)
        {
            Bullet generatedBullet;            
            Color color;
            ePositionOriginY positionOrigionY;
            eWeaponBelongsTo belongsTo;
            int verticalDirection;

            if (i_WeaponHolder is PlayerSpaceShip)
            {
                belongsTo = eWeaponBelongsTo.HumanPlayer;
                color = Color.Red;
                positionOrigionY = ePositionOriginY.Bottom;
                verticalDirection = -1;
            }
            else if (i_WeaponHolder is Enemy)
            {
                belongsTo = eWeaponBelongsTo.Enemy;
                color = Color.Blue;
                positionOrigionY = ePositionOriginY.Top;
                belongsTo = eWeaponBelongsTo.Enemy;
                verticalDirection = 1;
            }
            else
            {
                string errorMsg = string.Format("{0} cannot hold weapons.", i_WeaponHolder);
                throw new Exception(errorMsg);
            }

            generatedBullet = new Bullet(i_Game, verticalDirection, color, belongsTo, i_WeaponHolder, positionOrigionY);

            return generatedBullet;
        }
    }
}
