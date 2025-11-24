using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private IDamageable _damageable;

    public void TakeDamage(float damage)
    {
        if (_damageable == null)
        {
            _damageable = GetComponentInParent<IDamageable>();
        }
        
        _damageable.CalculateDamage(ref damage);
        _damageable.ApplyDamage(damage);
        _damageable.CheckState();
    }
}
