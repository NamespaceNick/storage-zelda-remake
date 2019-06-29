using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goriya : MonoBehaviour {
    public Sprite up1, up2, right1, right2;
    public Sprite down1, down2, left1, left2;
    public GameObject boomerang;
    public float goriyaSpeed;
    public float animTime;

    Vector3 dir;

    LayerMask enemyMask = 1 << 8 | 1 << 9 | 1 << 10 | 1 << 11 | 1 << 15 | 1 << 16 | 1 << 14;
    bool canMove = true;
    static Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
    GameObject boomer;
    Rigidbody rb;
    SpriteRenderer rend;
    EnemyInventory inventory;
    UnityAction dieAction;
    Coroutine directionRoutine, stunning;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<SpriteRenderer>();
        inventory = GetComponent<EnemyInventory>();
        dir = directions[Random.Range(0, directions.Length)];
        dieAction = Die;
        inventory.RegisterDeathCallbacks(dieAction);
        directionRoutine = StartCoroutine(DirectionAnimations(dir));
        StartCoroutine(GoriyaMove());
    }

    void OnTriggerEnter(Collider other)
    {
        if (!inventory.canBeHurt)
        {
            return;
        }
        if (other.CompareTag("sword") || other.CompareTag("bullet"))
        {
            inventory.DamageHealth();
            // TODO: Knockback when facing the same way
        }
        if (other.CompareTag("boomer"))
        {
            stunning = StartCoroutine(BoomerStun());
        }
    }

    IEnumerator GoriyaMove()
    {
        while(true)
        {
            if (canMove)
            {
                int numMoves = Random.Range(1, 8);
                Vector3 dir = directions[Random.Range(0, directions.Length)];
                for (int i = 0; i < numMoves; ++i)
                {
                    while (Physics.Raycast(transform.position, dir, 1, ~enemyMask))
                    {
                        //Debug.Log("Getting new direction");
                        dir = directions[Random.Range(0, directions.Length)];
                        yield return null;
                    }
                    StopCoroutine(directionRoutine);
                    directionRoutine = StartCoroutine(DirectionAnimations(dir));
                    for (float distanceMoved = 0; distanceMoved < 1; distanceMoved += goriyaSpeed * Time.deltaTime)
                    {
                        while(!canMove)
                        { yield return null; }
                        rb.velocity = dir * goriyaSpeed;
                        yield return null;
                    }
                }
                yield return null;
            }
            yield return null;
            rb.velocity = Vector3.zero;
            StartCoroutine(GoriyaAttack());
        }
    }

    IEnumerator GoriyaAttack()
    {
        /*
        canMove = false;
        GameObject boomerPrefab = boomerang;
        boomer = (GameObject)Instantiate(
            boomerPrefab, rb.transform.position, rb.transform.rotation);
        float time = 2;
        float width = 0;
        float dist = 2;

        Vector3 pos = transform.position;
        float height = transform.position.z;
        Quaternion q = Quaternion.FromToRotation(Vector3.forward, dir);
        float timer = 0.0f;
        boomer.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 15);
        while (timer < time / 2)// && !boomer_dir)
        {
            float t = Mathf.PI * 2.0f * timer / time - Mathf.PI / 2.0f;
            float x = width * Mathf.Cos(t);
            float y = dist * Mathf.Sin(t);
            Vector3 v = new Vector3(x, height, y + dist);
            boomer.GetComponent<Rigidbody>().MovePosition(pos + (q * v));
            timer += Time.deltaTime;
            yield return null;
        }
        // Debug.Log(boomer_dir);
        while (timer < time && Vector3.Magnitude(rb.transform.position - boomer.GetComponent<Rigidbody>().position) > 0.4f)
        {
            Vector3 d = rb.transform.position - boomer.GetComponent<Rigidbody>().position;
            float speed = Mathf.Max(Vector3.Magnitude(d) / (time - timer), 6);
            boomer.GetComponent<Rigidbody>().MovePosition(boomer.GetComponent<Rigidbody>().position + Vector3.Normalize(d) * speed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // boomer_dir = false;
        Destroy(boomer);
        canMove = true;
        */
        //Debug.Log("Goriya Attack()!");
        yield break;
    }

    IEnumerator DirectionAnimations(Vector3 direction)
    {
        Sprite s1 = left1, s2 = left2;
        if (direction == Vector3.up)
        {
            s1 = up1;
            s2 = up2;
        }
        else if (direction == Vector3.right)
        {
            s1 = right1;
            s2 = right2;
        }
        else if (direction == Vector3.down)
        {
            s1 = down1;
            s2 = down2;
        }
        else
        {
            s1 = left1;
            s2 = left2;
        }

        while (true)
        {
            rend.sprite = s1;
            yield return new WaitForSeconds(animTime);
            rend.sprite = s2;
            yield return new WaitForSeconds(animTime);
        }
    }

    IEnumerator BoomerStun()
    {
        canMove = false;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(inventory.stunTime);
        canMove = true;
    }

    void Die()
    {
        StopAllCoroutines();
        // TODO: Ask for death poof
        gameObject.SetActive(false);
    }
}
