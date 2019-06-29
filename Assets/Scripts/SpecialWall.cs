using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWall : MonoBehaviour
{
    public float necessaryTime = 2f;
    public bool canMove = false;
    public Door specialDoor;

    bool moved = false;
    float elapsed;
    Vector3 initialPosition;
    Rigidbody rb;
    GameUtilities utility;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        utility = Camera.main.GetComponent<GameUtilities>();
        initialPosition = transform.position;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!moved && collision.collider.CompareTag("Player") && canMove)
        {
            elapsed += Time.fixedDeltaTime;
            Debug.Log(elapsed);
            if (elapsed > necessaryTime)
            {
                StartCoroutine(MoveWall());
                moved = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            elapsed = 0;
        }
    }

    IEnumerator MoveWall()
    {
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        while (Mathf.Abs(transform.position.y - initialPosition.y) <=  1.0f)
        {
            yield return null;
        }
        rb.constraints = RigidbodyConstraints.FreezeAll;
        specialDoor.MasterUnlock();
        AudioSource.PlayClipAtPoint(utility.mysteryDiscovered, Camera.main.transform.position);

    }
}