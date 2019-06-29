using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Keese : MonoBehaviour
{
    public Sprite wingsOpen, wingsClosed;
    public float maxSpeed;
    public float maxTimeFast, minTimeFast, maxStopTime, minStopTime;
    public float maxChangeTime, minChangeTime;
    public float diffTime = 0.5f;


    Rigidbody rb;

    public float currSpeed = 0;
    public float changeFrequency;

    static Vector3 upRight = new Vector3(1, 1).normalized;
    static Vector3 downRight = new Vector3(1, -1).normalized;
    static Vector3 downLeft = new Vector3(-1, -1).normalized;
    static Vector3 upLeft = new Vector3(-1, 1).normalized;
    static Vector3[] directions = { Vector3.up, upRight, Vector3.right, downRight,
        Vector3.down, downLeft, Vector3.left, upLeft };
    public Vector3 dir;

    GameUtilities utility;
    UnityAction dieAction;
    EnemyInventory inventory;
    SpriteRenderer rend;
	// Use this for initialization
    void OnEnable()
    {
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<EnemyInventory>();
        StartCoroutine(KeeseSpeedUp());
        StartCoroutine(ChangeDirection());
        StartCoroutine(KeeseAnimation());
        utility = Camera.main.GetComponent<GameUtilities>();
        dieAction = Die;
        inventory.RegisterDeathCallbacks(dieAction);
    }

    // Collisions with the walls
    void OnCollisionEnter(Collision collision)
    {
        dir = collision.contacts[0].normal;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("boomer") || other.CompareTag("sword")
            || other.CompareTag("bullet"))
        {
            inventory.DamageHealth();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        rb.velocity = dir * currSpeed;
	}

    IEnumerator ChangeDirection()
    {
        int i = Random.Range(0, directions.Length);
        dir = directions[i];
        while (true)
        {
            float chance = Random.value;
            i = (chance > 0.5f) ? i + 1 : i - 1;
            if (i == -1) { i = directions.Length - 1; }
            i = i % directions.Length;
            dir = directions[i];
            yield return new WaitForSeconds(Random.Range(minChangeTime, maxChangeTime));
        }
    }

    IEnumerator KeeseSpeedUp()
    {
        float timeFast = Random.Range(minTimeFast, maxTimeFast);
        float t = 0.0f;
        while (currSpeed < maxSpeed - 0.1)
        {
            t += diffTime * Time.deltaTime;
            currSpeed = Mathf.Lerp(currSpeed, maxSpeed, t);
            yield return null;
        }
        // Stays moving fast for random amount of time
        yield return new WaitForSeconds(timeFast);
        StartCoroutine(KeeseSlowDown());
    }


    IEnumerator KeeseSlowDown()
    {
        float timeStop = Random.Range(minStopTime, maxStopTime);
        float t = 0.0f;
        while (currSpeed > 0.1)
        {
            t += diffTime * Time.deltaTime;
            currSpeed = Mathf.Lerp(currSpeed, 0, t);
            yield return null;
        }
        currSpeed = 0;
        yield return new WaitForSeconds(timeStop);
        StartCoroutine(KeeseSpeedUp());
    }

    IEnumerator KeeseAnimation()
    {
        while(true)
        {
            // TODO: Refine the timing for the wings changing
            rend.sprite = wingsOpen;
            yield return new WaitForSeconds(changeFrequency / (currSpeed + 0.1f));
            rend.sprite = wingsClosed;
            yield return new WaitForSeconds(changeFrequency / (currSpeed + 0.1f));
        }
    }

    void Die()
    {
        StopAllCoroutines();
        utility.OrderDeathPoof(transform.position);
        gameObject.SetActive(false);
    }
}
