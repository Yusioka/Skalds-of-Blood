using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float _defaultSpeed = 5f;
        [SerializeField] private Slider _staminaSlider;

        private bool _canRun = true;
        private float _currentSpeed;
        private float _additionalSpeed;
        private float _runningSpeed;
        private float _runningTimer;

        private Animator _animator;
        private Rigidbody _rigidBody;

        private float _staminaValue = 100;

        protected GameManager GameManager => GameManager.Instance;

        private float AdditionalSpeed
        {
            get => _additionalSpeed;
            set
            {
                if (_additionalSpeed == value)
                {
                    return;
                }

                _additionalSpeed = value;
                _currentSpeed += _additionalSpeed;
            }
        }

        void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();

            _currentSpeed = _defaultSpeed;
            _runningSpeed = _defaultSpeed * 2;

            AdditionalSpeed = GameManager.GetStat(StatType.Haste, CharacterType.Player);
            _currentSpeed += AdditionalSpeed;
        }


        void Update()
        {
            if (GameManager.IsGameOver)
            {
                _animator.SetFloat("Blend", 0);
                return;
            }

            AdditionalSpeed = GameManager.GetStat(StatType.Haste, CharacterType.Player);

            BaseMoving();
            Running();
            Jumping();

            _staminaSlider.value = _staminaValue;
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        void BaseMoving()
        {
            bool isMoving = false;

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * _currentSpeed * Time.deltaTime);
                isMoving = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * (_currentSpeed / 2) * Time.deltaTime);
                isMoving = true;
            }

            if (Input.GetKey(KeyCode.A))
            {
                var isNotSideMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);
                var sideSpeed = isNotSideMoving ? _currentSpeed / 2 : _currentSpeed;
                transform.Translate(Vector3.left * sideSpeed * Time.deltaTime);
                isMoving = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                var isNotSideMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);
                var sideSpeed = isNotSideMoving ? _currentSpeed / 2 : _currentSpeed;
                transform.Translate(Vector3.right * sideSpeed * Time.deltaTime);
                isMoving = true;
            }

            _animator.SetFloat("Blend", isMoving ? 5 : 0);
        }

        void Running()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S) == false && _canRun)
            {
                _staminaValue -= 0.2f;
                _currentSpeed = _runningSpeed;
                _animator.SetFloat("Blend", 10);
            }
            else if (Input.GetKey(KeyCode.LeftShift) == false)
            {
                _currentSpeed = _defaultSpeed;
            }
            CanRun();
        }

        void CanRun()
        {
            if (_staminaValue <= 0)
            {
                _canRun = false;
                _currentSpeed = _defaultSpeed;
            }
            else
            {
                _canRun = true;
            }

            if (_staminaValue < 100 && !Input.GetKey(KeyCode.LeftShift))
            {
                _runningTimer += Time.deltaTime;
                if (_runningTimer > 2f && _staminaValue != 100)
                {
                    _staminaValue += 0.2f;
                }
            }
            else if (_canRun == false)
            {
                _runningTimer = 0;
            }
        }

        void Jumping()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidBody.velocity = new Vector3(0, 5, 0);
                _animator.Play("jumping");
            }
        }
    }
}