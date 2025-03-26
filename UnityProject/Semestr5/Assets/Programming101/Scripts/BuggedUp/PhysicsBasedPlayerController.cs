using UnityEngine;

namespace BuggedUpCode
{
    /// <summary>
    ///     ** (30 points) ** 
    /// This script is supposed to control a physics-based player.
    /// The player should be able to move left and right using the arrow keys
    /// and jump when pressing the space bar. The movement should be smooth,
    /// and the jump should be physics-based.
    /// 
    /// Fix the code to match the expected behavior.
    /// </summary>
    public class PhysicsBasedPlayerController : MonoBehaviour
    {
        public float speed = 5f;
        public float jumpForce = 10f;

        private Rigidbody2D rb;
        private bool onGround = true;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Move(Vector2.left);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Move(Vector2.right);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void Move(Vector2 direction)
        {
            rb.velocity = direction * speed;
        }

        private void Jump()
        {
            if (onGround)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                onGround = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                onGround = false;
            }
        }
    }
}
