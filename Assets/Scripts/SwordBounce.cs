using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBounce : MonoBehaviour {
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
            player.gameObject.GetComponent<AttackMovement>().sword_hit = true;
        }
    }
}
