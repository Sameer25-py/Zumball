using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Zumball.GamePlay
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private int          ballCount = 250;
        [SerializeField] private List<Sprite> ballTypes;
        [SerializeField] private GameObject   ballPrefab;

        protected Queue<GameObject> BallPool;

        private void Start()
        {
            GeneratePool();
        }

        protected virtual void OnDisable()
        {   
            DestroyPool();
        }

        protected virtual void GeneratePool()
        {
            BallPool = new();
            for (int i = 0; i < ballCount; i++)
            {
                int        randomIndex = Random.Range(0, ballTypes.Count);
                GameObject obj         = Instantiate(ballPrefab, transform);
                obj.SetActive(false);
                obj.GetComponent<SpriteRenderer>()
                    .sprite = ballTypes[randomIndex];
                PoolDetector poolDetector =  obj.AddComponent<PoolDetector>();
                poolDetector.AssignedPool = this;
                BallPool.Enqueue(obj);
            }
        }

        public void AddBackToPool(GameObject obj)
        {   
            obj.SetActive(false);
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            BallPool.Enqueue(obj);
        }
        
        private void DestroyPool()
        {
            while (BallPool.Count != 0)
            {
                GameObject obj = BallPool.Dequeue();
                Destroy(obj);
            }
            
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i));
            }
        }
        
        public GameObject GetBall()
        {
            if (BallPool.Count == 0)
            {
                GeneratePool();
            }
            
            return BallPool.Dequeue();
        }
        
        
    }
}