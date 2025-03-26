using UnityEngine;

namespace Programming101
{
    public class Movement : MonoBehaviour
    {
        public float speed = 2;
        private Vector3 direction;

        // Update is called once per frame
        void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            direction = new Vector3(horizontal, vertical);
        }

        private void FixedUpdate()
        {
            this.transform.position += direction.normalized * speed * Time.deltaTime;
        }
    }
}
