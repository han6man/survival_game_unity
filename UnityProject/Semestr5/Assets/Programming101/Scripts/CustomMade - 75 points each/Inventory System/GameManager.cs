using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Programming101
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public ItemManager itemManager;
        public Player player;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(this.gameObject);

            itemManager = GetComponent<ItemManager>();
            player = FindObjectOfType<Player>();
        }
    }
}
