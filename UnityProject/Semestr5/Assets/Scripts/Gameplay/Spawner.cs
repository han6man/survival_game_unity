using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceInvaders.Logic;

namespace SpaceInvaders.Gameplay
{
    public class Spawner : MonoBehaviour
    {
        public GameObject testObject;

        private void Start()
        {
            var grid = new MapGrid();
            Spawn(testObject, grid.VectorPositions[0]);
        }
        public void Spawn(GameObject objectToSpawn, Vector2 spawnPosition)
        {
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
