using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using static Zumball.Core.Events.GameManagerEvents;

namespace Zumball.UI
{
    public class Score : MonoBehaviour
    {
        [FormerlySerializedAs("assignedBall")] public string AssignedBall;
        public void UpdateScore(int score)
        {
            GetComponent<TMP_Text>()
                .text = score.ToString();
        }
    }
}