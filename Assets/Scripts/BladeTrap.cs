using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrap : MonoBehaviour
{
    static LayerMask playerLayer = 1 << 10;

    public float speedAttack, speedRetract;
    public float horizontalDistance, verticalDistance;
    public bool upOption, rightOption, downOption, leftOption;


    bool waiting = true;
    Vector3 originPosition;
    Vector3 rayStart;
    Rigidbody rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originPosition = transform.position;
        rayStart = transform.position;
        SetRayStart();
    }

    void Update ()
    {
        if (waiting)
        {
            if (upOption)
            {
                RaycastHit hitUp;
                if (Physics.Raycast(transform.position, Vector3.up, out hitUp, verticalDistance * 2, playerLayer))
                {
                    Debug.Log("Up hit something");
                    StartCoroutine(BladeAttack(Vector3.up));
                }
            }
            if (rightOption)
            {
                RaycastHit hitRight;
                if (Physics.Raycast(transform.position, Vector3.right, out hitRight, horizontalDistance * 2, playerLayer))
                {
                    Debug.Log("Right hit something");
                    StartCoroutine(BladeAttack(Vector3.right));
                }
            }
            if (downOption)
            {
                RaycastHit hitDown;
                if (Physics.Raycast(transform.position, Vector3.down, out hitDown, verticalDistance * 2, playerLayer))
                {
                    Debug.Log("Down hit something");
                    StartCoroutine(BladeAttack(Vector3.down));
                }
            }
            if (leftOption)
            {
                RaycastHit hitLeft;
                if (Physics.Raycast(transform.position, Vector3.left, out hitLeft, horizontalDistance * 2, playerLayer))
                {
                    Debug.Log("Left hit something");
                    StartCoroutine(BladeAttack(Vector3.left));
                }
            }


        }
	}

    IEnumerator BladeAttack(Vector3 directionAttack)
    {
        // Go all the way to the middle point of the room
        // Retract until back at starting point
        // Begin looking for player again
        Debug.Log("In Blade Attack()");
        waiting = false;
        float distance = 0;
        if ((directionAttack == Vector3.up) || (directionAttack == Vector3.down))
        {
            distance = verticalDistance;
        }
        else if ((directionAttack == Vector3.left) || (directionAttack == Vector3.right))
        {
            distance = horizontalDistance;
        }
        else
        {
            Debug.Log("ERROR:BLADETRAP COULDN'T FIND DIRECTIONATTACK");

            distance = verticalDistance;
        }


        for (float distCovered = 0; distCovered < distance; distCovered += speedAttack * Time.deltaTime)
        {
            rb.velocity = directionAttack.normalized * speedAttack;
            Debug.Log(rb.velocity);
            yield return null;
        } // Finished going to center of room, need to return
        float origDist = Vector3.Distance(originPosition, transform.position);
        for (float distCov = 0; distCov < origDist; distCov += speedRetract * Time.deltaTime)
        {
            rb.velocity = -directionAttack.normalized * speedRetract;
            yield return null;
        } // Has returned to original starting point
        rb.velocity = Vector3.zero;
        rb.position = originPosition;
        waiting = true;
    }

    void SetRayStart()
    {
        if (upOption)
        {
            rayStart += new Vector3(0, 0.5f, 0);
        }
        if (rightOption)
        {
            rayStart += new Vector3(0.5f, 0, 0);
        }
        if (leftOption)
        {
            rayStart += new Vector3(-0.5f, 0, 0);
        }
        if (downOption)
        {
            rayStart += new Vector3(0, -0.5f, 0);
        }
    }
}
