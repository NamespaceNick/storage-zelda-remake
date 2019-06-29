using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
    public float turnStep;
    public float spikeSpeed;

    bool hasCollided = false;
    bool isAttacking = false;
    bool isDone = false;
    GameObject player;
    AttackMovementCustom custom;
    Rigidbody rb;
    Coroutine atk;
	void Start () {
        player = GameObject.Find("Player");
        if (!player) { Destroy(gameObject); }
        rb = GetComponent<Rigidbody>();
        custom = player.GetComponent<AttackMovementCustom>();
	}


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wall") || other.CompareTag("Player") ||
            other.CompareTag("door"))
        {
            rb.velocity = Vector3.zero;
            foreach (BoxCollider bC in GetComponentsInChildren<BoxCollider>())
            {
                bC.enabled = false;
            }
            if (other.CompareTag("wall"))
            {
                hasCollided = true;
            }
        }
    }


    public void OrderAttack()
    {

        Debug.Log("Attack order given");
        if (isAttacking)
        {
            Debug.Log("Already attacking");
            return;
        }
        isAttacking = true;
        isDone = false;
        atk = StartCoroutine(AttackPlayer());
    }


    IEnumerator AttackPlayer()
    {
        while (!hasCollided)
        {
            Vector3 desiredVector;
            desiredVector = (player.transform.position - transform.position).normalized;
            rb.velocity = transform.up * spikeSpeed;
            if (custom.is_attract)
            { // Spikes are attracted to player, just like sword
                transform.up = Vector3.Lerp(transform.up, desiredVector, turnStep);
            }
            yield return null;
        }
        Destroy(gameObject);
    }


    void OnDestroy()
    {
        StopAllCoroutines();
    }

}
