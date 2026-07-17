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
        [SerializeField] private GameObject _interactionIndicator;

        #endregion

        #region Private Fields

        private PlayerInputManager _inputManager;
        private PlayerEventManager _eventManager;
        private GameObject _currentTarget;

        #endregion

        #region Properties

        public GameObject CurrentTarget => _currentTarget;
        public bool HasInteractableTarget => _currentTarget != null;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _inputManager = GetComponent<PlayerInputManager>();
            _eventManager = PlayerEventManager.Instance;
            SetIndicatorActive(false);
        }

        private void OnEnable()
        {
            _inputManager.OnInteractEvent += HandleInteractInput;
        }

        private void OnDisable()
        {
            _inputManager.OnInteractEvent -= HandleInteractInput;
            _currentTarget = null;
            SetIndicatorActive(false);
        }
        private void Update()
        {
            UpdateInteractionTarget();
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetInteractionCenter(),_interactionRadius);
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

            IInteractable interactable = target.GetComponentInParent<IInteractable>();
            interactable?.Interact(gameObject);
        }

        #endregion

        #region Private Methods
        private void UpdateInteractionTarget()
        {
            GameObject newTarget = FindNearestTarget();

            if (_currentTarget == newTarget)
            {
                return;
            }

            _currentTarget = newTarget;

            SetIndicatorActive(_currentTarget != null);
        }

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

        private void SetIndicatorActive(bool isActive)
        {
            if (_interactionIndicator == null)
            {
                return;
            }

            if (_interactionIndicator.activeSelf == isActive)
            {
                return;
            }

            _interactionIndicator.SetActive(isActive);
        }

        private Vector2 GetInteractionCenter()
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