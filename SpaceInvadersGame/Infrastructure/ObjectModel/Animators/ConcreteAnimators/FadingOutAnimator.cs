using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class FadingOutAnimator : SpriteAnimator
    {
        private readonly float r_OriginalOpacity;
        private readonly float r_DesignatedOpacity;

        public FadingOutAnimator(string i_Name, TimeSpan i_AnimationLength, float i_OriginalOpacity, float i_DesignatedOpacity) : base(i_Name, i_AnimationLength)
        {
            r_OriginalOpacity = i_OriginalOpacity;
            r_DesignatedOpacity = i_DesignatedOpacity;
        }

        protected float OpacityToLowerPerSecond
        {
            get
            {
                float totalOpacityToLower = r_OriginalOpacity - r_DesignatedOpacity;
                float opacityToLowerPerSecond = totalOpacityToLower / (float)AnimationLength.TotalSeconds;

                return opacityToLowerPerSecond;
            }
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float elapsedTotalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            float opacityToLowerForThisFrame = OpacityToLowerPerSecond * elapsedTotalSeconds;

            BoundSprite.Opacity = Math.Max(0, BoundSprite.Opacity - opacityToLowerForThisFrame);
        }

        protected override void RevertToOriginal()
        {
            BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity;
        }
    }
}
