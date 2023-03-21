using System;
using UnityEngine;
using UnityEngine.UI;
using static Zumball.Core.Events.SettingsEvents;

namespace Zumball.UI
{
    public class SkinButton : MonoBehaviour
    {
        [SerializeField] private int assignedSkinIndex;

        private Button _button;
        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(SkinChange);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(SkinChange);
        }

        private void SkinChange()
        {
            ApplySkin?.Invoke(assignedSkinIndex);
        }
    }
}