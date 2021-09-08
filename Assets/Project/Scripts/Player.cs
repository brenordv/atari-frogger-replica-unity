using UnityEngine;

namespace Project.Scripts
{
    public class Player : MonoBehaviour
    {
        public float jumpDistance = 0.32f;

        private bool _jumped;
        private readonly float _verticalBoundaries = (Screen.height / 100f) / 2f;
        private readonly float _horizontalBoundaries = (Screen.width / 100f) / 2f;
        private Vector3 _initialPosition;
        private AudioSource _audioSource;

        public delegate void PlayerHandler();

        public event PlayerHandler OnPlayerMoved;
        public event PlayerHandler OnPlayerEscaped;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _initialPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            // Due to this projects configuration, this value can only be -1, 0 or 1.
            var horizontalMovement = Input.GetAxis("Horizontal");

            // Due to this projects configuration, this value can only be -1, 0 or 1.
            var verticalMovement = Input.GetAxis("Vertical");

            Movement(horizontalMovement, verticalMovement);
            EnsureMovementWithinBoundaries();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy")) return;
            Destroy(gameObject);
        }

        private void EnsureMovementWithinBoundaries()
        {
            var position = transform.position;

            if (position.y < -_verticalBoundaries)
            {
                transform.position = new Vector2(position.x, position.y + jumpDistance);
            }
            else if (position.y > _verticalBoundaries)
            {
                transform.position = _initialPosition;
                OnPlayerEscaped?.Invoke();
            }

            // Refresh position variable.
            position = transform.position;
            if (transform.position.x < -_horizontalBoundaries)
            {
                transform.position = new Vector2(position.x + jumpDistance, position.y);
            }
            else if (transform.position.x > _horizontalBoundaries)
            {
                transform.position = new Vector2(position.x - jumpDistance, position.y);
            }
        }

        private bool IsMovementBlocked(Vector2 targetPosition)
        {
            var hitCollider = Physics2D.OverlapCircle(targetPosition, .1f);
            return hitCollider != null && !hitCollider.CompareTag("Enemy");
        }

        private (bool, Vector2) GetTargetPosition(float horizontalMovement, float verticalMovement)
        {
            var targetPosition = Vector2.zero;
            var position = transform.position;
            var tryingToMove = false;
            if (horizontalMovement != 0)
            {
                targetPosition = new Vector2(
                    position.x + (jumpDistance * horizontalMovement),
                    position.y);
                tryingToMove = true;
            }
            else if (verticalMovement != 0)
            {
                targetPosition = new Vector2(
                    position.x,
                    position.y + (jumpDistance * verticalMovement));
                tryingToMove = true;
            }

            return (tryingToMove, targetPosition);
        }

        private void Movement(float horizontalMovement, float verticalMovement)
        {
            if (!_jumped)
            {
                var (tryingToMove, targetPosition) = GetTargetPosition(horizontalMovement, verticalMovement);
                if (!tryingToMove || IsMovementBlocked(targetPosition)) return;

                _audioSource.pitch = Random.Range(0.8f, 1.1f);
                _audioSource.Play();
                transform.position = targetPosition;
                _jumped = true;
                OnPlayerMoved?.Invoke();
            }
            else
            {
                _jumped = horizontalMovement != 0 || verticalMovement != 0;
            }
        }
    }
}