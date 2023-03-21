using System;
using TMPro;
using UnityEngine;
using Zumball.Core;
namespace Zumball.UI
{
    public class MultiText : MonoBehaviour
    {
        private TMP_Text             _text;
        public  string               EnglishString;
        public  string               RussianString;
        public void RenderText(GameManager.Language selectedLanguage)
        {
            _text = GetComponent<TMP_Text>();
            switch (selectedLanguage)
            {
                case GameManager.Language.English:
                    _text.text = EnglishString;
                    break;
                case GameManager.Language.Russian:
                    _text.text = RussianString;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedLanguage), selectedLanguage, null);
            }
        }
    }
}