using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWall2 : MonoBehaviour
{
    public float necessaryTime = 2f;
    public Door specialDoor;

    float elapsed;
    bool moved = false;
    Rigidbody rb;
    GameUtilities utility;
    Vector3 initialPosition;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        utility = Camera.main.GetComponent<GameUtilities>();
        initialPosition = transform.position;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!moved && collision.collider.CompareTag("Player"))
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
        Debug.Log("wall move");
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        while (Mathf.Abs(rb.transform.position.y - initialPosition.y) <= 1.0f)
        {
            yield return null;
        }
        rb.constraints = RigidbodyConstraints.FreezeAll;
        AudioSource.PlayClipAtPoint(utility.mysteryDiscovered, Camera.main.transform.position);
    }
}