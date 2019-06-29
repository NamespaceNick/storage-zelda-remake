using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlock : MonoBehaviour {
    public bool isLocked = false;


    GameObject player;

    // TODO: Door animation change when unlocked
    // TODO: Make usable door prefab
    // TODO: Possibly find better place for this code
	void Start () {
        player = GameObject.Find("Player");
        if (player == null)
            Debug.LogWarning("DoorUnlock failed to find a player object");
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("door in collision");
        if (collision.gameObject.CompareTag("Player") && (player.GetComponent<Inventory>().GetKeys() >= 1))
        {
            player.GetComponent<Inventory>().DeductKey();
        }
    }
}
