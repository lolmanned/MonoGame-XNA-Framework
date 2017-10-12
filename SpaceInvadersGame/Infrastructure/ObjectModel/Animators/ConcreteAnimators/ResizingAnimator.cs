using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class ResizingAnimator : SpriteAnimator
    {
        private readonly Vector2 r_OriginalScale;
        private readonly Vector2 r_DesignatedScale;

        public ResizingAnimator(string i_Name, TimeSpan i_AnimationLength, Vector2 i_OriginalScale, Vector2 i_DesignatedScale) : base(i_Name, i_AnimationLength)
        {
            r_OriginalScale = i_OriginalScale;
            r_DesignatedScale = i_DesignatedScale;
        }

        protected Vector2 ResizingVelocity
        {
            get
            {
                float x = (float)((r_OriginalScale.X - r_DesignatedScale.X) / AnimationLength.TotalSeconds);
                float y = (float)((r_OriginalScale.Y - r_DesignatedScale.Y) / AnimationLength.TotalSeconds);

                return new Vector2(x, y);
            }
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float elapsedTotalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            BoundSprite.Scales -= ResizingVelocity * elapsedTotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            BoundSprite.Scales = m_OriginalSpriteInfo.Scales;
        }
    }
}
