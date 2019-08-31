using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float _health = 100.0f;

    public void ApplyDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0.0f)
            Destroy(gameObject);
    }
}
