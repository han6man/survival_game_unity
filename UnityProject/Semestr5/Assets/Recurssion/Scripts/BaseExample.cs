using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recurssion
{
    public class BaseExample : MonoBehaviour
    {
        void Start()
        {
            RecursiveCounting(10);
        }

        void RecursiveCounting(int number)
        {
            if (number <= 0) return; // Base Case: Stop when number reaches 0
            Debug.Log("Counting: " + number);
            RecursiveCounting(number - 1); // Recursive call
        }

    }
}
