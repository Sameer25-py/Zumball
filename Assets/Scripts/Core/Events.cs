using UnityEngine;
using UnityEngine.Events;

namespace Zumball.Core
{
    public static class Events
    {
        public static class GamePlayEvents
        {
            public static readonly UnityEvent                         CannonBallPoolGenerated = new();
            public static readonly UnityEvent                         SplineBallPoolGenerated = new();
            public static readonly UnityEvent<GameObject>             BallExpired             = new();
            public static readonly UnityEvent<GameObject, GameObject> CannonBallHit           = new();
            public static readonly UnityEvent                         GameEnded               = new();
            public static readonly UnityEvent<string, int>            AddScore                = new();
        }

        public static class GameManagerEvents
        {
            public static readonly UnityEvent<int> SkinApplied = new();
            public static readonly UnityEvent<string,int> UpdateScore = new();
        }

        public static class SettingsEvents
        {
            public static readonly UnityEvent<int>                  ApplySkin      = new();
            public static readonly UnityEvent<GameManager.Language> ChangeLanguage = new();
            public static readonly UnityEvent                       ToggleAudio    = new();
        }

        public static class Navigation
        {
            public static readonly UnityEvent<int> ButtonPressed  = new();
            public static readonly UnityEvent      StartGame      = new();
            public static readonly UnityEvent      ShowScore      = new();
            public static readonly UnityEvent      ShowSkins      = new();
            public static readonly UnityEvent      ShowSettings   = new();
            public static readonly UnityEvent      BackToMainMenu = new();
        }
    }
}