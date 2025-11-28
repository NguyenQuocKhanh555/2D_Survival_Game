using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _damage = 1f;

    private Animator _animator;
    private Rigidbody2D _rb;
    private float _lifeTime = 3f;
    private Damageable _damageable;

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0 )
        {
            Destroy(gameObject);
        }
    }

    public void SetUp(Vector3 direction, Damageable damageable) 
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _damageable = damageable;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
        _rb.AddForce(direction * _moveSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null && damageable != _damageable)
        {
            _rb.linearVelocity = Vector2.zero;
            _animator.SetTrigger("destroy");
            damageable.TakeDamage(_damage);   
        }
    }

    public void Disable()
    {
        Destroy(gameObject); 
    }
}
