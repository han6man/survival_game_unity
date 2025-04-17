using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new BeeSword Tool Class", menuName = "Item/Tool/SpawnSword")]
public class SpawnSwordClass : ToolClass
{
    [SerializeField] private GameObject spawnObject;

    public override void Use(PlayerController caller)
    {
        base.Use(caller);
        //Debug.Log("BeeSword Time");
        Instantiate(spawnObject, caller.transform.position, Quaternion.identity);
    }
}
