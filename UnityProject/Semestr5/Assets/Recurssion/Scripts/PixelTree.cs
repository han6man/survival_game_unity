using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recurssion
{
    public class PixelTree : MonoBehaviour
    {
        [SerializeField] private GameObject pixelPrefab; // Assign a 2D sprite (like a square)
        [SerializeField] private int maxDepth = 5; // Tree depth
        [SerializeField] private float branchAngle = 30f; // Angle of branches
        [SerializeField] private float scaleFactor = 0.7f; // Scale reduction per branch

        void Start()
        {
            GenerateTree(transform.position, Quaternion.identity, 1, maxDepth);
        }

        void GenerateTree(Vector2 position, Quaternion rotation, float size, int depth)
        {
            if (depth <= 0) return; // Stop when depth limit is reached

            GameObject pixel = Instantiate(pixelPrefab, position, rotation);
            pixel.transform.localScale = Vector3.one * size;

            // Calculate new position for branches
            Vector2 newPosition = position + (Vector2)(rotation * Vector2.up * size);

            // Generate branches (left and right)
            GenerateTree(newPosition, rotation * Quaternion.Euler(0, 0, branchAngle), size * scaleFactor, depth - 1);
            GenerateTree(newPosition, rotation * Quaternion.Euler(0, 0, -branchAngle), size * scaleFactor, depth - 1);
        }
    }
}
