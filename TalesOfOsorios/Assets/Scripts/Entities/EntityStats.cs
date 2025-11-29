using UnityEngine;

[CreateAssetMenu(fileName = "EntityStats", menuName = "Scriptable Objects/Entity/EntityStats")]
public class EntityStats : ScriptableObject
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float minAttackDamage = 20f;
    [SerializeField] private float maxAttackDamage = 30f;
    
    public float MaxHealth => maxHealth;
    public float BaseSpeed => baseSpeed;
    public float AttackCooldown => attackCooldown;
    public float MinDamage => minAttackDamage;
    public float MaxDamage => maxAttackDamage;
}
