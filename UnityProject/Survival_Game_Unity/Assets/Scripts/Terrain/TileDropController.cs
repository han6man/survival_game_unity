using System.Collections;
using UnityEngine;

public class TileDropController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //adds to the player inventory
            Destroy(this.gameObject);
        }        
    }
}
