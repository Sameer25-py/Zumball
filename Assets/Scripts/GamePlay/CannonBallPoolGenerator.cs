using UnityEngine;
using static Zumball.Core.Events.GamePlayEvents;
using Random = UnityEngine.Random;

namespace Zumball.GamePlay
{
    public class CannonBallPoolGenerator : PoolManager
    {
        private void OnEnable()
        {
            BallExpired.AddListener(OnBallExpired);
        }
        
        protected override void OnDisable()
        {   
            base.OnDisable();
            BallExpired.AddListener(OnBallExpired);
        }

        private void OnBallExpired(GameObject expiredBall)
        {
            if (expiredBall.TryGetComponent(typeof(CannonBall), out Component component))
            {
                Destroy(component);
            }
            AddBackToPool(expiredBall);
        }

        protected override void GeneratePool()
        {
            base.GeneratePool();
            CannonBallPoolGenerated?.Invoke();
        }
    }
}