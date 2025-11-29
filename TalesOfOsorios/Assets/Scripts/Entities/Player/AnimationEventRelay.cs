using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Player
{
    public class AnimationEventRelay : MonoBehaviour
    {
        [SerializeField] private Entity entity;

        private void Awake()
        {
            if (entity == null)
            {
                entity = GetComponentInParent<Entity>();

                if (entity == null)
                {
                    Debug.LogError($"No PlayerEntity found in parent of {gameObject.name}");
                }
            }
        }

        public void OnAttackStart()
        {
            entity.OnAttackStart();
        }

        public void OnHeavyAttackStart()
        {
            entity.OnHeavyAttackStart();
        }

        public void OnAttackEnd()
        {
            entity.OnAttackEnd();
        }

        public void OnAttackAnimationEnd()
        {
            entity.OnAttackAnimationEnd();
        }
    }
}