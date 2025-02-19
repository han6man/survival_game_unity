using SpaceInvaders.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        public float interactionDistance = 2f;
        public LayerMask interactableLayer; // Set this in the inspector

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteract();
            }
        }

        void TryInteract()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, interactionDistance, interactableLayer);
            hit.collider?.TryInteract();
        }

        private void Start()
        {
            int num = 1;
            print("num: " + num);

            TestWithNumbers(out num);
            print("num: " + num);

        }

        private void TestWithNumbers(out int number)
        {
            number = 100;
        }
    }

    public interface IInteractable
    {
        void Interact();
    }
}
