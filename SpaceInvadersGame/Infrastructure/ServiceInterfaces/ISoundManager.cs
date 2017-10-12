using Microsoft.Xna.Framework.Audio;

namespace Infrastructure.ServiceInterfaces
{
    public interface ISoundManager
    {
        float BackgroundMusicVolume { get; }
        float DecimalBackgroundMusicVolume { get; }
        float SoundsEffectsVolume { get; }
        float DecimalSoundsEffectsVolume { get; }
        bool IsSoundOn { get; }
        void ToggleSound();
        void PlaySound(string i_AssetName, float i_Pitch = 0, float i_Pan = 0);
        SoundEffectInstance PlaySoundInLoop(string i_AssetName, float i_Pitch = 0, float i_Pan = 0);
    }
}
