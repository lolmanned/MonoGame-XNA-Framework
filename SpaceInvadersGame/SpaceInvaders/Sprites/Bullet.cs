using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using SpaceInvaders.GameData;
using SpaceInvaders.Interfaces;
using SpaceInvaders.Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Sprites
{
    public delegate void OutOfBoundsEventHandler(Bullet i_Bullet);
    public delegate void DestroyedEventHandler(Bullet i_Bullet);

    /// <summary>
    /// Represents a vertical bullet.
    /// </summary>
    public class Bullet : CollidableSprite, IWeapon
    {
        public enum eWeaponBelongsTo
        {
            Enemy,
            HumanPlayer
        }

        public enum ePositionOriginY
        {
            Top,
            Bottom
        }

        private const string k_BulletAssetName = SpritesAssetsNamesMapper.k_BulletAssetName;
        private const int k_PixelsPerSecond = 125;
        private readonly Vector2 r_VerticalDirection;
        private readonly eWeaponBelongsTo r_BelongsTo;
        private readonly ePositionOriginY r_PositionOriginY;
        private readonly FightingEntity r_BulletHolder;
        private static readonly Random sr_RandomEnemyBulletsDecider = new Random();

        public event OutOfBoundsEventHandler OnOutOfBounds;

        public event DestroyedEventHandler OnDestroyed;

        public Bullet(Game i_Game, int i_VerticalDirection, Color i_Color, eWeaponBelongsTo i_BelongsTo, FightingEntity i_BulletHolder, ePositionOriginY i_PositionOriginY) : base(k_BulletAssetName, i_Game)
        {
            r_VerticalDirection = new Vector2(0, i_VerticalDirection);
            DirectionVector = r_VerticalDirection;
            r_PositionOriginY = i_PositionOriginY;
            r_BelongsTo = i_BelongsTo;
            r_BulletHolder = i_BulletHolder;
            Velocity = r_VerticalDirection * k_PixelsPerSecond;
            TintColor = i_Color;
        }

        #region IWeapon interface implementation
        public FightingEntity WeaponHolder
        {
            get { return r_BulletHolder; }
        }
        #endregion IWeapon interface implementation

        #region properties
        protected virtual bool IsOutOfVerticalBounds
        {
            get { return Position.Y < Game.GraphicsDevice.Viewport.Y || Position.Y > Game.GraphicsDevice.Viewport.Height; }
        }

        public eWeaponBelongsTo BelongsTo
        {
            get { return r_BelongsTo; }
        }
        #endregion properties

        private void randomlyDestroyEnemyBullet(Bullet i_EnemyBullet)
        {
            int randomInt = sr_RandomEnemyBulletsDecider.Next(0, 5);
            bool v_IsAboutToDestroy = randomInt < 2;

            if (v_IsAboutToDestroy)
            {
                i_EnemyBullet.Destroy();
            }
        }

        public void Destroy()
        {
            Visible = false;
            CollisionsManager.RemoveObjectFromMonitor(this);
            Game.Components.Remove(this);
            if (OnDestroyed != null)
            {
                OnDestroyed.Invoke(this);
            }

            Dispose();
        }

        private void handleBulletDestroyDecisionOnKillingAttempt(Enemy i_EnemyTarget, MotherShip i_MotherShipTarget, PlayerSpaceShip i_SpaceShipTarget)
        {
            bool v_IsAboutToBeDestroyed = false;

            /// SpaceShip killed an hostile.
            if (BelongsTo == eWeaponBelongsTo.HumanPlayer)
            {
                bool v_IsHumanPlayerBulletHitAnHostile = i_EnemyTarget != null || i_MotherShipTarget != null;

                if (v_IsHumanPlayerBulletHitAnHostile)
                {
                    v_IsAboutToBeDestroyed = true;
                }
            }

            /// Enemy killed a SpaceShip.
            if (i_SpaceShipTarget != null && BelongsTo == eWeaponBelongsTo.Enemy)
            {
                v_IsAboutToBeDestroyed = true;
            }

            if (v_IsAboutToBeDestroyed)
            {
                Destroy();
            }
        }

        private void handleCollisionWithAnotherBullet(Bullet i_Bullet)
        {
            if (i_Bullet != null)
            {
                bool v_IsBulletTargetBelongsToPlayer = r_BelongsTo == eWeaponBelongsTo.Enemy && i_Bullet.BelongsTo == eWeaponBelongsTo.HumanPlayer;
                bool v_IsBulletTargetBelongsToEnemy = r_BelongsTo == eWeaponBelongsTo.HumanPlayer && i_Bullet.BelongsTo == eWeaponBelongsTo.Enemy;
                bool v_IsEnemyAndPlayerBulletsCollided = v_IsBulletTargetBelongsToPlayer || v_IsBulletTargetBelongsToEnemy;

                if (v_IsEnemyAndPlayerBulletsCollided)
                {
                    Bullet playerBullet = v_IsBulletTargetBelongsToPlayer ? i_Bullet : this;
                    Bullet enemyBullet = v_IsBulletTargetBelongsToPlayer ? this : i_Bullet;

                    playerBullet.Destroy();
                    randomlyDestroyEnemyBullet(enemyBullet);
                }
            }
        }

        private void handleCollisionWithBarrier(Barrier barrierTarget)
        {
            if (barrierTarget != null)
            {
                barrierTarget.OnDamageDone += Barrier_OnBulletDamageDone;
            }
        }

        protected void Barrier_OnBulletDamageDone(Barrier barrierTarget)
        {
            Destroy();
            barrierTarget.OnDamageDone -= Barrier_OnBulletDamageDone;
        }

        public override void Collided(ICollidable i_CollidableTarget)
        {
            Bullet bulletTarget = i_CollidableTarget as Bullet;
            Enemy enemyTarget = i_CollidableTarget as Enemy;
            MotherShip motherShipTarget = i_CollidableTarget as MotherShip;
            PlayerSpaceShip spaceShipTarget = i_CollidableTarget as PlayerSpaceShip;
            Barrier barrierTarget = i_CollidableTarget as Barrier;

            handleCollisionWithAnotherBullet(bulletTarget);
            handleBulletDestroyDecisionOnKillingAttempt(enemyTarget, motherShipTarget, spaceShipTarget);
            handleCollisionWithBarrier(barrierTarget);
        }

        public override void Initialize()
        {
            float y = r_PositionOriginY == ePositionOriginY.Bottom ? Height : 0;

            PositionOrigin = new Vector2(Width / 2, y);
            Enabled = false;
            Visible = false;
            base.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (IsOutOfVerticalBounds)
            {
                Destroy();
            }
        }
    }
}
