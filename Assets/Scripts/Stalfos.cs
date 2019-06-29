using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stalfos : MonoBehaviour
{
    // TODO: Reorganize into generic EnemyMovement script

    public float changeDirectionTime = 1.0f;
    public float moveSpeed = 3.0f;
    public float animationWait = 0.4f;
    public bool canMove = true;

    Rigidbody rb;
    SpriteRenderer sprite;
    EnemyInventory inventory;
    UnityAction dieAction;
    Coroutine stunning;
    Vector2 dir;
    Vector2[] directions = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    float[] distances = { 1f, 1.5f, 2f, 2.5f, 3f };

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        inventory = GetComponent<EnemyInventory>();
        dieAction = Die;
        inventory.RegisterDeathCallbacks(dieAction);

        StartCoroutine(StalfosMove());
        StartCoroutine(FlipSprite(animationWait));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("sword") || other.CompareTag("bullet"))
        {
            if (inventory.canBeHurt)
            {
                inventory.DamageHealth();
                Vector3 direction = transform.position - other.transform.position;
                Vector3 directionMove;
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                { // Hit came from horizontal plane
                    if (direction.x > 0)
                    { directionMove = Vector3.right; }
                    else
                    { directionMove = Vector3.left; }
                }
                else
                { // Hit came from vertical plane
                    if (direction.y > 0)
                    { directionMove = Vector3.up; }
                    else
                    { directionMove = Vector3.down; }
                }
                if (inventory.GetHealth() > 0)
                {
                    StartCoroutine(EnemyDamaged(directionMove));
                }
            }
        }
        else if (other.CompareTag("boomer"))
        {
            stunning = StartCoroutine(BoomerStunned());
        }
    }

    // Update is called once per frame
    // Manually flips Stalfos sprite on the x axis, faux-animating
    IEnumerator FlipSprite(float timeBetweenFlips)
    {
        bool spriteFlipped = true;
        while (true)
        {
            sprite.flipX = spriteFlipped;
            spriteFlipped = !spriteFlipped;
            yield return new WaitForSeconds(timeBetweenFlips);
        }

    }

    // Moves Stalfos in random directions, changes direction on collisions
    // TODO: Prevent Stalfos from going into other rooms/doorways
    IEnumerator StalfosMove()
    {
        float chosenDistance;
        while (true)
        {
            chosenDistance = distances[Random.Range(0, distances.Length)];
            if (canMove)
            {
                dir = directions[Random.Range(0, directions.Length)];
                for (float distanceMoved = 0; distanceMoved < chosenDistance; distanceMoved += moveSpeed * Time.deltaTime)
                {
                    if (!canMove)
                        break;
                    rb.velocity = dir * moveSpeed;
                    yield return null;
                }
                // Just finished moving left or right
                if (dir.normalized == Vector2.right || dir.normalized == Vector2.left)
                {
                    // Move to a vertical gridline
                    float dist = ClosedHorizontalGridline(rb);
                    if (Mathf.Round(Mathf.Abs(dist) * 10f) / 10f != 0.0f && Mathf.Round(Mathf.Abs(dist) * 10f) / 10f != 0.5)
                    {
                        dir = Vector2.right;

                        if (dist > 0)
                        {
                            dir = Vector2.left;
                        }
                        for (float distanceMoved = 0; distanceMoved < Mathf.Abs(dist); distanceMoved += moveSpeed * Time.deltaTime)
                        {
                            // TODO: Clean this up
                            if (!canMove)
                                break;
                            rb.velocity = dir * moveSpeed;
                            yield return null;
                        }
                    }
                }
                // Just finished moving up or down
                else if (dir.normalized == Vector2.up || dir.normalized == Vector2.down)
                {
                    // Align with a horizontal gridline
                    float dist = ClosedVerticalGridline(rb);
                    if (Mathf.Round(Mathf.Abs(dist) * 10f) / 10f != 0.0f && Mathf.Round(Mathf.Abs(dist) * 10f) / 10f != 0.5)
                    {
                        dir = Vector2.up;

                        if (dist > 0)
                        {
                            dir = Vector2.down;
                        }
                        for (float distanceMoved = 0; distanceMoved < Mathf.Abs(dist); distanceMoved += moveSpeed * Time.deltaTime)
                        {
                            if (!canMove)
                                break;
                            rb.velocity = dir * moveSpeed;
                            yield return null;
                        }
                    }

                }
            }
            yield return null;
        }
    }

    IEnumerator EnemyDamaged(Vector2 directionPushed)
    {
        inventory.canBeHurt = false;
        canMove = false;
        rb.velocity = directionPushed * 7;
        sprite.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        sprite.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.material.color = Color.white;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        sprite.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        canMove = true;
        inventory.canBeHurt = true;
    }

    IEnumerator BoomerStunned()
    {
        canMove = false;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(inventory.stunTime);
        canMove = true;
    }


    // TODO: Generalize gridline code to be accessible to all entities
    private float ClosedHorizontalGridline(Rigidbody rb)
    {
        if(Mathf.Abs(rb.gameObject.transform.position.x - Mathf.RoundToInt(rb.gameObject.transform.position.x)) <=
            Mathf.Abs(rb.gameObject.transform.position.x - Mathf.FloorToInt(rb.gameObject.transform.position.x) - 0.5f))
        {
            return rb.gameObject.transform.position.x - Mathf.RoundToInt(rb.gameObject.transform.position.x);
        } else
        {
            return rb.gameObject.transform.position.x - Mathf.FloorToInt(rb.gameObject.transform.position.x) - 0.5f;
        }
    }

    private float ClosedVerticalGridline(Rigidbody rb)
    {
        if (Mathf.Abs(rb.gameObject.transform.position.y - Mathf.RoundToInt(rb.gameObject.transform.position.y)) <=
             Mathf.Abs(rb.gameObject.transform.position.y - Mathf.FloorToInt(rb.gameObject.transform.position.y) - 0.5f))
        {
            return rb.gameObject.transform.position.y - Mathf.RoundToInt(rb.gameObject.transform.position.y);
        }
        else
        {
            return rb.gameObject.transform.position.y - Mathf.FloorToInt(rb.gameObject.transform.position.y) - 0.5f;
        }
    }

    void Pausing()
    {
        StopAllCoroutines();
    }

    void Die()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
