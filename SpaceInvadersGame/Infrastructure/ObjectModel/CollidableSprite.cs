using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.ObjectModel
{
    public delegate void CollisionEventHandler(ICollidable i_CollidedTarget);

    public class CollidableSprite : Sprite, ICollidable2D
    {
        protected static bool s_IsTextureInstanceCreated = false;
        private readonly CollisionsManager r_CollisionsManager;
        protected bool m_IsPixelsCollisionActive = false;
        protected Color[] m_TexturePixelsMatrix;
        protected float m_AnimationLength;
        protected int m_RotationsPerSecond;

        public event CollisionEventHandler OnCollisionDetected;

        public CollidableSprite(string i_AssetName, Game i_Game) : base(i_AssetName, i_Game)
        {
            r_CollisionsManager = (CollisionsManager)Game.Services.GetService(typeof(ICollisionsManager));
            r_CollisionsManager.AddObjectToMonitor(this);
        }

        public Vector2 DirectionVector { get; protected set; }

        protected virtual void InitAnimationsSettings()
        {
        }

        protected virtual void InitPosition()
        {
        }

        protected virtual void InitRotationOrigin()
        {
            RotationOrigin = new Vector2(WidthBeforeScale / 2, HeightBeforeScale / 2);
        }

        protected CollisionsManager CollisionsManager
        {
            get { return r_CollisionsManager; }
        }

        public virtual bool CheckCollision(ICollidable i_Source)
        {
            bool v_IsCollided = false;
            ICollidable2D source = i_Source as ICollidable2D;

            if (source != null)
            {
                v_IsCollided = source.Bounds.Intersects(Bounds);
            }

            return v_IsCollided;
        }

        public virtual void Collided(ICollidable i_CollidableTarget)
        {
            if (OnCollisionDetected != null)
            {
                OnCollisionDetected.Invoke(i_CollidableTarget);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            if (m_IsPixelsCollisionActive)
            {
                if (s_IsTextureInstanceCreated)
                {
                    m_TexturePixelsMatrix = new Color[Texture.Width * Texture.Height];
                    Texture.GetData(m_TexturePixelsMatrix);
                    Texture = new Texture2D(Game.GraphicsDevice, Texture.Width, Texture.Height);
                    Texture.SetData(m_TexturePixelsMatrix);
                }
                else
                {
                    s_IsTextureInstanceCreated = true;
                    m_TexturePixelsMatrix = new Color[Texture.Width * Texture.Height];
                    Texture.GetData(m_TexturePixelsMatrix);
                }
            }
        }
    }
}
