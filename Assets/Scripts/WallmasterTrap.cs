using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallmasterTrap : MonoBehaviour
{
    public WallmasterRoom room;

    void OnTriggerEnter(Collider other)
    {
        // Spawn a wallmaster and indicate the tile to target
        if(other.CompareTag("Player"))
        {
            Debug.Log("(trigger) Wallmaster trap with player at localPosition" + transform.localPosition);
            room.SpawnWallmaster(transform.localPosition);
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        // Spawn a wallmaster and indicate the tile to target
        if(collision.transform.CompareTag("Player"))
        {
            Debug.Log("(collision) Wallmaster trap with player at localPosition" + transform.localPosition);
            room.SpawnWallmaster(transform.localPosition);
        }
    }
}
