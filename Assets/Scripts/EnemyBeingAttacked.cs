using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeingAttacked : MonoBehaviour {


	Rigidbody rb;
	EnemyInventory inventory;
    Renderer renderer;
    Stalfos enemyMovement;

    readonly Coroutine enemyBlinking;
	// Use this for initialization
	void Start () {
		inventory = GetComponent<EnemyInventory> ();
        enemyMovement = this.GetComponent<Stalfos>();
        renderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
	}

    // Hit by Link's weapons
    // TODO: Should the weapons be triggers?
	private void OnTriggerEnter(Collider other)
	{
        // TODO: Change "sword" tag to "weapon" tag
        if (other.gameObject.tag == "sword")
        {
            // Determine direction of collision
            Vector3 direction = transform.position - other.gameObject.transform.position;
            Vector3 directionMove;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    Debug.Log("Hit from the left");
                    directionMove = Vector3.right;
                }
                else
                {
                    Debug.Log("Hit from the right");
                    directionMove = Vector3.left;
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    Debug.Log("Hit from below");
                    directionMove = Vector3.up;
                }
                else
                {
                    Debug.Log("Hit from above");
                    directionMove = Vector3.down;
                }
            }
            StartCoroutine(EnemyDamaged(directionMove));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator EnemyDamaged(Vector2 directionPushed)
    {
        inventory.canBeHurt = false;
        // TODO: Clean this up
        enemyMovement.canMove = false;
        rb.velocity = directionPushed * 7;
        renderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        renderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
        renderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        renderer.enabled = true;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        renderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        renderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
        enemyMovement.canMove = true;
        inventory.canBeHurt = true;
    }
}
