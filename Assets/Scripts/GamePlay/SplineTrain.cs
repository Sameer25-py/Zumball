using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;
using static Zumball.Core.Events.GamePlayEvents;
using static Zumball.Core.Events.Navigation;

namespace Zumball.GamePlay
{
    public class SplineTrain : MonoBehaviour
    {
        [SerializeField] private PathCreator             pathCreatorObject;
        [SerializeField] private SplineBallPoolGenerator ballPool;
        [SerializeField] private Transform               spawnPoint;
        [SerializeField] private int                     initialTrainCount = 5;
        [SerializeField] private List<GameObject>        ballTrain;
        [SerializeField] private float                   trainSpeed     = 1f;
        [SerializeField] private bool                    _isGameStarted = false;

        private                  Coroutine        _trainCoroutine;
        private                  Coroutine        _trainHeadCoroutine;
        private                  int              _firstUnmatchedBallIndex = -1;
        private                  int              _lastUnmatchedBallIndex  = -1;
        private                  float            _waitUntilDistanceTravelled;
        [SerializeField] private List<GameObject> matchedBalls;


        public void StartGame()
        {
            OnStartGameCalled();
        }

        public void StopGame()
        {
            if (!_isGameStarted)
            {
                return;
            }

            if (_trainCoroutine != null)
            {
                StopCoroutine(_trainCoroutine);
            }

            if (_trainHeadCoroutine != null)
            {
                StopCoroutine(_trainHeadCoroutine);
            }

            foreach (GameObject ball in ballTrain)
            {
                ball.GetComponent<PathFollower>()
                    .speed = 0f;
                Destroy(ball.GetComponent<PathFollower>());
                Destroy(ball.GetComponent<BallSpin>());
                ball.GetComponent<PoolDetector>()
                    .AssignedPool.AddBackToPool(ball);
            }
            
            _firstUnmatchedBallIndex = -1;
            _lastUnmatchedBallIndex  = -1;
            ballTrain                = new();
            matchedBalls             = new();
        }

        private void OnEnable()
        {
            CannonBallHit.AddListener(OnCannonBallHit);
            VertexPath.GameEnd.AddListener(OnGameEnd);
        }

        private void OnDisable()
        {
            CannonBallHit.RemoveListener(OnCannonBallHit);
            VertexPath.GameEnd.RemoveListener(OnGameEnd);
        }

        private void OnStartGameCalled()
        {
            if (_trainCoroutine != null)
            {
                StopCoroutine(_trainCoroutine);
            }

            if (_trainHeadCoroutine != null)
            {
                StopCoroutine(_trainHeadCoroutine);
            }

            ballTrain                = new();
            _firstUnmatchedBallIndex = -1;
            _lastUnmatchedBallIndex  = -1;
            _isGameStarted           = true;
            _trainCoroutine          = StartCoroutine(SpawnTrain());
        }

        private void OnGameEnd()
        {
            StopGame();
            GameEnded?.Invoke();
        }

        private void OnCannonBallHit(GameObject cannonBall, GameObject trainBall)
        {
            StopTrain();
            if (cannonBall.TryGetComponent(typeof(CannonBall), out Component component))
            {
                Destroy(component);
            }

            PathFollower pathFollower      = trainBall.GetComponent<PathFollower>();
            float        distanceTravelled = pathFollower.distanceTravelled;
            int          index             = GetHitBallIndex(trainBall);
            MakeSpaceForNewBall(index);
            MakeFollower(cannonBall, distanceTravelled, index + 1, 0f);
            bool isMatchingAvailable = DetectMatching(cannonBall, index + 1);
            if (isMatchingAvailable)
            {
                MoveBallsBackToPool(matchedBalls);
                _lastUnmatchedBallIndex -= matchedBalls.Count;
                AddScore?.Invoke(matchedBalls[0]
                    .GetComponent<SpriteRenderer>()
                    .sprite.name, matchedBalls.Count);

                _trainCoroutine = StartCoroutine(StopTrainHeadUntil());
            }
            else
            {
                _firstUnmatchedBallIndex = -1;
                _lastUnmatchedBallIndex  = -1;
            }

            StartTrain();
        }

        private void MoveBallsBackToPool(List<GameObject> balls)
        {
            foreach (GameObject ball in balls)
            {
                ball.GetComponent<PathFollower>()
                    .speed = 0f;
                ballTrain.Remove(ball);
                Destroy(ball.GetComponent<PathFollower>());
                Destroy(ball.GetComponent<BallSpin>());
                ball.GetComponent<PoolDetector>()
                    .AssignedPool.AddBackToPool(ball);
            }
        }

