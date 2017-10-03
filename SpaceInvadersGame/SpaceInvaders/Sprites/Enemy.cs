using Microsoft.Xna.Framework;
using SpaceInvaders.GameData;
using SpaceInvaders.Helpers;
using SpaceInvaders.Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using static SpaceInvaders.Sprites.Bullet;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.Managers;

namespace SpaceInvaders.Sprites
{
    public delegate void NotificationEventHandler();

    public delegate void DeadEventHandler(Enemy i_Enemy);

    public delegate void JumpDistanceShortenEventHandler(object sender, float distance);

    public abstract class Enemy : FightingEntity
    {
        #region class members
        private const int k_NumOfTextures = 2;
        private const eWeaponBelongsTo k_EntityType = eWeaponBelongsTo.Enemy;
        private const int k_MaxBulletsAllowedOnScreen = 1;
        private const int k_InitialSouls = 1;
        private const int k_MinElapsedSecondsToShoot = 7;
        private const int k_MaxElapsedSecondsToShoot = 20;
        private const double k_TextureWidthPercentPerJumpDistance = 0.5;
        private readonly ClockMachine r_ShootingAlarmClock;
        private readonly ClockMachine r_JumpingAlarmClock;
        private double m_TimeToJumpInSeconds = 0.5;
        private bool m_IsItTimeToShoot = false;
        private bool m_IsItTimeToJump = false;
        private bool m_IsDyingAnimationActive = false;
        private int m_TextureIndicator = 0;
        private float m_JumpDistance;
        protected Texture2D[] m_AlternatingTextures = new Texture2D[k_NumOfTextures];
        #endregion class members

        #region delegates event handlers
        public event NotificationEventHandler AfterWallsHit;

        public event NotificationEventHandler OnTimeToJump;

        public event JumpDistanceShortenEventHandler OnJumpDistanceChanged;

        public event DeadEventHandler OnDead;
        #endregion delegates event handlers

        public Enemy(Game i_Game, Color i_EnemyColor, string i_FirstAssetName, int i_StartingAssetIndex) :
            base(i_FirstAssetName, i_Game, k_InitialSouls, k_MaxBulletsAllowedOnScreen, LevelEntitiesMapper.EnemyColorToLifeWorth(i_EnemyColor))
        {
            r_ShootingAlarmClock = new ClockMachine(k_MinElapsedSecondsToShoot, k_MaxElapsedSecondsToShoot);
            r_JumpingAlarmClock = new ClockMachine(m_TimeToJumpInSeconds);
            m_TextureIndicator = i_StartingAssetIndex;
            UpdateOrder = 1;
        }

        #region class properties
        /// <summary>
        /// Returns the jump distance on the x-Axis as Pixels Per Second.
        /// </summary>
        public float InitialJumpDistance
        {
            get { return (float)(Texture.Width * k_TextureWidthPercentPerJumpDistance); }
        }

        public virtual float JumpDistance
        {
            get { return m_JumpDistance; }
        }

        public bool IsItTimeToJump
        {
            get { return m_IsItTimeToJump; }
        }

        public double JumpTimeInSeconds
        {
            get { return r_JumpingAlarmClock.AlarmTimeInSeconds; }
            set { r_JumpingAlarmClock.AlarmTimeInSeconds = value; }
        }

        public bool IsDyingAnimationActive
        {
            get { return m_IsDyingAnimationActive; }
        }

        /// <summary>
        /// Returns true if a the random shooting time has been reached and there is no bullet on the screen.
        /// </summary>
        public bool IsItTimeToShoot
        {
            get { return m_IsItTimeToShoot; }
        }
        #endregion class properties

        #region class methods

        public virtual void AlternateTexture()
        {
            m_TextureIndicator = ++m_TextureIndicator % k_NumOfTextures;
            Texture = m_AlternatingTextures[m_TextureIndicator];
        }

        public override void Initialize()
        {
            base.Initialize();
            m_JumpDistance = InitialJumpDistance;
            DirectionVector = Vector2.UnitX;
            Velocity = new Vector2(JumpDistance, 0);
            m_RotationsPerSecond = 6;
            InitRotationOrigin();
            InitAnimationsSettings();
        }

        protected override void InitAnimationsSettings()
        {
            m_AnimationLength = 1.6f;
            Vector2 sizeAfterDying = new Vector2(0, 0);
            ResizingAnimator shrinker = new ResizingAnimator("Shrinker", TimeSpan.FromSeconds(m_AnimationLength), Scales, sizeAfterDying);
            RotationAnimator rotator = new RotationAnimator("Rotator", TimeSpan.FromSeconds(m_AnimationLength), m_RotationsPerSecond);
            CompositeAnimator dyingAnimator = new CompositeAnimator("DyingAnimator", TimeSpan.FromSeconds(m_AnimationLength), this, rotator, shrinker);

            Animations.Add(dyingAnimator);
            Animations["DyingAnimator"].Finished += EnemyDyingAnimation_Finished;
            Animations.Enabled = false;
        }

        private void EnemyDyingAnimation_Finished(object sender, EventArgs e)
        {
            Visible = false;
            m_IsDyingAnimationActive = false;
            if (!IsAlive && OnDead != null)
            {
                OnDead.Invoke(this);
            }
        }

        protected override void LivingEntity_OnLoosingSoul()
        {
            CollisionsManager.RemoveObjectFromMonitor(this);
            m_IsDyingAnimationActive = true;
            Animations.Restart();
            Animations.Resume();
        }

        protected override void HumanPlayerBullet_OnCollision()
        {
            if (IsAlive)
            {
                LooseSoul();
            }
        }

        /// <summary>
        /// Updates the boolean indicators according to their corresponding updated clock timers.
        /// </summary>
        /// <param name="i_GameTime"></param>
        public override void Update(GameTime i_GameTime)
        {
            float elapsedTimeInSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            r_JumpingAlarmClock.IncrementClockTimer(elapsedTimeInSeconds);
            m_IsItTimeToJump = r_JumpingAlarmClock.IsClockTimeReached;

            if (IsAlive)
            {
                r_ShootingAlarmClock.IncrementClockTimer(elapsedTimeInSeconds);
                m_IsItTimeToShoot = r_ShootingAlarmClock.IsClockTimeReached;
                if (m_IsItTimeToJump)
                {
                    AlternateTexture();
                }

                if (m_IsItTimeToShoot)
                {
                    ShootBullet();
                    r_ShootingAlarmClock.SetRandomAlarmTime(k_MinElapsedSecondsToShoot, k_MaxElapsedSecondsToShoot);
                }
            }
            else if (m_IsDyingAnimationActive)
            {
                Animations.Resume();
            }

            Animations.Update(i_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
        }
        #endregion class methods
    }
}
