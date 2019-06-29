using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float transitionSpeed = 10f;
    public float timeWait;
    public RoomEvents roomTrigger;
    public Vector3 initPosition;
    public Vector3 initPlayerPosition;


    Rigidbody rb;
    GameUtilities utility;
    float horizontalTravel = 16;
    float verticalTravel = 11;
    bool isTransitioning = false;

	// Use this for initialization
	void Start ()
    {
		initPosition = transform.position;
        Camera.main.aspect = 256f / 240f;
        player = GameObject.Find("Player");
        if (player == null)
            Debug.LogWarning("Camera failed to find a player object");
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogWarning("Camera failed to find a Rigidbody component");
        initPlayerPosition = new Vector3(39f, 4f, 0);
        utility = GetComponent<GameUtilities>();
	}

    // TODO: Need to animate player walking out of the door
    // TODO: Mark all doorways as triggers

    public IEnumerator RoomTransition(Vector3 travelDirection, int roomEntering, int roomLeaving, bool isTrap)
    {
        if (isTransitioning)
        {
            yield break;
        }
        // Freeze player controls
        player.GetComponent<ArrowKeyMovement>().canWalk = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        float travelDistance = verticalTravel;

        utility.rooms[roomLeaving].RoomExited();
        // Move camera
        if (travelDirection == Vector3.right || travelDirection == Vector3.left)
            travelDistance = horizontalTravel;

        for (float d = 0; d < Mathf.Abs(travelDistance); d += transitionSpeed * Time.deltaTime)
        {
            rb.velocity = travelDirection * transitionSpeed;
            yield return null;
        }
        rb.velocity = Vector3.zero;


        player.GetComponent<Rigidbody>().velocity = travelDirection * player.GetComponent<ArrowKeyMovement>().speed;
        if (isTrap)
        {
            timeWait += 0.25f;
        }
        yield return new WaitForSeconds(timeWait);
        utility.rooms[roomEntering].RoomEntered();
        // Return player controls
        player.GetComponent<ArrowKeyMovement>().canWalk = true;
        yield return null;
    }
}
