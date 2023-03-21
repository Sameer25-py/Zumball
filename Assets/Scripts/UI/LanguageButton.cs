using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zumball.Core;
using static Zumball.Core.Events.SettingsEvents;

namespace Zumball.UI
{
    public class LanguageButton : MonoBehaviour
    {
        [SerializeField] private GameManager.Language assignedLanguage;

        private Button _button;
        private Image  _image;

        private readonly Color _unselectedColor = new Color(1f, 1f, 1f, 0.4f);
        private readonly Color _selectedColor   = new Color(1f, 1f, 1f, 1f);

        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _image  = GetComponent<Image>();
            if (assignedLanguage == GameManager.Language.English)
            {
                _image.color = _selectedColor;
            }
            else if (assignedLanguage == GameManager.Language.Russian)
            {
                _image.color = _unselectedColor;
            }
            _button.onClick.AddListener(LanguageChange);
            ChangeLanguage.AddListener(OnChangeLanguageCalled);
        }

        private void OnDisable()
        {   
            _button.onClick.RemoveListener(LanguageChange);
            ChangeLanguage.RemoveListener(OnChangeLanguageCalled);
        }

        private void OnChangeLanguageCalled(GameManager.Language lang)
        {
            _image.color = assignedLanguage != lang ? _unselectedColor : _selectedColor;
        }

        private void LanguageChange()
        {
            ChangeLanguage?.Invoke(assignedLanguage);
        }
    }
}