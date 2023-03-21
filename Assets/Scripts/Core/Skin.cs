using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Zumball.Core.Events.SettingsEvents;

namespace Zumball.Core
{
    public class Skin : MonoBehaviour
    {
        [SerializeField] private List<Sprite> skinsList;
        [SerializeField] private Image        backGroundImage;

        private void OnEnable()
        {
            ApplySkin.AddListener(OnApplySkinCalled);
        }

        private void OnDisable()
        {
            ApplySkin.RemoveListener(OnApplySkinCalled);
        }

        private void OnApplySkinCalled(int skinIndex)
        {
            backGroundImage.sprite = skinsList[skinIndex];
        }
    }
}