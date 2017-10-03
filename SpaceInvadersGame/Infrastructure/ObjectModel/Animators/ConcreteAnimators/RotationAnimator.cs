using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class RotationAnimator : SpriteAnimator
    {
        private readonly int r_RotationsPerSecond;

        public RotationAnimator(string i_Name, TimeSpan i_AnimationLength, int i_RotationsPerSecond) : base(i_Name, i_AnimationLength)
        {
            r_RotationsPerSecond = i_RotationsPerSecond;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float elapsedTimeInSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            BoundSprite.Rotation += MathHelper.TwoPi * r_RotationsPerSecond * elapsedTimeInSeconds;
        }

        protected override void RevertToOriginal()
        {
            BoundSprite.Rotation = m_OriginalSpriteInfo.Rotation;
        }
    }
}
