using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BuggedUpCode
{
    /// <summary>
    ///      ** (30 points) ** 
    /// This script should manage an object pool, reusing objects instead of instantiating new ones.
    /// Fix the issues so that objects are correctly reused.
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int poolSize = 15;

        private List<GameObject> pool = new List<GameObject>();

        private void Start()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

        public GameObject GetObject()
        {
            foreach (GameObject obj in pool)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                }
                return obj;
            }
            //return Instantiate(prefab);
            return null;
        }

        public void ReleaseObject(GameObject obj)
        {
            obj?.SetActive(false);
        }
    }
}
