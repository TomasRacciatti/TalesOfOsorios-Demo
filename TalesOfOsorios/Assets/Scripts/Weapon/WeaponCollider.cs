using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class WeaponCollider : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    
    private Collider2D _weaponCollider;
    private float _currentDamage;
    private HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

    private void Awake()
    {
        _weaponCollider = GetComponent<Collider2D>();
        _weaponCollider.isTrigger = true;
        DisableCollider();
    }

    public void EnableCollider(float damage)
    {
        _currentDamage = damage;
        hitTargets.Clear();
        _weaponCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _weaponCollider.enabled = false;
        hitTargets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsInTargetLayer(collision.gameObject.layer)) return;
        
        IDamageable target = collision.GetComponent<IDamageable>();
        
        if (target != null && !hitTargets.Contains(target))
        {
            target.TakeDamage(_currentDamage);
            hitTargets.Add(target);
        }
    }

    private bool IsInTargetLayer(int layer)
    {
        return ((1<<layer) & targetLayer) != 0;
    }
}
