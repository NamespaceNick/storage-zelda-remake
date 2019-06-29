using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransition : MonoBehaviour {
    public bool isTrap = false;
    public int roomEntering;
    public int roomExiting;

    CameraController cam;
    GameUtilities utility;
    GameObject player;
    private Vector3 dirVector;
    private bool dirLeft, dirRight, dirUp, dirDown;


    // Use this for initialization
    void Start () {
        // Determine which direction camera will go based on trigger prefab
        player = GameObject.Find("Player");
        cam = Camera.main.GetComponent<CameraController>();
        dirLeft = dirRight = dirUp = dirDown = false;
        switch (transform.name)
        {
            case "Tile_UPTR":
                dirVector = Vector3.up;
                dirUp = true;
                break;
            case "Tile_DOTR":
                dirVector = Vector3.down;
                dirDown = true;
                break;
            case "Tile_LETR":
                dirVector = Vector3.left;
                dirLeft = true;
                break;
            case "Tile_RITR":
                dirVector = Vector3.right;
                dirRight = true;
                break;
        }
        utility = Camera.main.GetComponent<GameUtilities>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Determine if player is colliding with trigger from correct direction
        if ((dirUp && (collision.transform.position.y < transform.position.y)) ||
            (dirDown && (collision.transform.position.y > transform.position.y)) ||
            (dirLeft && (collision.transform.position.x > transform.position.x)) ||
            (dirRight && (collision.transform.position.x < transform.position.x))
            )
        {
			bool isCustom = (player.GetComponent<BeingAttacked> () == null);
            // WARNING: THIS MIGHT CAUSE PROBLEMS WITH THE CUSTOM LEVEL
			if (isCustom || !player.GetComponent<BeingAttacked>().is_captured)
			{
	            StartCoroutine(cam.RoomTransition(dirVector, roomEntering, roomExiting, isTrap));
			}
        }
    }
}
