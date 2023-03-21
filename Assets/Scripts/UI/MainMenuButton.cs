using UnityEngine;
using static Zumball.Core.Events.Navigation;
using UnityEngine.UI;

namespace Zumball.UI
{
    public class MainMenuButton : MonoBehaviour
    {
        [SerializeField] private int    buttonIndex;
        private                  Button _button;

        private void OnEnable()
        {
            _button         =  GetComponent<Button>();
            _button.onClick.AddListener(OnButtonPressed);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonPressed);
        }

        private void OnButtonPressed()
        {
            ButtonPressed?.Invoke(buttonIndex);
        }
        
    }
}