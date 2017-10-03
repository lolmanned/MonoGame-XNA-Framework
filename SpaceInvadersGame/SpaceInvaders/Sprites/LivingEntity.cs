using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public delegate void LoosingSoulEventHandler();
    public delegate void HumanPlayerBulletCollisionEventHandler();
    public delegate void EnemyBulletCollisionEventHandler();

    public class LivingEntity : CollidableSprite
    {
        private readonly eLevelEntitiesPoints r_LifeWorth;
        private readonly Health r_Health;
        private bool m_IsAlive = true;

        public LivingEntity(string i_AssetName , Game i_Game, int i_InitialSouls, eLevelEntitiesPoints i_LifeWorth) : base(i_AssetName, i_Game)
        {
            r_Health = new Health(i_InitialSouls);
            r_LifeWorth = i_LifeWorth;
            OnLoosingSoul += LivingEntity_OnLoosingSoul;
            OnEnemyBulletCollision += EnemyBullet_OnCollision;
            OnHumanPlayerBulletCollision += HumanPlayerBullet_OnCollision;
        }

        public event LoosingSoulEventHandler OnLoosingSoul;

        public event HumanPlayerBulletCollisionEventHandler OnHumanPlayerBulletCollision;

        public event EnemyBulletCollisionEventHandler OnEnemyBulletCollision;

        #region class properties
        public virtual int LifeWorth
        {
            get { return (int)r_LifeWorth; }
        }

        public bool IsAlive
        {
            get { return m_IsAlive; }
        }

        public int Souls
        {
            get { return r_Health.Souls; }
        }

        public virtual bool IsHorizontalWallDetected
        {
            get { return IsLeftWallDetected || IsRightWallDeteced; }
        }

        public virtual bool IsLeftWallDetected
        {
            get { return Position.X <= Game.GraphicsDevice.Viewport.X; }
        }

        public virtual bool IsRightWallDeteced
        {
            get { return Position.X >= Game.GraphicsDevice.Viewport.Width - Texture.Width; }
        }
        #endregion class properties

        #region class methods
        public void LooseSoul()
        {
            bool v_IsDead;

            r_Health.LooseSoul();
            v_IsDead = r_Health.Souls == 0;
            if (v_IsDead)
            {
                m_IsAlive = false;
            }

            if (OnLoosingSoul != null)
            {
                OnLoosingSoul.Invoke();
            }
        }

        protected virtual void HumanPlayerBullet_OnCollision()
        {
        }

        protected virtual void EnemyBullet_OnCollision()
        {
        }

        public override void Collided(ICollidable i_CollidableTarget)
        {
            Bullet bullet = i_CollidableTarget as Bullet;

            if (bullet != null)
            {
                switch (bullet.BelongsTo)
                {
                    case eWeaponBelongsTo.HumanPlayer:
                        OnHumanPlayerBulletCollision.Invoke();
                        break;

                    case eWeaponBelongsTo.Enemy:
                        OnEnemyBulletCollision.Invoke();
                        break;
                }
            }

            base.Collided(i_CollidableTarget);
        }

        /// <summary>
        /// Checks whether the living collidable sprite is not alive, if true: sets it to be invisible and removes it from CollisionsManager.
        /// </summary>
        protected virtual void LivingEntity_OnLoosingSoul()
        {
            if (!IsAlive)
            {
                Visible = false;
            }
        }

        public void Revive()
        {
            r_Health.ResetSouls();
            m_IsAlive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!m_IsAlive)
            {
                Visible = false;
            }

            base.Update(gameTime);
        }
        #endregion class methods
    }
}
