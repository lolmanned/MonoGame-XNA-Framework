using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using SpaceInvaders.GameData;
using SpaceInvaders.Interfaces;
using SpaceInvaders.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Managers
{
    public class ScoreManager : GameService, IScoreManager
    {
        private readonly CollisionsManager r_CollisionsManager;
        private readonly Dictionary<ICollidable, int> r_ScoresTrackerDict;

        public ScoreManager(Game i_Game) : base(i_Game, int.MaxValue)
        {
            r_ScoresTrackerDict = new Dictionary<ICollidable, int>();
            r_CollisionsManager = (CollisionsManager)Game.Services.GetService(typeof(ICollisionsManager));
            r_CollisionsManager.OnCollisionDetected += Collision_OnDetected;
        }

        #region IScoreManager interface implementation
        public void AddObjectToMonitor(ICollidable i_ScoringSprite)
        {
            bool v_IsFirstSeen = !isScoringSpriteContainedInMonitor(i_ScoringSprite);

            if (v_IsFirstSeen)
            {
                r_ScoresTrackerDict.Add(i_ScoringSprite, InitialScore);
            }
            else
            {
                string msg = "Object has already been added to scoring monitor.";
                throw new Exception(msg);
            }
        }

        public void RemoveObjectFromMonitor(ICollidable i_ScoringSprite)
        {
            handleNonMonitoredObjectException(i_ScoringSprite);
            r_ScoresTrackerDict.Remove(i_ScoringSprite);
        }

        /// <summary>
        /// Gets the current monitored sprite's score.
        /// </summary>
        /// <param name="i_ScoringSprite"></param>
        /// <returns></returns>
        public int Points(ICollidable i_ScoringSprite)
        {
            int currentScore;

            handleNonMonitoredObjectException(i_ScoringSprite);
            currentScore = r_ScoresTrackerDict[i_ScoringSprite];

            return currentScore;
        }
        #endregion IScoreManager interface implementation

        #region overriding inherited methods
        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(IScoreManager), this);
        }
        #endregion overriding inherited methods

        #region protected virtual methods & properties
        protected virtual void IncrementScore(ICollidable i_ScoringSprite, LivingEntity i_ScoredLivingSprite)
        {
            int gainedPoints = i_ScoredLivingSprite.LifeWorth;
            int currentPoints = r_ScoresTrackerDict[i_ScoringSprite];
            int newScore = gainedPoints + currentPoints;

            r_ScoresTrackerDict[i_ScoringSprite] = newScore < 0 ? 0 : newScore;
        }

        /// <summary>
        /// Checks whether exactly one is a bullet AND exactly one is a living sprite.
        /// If true: checks if the bullet's holder didnt hit itself.
        /// If true: checks whether the bullet's holder is monitored.
        /// If true: increments the bullet's holder score.
        /// </summary>
        /// <param name="i_FirstCollidableSprite"></param>
        /// <param name="i_SecondCollidableSprite"></param>
        protected virtual void Collision_OnDetected(ICollidable i_FirstCollidableSprite, ICollidable i_SecondCollidableSprite)
        {
            bool v_IsExactlyOneBullet = isExactlyOneBullet(i_FirstCollidableSprite, i_SecondCollidableSprite);
            bool v_IsExactlyOneLivingSprite = isExactlyOneLivingSprite(i_FirstCollidableSprite, i_SecondCollidableSprite);

            if (v_IsExactlyOneBullet && v_IsExactlyOneLivingSprite)
            {
                Bullet bullet;
                FightingEntity bulletHolder;
                LivingEntity scoredLivingSprite;

                if ((i_FirstCollidableSprite as Bullet) != null)
                {
                    bullet = i_FirstCollidableSprite as Bullet;
                    scoredLivingSprite = i_SecondCollidableSprite as LivingEntity;
                }
                else
                {
                    bullet = i_SecondCollidableSprite as Bullet;
                    scoredLivingSprite = i_FirstCollidableSprite as LivingEntity;
                }

                bulletHolder = bullet.WeaponHolder;
                if (bulletHolder is PlayerSpaceShip)
                {
                    handlePlayerSpaceShipGainingScore(bulletHolder as PlayerSpaceShip, scoredLivingSprite);
                }
                else if (scoredLivingSprite is PlayerSpaceShip && bulletHolder is Enemy)
                {
                    handlePlayerSpaceShipLoosingScore(bulletHolder as Enemy, scoredLivingSprite as PlayerSpaceShip);
                }
            }
        }

        protected virtual int InitialScore
        {
            get { return 0; }
        }
        #endregion protected virtual methods & properties

        #region private methods
        private bool isExactlyOneLivingSprite(ICollidable i_First, ICollidable i_Second)
        {
            bool v_IsFirstLivingSecondNot = (i_First as LivingEntity) != null && (i_Second as LivingEntity) == null;
            bool v_IsSecondLivingFirstNot = (i_Second as LivingEntity) != null && (i_First as LivingEntity) == null;

            return v_IsFirstLivingSecondNot || v_IsSecondLivingFirstNot;
        }

        private bool isExactlyOneBullet(ICollidable i_First, ICollidable i_Second)
        {
            bool v_IsFirstBulletSecondNot = (i_First as Bullet) != null && (i_Second as Bullet) == null;
            bool v_IsSecondBulletFirstNot = (i_Second as Bullet) != null && (i_First as Bullet) == null;

            return v_IsFirstBulletSecondNot || v_IsSecondBulletFirstNot;
        }

        private void handleNonMonitoredObjectException(ICollidable i_ScoringSprite)
        {
            bool v_IsScoringSpriteMonitored = isScoringSpriteContainedInMonitor(i_ScoringSprite);

            if (!v_IsScoringSpriteMonitored)
            {
                string msg = "Object has not been found in the scoring monitor.";
                throw new Exception(msg);
            }
        }

        private bool isScoringSpriteContainedInMonitor(ICollidable i_ScoringSprite)
        {
            return r_ScoresTrackerDict.ContainsKey(i_ScoringSprite);
        }

        /// <summary>
        /// The given params are exatcly one bullet and exactly one living sprite
        /// </summary>
        /// <param name="i_FirstCollidableSprite"></param>
        /// <param name="i_SecondCollidableSprite"></param>
        private void handlePlayerSpaceShipLoosingScore(Enemy i_BulletHolder, PlayerSpaceShip i_LoosingScoreSprite)
        {
            bool v_IsLoosingScoreSpriteMonitored = isScoringSpriteContainedInMonitor(i_LoosingScoreSprite);

            if (v_IsLoosingScoreSpriteMonitored)
            {
                IncrementScore(i_LoosingScoreSprite, i_LoosingScoreSprite);
            }
        }

        private void handlePlayerSpaceShipGainingScore(PlayerSpaceShip i_BulletHolder, LivingEntity i_ScoredLivingSprite)
        {
            bool v_IsBulletHolderMonitored = false;
            bool v_IsBulletHolderDidNotHitItsOwnType = false;
            
            v_IsBulletHolderDidNotHitItsOwnType = i_BulletHolder.GetType() != i_ScoredLivingSprite.GetType();
            if (v_IsBulletHolderDidNotHitItsOwnType)
            {
                v_IsBulletHolderMonitored = isScoringSpriteContainedInMonitor(i_BulletHolder);
                if (v_IsBulletHolderMonitored)
                {
                    IncrementScore(i_BulletHolder, i_ScoredLivingSprite);
                }
            }
        }
        #endregion private methods
    }
}