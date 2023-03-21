using System;
using PathCreation.Examples;
using UnityEngine;
using static Zumball.Core.Events.GamePlayEvents;

namespace Zumball.GamePlay
{
    public class CannonBall : MonoBehaviour
    {
        private float     _speed       = 20f;
        private float     _expireTime  = 5f;
        private float     _elapsedTime = 0f;
        private Transform _cachedTransform;
        private bool      _firstHit;

        private void Start()
        {
            _cachedTransform = transform;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_firstHit) return;
            _firstHit = true;
            if (other.gameObject.TryGetComponent(typeof(PathFollower),out Component component))
            {
                CannonBallHit?.Invoke(gameObject,other.gameObject);
            }

        }

        private void Update()
        {
            transform.position += transform.up * Time.deltaTime * _speed;
            _elapsedTime              += Time.deltaTime;
            if (_elapsedTime > _expireTime)
            {
                BallExpired?.Invoke(gameObject);
            }
            
        }
    }
}