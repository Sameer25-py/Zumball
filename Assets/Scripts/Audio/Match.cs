using System;
using UnityEngine;
using static Zumball.Core.Events.GamePlayEvents;
namespace Zumball.Audio
{
    public class Match : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void OnEnable()
        {
            _audioSource = GetComponent<AudioSource>();
            AddScore.AddListener(OnScoreAdded);
            
        }

        private void OnDisable()
        {
            AddScore.RemoveListener(OnScoreAdded);
        }

        private void OnScoreAdded(string arg0, int arg1)
        {
            _audioSource.Play();
        }
    }
}