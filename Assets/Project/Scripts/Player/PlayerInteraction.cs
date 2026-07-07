using UnityEngine;

namespace JUNBEOM.Player
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerInteraction : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Interaction Settings")]
        [SerializeField] private Transform _interactionPoint;
        [SerializeField] private float _interactionRadius = 1.2f;
        [SerializeField] private LayerMask _interactableLayerMask;

        #endregion

        #region Private Fields

        private PlayerInputManager _inputManager;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _inputManager = GetComponent<PlayerInputManager>();
        }

        private void OnEnable()
        {
            _inputManager.OnInteractEvent += HandleInteractInput;
        }

        private void OnDisable()
        {
            _inputManager.OnInteractEvent -= HandleInteractInput;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(
                GetInteractionCenter(),
                _interactionRadius);
        }

        #endregion

        #region Input Event Handlers

        private void HandleInteractInput()
        {
            GameObject target = FindNearestTarget();

            if (target == null)
            {
                return;
            }

            PlayerEventChannel.BroadcastInteraction(
                target,
                gameObject);
        }

        #endregion

        #region Private Methods

        private GameObject FindNearestTarget()
        {
            Vector2 center = GetInteractionCenter();

            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                center,
                _interactionRadius,
                _interactableLayerMask);

            GameObject nearestTarget = null;
            float nearestDistanceSqr = float.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                IInteractable interactable =
                    colliders[i].GetComponentInParent<IInteractable>();

                Component interactableComponent =
                    interactable as Component;

                if (interactableComponent == null)
                {
                    continue;
                }

                float distanceSqr =
                    ((Vector2)interactableComponent.transform.position - center)
                    .sqrMagnitude;

                if (distanceSqr >= nearestDistanceSqr)
                {
                    continue;
                }

                nearestDistanceSqr = distanceSqr;
                nearestTarget = interactableComponent.gameObject;
            }

            return nearestTarget;
        }

        private Vector3 GetInteractionCenter()
        {
            if (_interactionPoint != null)
            {
                return _interactionPoint.position;
            }

            return transform.position;
        }

        #endregion
    }
}