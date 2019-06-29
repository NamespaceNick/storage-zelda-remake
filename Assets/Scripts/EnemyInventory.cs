using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyInventory : MonoBehaviour {
    public float maxHealth;
    public float hurtCooldown = 0.5f;
    public float stunTime = 2f;
	public bool hasKey = false;
    public bool hasBoomerang = false;
    public bool hasHeart = false;
    public bool hasRupee = false;
    public bool hasBomb = false;
    public bool canBeHurt = true;
    public bool isDead = false;


    GameUtilities utility;
	Transform temporaryTransform;
    // QUESTION : Why do I have to explicitly say new here?
    UnityEvent OnDeath = new UnityEvent();
    public Vector3 initialPosition;
	private float currentHealth;

	void Start () {
        currentHealth = maxHealth;
        initialPosition = transform.position;
        utility = Camera.main.GetComponent<GameUtilities>();
	}

    public void Respawned()
    {
        currentHealth = maxHealth;
        transform.position = initialPosition;
        isDead = false;
    }

    // Decrements health of the entity by the given amount
    // TODO: Resolve death situation here
    public void DamageHealth()
    {
        if (canBeHurt)
        {
            canBeHurt = false;
            StartCoroutine(HurtCooldown());
            currentHealth -= 1;
            if (currentHealth <= 0)
            {
                AudioSource.PlayClipAtPoint(utility.enemyKilled, Camera.main.transform.position);
                Die();
            }
            else
            {
                AudioSource.PlayClipAtPoint(utility.enemyHurt, Camera.main.transform.position);
            }
        }
    }

    public void RegisterDeathCallbacks(UnityAction callback)
    {
        OnDeath.AddListener(callback);
    }

    IEnumerator HurtCooldown()
    {
        yield return new WaitForSeconds(hurtCooldown);
        canBeHurt = true;
    }

    // Returns health of the entity
    public float GetHealth ()
	{
		return currentHealth;
	}

    void Die()
    {
        // TODO: Needs to be redone to be generalized for all enemies
        if (hasKey)
            Instantiate(utility.key, transform.position, Quaternion.identity);
        if (hasBomb)
            Instantiate(utility.bomb, transform.position, Quaternion.identity);
        if (hasRupee)
            Instantiate(utility.rupee, transform.position, Quaternion.identity);
        if (hasBoomerang)
            Instantiate(utility.boomerang, transform.position, Quaternion.identity);
        isDead = true;
        OnDeath.Invoke();
    }
}
