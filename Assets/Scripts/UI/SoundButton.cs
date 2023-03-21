using System;
using UnityEngine;
using UnityEngine.UI;
using static Zumball.Core.Events.SettingsEvents;

namespace Zumball.UI
{
    public class SoundButton : MonoBehaviour
    {
        private          Button _button;
        private          Image  _img;
        private readonly Color  _unselectedColor = new Color(1f, 1f, 1f, 1f);
        private readonly Color  _selectedColor   = new Color(1f, 1f, 1f, 0.4f);


        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _img    = GetComponent<Image>();
            _button.onClick.AddListener(OnSoundButtonPressed);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnSoundButtonPressed);
        }

        private void OnSoundButtonPressed()
        {   
            _img.color = _img.color.Equals(_unselectedColor) ? _selectedColor : _unselectedColor;
            ToggleAudio?.Invoke();
        }
    }
}