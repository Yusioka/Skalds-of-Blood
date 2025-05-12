using Combat;
using Control;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float LifeTime = 10f;

    [SerializeField] private GameObject hitEffect;

    private int _damage;
    private float _speed;
    private Vector3 _targetPoint;
    private Faction _sourceFaction;

    private void Update()
    {
        transform.LookAt(_targetPoint);
        transform.position = Vector3.Lerp(transform.position, _targetPoint, _speed * Time.deltaTime);
    }

    public void Initialize(Vector3 targetPoint, Faction sourceFaction, int damage, float speed)
    {
        _speed = speed;
        _damage = damage;
        _targetPoint = targetPoint;
        _sourceFaction = sourceFaction;

        Destroy(gameObject, LifeTime);
    }

    private void Complete(EnemyHealth target)
    {
        _speed = 0f;

        if (target != null)
        {
            target.TakeHit(_damage);
        }

        if (hitEffect != null)
        {
            var hitInstance = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(hitInstance, 5f);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyHealth>(out var health) == false ||
            other.TryGetComponent<Entity>(out var entity) == false)
        {
            return;
        }

        if (entity.Faction == _sourceFaction)
        {
            return;
        }

        Complete(health);
    }
}