using UnityEngine;

namespace Project.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public float speed = 1f;

        private Rigidbody2D _rigidbody2D;
        private readonly float _horizontalBound = ((Screen.width / 100f) / 2f) + 1f;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            _rigidbody2D.velocity = new Vector2(speed, 0);
            var position = transform.position;
            if (position.x > _horizontalBound)
            {
                transform.position = new Vector2(-position.x + 1f, position.y);
            }
            else if (position.x < -_horizontalBound)
            {
                transform.position = new Vector2(-position.x - 1f, position.y);
            }
        }
    }
}