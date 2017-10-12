using Microsoft.Xna.Framework;

namespace Infrastructure.Settings
{
    public delegate void SettingsChangedEventHandler();

    public sealed class AppSettings
    {
        private static AppSettings s_AppSettingsInstance;
        private static int s_PlayersCount;
        private static int s_BackgroundMusicVolume;
        private static float s_SoundsEffectsVolume;
        private static bool s_IsFullScreenOn;

        public event SettingsChangedEventHandler OnFullScreenChanged;

        #region ctors
        /// <summary>
        /// Constructs a default settings singleton instance. 
        /// </summary>
        private AppSettings()
        {
            IsSoundOn = true;
            IsMouseVisible = true;
            IsResizingAllowed = false;
            s_IsFullScreenOn = false;
            s_PlayersCount = 1;
            s_SoundsEffectsVolume = 20;
            s_BackgroundMusicVolume = 40;
        }
        #endregion ctors

        #region properties
        public int MaxPlayersCount
        {
            get { return 2; }
        }

        public int MinPlayersCount
        {
            get { return 1; }
        }

        public int MinBackgroundMusicVol
        {
            get { return 0; }
        }

        public int MaxBackgroundMusicVol
        {
            get { return 100; }
        }

        public int MinSoundsEffectsVol
        {
            get { return 0; }
        }

        public int MaxSoundsEffectsVol
        {
            get { return 100; }
        }

        public int PlayersCount
        {
            get
            {
                return s_PlayersCount;
            }

            set
            {
                s_PlayersCount = value == MinPlayersCount - 1 ? MaxPlayersCount : value;
                s_PlayersCount = value == MaxPlayersCount + 1 ? MinPlayersCount : s_PlayersCount;
            }
        }

        public int BackgroundMusicVolume
        {
            get { return s_BackgroundMusicVolume; }
            set { s_BackgroundMusicVolume = MathHelper.Clamp(value, MinBackgroundMusicVol, MaxBackgroundMusicVol); }
        }

        public float SoundsEffectsVolume
        {
            get { return s_SoundsEffectsVolume; }
            set { s_SoundsEffectsVolume = MathHelper.Clamp(value, MinSoundsEffectsVol, MaxSoundsEffectsVol); }
        }

        public bool IsMouseVisible { get; set; }
        public bool IsResizingAllowed { get; set; }
        public bool IsSoundOn { get; set; }
        public bool IsFullScreenOn
        {
            get
            {
                return s_IsFullScreenOn;
            }

            set
            {
                s_IsFullScreenOn = value;
                if (OnFullScreenChanged != null)
                {
                    OnFullScreenChanged.Invoke();
                }
            }
        }
        #endregion properties

        /// <summary>
        /// Returns the singelton AppSettings instance.
        /// </summary>
        public static AppSettings Instance
        {
            get
            {
                if (s_AppSettingsInstance == null)
                {
                    s_AppSettingsInstance = new AppSettings();
                }

                return s_AppSettingsInstance;
            }
        }
    }
}
