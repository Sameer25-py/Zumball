using UnityEngine;

using static Zumball.Core.Events.Navigation;
namespace Zumball.UI
{
    public class BackButton : MonoBehaviour
    {
        public void PressBackButton()
        {
            BackToMainMenu?.Invoke();
        }
    }
}