using Control;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public abstract class HealthController : MonoBehaviour
    {
        [SerializeField] protected float _timeSinceLastHit = 1f;
        [SerializeField] protected TextMeshProUGUI _healthValueText; 
        [SerializeField] protected Slider _UISlider;

        protected Animator _animator;
        protected AudioSource _audioSource;
        private DamageTextSpawner _damageTextSpawner;
        protected ParticleSystem _bloodParticle;
        protected float _startHealthValue;
        protected float _currentHealthValue;
        protected float _timer = 0f;
        protected bool _isDead;

        public bool IsDead => _isDead;

        protected GameManager GameManager => GameManager.Instance;

        protected virtual float StartHealthValue
        {
            get => _startHealthValue;
            set
            {
                if (_startHealthValue == value)
                {
                    return;
                }

                _startHealthValue = value;

                if (_UISlider == null)
                {
                    return;
                }

                _UISlider.maxValue = _startHealthValue;
            }
        }

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _bloodParticle = GetComponentInChildren<ParticleSystem>();
            _damageTextSpawner = GetComponentInChildren<DamageTextSpawner>();

            StartHealthValue = GameManager.GetStat(StatType.Health, GetComponent<Entity>().Type);
            _currentHealthValue = StartHealthValue;
        }

        protected virtual void Update()
        {
            _timer += Time.deltaTime;

            if (_UISlider == null)
            {
                return;
            }

            StartHealthValue = GameManager.GetStat(StatType.Health, GetComponent<Entity>().Type);
            _UISlider.value = _currentHealthValue;

            if (_healthValueText == null)
            {
                return;
            }

            _healthValueText.text = $"{_currentHealthValue} / {StartHealthValue}";
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (_timer < _timeSinceLastHit || CanTakeDamage(other) == false)
            {
                return;
            }

            var damageValue = other.CompareTag("Arrow") ? GameManager.GetStat(StatType.Damage, CharacterType.Ranger) :
                                                          GameManager.GetStat(StatType.Damage, other.GetComponentInParent<Entity>().Type);
            TakeHit(damageValue);
        }

        protected abstract bool CanTakeDamage(Collider other);

        public virtual void TakeHit(float value)
        {
            if (_currentHealthValue - value > 0)
            {
                _animator.Play("hurt");
                UnityEngine.Debug.LogError("Hurt");
                _bloodParticle.Play();
                _audioSource.PlayOneShot(_audioSource.clip);

                _currentHealthValue -= System.Math.Abs(value);
            }
            else if (_currentHealthValue - value <= 0)
            {
                _isDead = true;
                Die();
            }

            if (_damageTextSpawner != null)
            {
                _damageTextSpawner.SpawnDamage(value);
            }

            _timer = 0;
        }

        public virtual void Die()
        {
            _bloodParticle.Play();
            _animator.SetTrigger("die");
            UnityEngine.Debug.LogError("Die");
        }

        public void RestoreHealth()
        {
            _currentHealthValue = StartHealthValue;
        }
    }
}