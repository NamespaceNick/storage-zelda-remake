using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Sprite fireball1, fireball2;
    public float fireballSpeed;

    float animTime = 0.04f;
    float setupTime = 0.1f;
    SpriteRenderer rend;
    Rigidbody rb;
    Coroutine anim;

	void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<SpriteRenderer>();
        anim = StartCoroutine(FireballAnimation());
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wall") || other.CompareTag("door") ||
            other.CompareTag("Player"))
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    public void OrderFireballMove(Vector3 firstDirection, Vector3 secondDirection)
    {
        StartCoroutine(FireballMove(firstDirection, secondDirection));
    }

    // TODO: IF TIME: Synchronize x-component speed of all 3 fireballs
    IEnumerator FireballMove(Vector3 firstDirection, Vector3 secondDirection)
    {
        rb.velocity = firstDirection * fireballSpeed;
        yield return new WaitForSeconds(setupTime);
        rb.velocity = secondDirection * fireballSpeed;
    }

    // Faux-animation for fireball
    IEnumerator FireballAnimation()
    {
        while(true)
        {
            rend.sprite = fireball1;
            yield return new WaitForSeconds(animTime);
            rend.sprite = fireball2;
            yield return new WaitForSeconds(animTime - 0.02f);
        }
    }
}
