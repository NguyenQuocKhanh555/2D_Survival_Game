using UnityEngine;

public class Damageable : MonoBehaviour
{
    private IDamageable _damageable;

    public void TakeDamage(float damage)
    {
        if (_damageable == null)
        {
            _damageable = GetComponent<IDamageable>();
        }
        
        _damageable.CalculateDamage(ref damage);
        _damageable.ApplyDamage(damage);
        _damageable.CheckState();
    }
}
