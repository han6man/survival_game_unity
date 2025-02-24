using UnityEngine;

namespace Recurssion
{
    public class MissileSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject missilePrefab;
        [SerializeField] private int maxSplits = 3;
        [SerializeField] private float launchForce = 5f; 

        void Start()
        {
            SpawnMissiles(transform.position, maxSplits);
        }

        public void SpawnMissiles(Vector3 position, int remainingSplits)
        {
            if (remainingSplits <= 0) return;

            GameObject missile1 = Instantiate(missilePrefab, position, Quaternion.Euler(0, 0, 20));
            GameObject missile2 = Instantiate(missilePrefab, position, Quaternion.Euler(0, 0, -20));
            missile1.GetComponent<Missile>().Launch(remainingSplits - 1);
            missile2.GetComponent<Missile>().Launch(remainingSplits - 1);
        }
    }
}
