using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.GameData;
using SpaceInvaders.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders.Sprites
{
    public delegate void AllEnemiesAreDeadEventHandler();
    public delegate void BottomReachedEventHandler();

    public class EnemiesMatrix : GameComponent
    {
        public enum eJumpingDirection
        {
            Left = 0,
            Right = 1
        }

        private struct MatrixPosition
        {
            private readonly int r_Row;
            private readonly int r_Col;

            public MatrixPosition(int i_Row, int i_Col)
            {
                r_Row = i_Row;
                r_Col = i_Col;
            }

            public int Row
            {
                get { return r_Row; }
            }

            public int Col
            {
                get { return r_Col; }
            }
        }

        private const int k_NumOfDeadEnemiesToSpeedUp = 5;
        private const double k_PercentageToShortenAfterWallCollision = 0.07;
        private const double k_PercentageToShortenAfterXEnemiesAreDead = 0.05;
        private readonly Enemy[,] r_EnemiesMatrix;
        private readonly MatrixPosition r_EnemiesMatrixSize;
        private readonly Color r_PinkEnemyColor;
        private readonly Color r_BlueEnemyColor;
        private readonly Color r_YellowEnemyColor;
        private readonly Game r_Game;
        private int m_TotalAliveEnemies;
        private int m_DeadEnemiesSpeedingUpIndicator = 0;
        private eJumpingDirection m_JumpingDirection = eJumpingDirection.Right;
        private eJumpingDirection m_PrevJumpingDirection = eJumpingDirection.Right;
        public float BottomCoordY { get; set; }

        public event BottomReachedEventHandler OnBottomReached;

        public event AllEnemiesAreDeadEventHandler OnAllEnemiesAreDead;
        /// <summary>
        /// Constructs a new EnemiesMatrix in the specified size.
        /// </summary>
        /// <param name="i_NumOfRows"></param>
        /// <param name="i_NumOfCols"></param>
        public EnemiesMatrix(int i_NumOfRows, int i_NumOfCols, Game i_Game) : base(i_Game)
        {
            Game.Components.Add(this);
            r_EnemiesMatrix = new Enemy[i_NumOfRows, i_NumOfCols];
            r_EnemiesMatrixSize = new MatrixPosition(i_NumOfRows, i_NumOfCols);
            r_PinkEnemyColor = Color.LightPink;
            r_BlueEnemyColor = Color.LightBlue;
            r_YellowEnemyColor = Color.LightYellow;
            r_Game = i_Game;
            m_TotalAliveEnemies = r_EnemiesMatrixSize.Row * r_EnemiesMatrixSize.Col;
            UpdateOrder = int.MaxValue;
        }

        #region class properties

        public eJumpingDirection OppositeJumpingDirection
        {
            get { return 1 - m_JumpingDirection; }
        }

        public eJumpingDirection JumpingDirection
        {
            get { return m_JumpingDirection; }
        }
        #endregion class properties

        #region methods
        public virtual void InitEnemiesPositions()
        {
            int offsetFromCeiling = 3;
            int initialX = r_Game.GraphicsDevice.Viewport.X + 1;
            int x = initialX;
            int initialY = r_Game.GraphicsDevice.Viewport.Y;
            int y = initialY;
            int yOffset = 0;
            double enemiesSpaceOffset = 32 * 0.6;

            for (int i = 0; i < r_EnemiesMatrixSize.Row; i++)
            {
                for (int j = 0; j < r_EnemiesMatrixSize.Col; j++)
                {
                    Enemy currentEnemy = r_EnemiesMatrix[i, j];

                    y = yOffset + currentEnemy.Texture.Height * offsetFromCeiling;
                    currentEnemy.Position = new Vector2(x, y);
                    if (j == r_EnemiesMatrixSize.Col - 1)
                    {
                        yOffset += (int)Math.Round(currentEnemy.Texture.Height + enemiesSpaceOffset);
                    }

                    x += (int)Math.Round(currentEnemy.Texture.Width + enemiesSpaceOffset);
                }

                x = initialX;
            }
        }

        /// <summary>
        /// Starts cheking from the last row by running on the cols. this routine skips invisible enemies.
        /// </summary>
        /// <returns></returns>
        private bool isBottomReached()
        {
            bool v_IsBottomReached = false;

            for (int j = r_EnemiesMatrixSize.Col - 1; j >= 0; j--)
            {
                for (int i = r_EnemiesMatrixSize.Row - 1; i >= 0; i--)
                {
                    Enemy currentEnemy = r_EnemiesMatrix[i, j];

                    if (currentEnemy.Visible)
                    {
                        v_IsBottomReached = currentEnemy.Position.Y + currentEnemy.Height >= BottomCoordY;
                        if (v_IsBottomReached)
                        {
                            i = -1;
                            j = -1;
                        }
                    }
                }
            }

            return v_IsBottomReached;
        }

        /// <summary>
        /// Initializes EnemiesMatrix[i_Row, i_Col] with the specified enemy.
        /// Throws excpetion if the specified i_Row or i_Col are out of range.
        /// </summary>
        /// <param name="i_Enemy">The enemy to be stored.</param>
        /// <param name="i_Row"></param>
        /// <param name="i_Col"></param>
        private void initializeEnemy(Enemy i_Enemy, int i_Row, int i_Col)
        {
            isInMatrixSizeRange(i_Row, i_Col);
            r_EnemiesMatrix[i_Row, i_Col] = i_Enemy;
        }

        private void isInMatrixSizeRange(int i_Row, int i_Col)
        {
            int minPossibleIndex = 0;

            if (!ValidityHelper.IsInRange(i_Row, r_EnemiesMatrixSize.Row, minPossibleIndex))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (!ValidityHelper.IsInRange(i_Col, r_EnemiesMatrixSize.Col, minPossibleIndex))
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        #endregion private methods

        protected virtual void Enemy_OnDead()
        {
            m_TotalAliveEnemies--;
            m_DeadEnemiesSpeedingUpIndicator++;
        }

        protected virtual float EnemyJumpDownOffset(Enemy i_Enemy)
        {
            return i_Enemy.Height / 2;
        }

        /// <summary>
        /// Returns the enemy row or -1 if enemy has not been found.
        /// </summary>
        /// <param name="i_Enemy"></param>
        /// <returns></returns>
        public int GetEnemyRow(Enemy i_Enemy)
        {
            MatrixPosition enemyPosition = getEnemyPosition(i_Enemy);

            return enemyPosition.Row;
        }

        public int GetEnemyCol(Enemy i_Enemy)
        {
            MatrixPosition enemyPosition = getEnemyPosition(i_Enemy);

            return enemyPosition.Col;
        }

        private MatrixPosition getEnemyPosition(Enemy i_Enemy)
        {
            MatrixPosition foundEnemyPosition = new MatrixPosition(-1, -1);

            for (int i = 0; i < r_EnemiesMatrixSize.Row; i++)
            {
                for (int j = 0; j < r_EnemiesMatrixSize.Col; j++)
                {
                    if (r_EnemiesMatrix[i, j] != null)
                    {
                        if (r_EnemiesMatrix[i, j].Equals(i_Enemy))
                        {
                            foundEnemyPosition = new MatrixPosition(i, j);
                            i = r_EnemiesMatrixSize.Row;
                            j = r_EnemiesMatrixSize.Col;
                        }
                    }
                }
            }

            return foundEnemyPosition;
        }

        private void handleWallsHit(object sender)
        {
            m_JumpingDirection = (eJumpingDirection)(1 - (int)m_JumpingDirection);
            foreach (Enemy enemy in r_EnemiesMatrix)
            {
                enemy.Velocity *= -1;
            }
        }

        /// <summary>
        /// Returns an enemy in a specified position saved in the EnemiesMatrix.
        /// </summary>
        /// <param name="i_Row"></param>
        /// <param name="i_Col"></param>
        /// <returns>The specified enemy in EnemiesMatrix[i_Row, i_Col].</returns>
        public Enemy EnemyInPosition(int i_Row, int i_Col)
        {
            isInMatrixSizeRange(i_Row, i_Col);

            return r_EnemiesMatrix[i_Row, i_Col];
        }

        public void KillEnemy(Enemy i_Enemy)
        {
            MatrixPosition enemyPosition = getEnemyPosition(i_Enemy);

            if (enemyPosition.Row != -1 && enemyPosition.Col != -1)
            {
                bool v_IsEnemyDead;
                Enemy enemy = r_EnemiesMatrix[enemyPosition.Row, enemyPosition.Col];

                enemy.LooseSoul();
                v_IsEnemyDead = !enemy.IsAlive;
                m_TotalAliveEnemies -= v_IsEnemyDead ? 1 : 0;
            }
        }

        public void UpdateAllEnemiesHeight(float i_ValueToAddToHeight)
        {
            foreach (Enemy enemy in r_EnemiesMatrix)
            {
                enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y + i_ValueToAddToHeight);
            }
        }

        protected virtual void ChangeAllEnemiesHeightOnScreen()
        {
            foreach (Enemy enemy in r_EnemiesMatrix)
            {
                enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y + EnemyJumpDownOffset(enemy));
            }
        }

        protected virtual void SpeedUpAllEnemiesJumpTime(double i_PercentageToShorten)
        {
            foreach (Enemy enemy in r_EnemiesMatrix)
            {
                enemy.JumpTimeInSeconds -= enemy.JumpTimeInSeconds * i_PercentageToShorten;
            }
        }

        protected virtual void AlternateAllEnemiesTextures()
        {
            foreach (Enemy enemy in r_EnemiesMatrix)
            {
                enemy.AlternateTexture();
            }
        }

        private void changeAllEnemiesJumpingDirection()
        {
            foreach (Enemy enemy in r_EnemiesMatrix)
            {
                enemy.Velocity *= -1;
            }
        }

        private bool isEnemyAboutToHitWall()
        {
            bool v_IsAnyWallDetected = false;

            foreach (Enemy enemy in r_EnemiesMatrix)
            {
                bool v_IsRightWallDetected = enemy.Position.X + enemy.Velocity.X >= Game.GraphicsDevice.Viewport.Width - enemy.Width;
                bool v_IsLeftWallDetected = enemy.Position.X + enemy.Velocity.X <= Game.GraphicsDevice.Viewport.X;

                v_IsAnyWallDetected = v_IsRightWallDetected || v_IsLeftWallDetected;
                if (v_IsAnyWallDetected)
                {
                    break;
                }
            }

            return v_IsAnyWallDetected;
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Enemy enemy in r_EnemiesMatrix)
            {
                yield return enemy;
            }
        }

        #region framework methods names

        private float getPixelsToMoveOnXAxis(Enemy i_Enemy)
        {
            bool v_IsMovingRight = m_JumpingDirection == eJumpingDirection.Right;
            float x;

            if (v_IsMovingRight)
            {
                x = (Game.GraphicsDevice.Viewport.Width - i_Enemy.Position.X);
            }
            else
            {
                x = i_Enemy.Position.X - Game.GraphicsDevice.Viewport.X;
            }

            return x;
        }

        private void moveAllEnemiesByPixels(float i_AddToXAxis, float i_AddToYAxis)
        {
            foreach (Enemy enemy in r_EnemiesMatrix)
            {
                enemy.Position = new Vector2(enemy.Position.X + i_AddToXAxis, enemy.Position.Y + i_AddToYAxis);
            }
        }

        public override void Initialize()
        {
            BottomCoordY = Game.GraphicsDevice.Viewport.Height;
            Enemy currentEnemy;

            for (int i = 0; i < r_EnemiesMatrixSize.Row; i++)
            {
                for (int j = 0; j < r_EnemiesMatrixSize.Col; j++)
                {
                    currentEnemy = EnemiesFactory.CreateEnemy(Game, i);
                    currentEnemy.OnDead += Enemy_OnDead;
                    initializeEnemy(currentEnemy, i, j);
                }
            }
        }

        private void Enemy_OnDead(Enemy i_Enemy)
        {
            m_TotalAliveEnemies--;
            m_DeadEnemiesSpeedingUpIndicator++;
        }

        private float getPixelsToMoveRight()
        {
            float pixelsToMove = r_EnemiesMatrix[0, 0].Velocity.X;
            bool v_IsAboutToHitTheWall = false;
            float clampedX;
            Viewport viewport = Game.GraphicsDevice.Viewport;

            for (int j = r_EnemiesMatrixSize.Col - 1; j >= 0; j--)
            {
                for (int i = r_EnemiesMatrixSize.Row - 1; i >= 0; i--)
                {
                    Enemy enemy = r_EnemiesMatrix[i, j];

                    if (enemy.Visible)
                    {
                        v_IsAboutToHitTheWall = enemy.Velocity.X + enemy.Position.X >= viewport.Width - enemy.Width;
                        if (v_IsAboutToHitTheWall)
                        {
                            clampedX = MathHelper.Clamp(enemy.Velocity.X + enemy.Position.X, viewport.X, viewport.Width - enemy.Width);
                            pixelsToMove = clampedX - enemy.Position.X;
                            i = -1;
                            j = -1;
                        }
                    }
                }
            }

            return pixelsToMove;
        }

        private float getPixelsToMoveLeft()
        {
            float clampedX;
            float pixelsToMove = r_EnemiesMatrix[0, 0].Velocity.X;
            bool v_IsAboutToHitTheWall = false;
            Viewport viewport = Game.GraphicsDevice.Viewport;

            for (int j = 0; j < r_EnemiesMatrixSize.Col; j++)
            {
                for (int i = r_EnemiesMatrixSize.Row - 1; i >= 0; i--)
                {
                    Enemy enemy = r_EnemiesMatrix[i, j];

                    if (enemy.Visible)
                    {
                        v_IsAboutToHitTheWall = enemy.Velocity.X + enemy.Position.X <= viewport.X;
                        if (v_IsAboutToHitTheWall)
                        {
                            clampedX = MathHelper.Clamp(enemy.Velocity.X + enemy.Position.X, viewport.X, viewport.Width - enemy.Width);
                            pixelsToMove = clampedX - enemy.Position.X;
                            i = -1;
                            j = r_EnemiesMatrixSize.Col;
                        }
                    }
                }
            }

            return pixelsToMove;
        }

        public override void Update(GameTime i_GameTime)
        {
            float xToAdd;
            if (m_PrevJumpingDirection != m_JumpingDirection)
            {
                m_PrevJumpingDirection = m_JumpingDirection;
                changeAllEnemiesJumpingDirection();
            }

            xToAdd = m_JumpingDirection == eJumpingDirection.Right ? getPixelsToMoveRight() : getPixelsToMoveLeft();
            foreach (Enemy enemy in r_EnemiesMatrix)
            {
                if (enemy.IsItTimeToJump)
                {
                    bool v_IsHorizontalWallDetectedWithinJumpingDirection = enemy.IsRightWallDeteced && m_JumpingDirection == eJumpingDirection.Right || enemy.IsLeftWallDetected && m_JumpingDirection == eJumpingDirection.Left;

                    if (v_IsHorizontalWallDetectedWithinJumpingDirection && enemy.Visible)
                    {
                        ChangeAllEnemiesHeightOnScreen();
                        SpeedUpAllEnemiesJumpTime(k_PercentageToShortenAfterWallCollision);
                        m_JumpingDirection = OppositeJumpingDirection;
                        break;
                    }
                    else
                    {
                        enemy.Position = new Vector2(enemy.Position.X + xToAdd, enemy.Position.Y);
                    }
                }
            }

            if (m_DeadEnemiesSpeedingUpIndicator >= k_NumOfDeadEnemiesToSpeedUp)
            {
                m_DeadEnemiesSpeedingUpIndicator = 0;
                SpeedUpAllEnemiesJumpTime(k_PercentageToShortenAfterXEnemiesAreDead);
            }

            if (isBottomReached() && OnBottomReached != null)
            {
                OnBottomReached.Invoke();
            }

            bool v_IsAllEnemiesDead = m_TotalAliveEnemies == 0;
            if (v_IsAllEnemiesDead && OnAllEnemiesAreDead != null)
            {
                OnAllEnemiesAreDead.Invoke();
            }
        }
        #endregion framework methods names
    }
}
