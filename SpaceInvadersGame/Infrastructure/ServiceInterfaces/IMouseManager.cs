using Microsoft.Xna.Framework;

namespace Infrastructure.ServiceInterfaces
{
    public enum eMouseKeys
    {
        Left,
        Right,
        Scroller
    }

    public interface IMouseManager
    {
        Point MousePosition { get; }
        bool IsMouseVisible { get; }
        bool IsWheelScrolledUp { get; }
        bool IsWheelScrolledDown { get; }
        bool IsMouseKeyPressed(eMouseKeys i_MouseButton);
        bool IsMouseKeyReleased(eMouseKeys i_MouseButton);
        bool IsMouseKeyHeld(eMouseKeys i_MouseButton);
    }
}
