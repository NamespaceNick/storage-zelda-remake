using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSword : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("sword"))
        {
            if (GameObject.Find("Bullet"))
            {
                GameObject.Find("Bullet").GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}
