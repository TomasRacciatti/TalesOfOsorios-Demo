using UnityEngine;

namespace Entities.Enemy
{
    public class EnemyEntity : Entity
    {
        // Simple death, destroying go after 30 secs
        protected override void HandleDeath()
        {
            base.HandleDeath();
            
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
            
            EnemyController controller = GetComponent<EnemyController>();
            if (controller != null)
            {
                controller.enabled = false;
            }
            
            Destroy(gameObject, 3f);
        }
    }
}
