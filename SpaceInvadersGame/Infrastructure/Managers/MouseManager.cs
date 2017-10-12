using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Infrastructure.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.Managers
{
    public class MouseManager : GameService, IMouseManager
    {
        private MouseState m_CurrentMouseState;
        private MouseState m_PreviousMouseState;
        private int m_CurrentScrollWheelValue;
        private int m_PreviousScrollWheelValue;

        #region ctors
        public MouseManager(Game i_Game) : base(i_Game)
        {
        }
        #endregion ctors

        #region properties
        public bool IsMousePositionChanged
        {
            get { return m_CurrentMouseState != m_PreviousMouseState; }
        }

        public Point MousePosition
        {
            get { return m_CurrentMouseState.Position; }
        }

        public bool IsMouseVisible
        {
            get { return AppSettings.Instance.IsMouseVisible; }
            set { AppSettings.Instance.IsMouseVisible = value; }
        }

        public bool IsWheelScrolledUp
        {
            get { return m_CurrentScrollWheelValue > m_PreviousScrollWheelValue; }
        }

        public bool IsWheelScrolledDown
        {
            get { return m_CurrentScrollWheelValue < m_PreviousScrollWheelValue; }
        }
        #endregion properties

        #region public API
        /// <summary>
        /// Determines whether the specified bounding rectangle is hovered by the mouse cursor's.
        /// </summary>
        /// <param name="i_BoundingRectangle">The specified bounding rectangle is given in the following form: top left x,y position & width, height.</param>
        /// <returns></returns>
        public bool IsHoveredByMouse(Rectangle i_BoundingRectangle)
        {
            bool v_IsMousePlacedWithinRect =
                m_CurrentMouseState.X >= i_BoundingRectangle.X &&
                m_CurrentMouseState.X <= i_BoundingRectangle.X + i_BoundingRectangle.Width &&
                m_CurrentMouseState.Y >= i_BoundingRectangle.Y &&
                m_CurrentMouseState.Y <= i_BoundingRectangle.Y + i_BoundingRectangle.Height;

            return v_IsMousePlacedWithinRect;
        }

        /// <summary>
        /// Returns true if for this current frame, the specified mouse key was pressed on its previous state && is being pressed now.
        /// </summary>
        /// <param name="i_MouseKey">The specified mouse key.</param>
        /// <returns></returns>
        public bool IsMouseKeyHeld(eMouseKeys i_MouseKey)
        {
            bool v_IsKeyCurrentStatePressed = MouseKeyState(m_CurrentMouseState, i_MouseKey) == ButtonState.Pressed;
            bool v_IsKeyPreviousStatePressed = MouseKeyState(m_PreviousMouseState, i_MouseKey) == ButtonState.Pressed;

            return v_IsKeyPreviousStatePressed && v_IsKeyCurrentStatePressed;
        }

        /// <summary>
        /// Returns true if for this current frame, the specified mouse key was released on its previous state && is being pressed now.
        /// </summary>
        /// <param name="i_MouseKey">The specified mouse key.</param>
        /// <returns></returns>
        public bool IsMouseKeyPressed(eMouseKeys i_MouseKey)
        {
            bool v_IsKeyPreviousStateReleased = MouseKeyState(m_PreviousMouseState, i_MouseKey) == ButtonState.Released;
            bool v_IsKeyCurrentStatePressed = MouseKeyState(m_CurrentMouseState, i_MouseKey) == ButtonState.Pressed;

            return v_IsKeyPreviousStateReleased && v_IsKeyCurrentStatePressed;
        }

        /// <summary>
        /// Returns true if for this current frame, the specified mouse key was pressed on its previous state && is being released now.
        /// </summary>
        /// <param name="i_MouseKey">The specified mouse key.</param>
        public bool IsMouseKeyReleased(eMouseKeys i_MouseKey)
        {
            bool v_IsKeyCurrentStateReleased = MouseKeyState(m_CurrentMouseState, i_MouseKey) == ButtonState.Released;
            bool v_IsKeyPreviousStatePressed = MouseKeyState(m_PreviousMouseState, i_MouseKey) == ButtonState.Pressed;

            return v_IsKeyCurrentStateReleased && v_IsKeyPreviousStatePressed;
        }
        #endregion public API

        #region overriden methods
        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(IMouseManager), this);
        }
        #endregion overriden methods

        #region protected virtual methods
        /// <summary>
        /// Returns the mouse state of the specified mouse key according to the given MouseState reference param's data.
        /// </summary>
        /// <param name="i_MouseState"></param>
        /// <param name="i_MouseKey"></param>
        /// <returns></returns>
        protected virtual ButtonState MouseKeyState(MouseState i_MouseState, eMouseKeys i_MouseKey)
        {
            //// defining a mouse key default state:
            ButtonState mouseKeyState = ButtonState.Released;

            switch (i_MouseKey)
            {
                case eMouseKeys.Left:
                    mouseKeyState = i_MouseState.LeftButton;
                    break;

                case eMouseKeys.Right:
                    mouseKeyState = i_MouseState.RightButton;
                    break;

                case eMouseKeys.Scroller:
                    mouseKeyState = i_MouseState.MiddleButton;
                    break;
            }

            return mouseKeyState;
        }

        protected virtual void UpdateMouseKeysIndicatorsStates()
        {
            m_PreviousMouseState = m_CurrentMouseState;
            m_CurrentMouseState = Mouse.GetState();
        }

        protected virtual void UpdateScrollWheelIndicatorsStates()
        {
            m_CurrentScrollWheelValue = m_CurrentMouseState.ScrollWheelValue;
            m_PreviousScrollWheelValue = m_PreviousMouseState.ScrollWheelValue;
        }
        #endregion protected virtual methods

        #region XNA overriden methods
        public override void Initialize()
        {
            base.Initialize();
            m_CurrentMouseState = Mouse.GetState();
            m_CurrentScrollWheelValue = m_CurrentMouseState.ScrollWheelValue;
        }

        public override void Update(GameTime i_GameTime)
        {
            UpdateMouseKeysIndicatorsStates();
            UpdateScrollWheelIndicatorsStates();
            base.Update(i_GameTime);
        }
        #endregion XNA overriden methods
    }
}
