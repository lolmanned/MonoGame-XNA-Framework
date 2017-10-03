using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.GameData;
using SpaceInvaders.Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Animators;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Managers;

namespace SpaceInvaders.Sprites
{
    public class MotherShip : LivingEntity
    {
        private const int k_InitialSouls = 1;
        private const int k_PixelsPerSecond = 90;
        private const int k_MinElapsedSecondsToSpawn = 15;
        private const int k_MaxElapsedSecondsToSpawn = 23;
        private const string k_AssetName = SpritesAssetsNamesMapper.k_MotherShipAssetName;
        private readonly ClockMachine r_SpawningTimer;
        private bool m_IsItTimeToSpawn = false;
        protected float m_DyingBlinksPerSecond;
        private bool m_IsDyingAnimationActive = false;

        public MotherShip(Game i_Game) : base(k_AssetName, i_Game, k_InitialSouls, (eLevelEntitiesPoints)LevelEntitiesMapper.LevelEntityToPoints(eLevelEntities.MotherShip))
        {
            TintColor = Color.Red;
            DirectionVector = Vector2.UnitX;
            r_SpawningTimer = new ClockMachine(k_MinElapsedSecondsToSpawn, k_MaxElapsedSecondsToSpawn);
            Velocity = DirectionVector * k_PixelsPerSecond;
        }

        protected override void InitPosition()
        {
            Viewport viewport = Game.GraphicsDevice.Viewport;

            Position = new Vector2(viewport.X - Width, viewport.Y + Height);
        }

        protected override void InitAnimationsSettings()
        {
            m_AnimationLength = 2.4f;
            m_DyingBlinksPerSecond = 1f / 4f;

            Vector2 scaleAfterResizing = Vector2.Zero;
            float opacityAfterFadingOut = 0;

            ResizingAnimator shrinker = new ResizingAnimator("MotherShipDyingShrinker", TimeSpan.FromSeconds(m_AnimationLength), Scales, scaleAfterResizing);
            BlinkAnimator blinker = new BlinkAnimator("MotherShipDyingBlinker", TimeSpan.FromSeconds(m_DyingBlinksPerSecond), TimeSpan.FromSeconds(m_AnimationLength));
            FadingOutAnimator fader = new FadingOutAnimator("MotherShipDyingFader", TimeSpan.FromSeconds(m_AnimationLength), Opacity, opacityAfterFadingOut);
            CompositeAnimator dyingAnimator = new CompositeAnimator("MotherShipDyingAnimator", TimeSpan.FromSeconds(m_AnimationLength), this, shrinker, blinker, fader);

            Animations.Add(dyingAnimator);
            Animations["MotherShipDyingAnimator"].Finished += MotherShipDyingAnimation_Finished;
            Animations.Enabled = false;
        }

        protected virtual void MotherShipDyingAnimation_Finished(object sender, EventArgs e)
        {
            Visible = false;
            m_IsDyingAnimationActive = false;
        }

        protected virtual void FloatToTheRight(double i_ElapsedTimeInSeconds)
        {
            double pixelsToMoveOnXAxis = i_ElapsedTimeInSeconds * Velocity.X;

            Position = new Vector2((float)(Position.X + pixelsToMoveOnXAxis), Position.Y);
        }

        protected bool IsDyingAnimationActive
        {
            get { return m_IsDyingAnimationActive; }
        }

        /// <summary>
        /// The right wall will be detected once the mothership is out of bounds.
        /// </summary>
        public override bool IsRightWallDeteced
        {
            get { return Position.X > GraphicsDevice.Viewport.Width; }
        }

        private void motherShipRespawn()
        {
            Visible = true;
            InitPosition();
            Revive();
            CollisionsManager.AddObjectToMonitor(this);
        }

        protected override void HumanPlayerBullet_OnCollision()
        {
            if (IsAlive)
            {
                LooseSoul();
            }
        }

        protected override void LivingEntity_OnLoosingSoul()
        {
            m_IsDyingAnimationActive = true;
            Animations.Restart();
            CollisionsManager.RemoveObjectFromMonitor(this);
        }
        
        public override void Initialize()
        {
            base.Initialize();
            Visible = false;
            InitPosition();
            InitAnimationsSettings();
        }

        public override void Update(GameTime i_GameTime)
        {
            double elapsedTimeInSeconds = i_GameTime.ElapsedGameTime.TotalSeconds;

            r_SpawningTimer.IncrementClockTimer(elapsedTimeInSeconds);
            m_IsItTimeToSpawn = r_SpawningTimer.IsClockTimeReached;
            if (m_IsItTimeToSpawn)
            {
                motherShipRespawn();
            }

            if (Visible && !m_IsDyingAnimationActive)
            {
                FloatToTheRight(elapsedTimeInSeconds);
                if (IsRightWallDeteced)
                {
                    Visible = false;
                    r_SpawningTimer.SetRandomAlarmTime(k_MinElapsedSecondsToSpawn, k_MaxElapsedSecondsToSpawn);
                }
            }
            
            Animations.Update(i_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
        }
    }
}
