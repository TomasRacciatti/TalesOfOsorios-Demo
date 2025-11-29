using UnityEngine;

namespace Entities.Enemy
{
    public class EnemyEntity : Entity
    {
        // Simple death, destroying go after 30 secs
        protected override void HandleDeath()
        {
            base.HandleDeath();
            
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            
            Destroy(gameObject, 3f);
        }
    }
}
