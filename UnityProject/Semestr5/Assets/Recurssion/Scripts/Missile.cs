using System.Collections;
using UnityEngine;

namespace Recurssion
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f; 

        public void Launch(int remainingSplits)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
            float randomSideForce = Random.Range(-2, 2);
            rb.AddForce(new Vector2(randomSideForce, 0), ForceMode2D.Impulse);
            StartCoroutine(DelayedSplit(remainingSplits));
        }

        IEnumerator DelayedSplit(int remainingSplits)
        {
            yield return new WaitForSeconds(1f); 
            FindObjectOfType<MissileSpawner>().SpawnMissiles(transform.position, remainingSplits);
            Destroy(gameObject); 
        }
    }
}
