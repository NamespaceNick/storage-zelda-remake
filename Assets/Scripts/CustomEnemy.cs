using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEnemy : MonoBehaviour
{
    public int numSpikes;
    public int numFlashes;
    public float customSpeed;
    public float customHealth;
    public float cooldownTime;
    public float flashTime;
    public GameObject spikePrefab;
    public AudioClip customKilled;



    bool isAttacking = false;
    bool isFlashing = false;
    bool onCooldown = false;
    GameObject player;
    Rigidbody rb;
    EnemyInventory inventory;
    Coroutine cooling, attacking, flashing;
    SpriteRenderer rend;
	UnityAction dieAction;
    public List<GameObject> spikeList = new List<GameObject>();
	void Start ()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<EnemyInventory>();
        rend = GetComponent<SpriteRenderer>();
		dieAction = Die;
		inventory.RegisterDeathCallbacks (dieAction);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!onCooldown)
        {
            onCooldown = true;
            cooling = StartCoroutine(Cooldown());
            attacking = StartCoroutine(Attack());
        }
         transform.up = (player.transform.position - transform.position).normalized;
        // transform.up = Vector3.Lerp(transform.up, (player.transform.position - transform.position).normalized, 0.01f);
	}

    // Shoots out spikes from the side which hone in on the player.
    // Spikes can be repelled with the magnet
    IEnumerator Attack()
    {
        spikeList.Clear();
        isAttacking = true;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(1);
        Vector3 placementVector;
        placementVector = transform.right;
        for (int i = 0; i < numSpikes; ++i)
        {
            GameObject newSpike = (GameObject)Instantiate(spikePrefab, transform.position + placementVector, transform.rotation);
            spikeList.Add(newSpike);
            spikeList[spikeList.Count - 1].transform.Rotate(0, 0, -90 + (360 / (numSpikes-1)) * i);
            placementVector = Quaternion.Euler(0, 0, 360 / (numSpikes)) * placementVector;
        }
        yield return null;
        foreach (GameObject s in spikeList)
        {
            s.GetComponent<Spike>().OrderAttack();
        }

        yield return new WaitForSeconds(2);

        isAttacking = false;

        yield break;
    }

    public void Damaged()
    {
        if (inventory.canBeHurt)
        {
            inventory.DamageHealth();
        }

    }


    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }


    IEnumerator HurtFlash()
    {
        if (isFlashing)
        {
            yield break;
        }
        isFlashing = true;

        Debug.Log(rend.material.color);
        for (int numTimes = 0; numTimes < numFlashes; ++numTimes)
        {
            rend.material.color = Color.red;
            yield return new WaitForSeconds(flashTime);
            rend.material.color = Color.white;
            yield return new WaitForSeconds(flashTime);
        }
        isFlashing = false;
    }

	void Die()
	{
		StopAllCoroutines ();
        AudioSource.PlayClipAtPoint(customKilled, Camera.main.transform.position);
		gameObject.SetActive (false);
	}
}
