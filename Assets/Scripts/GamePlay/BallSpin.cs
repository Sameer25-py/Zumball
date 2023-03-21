using System;
using UnityEngine;

namespace Zumball.GamePlay
{
    public class BallSpin : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(Vector3.forward * 200f * Time.deltaTime, Space.Self);
        }
    }
}