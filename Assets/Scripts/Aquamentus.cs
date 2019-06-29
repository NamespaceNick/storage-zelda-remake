using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Aquamentus : MonoBehaviour
{
    public Sprite closed1, closed2, open1, open2;
    public float cooldown;
    public float animTime;
    public float aquamentusSpeed;

    bool onCooldown = false;
    bool isAttacking = false;
    GameUtilities utility;
    EnemyInventory inventory;
    UnityAction dieAction;
    Vector3 direction;
    Rigidbody rb;
    SpriteRenderer rend;
    Coroutine anim, move, attack, cooling;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<SpriteRenderer>();
        utility = Camera.main.GetComponent<GameUtilities>();
        direction = Vector3.left;
        inventory = GetComponent<EnemyInventory>();
        move = StartCoroutine(AquamentusMovement());
        anim = StartCoroutine(AquamentusAnimation());
        dieAction = Die;
        inventory.RegisterDeathCallbacks(dieAction);
    }

    void Update()
    {
        if (!onCooldown)
        {
            onCooldown = true;
            isAttacking = true;
            cooling = StartCoroutine(CooldownTimer());
            attack = StartCoroutine(AquamentusAttack());
        }
    }

    // TODO: Might need to change to trigger when we figure out player/enemy trigger/collider
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            rb.velocity = -direction * aquamentusSpeed;
        }
    }

    public void Damaged()
    {
        if (inventory.canBeHurt)
        {
            AudioSource.PlayClipAtPoint(utility.aquamentusHurt, Camera.main.transform.position);
            inventory.DamageHealth();
        }
    }


    IEnumerator AquamentusMovement()
    {
        while(true)
        {
            float timeMoving = Random.Range(2f, 5f);
            rb.velocity = direction * aquamentusSpeed;
            yield return new WaitForSeconds(timeMoving);
            direction *= -1;
        }
    }

    IEnumerator AquamentusAnimation()
    {
        // TODO: Will this be problematic when I try to stop it? Like with Wallmaster?
        while (true)
        {
            if (isAttacking)
            { rend.sprite = open1; }
            else
            { rend.sprite = closed1; }
            yield return new WaitForSeconds(animTime);
            if (isAttacking)
            { rend.sprite = open2; }
            else
            { rend.sprite = closed2; }
            yield return new WaitForSeconds(animTime);
        }
    }

    IEnumerator AquamentusAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1);
        isAttacking = false;
        GameObject topFireball = (GameObject)Instantiate(utility.fireball,
            transform.position, transform.rotation);
        GameObject middleFireball = (GameObject)Instantiate(utility.fireball,
            transform.position, transform.rotation);
        GameObject bottomFireball = (GameObject)Instantiate(utility.fireball,
            transform.position, transform.rotation);
        Vector3 playerDirection = (GameObject.Find("Player").transform.position - transform.position).normalized;
        Vector3 abovePlayer = Quaternion.Euler(0, 0, -30) * playerDirection;
        Vector3 belowPlayer = Quaternion.Euler(0, 0, 30) * playerDirection;
        topFireball.GetComponent<Fireball>().OrderFireballMove(Vector3.up, abovePlayer);
        middleFireball.GetComponent<Fireball>().OrderFireballMove(Vector3.zero, playerDirection);
        bottomFireball.GetComponent<Fireball>().OrderFireballMove(Vector3.down, belowPlayer);
        yield break; // TODO: Delete this
    }

    IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    void Die()
    {
        utility.aquamentusDead = true;
        StopAllCoroutines();
        utility.OrderDeathPoof(transform.position);
        gameObject.SetActive(false);
    }
}
