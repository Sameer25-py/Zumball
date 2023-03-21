using UnityEngine;

namespace Zumball.Core
{
    using static Events.Navigation;

    public class MainMenu : MonoBehaviour
    {
        private void OnEnable()
        {
            ButtonPressed.AddListener(OnButtonPressed);
        }

        private void OnDisable()
        {
            ButtonPressed.RemoveListener(OnButtonPressed);
        }

        public void OnButtonPressed(int buttonIndex)
        {
            switch (buttonIndex)
            {
                case 0:
                    StartGame?.Invoke();
                    break;
                case 1:
                    ShowScore?.Invoke();
                    break;
                case 2:
                    ShowSkins?.Invoke();
                    break;
                case 3:
                    ShowSettings?.Invoke();
                    break;
            }
        }
    }
}