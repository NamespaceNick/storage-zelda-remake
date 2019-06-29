using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gel : MonoBehaviour
{
    public Sprite sprite1, sprite2;
    public float speed;
    public float animTime;
    public float distance = 1f;
    public float minMoveTime, maxMoveTime;
    LayerMask enemyMask = 1 << 8 | 1 << 9 | 1 << 10;

    static Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
    Vector3 direction;
    EnemyInventory inventory;
    SpriteRenderer rend;
    Rigidbody rb;
    UnityAction dieAction;

	// Use this for initialization

    void OnEnable()
    {
        inventory = GetComponent<EnemyInventory>();
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        dieAction = Die;
        inventory.RegisterDeathCallbacks(dieAction);
        StartCoroutine(GelAnimation());
        StartCoroutine(GelMove());
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("sword") || 
            collision.transform.CompareTag("boomer") ||
            collision.transform.CompareTag("bullet"))
        {
            inventory.DamageHealth();
        }
        
    }


    IEnumerator GelAnimation()
    {
        while(true)
        {
            rend.sprite = sprite1;
            yield return new WaitForSeconds(animTime);
            rend.sprite = sprite2;
            yield return new WaitForSeconds(animTime);
        }
    }

    IEnumerator GelMove()
    {
        while(true)
        {
            int numMoves = Random.Range(0, 4);
            Vector3 dir = directions[Random.Range(0, directions.Length)];
            for (int i = 0; i < numMoves; ++i)
            {
                while (Physics.Raycast(transform.position, dir, distance, enemyMask))
                {
                    //Debug.Log("Getting new direction");
                    dir = GetDirection();
                    yield return null;
                }
                for (float distanceMoved = 0; distanceMoved < 1; distanceMoved += speed * Time.deltaTime)
                {
                    rb.velocity = dir * speed;
                    yield return null;
                }
                rb.velocity = Vector3.zero;
                yield return new WaitForSeconds(Random.Range(minMoveTime, maxMoveTime));
            }
        }
    }

    Vector3 GetDirection()
    {
        return directions[Random.Range(0, directions.Length)];
    }

    void Die()
    {
        StopAllCoroutines();
        // TODO: Ask for death poof
        gameObject.SetActive(false);

    }
}
