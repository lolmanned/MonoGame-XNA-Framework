using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Sprites
{
    public class BarriersLine : GameComponent, IEnumerable
    {
        private readonly int r_BarriersInitialCount;
        private readonly Barrier[] r_BarriersLine;
        private const int k_PixelsPerSecondPerBarrier = 60;

        public BarriersLine(int i_BarriersCount, Game i_Game) : base(i_Game)
        {
            if (i_BarriersCount <= 0)
            {
                throw new Exception("Barriers initial number must be a positive integer.");
            }

            Game.Components.Add(this);
            r_BarriersInitialCount = i_BarriersCount;
            r_BarriersLine = new Barrier[i_BarriersCount];
        }

        /// <summary>
        /// Each Barrier located above the PlayerSpaceShip. This method calculates the Y coordinate according to where each Barrier's bottom should be placed. 
        /// </summary>
        /// <returns>Y coordinate on the screen per Barrier in the BarriersLine.</returns>
        public virtual float HeightOnScreenPerBarrier
        {
            get
            {
                var querySpaceShips = from component in Game.Components where component is PlayerSpaceShip select component;
                List<PlayerSpaceShip> foundSpaceShips = new List<PlayerSpaceShip>();
                PlayerSpaceShip relevantPlayerSpaceShip = querySpaceShips.ElementAt(0) as PlayerSpaceShip;
                float heightOnScreenPerBarrier = relevantPlayerSpaceShip != null ? relevantPlayerSpaceShip.Bounds.Top - relevantPlayerSpaceShip.Height * 2 : Game.GraphicsDevice.Viewport.Height - r_BarriersLine[0].Height;

                return heightOnScreenPerBarrier;
            }
        }

        /// <summary>
        /// Returns the total length of this BarriersLine.
        /// </summary>
        public virtual float BarriersLineWidth
        {
            get { return r_BarriersLine[0].Width * r_BarriersInitialCount + BarriersOffest * (r_BarriersInitialCount - 1) / 2; }
        }

        /// <summary>
        /// Returns the starting Position of the LeftmostBarrier.
        /// </summary>
        public virtual Vector2 BarriersLinePosition
        {
            get { return LeftmostBarrier.Position; }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Barrier barrier in r_BarriersLine)
            {
                yield return barrier;
            }
        }

        protected virtual void InitBarriers()
        {
            for (int i = 0; i < r_BarriersInitialCount; i++)
            {
                r_BarriersLine[i] = new Barrier(Game);
            }
        }

        protected virtual void BarriersLine_OnFloatingDistanceReached(Barrier i_TargetBarrier)
        {
            foreach (Barrier barrier in r_BarriersLine)
            {
                barrier.ResetFloatingDistancePixelage();
                barrier.Velocity *= -1;
            }
        }

        public virtual Barrier RightmostBarrier
        {
            get { return r_BarriersLine[r_BarriersLine.Length - 1]; }
        }

        public virtual Barrier LeftmostBarrier
        {
            get { return r_BarriersLine[0]; }
        }

        protected virtual void CenterBarriersLine()
        {
            float xCoordScreenCenter = (Game.GraphicsDevice.Viewport.X + Game.GraphicsDevice.Viewport.Width) / 2;
            float xCoordBarriersLineCenter = (BarriersLinePosition.X + BarriersLineWidth) / 2;
            float x;
            float y = HeightOnScreenPerBarrier;

            foreach (Barrier barrier in r_BarriersLine)
            {
                x = barrier.Position.X + (xCoordScreenCenter - xCoordBarriersLineCenter);
                barrier.Position = new Vector2(x, y);
                x += BarriersOffest;
            }
        }

        /// <summary>
        /// Returns doubled Barrier's Width.
        /// </summary>
        public virtual float BarriersOffest
        {
            get { return r_BarriersLine[0].Width * 2; }
        }
        
        public virtual void InitBarriersPositions()
        {
            float x = Game.GraphicsDevice.Viewport.X;
            float y = HeightOnScreenPerBarrier;

            for (int i = 0; i < r_BarriersInitialCount; i++)
            {
                Barrier currentBarrier = r_BarriersLine[i];
                currentBarrier.Position = new Vector2(x, y);
                x += BarriersOffest;
            }

            CenterBarriersLine();
        }

        private void changeAllBarriersFloatingDirection()
        {
            foreach (Barrier barrier in r_BarriersLine)
            {
                barrier.Velocity *= -1;
            }
        }

        private void resetAllBarriersPixelage()
        {
            foreach (Barrier barrier in r_BarriersLine)
            {
                barrier.ResetFloatingDistancePixelage();
            }
        }

        public override void Initialize()
        {
            InitBarriers();
            base.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            foreach (Barrier barrier in r_BarriersLine)
            {
                if (barrier.IsFloatingDistanceReached)
                {
                    changeAllBarriersFloatingDirection();
                    resetAllBarriersPixelage();
                    break;
                }
            }

            base.Update(i_GameTime);
        }
    }
}
