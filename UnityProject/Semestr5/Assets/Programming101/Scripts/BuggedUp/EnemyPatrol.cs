using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuggedUpCode
{
    /// <summary>
    ///      ** (30 points) ** 
    /// This script makes an enemy patrol back and forth between two points.
    /// When detecting the player (via collider), the enemy follows them.
    /// Fix any issues to make it work as expected.
    /// You can refactor and remove uneeded or unused code
    /// </summary>
    public class EnemyPatrol : MonoBehaviour
    {
        [SerializeField] private Transform pointA, pointB;
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float chaseSpeed = 4f;
        //[SerializeField] private float detectionRange = 5f;
        //[SerializeField] private LayerMask playerLayer;

        private Transform target;
        private bool isChasing = false;
        private Coroutine moveRoutine;

        private void Start()
        {
            target = pointA;
            moveRoutine = StartCoroutine(Patrol());
        }

        private IEnumerator Patrol()
        {
            while (!isChasing)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, target.position) < 0.1f)
                {
                    target = (target == pointA) ? pointB : pointA;
                    yield return new WaitForSeconds(1f); // Pause at each point
                }

                yield return null;
            }
        }


        private void StartChasing(Transform player)
        {
            if (moveRoutine != null)
                StopCoroutine(moveRoutine);

            isChasing = true;
            target = player;
            StartCoroutine(ChasePlayer());
        }

        private IEnumerator ChasePlayer()
        {
            while (isChasing)
            {
                if (target != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
                }
                yield return null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isChasing && other.CompareTag("Player"))
            {
                StartChasing(other.transform);
            }
        }
    }
}
