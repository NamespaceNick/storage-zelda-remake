using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUnlock : MonoBehaviour
{
    public GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("door in collision");
        if (other.CompareTag("sword"))
        {
            door.GetComponent<CustomDoor>().Unlock();
        }
    }
}
