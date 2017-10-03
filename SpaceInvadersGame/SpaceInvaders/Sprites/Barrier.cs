using Microsoft.Xna.Framework;
using SpaceInvaders.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework.Graphics;
using static SpaceInvaders.Sprites.Bullet;
using Infrastructure.ObjectModel;

namespace SpaceInvaders.Sprites
{
    public delegate void DamageDoneEventHandler(Barrier i_Barrier);

    public class Barrier : CollidableSprite
    {
        private const string k_BarrierAssetName = SpritesAssetsNamesMapper.k_BarrierAssetName;
        private const int k_PixelsPerSecond = 60;
        private float m_FloatingDistance;
        private float m_FloatingDistancePixelage;
        protected float m_PercentageOfBulletHeight = 0.45f;

        public event DamageDoneEventHandler OnDamageDone;

        public Barrier(Game i_Game) : base(k_BarrierAssetName, i_Game) { }

        public virtual float FloatingDistance
        {
            get { return Width / 2; }
            protected set { m_FloatingDistance = value; }
        }

        public float Pixelage
        {
            get { return m_FloatingDistancePixelage; }
        }

        public bool IsFloatingDistanceReached
        {
            get { return Math.Abs(m_FloatingDistancePixelage) >= FloatingDistance; }
        }

        public virtual void ResetFloatingDistancePixelage()
        {
            m_FloatingDistancePixelage = 0;
        }

        /// <summary>
        /// A Bullet leaves a hole of 45% of its height.
        /// </summary>
        private void handleBulletPixelsCollision(Bullet i_Bullet, ref bool i_IsDamageDone, ref Color[] i_Pixels)
        {
            int startX = Math.Max(i_Bullet.Bounds.X, Bounds.X);
            int endX = Math.Min(i_Bullet.Bounds.X + i_Bullet.Bounds.Width - 1, Bounds.X + Bounds.Width - 1);
            int endY = Math.Min(i_Bullet.Bounds.Y + 2, Bounds.Y + Bounds.Height - 1);
            int startY = Math.Max(endY - (int)(i_Bullet.Bounds.Height * m_PercentageOfBulletHeight), Bounds.Y);

            if (i_Bullet.BelongsTo == eWeaponBelongsTo.Enemy)
            {
                startY = Math.Max(i_Bullet.Bounds.Y + i_Bullet.Bounds.Height - 2, Bounds.Y);
                endY = Math.Min(startY + (int)(i_Bullet.Bounds.Height * m_PercentageOfBulletHeight), Bounds.Y + Bounds.Height);
            }

            eatPixels(startX, endX, startY, endY, i_Bullet, ref i_IsDamageDone, ref i_Pixels);
        }

        private void handleEnemyPixelsCollision(Enemy i_Enemy, ref bool i_IsDamageDone, ref Color[] i_Pixels)
        {
            int startX = Math.Max(i_Enemy.Bounds.X, Bounds.X);
            int endX = Math.Min(i_Enemy.Bounds.X + i_Enemy.Bounds.Width - 1, Bounds.X + Bounds.Width - 1);
            int startY = Math.Max(Bounds.Y, i_Enemy.Bounds.Y);
            int endY = Math.Min(Bounds.Y + Bounds.Height - 1, i_Enemy.Bounds.Y + i_Enemy.Bounds.Height - 1);

            eatPixels(startX, endX, startY, endY, i_Enemy, ref i_IsDamageDone, ref i_Pixels);
        }

        private void eatPixels(int startX, int endX, int startY, int endY, ICollidable i_CollidedTarget, ref bool i_IsDamageDone, ref Color[] i_Pixels)
        {
            int inBoundsEndY = Math.Abs(endY - Bounds.Y);
            int inBoundsStartY = Math.Abs(startY - Bounds.Y);
            int inBoundsEndX = Math.Abs(endX - Bounds.X);
            int inBoundsStartX = Math.Abs(startX - Bounds.X);

            for (int x = inBoundsStartX; x <= inBoundsEndX && inBoundsEndX < Bounds.Width; x++)
            {
                for (int y = inBoundsStartY; y <= inBoundsEndY && inBoundsEndY < Bounds.Height; y++)
                {
                    Color pixel = m_TexturePixelsMatrix[(y * Bounds.Width) + x];

                    if (pixel.A != 0)
                    {
                        //m_TexturePixelsMatrix[(y * Bounds.Width) + x] = Color.Transparent;
                        i_Pixels[(y * Bounds.Width) + x] = Color.Transparent;
                        i_IsDamageDone = true;
                    }
                }
            }
        }

        public override void Collided(ICollidable i_CollidableTarget)
        {
            Bullet bullet = i_CollidableTarget as Bullet;
            Enemy enemy = i_CollidableTarget as Enemy;
            bool v_IsDamageDone = false;

            if (bullet != null || enemy != null)
            {
                Texture.GetData(m_TexturePixelsMatrix);
            }

            if (bullet != null)
            {
                handleBulletPixelsCollision(bullet, ref v_IsDamageDone, ref m_TexturePixelsMatrix);
                if (v_IsDamageDone)
                {
                    if (OnDamageDone != null)
                    {
                        OnDamageDone.Invoke(this);
                    }

                    Texture.SetData(m_TexturePixelsMatrix);
                    //Texture.SetData(pixels);
                    bullet.Destroy();
                }
            }

            if (enemy != null)
            {
                handleEnemyPixelsCollision(enemy, ref v_IsDamageDone, ref m_TexturePixelsMatrix);
                if (v_IsDamageDone)
                {
                    Texture.SetData(m_TexturePixelsMatrix);
                }
            }
            base.Collided(i_CollidableTarget);
        }

        public override void Initialize()
        {
            DirectionVector = Vector2.UnitX;
            Velocity = DirectionVector * k_PixelsPerSecond;
            PositionOrigin = new Vector2(Width / 2, Height / 2);
            ResetFloatingDistancePixelage();
            m_IsPixelsCollisionActive = true;

            base.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            double distanceDone = i_GameTime.ElapsedGameTime.TotalSeconds * Velocity.X;

            m_FloatingDistancePixelage += (float)distanceDone;

            base.Update(i_GameTime);
        }
    }
}