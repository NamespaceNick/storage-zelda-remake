using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowRoomExit : MonoBehaviour {
    public GameObject player, cam;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        cam = GameObject.Find("Main Camera");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.transform.position = new Vector3(23, 60, 0);
            cam.transform.position = new Vector3(23.5f, 62, -10.86f);
        }
    }
}