        private IEnumerator StopTrainHeadUntil()
        {
            for (int i = _firstUnmatchedBallIndex; i >= 0; i--)
            {
                ballTrain[i]
                    .GetComponent<PathFollower>()
                    .speed = 0f;
            }

            PathFollower pathFollowercomponent = ballTrain[_lastUnmatchedBallIndex]
                .GetComponent<PathFollower>();
            yield return new WaitUntil(() => pathFollowercomponent.distanceTravelled >= _waitUntilDistanceTravelled);

            _firstUnmatchedBallIndex = -1;
            _lastUnmatchedBallIndex  = -1;
            StartTrain();
        }

        private bool DetectMatching(GameObject targetBall, int index)
        {
            string spriteName = targetBall.GetComponent<SpriteRenderer>()
                .sprite.name;
            matchedBalls = new List<GameObject> { targetBall };
            bool isMatched = false;
            for (int i = index - 1; i >= 0; i--)
            {
                GameObject ballInTrain = ballTrain[i];
                if (ballInTrain.GetComponent<SpriteRenderer>()
                    .sprite.name == spriteName)
                {
                    matchedBalls.Add(ballInTrain);
                }
                else
                {
                    _firstUnmatchedBallIndex = i;
                    _waitUntilDistanceTravelled = ballTrain[i + 1]
                        .GetComponent<PathFollower>()
                        .distanceTravelled;
                    break;
                }
            }

            for (int i = index + 1; i < ballTrain.Count; i++)
            {
                GameObject ballInTrain = ballTrain[i];
                if (ballInTrain.GetComponent<SpriteRenderer>()
                    .sprite.name == spriteName)
                {
                    matchedBalls.Add(ballInTrain);
                }
                else
                {
                    _lastUnmatchedBallIndex = i;
                    break;
                }
            }

            if (matchedBalls.Count >= 3)
            {
                isMatched = true;
            }

            return isMatched;
        }

        private void MakeSpaceForNewBall(int index)
        {
            for (int i = index; i >= 0; i--)
            {
                ballTrain[i]
                    .GetComponent<PathFollower>()
                    .distanceTravelled += 0.55f;
            }
        }

        private int GetHitBallIndex(GameObject hitBall)
        {
            for (int i = 0; i < ballTrain.Count; i++)
            {
                if (ReferenceEquals(ballTrain[i], hitBall))
                {
                    return i;
                }
            }

            return -1;
        }

        private void MakeFollower(GameObject obj, float distanceTravelled = 0f, int currentIndex = -1, float speed = 1f)
        {
            obj.transform.position = spawnPoint.position;
            obj.transform.rotation = Quaternion.identity;
            PathFollower pathFollowerComponent = obj.AddComponent<PathFollower>();
            pathFollowerComponent.pathCreator          = pathCreatorObject;
            pathFollowerComponent.speed                = speed;
            pathFollowerComponent.distanceTravelled    = distanceTravelled;
            pathFollowerComponent.endOfPathInstruction = EndOfPathInstruction.Stop;
            obj.AddComponent<BallSpin>();
            obj.SetActive(true);
            if (currentIndex == -1)
            {
                ballTrain.Add(obj);
            }
            else
            {
                ballTrain.Insert(currentIndex, obj);
            }
        }

        private void StopTrain()
        {
            foreach (GameObject ball in ballTrain)
            {
                ball.GetComponent<PathFollower>()
                    .speed = 0f;
            }
        }

        private void StartTrain()
        {
            for (int i = 0; i < ballTrain.Count; i++)
            {
                if (i > _firstUnmatchedBallIndex)
                {
                    ballTrain[i]
                        .GetComponent<PathFollower>()
                        .speed = trainSpeed;
                }
            }
        }

        private IEnumerator SpawnTrain()
        {
            for (int i = 0; i < initialTrainCount; i++)
            {
                if (ballTrain.Count != 0)
                {
                    PathFollower pathFollower = ballTrain[^1]
                        .GetComponent<PathFollower>();
                    yield return new WaitUntil(() => pathFollower.distanceTravelled >= 0.55f);
                }

                GameObject splineBall = ballPool.GetBall();
                MakeFollower(splineBall, 0f, -1, trainSpeed);
                yield return null;
            }
        }
    }
}