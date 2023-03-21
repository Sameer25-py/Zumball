using UnityEngine;
using UnityEngine.Serialization;
using static Zumball.Core.Events.GamePlayEvents;

namespace Zumball.GamePlay
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private GameObject     activeBallSpawn;
        [SerializeField] private SpriteRenderer activeBallRenderer;
        [SerializeField] private SpriteRenderer nextBallRenderer;

        [SerializeField] private CannonBallPoolGenerator cannonBallPoolGenerator;

        private                  Camera      _mainCamera;
        private                  Rigidbody2D _rb;
        [SerializeField] private GameObject  _activeBall;
        [SerializeField] private GameObject  _nextBall;

        [SerializeField] private float angleOffset = 90f;

        private bool _shootNextFrame = false;

        private AudioSource _audioSource;


        private void OnEnable()
        {
            _audioSource = GetComponent<AudioSource>();
           LoadCannon();
        }
        
        private void Start()
        {
            _mainCamera = Camera.main;
            _rb         = GetComponent<Rigidbody2D>();
        }

        private void LoadCannon()
        {
            if (_nextBall == null && _activeBall == null)
            {
                _nextBall   = cannonBallPoolGenerator.GetBall();
                _activeBall = cannonBallPoolGenerator.GetBall();
            }
            else
            {
                _activeBall = _nextBall;
                _nextBall   = cannonBallPoolGenerator.GetBall();
            }

            activeBallRenderer.sprite = _activeBall.GetComponent<SpriteRenderer>()
                .sprite;
            nextBallRenderer.sprite = _nextBall.GetComponent<SpriteRenderer>()
                .sprite;
        }

        private void Shoot()
        {
            _activeBall.transform.position = activeBallSpawn.transform.position;
            _activeBall.transform.rotation = Quaternion.Euler(0f, 0f, _rb.rotation);
            _activeBall.SetActive(true);
            _activeBall.AddComponent<CannonBall>();
            _audioSource.Play();

            LoadCannon();
        }

        private void Update()
        {
            if (_shootNextFrame)
            {
                _shootNextFrame = false;
                Shoot();
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos      = Input.mousePosition;
                Vector2 mouseWorldPos = _mainCamera.ScreenToWorldPoint(mousePos);
                Vector2 direction     = _rb.position - mouseWorldPos;
                float   angle         = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
                _rb.rotation    = angle;
                _shootNextFrame = true;
            }
        }
    }
}