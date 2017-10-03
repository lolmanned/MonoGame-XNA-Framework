using Infrastructure.Managers;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.GameData;
using SpaceInvaders.Interfaces;
using SpaceInvaders.Machines;
using SpaceInvaders.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpaceInvaders.Sprites.Bullet;

namespace SpaceInvaders.Sprites
{
    public delegate void PositionInitializedEventHandler(PlayerSpaceShip i_PlayerSpaceShip);

    public class PlayerSpaceShip : FightingEntity, IScoring
    {
        private const eWeaponBelongsTo k_EntityType = eWeaponBelongsTo.HumanPlayer;
        private const int k_InitialSouls = 3;
        private const int k_MaxBulletsAllowed = 3;
        private const int k_PixelsPerSecond = 160;
        private const string k_AssetName = SpritesAssetsNamesMapper.k_FirstShipAssetName;
        private const eLevelEntities k_Entity = eLevelEntities.Ship;
        private readonly InputManager r_InputManager;
        private readonly ScoreManager r_ScoreManager;
        private bool m_IsPlayerActive = true;
        public bool IsMouseActive { get; set; }
        public Vector2 InitialPosition { get; set; }
        public Keys LeftController { get; set; }
        public Keys RightController { get; set; }
        public Keys KeyboardFireController { get; set; }
        protected float m_DyingBlinksSpeedPerSecond = 1f / 7f;

        public event PositionInitializedEventHandler OnPositionInit;

        public PlayerSpaceShip(Game i_Game, string i_AssetName) : base(i_AssetName, i_Game, k_InitialSouls, k_MaxBulletsAllowed, (eLevelEntitiesPoints)LevelEntitiesMapper.LevelEntityToPoints(k_Entity))
        {
            r_InputManager = (InputManager)Game.Services.GetService(typeof(IInputManager));
            r_ScoreManager = (ScoreManager)Game.Services.GetService(typeof(IScoreManager));
            r_ScoreManager.AddObjectToMonitor(this);
        }

        public int Points
        {
            get { return r_ScoreManager.Points(this); }
        }
        
        public bool IsPlayerActive
        {
            get { return m_IsPlayerActive; }
        }

        protected override void LivingEntity_OnLoosingSoul()
        {
            m_IsPlayerActive = false;
            CollisionsManager.RemoveObjectFromMonitor(this);
            Animations.Restart();
            Animations.Resume();
        }

        /// <summary>
        /// Travels on the negative x-Axis with no velocity change on the y-Axis.
        /// </summary>
        protected virtual void MoveLeft()
        {
            Velocity = k_PixelsPerSecond * DirectionVector * -1;
        }

        /// <summary>
        /// Travels on the positive x-Axis with no velocity change on the y-Axis.
        /// </summary>
        protected virtual void MoveRight()
        {
            Velocity = k_PixelsPerSecond * DirectionVector;
        }

        protected virtual void HandleKeyboardInput(GameTime i_GameTime)
        {
            bool v_IsCommandedToMoveRight = r_InputManager.KeyHeld(RightController);
            bool v_IsCommandedToMoveLeft = r_InputManager.KeyHeld(LeftController);
            bool v_IsTimeToFire = r_InputManager.KeyPressed(KeyboardFireController);

            if (v_IsCommandedToMoveLeft)
            {
                MoveLeft();
            }

            if (v_IsCommandedToMoveRight)
            {
                MoveRight();
            }

            if (v_IsTimeToFire)
            {
                ShootBullet();
            }

            Position += Velocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected virtual void HandleMouseInput()
        {
            bool v_FirePreviousStateUp = r_InputManager.PrevMouseState.LeftButton == ButtonState.Released;
            bool v_FireCurrentStateDown = r_InputManager.MouseState.LeftButton == ButtonState.Pressed;
            bool v_IsTimeToFire = v_FirePreviousStateUp && v_FireCurrentStateDown;

            Velocity = new Vector2(r_InputManager.MousePositionDelta.X, Velocity.Y);
            Position = new Vector2(Position.X + Velocity.X, Position.Y);
            if (v_IsTimeToFire)
            {
                ShootBullet();
            }
        }

        protected override void InitAnimationsSettings()
        {
            m_AnimationLength = 2.4f;
            m_RotationsPerSecond = 4;

            float noOpacity = 0;
            FadingOutAnimator fader = new FadingOutAnimator("DyingFader", TimeSpan.FromSeconds(m_AnimationLength), Opacity, noOpacity);
            BlinkAnimator blinker = new BlinkAnimator("DyingBlinker", TimeSpan.FromSeconds(m_DyingBlinksSpeedPerSecond), TimeSpan.FromSeconds(m_AnimationLength));
            RotationAnimator rotator = new RotationAnimator("DyingRotator", TimeSpan.FromSeconds(m_AnimationLength), m_RotationsPerSecond);
            CompositeAnimator dyingAnimator = new CompositeAnimator("DyingAnimator", TimeSpan.FromSeconds(m_AnimationLength), this, blinker, rotator, fader);

            Animations.Add(dyingAnimator);
            Animations["DyingAnimator"].Finished += PlayerSpaceShipDyingAnimation_Finished;
            Animations.Enabled = false;
        }

        protected virtual void PlayerSpaceShipDyingAnimation_Finished(object sender, EventArgs e)
        {
            InitPosition();
            m_IsPlayerActive = true;
            if (!IsAlive)
            {
                Enabled = false;
            }
            else
            {
                CollisionsManager.AddObjectToMonitor(this);
            }
        }

        /// <summary>
        /// The default initial position for a PlayerSpaceShip.
        /// </summary>
        protected override void InitPosition()
        {
            PositionOrigin = new Vector2(Game.GraphicsDevice.Viewport.X, Height / 2);
            Position = new Vector2(Game.GraphicsDevice.Viewport.X, Game.GraphicsDevice.Viewport.Height - Height);

            if (OnPositionInit != null)
            {
                OnPositionInit.Invoke(this);
            }
            if (IsMouseActive)
            {
                Mouse.SetPosition((int)InitialPosition.X, (int)InitialPosition.Y);
            }
        }

        protected virtual void ClampPosition()
        {
            Viewport viewport = Game.GraphicsDevice.Viewport;
            float x = MathHelper.Clamp(Position.X, viewport.X, viewport.Width - Width);
            float y = MathHelper.Clamp(Position.Y, viewport.Height - Height, viewport.Height - Height);

            Position = new Vector2(x, y);
        }

        public override void Collided(ICollidable i_CollidableTarget)
        {
            Bullet bullet = i_CollidableTarget as Bullet;

            if (bullet != null && m_IsPlayerActive)
            {
                if (bullet.BelongsTo == eWeaponBelongsTo.Enemy && IsAlive)
                {
                    LooseSoul();
                }
            }

            base.Collided(i_CollidableTarget);
        }

        public override void Initialize()
        {
            base.Initialize();
            DirectionVector = Vector2.UnitX;
            InitRotationOrigin();
            InitPosition();
            InitAnimationsSettings();
        }

        public override void Update(GameTime i_GameTime)
        {
            Velocity = Vector2.Zero;
            if (m_IsPlayerActive)
            {
                HandleKeyboardInput(i_GameTime);
                if (IsMouseActive)
                {
                    HandleMouseInput();
                }
            }

            ClampPosition();
            Animations.Update(i_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
        }
    }
}
