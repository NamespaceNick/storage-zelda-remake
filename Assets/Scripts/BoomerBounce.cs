using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerBounce : MonoBehaviour {
    //public bool boomer_dir = false;
    public GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wall") || other.CompareTag("enemy")
            || other.CompareTag("door"))
        {
            //Debug.Log("enter");
            //Debug.Log(player.GetComponent<AttackMovement>().boomer_dir);
            player.gameObject.GetComponent<AttackMovement>().boomer_dir = true;
            //Debug.Log(player.GetComponent<AttackMovement>().boomer_dir);
        }
    }
}
