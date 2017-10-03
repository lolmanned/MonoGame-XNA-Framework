using Infrastructure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.DrawableObjects;
using SpaceInvaders.GameData;
using SpaceInvaders.Managers;
using SpaceInvaders.Sprites;
using System;
using System.Text;

namespace SpaceInvaders
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SpaceInvaders : Game
    {
        private const int k_EnemiesMatrixRows = 5;
        private const int k_EnemiesMatrixCols = 9;
        private const int k_BarriersCount = 4;
        private readonly Background r_Background;
        private readonly InputManager r_InputManager;
        private readonly CollisionsManager r_CollisionManager;
        private readonly ScoreManager r_ScoreManager;
        private readonly EnemiesMatrix r_EnemiesMatrix;
        private readonly MotherShip r_MotherShip;
        private readonly PlayerSpaceShip r_PlayerOne;
        private readonly PlayerSpaceShip r_PlayerTwo;
        private readonly BarriersLine r_BarriersLine;
        private readonly PlayerData r_PlayerOneData;
        private readonly PlayerData r_PlayerTwoData;
        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private bool m_IsGameOver = false;

        public SpaceInvaders()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            //m_Graphics.IsFullScreen = true;
            //m_Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            r_Background = new Background(this);
            r_InputManager = new InputManager(this);
            r_CollisionManager = new CollisionsManager(this);
            r_ScoreManager = new ScoreManager(this);
            r_EnemiesMatrix = new EnemiesMatrix(k_EnemiesMatrixRows, k_EnemiesMatrixCols, this);
            r_MotherShip = new MotherShip(this);
            r_PlayerOne = new PlayerSpaceShip(this, SpritesAssetsNamesMapper.k_FirstShipAssetName);
            r_PlayerTwo = new PlayerSpaceShip(this, SpritesAssetsNamesMapper.k_SecondShipAssetName);
            r_BarriersLine = new BarriersLine(k_BarriersCount, this);
            r_PlayerOneData = new PlayerData(this, r_PlayerOne, Color.Blue);
            r_PlayerTwoData = new PlayerData(this, r_PlayerTwo, Color.Green);
            initEvents();
        }

        private void initEvents()
        {
            r_PlayerOne.OnPositionInit += playerOne_PositionInitialized;
            r_EnemiesMatrix.OnBottomReached += enemiesMatrix_GameOver;
            r_EnemiesMatrix.OnAllEnemiesAreDead += enemiesMatrix_GameOver;
        }    

        private Vector2 playerOneInitialPosition
        {
            get { return new Vector2(playerTwoInitialPosition.X + r_PlayerTwo.Width + 1, r_PlayerOne.Position.Y); }
        }

        private Vector2 playerTwoInitialPosition
        {
            get { return new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Height - r_PlayerTwo.Height); }
        }

        private void enemiesMatrix_GameOver()
        {
            m_IsGameOver = true;
        }

        private void playerOne_PositionInitialized(PlayerSpaceShip i_Player)
        {
            i_Player.Position = playerOneInitialPosition;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            r_PlayerOne.Position = playerOneInitialPosition;
            r_PlayerOne.IsMouseActive = true;
            r_PlayerOne.RightController = Keys.Right;
            r_PlayerOne.LeftController = Keys.Left;
            r_PlayerOne.KeyboardFireController = Keys.Up;
            r_PlayerTwo.RightController = Keys.G;
            r_PlayerTwo.LeftController = Keys.D;
            r_PlayerTwo.KeyboardFireController = Keys.R;

            //r_PlayerOne.Opacity /= 2;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new m_SpriteBatch, which can be used to draw textures.
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            r_EnemiesMatrix.InitEnemiesPositions();
            r_BarriersLine.InitBarriersPositions();
            r_PlayerOne.InitialPosition = playerOneInitialPosition;
            r_PlayerOne.Position = playerOneInitialPosition;
            r_EnemiesMatrix.BottomCoordY = r_PlayerOne.Position.Y - r_PlayerOne.Height;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private string getGameScores()
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine(string.Format("P1 Score: {0}", r_PlayerOne.Points));
            output.AppendLine(string.Format("P2 Score: {0}", r_PlayerTwo.Points));
            
            return output.ToString();
        }

        private string declareWinner()
        {
            bool v_IsPlayerOneWins = r_PlayerOne.Points > r_PlayerTwo.Points;
            bool v_IsPlayerTwoWins = r_PlayerTwo.Points > r_PlayerOne.Points;
            bool v_IsItATie = !v_IsPlayerOneWins && !v_IsPlayerTwoWins;
            string winnerIndex = v_IsPlayerOneWins ? "1" : "2";
            string tieMsg = "It's A Tie!";
            string gameResult;

            gameResult = v_IsItATie ? tieMsg : string.Format("Player {0} Wins!", winnerIndex);

            return gameResult;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here
            if (m_IsGameOver)
            {
                string msgBoxText = getGameScores();
                string title = declareWinner();

                if (System.Windows.Forms.MessageBox.Show(msgBoxText, title, System.Windows.Forms.MessageBoxButtons.OK) == System.Windows.Forms.DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }

            bool v_IsBothPlayersDead = r_PlayerOne.Souls == 0 && r_PlayerTwo.Souls == 0;

            m_IsGameOver = v_IsBothPlayersDead;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime i_GameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            base.Draw(i_GameTime);
        }
    }
}
