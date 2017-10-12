using Infrastructure.Managers;
using Infrastructure.Screens;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public delegate void CollisionEventHandler(ICollidable i_CollidedTarget);

    public class CollidableSprite : Sprite, ICollidable2D
    {
        private readonly CollisionsManager r_CollisionsManager;
        protected Color[] m_TexturePixelsMatrix;
        protected float m_AnimationLength;
        protected int m_RotationsPerSecond;

        public event CollisionEventHandler OnCollisionDetected;

        public CollidableSprite(string i_AssetName, GameScreen i_GameScreen) : base(i_AssetName, i_GameScreen)
        {
            r_CollisionsManager = (CollisionsManager)Game.Services.GetService(typeof(ICollisionsManager));
            r_CollisionsManager.AddObjectToMonitor(this);
        }

        public Vector2 DirectionVector { get; protected set; }

        protected Color[] TexturePixelsMatrix
        {
            get
            {
                return m_TexturePixelsMatrix;
            }

            set
            {
                m_TexturePixelsMatrix = value;
                Texture.SetData(value);
            }
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
            m_TexturePixelsMatrix = new Color[Texture.Width * Texture.Height];
            InitBounds();
            Texture.GetData(m_TexturePixelsMatrix);
        }
    }
}
