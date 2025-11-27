using UnityEngine;

namespace Entities.Player
{
    public class AnimationEventRelay : MonoBehaviour
    {
        [SerializeField] private PlayerEntity playerEntity;

        private void Awake()
        {
            if (playerEntity == null)
            {
                playerEntity = GetComponentInParent<PlayerEntity>();

                if (playerEntity == null)
                {
                    Debug.LogError($"No PlayerEntity found in parent of {gameObject.name}");
                }
            }
        }

        public void OnAttackStart()
        {
            playerEntity.OnAttackStart();
        }

        public void OnHeavyAttackStart()
        {
            playerEntity.OnHeavyAttackStart();
        }

        public void OnAttackEnd()
        {
            playerEntity.OnAttackEnd();
        }

        public void OnAttackAnimationEnd()
        {
            playerEntity.OnAttackAnimationEnd();
        }
    }
}