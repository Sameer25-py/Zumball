using UnityEngine;
using static Zumball.Core.Events.GamePlayEvents;

namespace Zumball.GamePlay
{
    public class SplineBallPoolGenerator : PoolManager
    {
        private void OnEnable()
        {
            
        }
        protected override void OnDisable()
        {   
            base.OnDisable();
        }
        
        protected override void GeneratePool()
        {
            base.GeneratePool();
            SplineBallPoolGenerated?.Invoke();
        }
        
    }
}