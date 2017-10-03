using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Pong : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D m_Background;

        private Ball m_Ball;
        private Paddle m_LeftPaddle;
        private Paddle m_RightPaddle;

        private readonly ScoreManager r_LeftPaddleScore;
        private readonly ScoreManager r_RightPaddleScore;

        private readonly InputHandler r_GameKeys;
        private readonly List<Sprite> r_SpritesList;

        private SpriteFont m_WinningMsgSpriteFont;

        private bool m_IsRoundStarted;

        private const int k_MaxPointsForWinning = 7;

        public Pong()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            r_SpritesList = new List<Sprite>();
            r_LeftPaddleScore = new ScoreManager(Color.White);
            r_RightPaddleScore = new ScoreManager(Color.White);
            r_GameKeys = new InputHandler(new Dictionary<string, Keys>()
            {
                    { "StartKey", Keys.Enter },
                    { "LeftPaddleUpKey", Keys.W },
                    { "LeftPaddleDownKey", Keys.S },
                    { "RightPaddleUpKey", Keys.Up },
                    { "RightPaddleDownKey", Keys.Down },
            });
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
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width - (GraphicsDevice.DisplayMode.Width / 4);
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height - (int)(GraphicsDevice.DisplayMode.Height / 8.5);
            graphics.ApplyChanges();
            IsMouseVisible = true;
            InitPaddlesScoresDrawingPositions();

            base.Initialize();
        }

        private void InitPaddlesScoresDrawingPositions()
        {
            int quarterViewportWidth = graphics.GraphicsDevice.Viewport.Width / 4;
            int eightsViewportHeight = graphics.GraphicsDevice.Viewport.Height / 8;
            int x = graphics.GraphicsDevice.Viewport.X + quarterViewportWidth;
            int y = graphics.GraphicsDevice.Viewport.Y + eightsViewportHeight;

            r_LeftPaddleScore.DrawingPosition = new Vector2(x, y);
            x = graphics.GraphicsDevice.Viewport.Width - quarterViewportWidth;
            r_RightPaddleScore.DrawingPosition = new Vector2(x, y);
        }

        private void Restart()
        {
            r_SpritesList.Clear();
            r_LeftPaddleScore.ResetScore();
            r_RightPaddleScore.ResetScore();
            m_IsRoundStarted = false;
        }

        private void Reset()
        {
            Viewport viewport = graphics.GraphicsDevice.Viewport;
            Sprite paddleSpriteBehavior = new Sprite(Content.Load<Texture2D>(@"Sprites\Paddle"));
            Sprite ballSpriteBehavior = new Sprite(Content.Load<Texture2D>(@"Sprites\Ball"));
            int yCoordToCenterPaddles = viewport.Height / 2 - paddleSpriteBehavior.Texture.Height / 2;

            r_SpritesList.Clear();
            m_Background = Content.Load<Texture2D>(@"Sprites\Background");

            ballSpriteBehavior.Position = new Vector2(viewport.Width / 2, viewport.Height / 2);
            ballSpriteBehavior.Speed = 10f;
            ballSpriteBehavior.Color = Color.White;
            m_Ball = new Ball(ballSpriteBehavior, 0.3f);

            paddleSpriteBehavior.Position = new Vector2(viewport.X, yCoordToCenterPaddles);
            paddleSpriteBehavior.Speed = 10f;
            paddleSpriteBehavior.Color = Color.White;

            m_LeftPaddle = new Paddle(r_GameKeys.Key("LeftPaddleUpKey"), r_GameKeys.Key("LeftPaddleDownKey"), paddleSpriteBehavior);
            paddleSpriteBehavior.Position = new Vector2(viewport.Width - paddleSpriteBehavior.Texture.Width, yCoordToCenterPaddles);
            m_RightPaddle = new Paddle(r_GameKeys.Key("RightPaddleUpKey"), r_GameKeys.Key("RightPaddleDownKey"), paddleSpriteBehavior);

            r_SpritesList.Add(m_LeftPaddle);
            r_SpritesList.Add(m_RightPaddle);
            r_SpritesList.Add(m_Ball);


            m_IsRoundStarted = Keyboard.GetState().IsKeyDown(r_GameKeys.Key("StartKey"));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            r_LeftPaddleScore.Font = Content.Load<SpriteFont>(@"ScoreFont");
            r_RightPaddleScore.Font = Content.Load<SpriteFont>(@"ScoreFont");
            m_WinningMsgSpriteFont = Content.Load<SpriteFont>(@"WinningFont");
            Reset();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void HandleWinnerPaddleScore()
        {
            bool v_IsLeftPaddleScored = m_Ball.Position.X > graphics.GraphicsDevice.Viewport.Width / 2;

            if (v_IsLeftPaddleScored)
            {
                r_LeftPaddleScore.IncrementScore();
            }
            else
            {
                r_RightPaddleScore.IncrementScore();
            }
        }

        public bool IsGameOver()
        {
            return r_RightPaddleScore.Score == k_MaxPointsForWinning || r_LeftPaddleScore.Score == k_MaxPointsForWinning;
        }

        private void DeclareWinner()
        {
            string winningMsg = r_LeftPaddleScore.Score > r_RightPaddleScore.Score ? "Left" : "Right";
            Vector2 winningMsgSizeVector;
            Vector2 winningMsgDrawingPosition;
            float xCoordOfCenter = graphics.GraphicsDevice.Viewport.Width / 2;
            float yCoordOfCenter = graphics.GraphicsDevice.Viewport.Height / 2;

            winningMsg += " Paddle Wins !";
            winningMsgSizeVector = m_WinningMsgSpriteFont.MeasureString(winningMsg);
            winningMsgDrawingPosition = new Vector2(xCoordOfCenter - winningMsgSizeVector.X / 2, yCoordOfCenter);
            spriteBatch.DrawString(m_WinningMsgSpriteFont, winningMsg, winningMsgDrawingPosition, Color.BlueViolet);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // original code of Xna framwork.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (IsGameOver())
            {
                //DeclareWinner();
                if (Keyboard.GetState().IsKeyDown(r_GameKeys.Key("StartKey")))
                {
                    Restart();
                }
            }
            if (m_Ball.IsGoal)
            {
                HandleWinnerPaddleScore();
            }
            if (!m_IsRoundStarted || m_Ball.IsGoal)
            {
                Reset();
            }
            else
            {
                foreach (Sprite sprite in r_SpritesList)
                {
                    sprite.Update(gameTime, graphics.GraphicsDevice, r_SpritesList);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(m_Background, graphics.GraphicsDevice.Viewport.Bounds, Color.White);
            foreach (Sprite sprite in r_SpritesList)
            {
                sprite.Draw(spriteBatch);
            }
            
            r_LeftPaddleScore.Draw(spriteBatch);
            r_RightPaddleScore.Draw(spriteBatch);
            if (IsGameOver())
            {
                DeclareWinner();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
