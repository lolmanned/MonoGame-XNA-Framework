using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Infrastructure.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Infrastructure.Managers
{
    public class SoundManager : GameService, ISoundManager
    {
        private readonly List<SoundEffectInstance> r_BGMusic;
        private readonly List<SoundEffectInstance> r_Sounds;

        public SoundManager(Game i_Game) : base(i_Game)
        {
            r_BGMusic = new List<SoundEffectInstance>();
            r_Sounds = new List<SoundEffectInstance>();
        }

        #region properties
        public int MinBackgroundMusicVol
        {
            get { return AppSettings.Instance.MinBackgroundMusicVol; }
        }

        public int MaxBackgroundMusicVol
        {
            get { return AppSettings.Instance.MaxBackgroundMusicVol; }
        }

        public int MinSoundsEffectsVol
        {
            get { return AppSettings.Instance.MinSoundsEffectsVol; }
        }

        public int MaxSoundsEffectsVol
        {
            get { return AppSettings.Instance.MaxSoundsEffectsVol; }
        }

        public float BackgroundMusicVolume
        {
            get
            {
                return AppSettings.Instance.BackgroundMusicVolume;
            }

            set
            {
                AppSettings.Instance.BackgroundMusicVolume = (int)value;
                OnBackgroundMusicVolumeChanged();
            }
        }

        public float SoundsEffectsVolume
        {
            get
            {
                return AppSettings.Instance.SoundsEffectsVolume;
            }

            set
            {
                AppSettings.Instance.SoundsEffectsVolume = value;
                OnSoundsEffectsVolumeChanged();
            }
        }

        public float DecimalSoundsEffectsVolume
        {
            get { return AppSettings.Instance.SoundsEffectsVolume / 100; }
        }

        public float DecimalBackgroundMusicVolume
        {
            get { return (float)AppSettings.Instance.BackgroundMusicVolume / 100; }
        }

        public bool IsSoundOn
        {
            get { return AppSettings.Instance.IsSoundOn; }
        }
        #endregion properties

        #region private methods
        private SoundEffect loadSoundEffect(string i_AssetName)
        {
            return Game.Content.Load<SoundEffect>(i_AssetName);
        }
        #endregion private methods

        #region overriden methods
        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(ISoundManager), this);
        }
        #endregion overriden methods

        #region public methods / public API
        public void PlaySound(string i_AssetName, float i_Pitch = 0, float i_Pan = 0)
        {
            if (SoundsEffectsVolume > 0 && IsSoundOn)
            {
                SoundEffect soundEffect = loadSoundEffect(i_AssetName);
                soundEffect.Play(DecimalSoundsEffectsVolume, i_Pitch, i_Pan);
            }
        }

        public SoundEffectInstance PlaySoundInLoop(string i_AssetName, float i_Pitch = 0, float i_Pan = 0)
        {
            SoundEffectInstance soundEffectInstance = loadSoundEffect(i_AssetName).CreateInstance();
            soundEffectInstance.Volume = DecimalBackgroundMusicVolume;
            soundEffectInstance.IsLooped = true;
            soundEffectInstance.Play();
            r_BGMusic.Add(soundEffectInstance);

            return soundEffectInstance;
        }

        public void ToggleSound()
        {
            AppSettings.Instance.IsSoundOn = !AppSettings.Instance.IsSoundOn;
            OnSoundToggled();
        }
        #endregion public methods

        #region protected virtual methods
        protected virtual void OnBackgroundMusicVolumeChanged()
        {
            foreach (SoundEffectInstance backgroundMusicInstance in r_BGMusic)
            {
                backgroundMusicInstance.Volume = DecimalBackgroundMusicVolume;
            }
        }

        protected virtual void OnSoundsEffectsVolumeChanged()
        {
            foreach (SoundEffectInstance soundEffectInstance in r_Sounds)
            {
                soundEffectInstance.Volume = DecimalSoundsEffectsVolume;
            }
        }

        protected virtual void OnSoundToggled()
        {
            foreach (SoundEffectInstance soundBgMusicInstance in r_BGMusic)
            {
                soundBgMusicInstance.Volume = IsSoundOn ? DecimalBackgroundMusicVolume : 0;
            }

            foreach (SoundEffectInstance soundEffectInstance in r_Sounds)
            {
                soundEffectInstance.Volume = IsSoundOn ? SoundsEffectsVolume : 0;
            }
        }
        #endregion protected virtual methods

        #region XNA overriden methods
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
        }
        #endregion XNA overriden methods
    }
}
