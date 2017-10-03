using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using SpaceInvaders.GameData;
using SpaceInvaders.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpaceInvaders.Sprites.Bullet;

namespace SpaceInvaders.Machines
{
    public class ShootingMachine : GameComponent
    {
        private readonly int r_MaxBulletsOnScreen;
        private readonly Queue<Bullet> r_BulletsPack;
        private readonly FightingEntity r_ShootingMachineHolder;

        private Bullet m_PrevBullet;

        public ShootingMachine(Game i_Game, int i_MaxBulletsOnScreen, FightingEntity i_ShootingMachineHolder) : base(i_Game)
        {
            r_MaxBulletsOnScreen = i_MaxBulletsOnScreen;
            r_BulletsPack = new Queue<Bullet>(i_MaxBulletsOnScreen);
            r_ShootingMachineHolder = i_ShootingMachineHolder;
            Game.Components.Add(this);
        }

        public Bullet BulletPrototype
        {
            get
            {
                Bullet bullet = BulletsFactory.CreateBullet(Game, r_ShootingMachineHolder);
                bullet.OnDestroyed += Bullet_OnDestroyed;

                return bullet;
            }
        }

        protected virtual void Bullet_OnDestroyed(Bullet i_Bullet)
        {
            if (r_BulletsPack.Count < r_MaxBulletsOnScreen)
            {
                AddBullet();
            }
        }

        protected virtual void AddBullet()
        {
            if (r_BulletsPack.Count < r_MaxBulletsOnScreen)
            {
                r_BulletsPack.Enqueue(BulletPrototype);
            }
        }

        /// <summary>
        /// Adds to the bullets pack the missing bullets in order to make it full.
        /// </summary>
        public virtual void Load()
        {
            if (r_BulletsPack.Count < r_MaxBulletsOnScreen)
            {
                for (int i = r_BulletsPack.Count; i < r_MaxBulletsOnScreen; i++)
                {
                    r_BulletsPack.Enqueue(BulletPrototype);
                }
            }
        }

        /// <summary>
        /// Releases a bullet to the air.
        /// </summary>
        /// <param name="i_Fighter"></param>
        public virtual void Shoot(FightingEntity i_Fighter)
        {
            if (r_BulletsPack.Count > 0)
            {
                Bullet bullet = r_BulletsPack.Dequeue();

                bullet.Enabled = true;
                bullet.Visible = true;
                bullet.Position = new Vector2(i_Fighter.Position.X + i_Fighter.Width / 2, i_Fighter.Position.Y);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            Load();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
        }
    }
}
