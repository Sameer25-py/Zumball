using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zumball.GamePlay;
using Zumball.UI;
using static Zumball.Core.Events.GamePlayEvents;
using static Zumball.Core.Events.Navigation;
using static Zumball.Core.Events.SettingsEvents;
using static Zumball.Core.Events.GameManagerEvents;

namespace Zumball.Core
{
    public class GameManager : MonoBehaviour
    {
        public Language                ActiveLanguage      = Language.English;
        public int                     BackgroundIndex     = 4;
        public Dictionary<string, int> Score               = new();
        public bool                    IsBallPoolGenerated = false;

        public GameObject        EndTransition;
        public GameObject        Environment;
        public GameObject        BackGround;
        public GameObject        UI;
        public GameObject        GameUI;
        public GameObject        MainMenuObject;
        public GameObject        ScoreObject;
        public GameObject        SettingsObject;
        public GameObject        SkinsObject;
        public SplineTrain       splineTrain;
        public List<MultiText>   MultiTextObjects;
        public List<Score>       ScoreObjects;
        public List<AudioSource> AudioSources;

        private delegate void LastOperation();

        private LastOperation _lastOp;

        [Serializable]
        public enum Language
        {
            English,
            Russian
        }

        private void Start()
        {
            MainMenuShow();
        }

        private void MainMenuShow()
        {   
            Environment.SetActive(false);
            SkinsObject.SetActive(false);
            SettingsObject.SetActive(false);
            ScoreObject.SetActive(false);
            GameUI.SetActive(false);
            splineTrain.StopGame();
            MainMenuObject.SetActive(true);

            _lastOp = null;
        }

        private void ScoreShow()
        {
            MainMenuObject.SetActive(false);
            SkinsObject.SetActive(false);
            SettingsObject.SetActive(false);
            GameUI.SetActive(false);
            ScoreObject.SetActive(true);
            
            _lastOp = MainMenuShow;
        }

        private void SkinsShow()
        {
            MainMenuObject.SetActive(false);
            SettingsObject.SetActive(false);
            GameUI.SetActive(false);
            ScoreObject.SetActive(false);
            SkinsObject.SetActive(true);

            _lastOp = MainMenuShow;
        }

        private void SettingsShow()
        {
            MainMenuObject.SetActive(false);
            SkinsObject.SetActive(false);
            GameUI.SetActive(false);
            ScoreObject.SetActive(false);
            SettingsObject.SetActive(true);
            
            _lastOp = MainMenuShow;
        }

        private void GameStart()
        {
            MainMenuObject.SetActive(false);
            SkinsObject.SetActive(false);
            SettingsObject.SetActive(false);
            ScoreObject.SetActive(false);
            GameUI.SetActive(true);
            Environment.SetActive(true);
            splineTrain.StartGame();

            _lastOp = MainMenuShow;
        }

        private void OnBackToMainMenuPressed()
        {
            if (_lastOp != null)
            {
                _lastOp();
            }
        }
        
        private void RestartGame()
        {
            EndTransition.SetActive(true);
            Invoke(nameof(GameStart), 2f);
        }

        private void OnGameEnded()
        {
            RestartGame();
        }

        private void OnAddScoreCalled(string ballName, int matchedBalls)
        {
            if (Score.TryGetValue(ballName, out int _))
            {
                Score[ballName] += matchedBalls;
            }
            else
            {
                Score.Add(ballName, matchedBalls);
            }

            foreach (Score scoreObject in ScoreObjects.Where(scoreObject => scoreObject.AssignedBall == ballName))
            {
                scoreObject.UpdateScore(Score[ballName]);
            }
        }

        private void OnLanguageChanged(Language selectedLanguage)
        {
            foreach (MultiText textObject in MultiTextObjects)
            {
                textObject.RenderText(selectedLanguage);
            }
        }
        
        private void OnToggleAudioCalled()
        {
            foreach (AudioSource audioSource in AudioSources)
            {
                audioSource.volume = audioSource.volume == 1f ? 0f : 1f;
            }
        }

        private void OnEnable()
        {
            StartGame.AddListener(GameStart);
            ShowScore.AddListener(ScoreShow);
            ShowSkins.AddListener(SkinsShow);
            ShowSettings.AddListener(SettingsShow);
            GameEnded.AddListener(OnGameEnded);
            AddScore.AddListener(OnAddScoreCalled);
            ChangeLanguage.AddListener(OnLanguageChanged);
            BackToMainMenu.AddListener(OnBackToMainMenuPressed);
            ToggleAudio.AddListener(OnToggleAudioCalled);
        }

        private void OnDisable()
        {
            StartGame.RemoveListener(GameStart);
            ShowScore.RemoveListener(ScoreShow);
            ShowSkins.RemoveListener(SkinsShow);
            ShowSettings.RemoveListener(SettingsShow);
            GameEnded.RemoveListener(OnGameEnded);
            AddScore.RemoveListener(OnAddScoreCalled);
            ChangeLanguage.RemoveListener(OnLanguageChanged);
            BackToMainMenu.RemoveListener(OnBackToMainMenuPressed);
            ToggleAudio.RemoveListener(OnToggleAudioCalled);
        }
    }
}