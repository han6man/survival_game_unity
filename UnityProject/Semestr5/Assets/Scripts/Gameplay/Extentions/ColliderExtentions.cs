using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    public static class ColliderExtentions 
    {
        public static void TryInteract(this Collider2D col)
        {
            col.TryGetComponent(out IInteractable interactable);
            interactable?.Interact();
        }

        public static void ResetVec(this Vector2 vec)
        {
            vec = Vector2.zero;
        }

        //public static string GetInteractableName(this IInteractable interactable)
        //{
        //    return interactable
        //}
    }
}
