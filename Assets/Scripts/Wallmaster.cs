using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wallmaster : MonoBehaviour
{
    public float animTime;
    public float wallmasterSpeed;
    public bool isBusy = false;
    public bool flippedY = false;
    public bool flippedX = false;
    public bool canMove = true;
    public Sprite spriteClosed, spriteOpen;

    bool hasCaptured = false;

    Vector3 firstDirection, secondDirection;
    static int[] distanceFrom = { 2, 3 };

    GameUtilities utility;
    UnityAction dieAction;
    SpriteRenderer rend;
    Rigidbody rb;
    Coroutine anim, movePlayer, stunning;
    GameObject player;
    BeingAttacked playerAttacked;
    EnemyInventory inventory;
    Camera cam;

    void OnEnable()
    {
        // TODO: Wallmaster sounds for when you enter/exit the room
        utility = Camera.main.GetComponent<GameUtilities>();
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerAttacked = player.GetComponent<BeingAttacked>();
        inventory = GetComponent<EnemyInventory>();
        cam = Camera.main;
        dieAction = Die;
        inventory.RegisterDeathCallbacks(dieAction);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerAttacked.is_captured)
        {

            playerAttacked.is_captured = true;
            hasCaptured = true;
            movePlayer = StartCoroutine(MovePlayer());
            StopCoroutine(anim);
            rend.sprite = spriteClosed;
        }
        else if (inventory.canBeHurt && (other.CompareTag("bullet") || other.CompareTag("sword")))
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
            // TODO: Knocked back
        }
        else if (other.CompareTag("boomer"))
        {
            stunning = StartCoroutine(BoomerStun());
        }
    }

    // Entire Wallmaster movement, destroys self at the end of game
    // NOTICE: linkLocation is the localPosition of the trap
    public IEnumerator GrabLink(Vector3 linkLocation)
    {
        Debug.Log("Grab Link function begun");
        Vector3 startLocation = Vector3.zero;

        if (linkLocation.y == 2)
        { // Bottom edge of the room
            startLocation.y = 33;
            startLocation.x = linkLocation.x + 64 + 
                distanceFrom[Random.Range(0, distanceFrom.Length)];
            firstDirection = Vector3.up;
            secondDirection = Vector3.left;
            flippedX = true;
            flippedY = false;
        }
        else if (linkLocation.y == 8)
        { // Top edge of the room
            startLocation.y = 43;
            startLocation.x = linkLocation.x + 64 + 
                distanceFrom[Random.Range(0, distanceFrom.Length)];
            firstDirection = Vector3.down;
            secondDirection = Vector3.left;
            flippedY = true;
            flippedX = true;
        }
        else if (linkLocation.x == 2)
        { // Left edge of the room
            startLocation.x = 64;
            startLocation.y = linkLocation.y + 33 - 
                distanceFrom[Random.Range(0, distanceFrom.Length)];
            firstDirection = Vector3.right;
            secondDirection = Vector3.up;
            flippedX = false;
            flippedY = false;
        }
        else if (linkLocation.x == 13)
        { // Right edge of the room
            startLocation.x = 79;
            startLocation.y = linkLocation.y + 33 - 
                distanceFrom[Random.Range(0, distanceFrom.Length)];
            firstDirection = Vector3.left;
            secondDirection = Vector3.up;
            flippedX = true;
            flippedY = false;
        }
        else
        {
            Debug.Log("ERROR::INVALID_POSITION_WALLMASTER_TRAP");
            yield break;
        }

        // Place wallmaster in correct start position & begin moving
        transform.localPosition = startLocation;
        anim = StartCoroutine(WallmasterAnimation());

        // Wallmaster move animation
        for (float firstDistMoved = 0; firstDistMoved < 2; firstDistMoved += wallmasterSpeed * Time.deltaTime)
        {
            while(!canMove)
            { yield return null; }
            rb.velocity = firstDirection * wallmasterSpeed;
            yield return null;
        }
        for (float secondDistMoved = 0; secondDistMoved < 3; secondDistMoved += wallmasterSpeed * Time.deltaTime)
        {
            while(!canMove)
            { yield return null; }
            rb.velocity = secondDirection * wallmasterSpeed;
            yield return null;
        }
        for (float firstDistMoved = 0; firstDistMoved < 2.5; firstDistMoved += wallmasterSpeed * Time.deltaTime)
        {
            while(!canMove)
            { yield return null; }
            rb.velocity = -firstDirection * wallmasterSpeed;
            yield return null;
        }
        rb.velocity = Vector3.zero;
        isBusy = false;
        StopCoroutine(anim);
        if (hasCaptured)
        {
            // TODO: Set respawnRequirement to true
            // TODO: Animation change to beginning of room
            cam.transform.position = cam.GetComponent<CameraController>().initPosition;
            playerAttacked.is_captured = false;
            player.transform.position = cam.GetComponent<CameraController>().initPlayerPosition;
            utility.WallmasterReset();
            // Indicator that the room has been left by link
        }
        // TODO: Anything else to do at the end of movement?
    }


    IEnumerator MovePlayer()
    {
        while(playerAttacked.is_captured)
        {
            player.transform.position = transform.position;
            yield return null;
        }
    }


    // Faux-animation for Wallmaster, oscillates sprites
    IEnumerator WallmasterAnimation()
    {
        if (flippedX)
        {
            rend.flipX = true;
        }
        if (flippedY)
        {
            rend.flipY = true;
        }
        while(true)
        {
            rend.sprite = spriteClosed;
            yield return new WaitForSeconds(animTime);
            rend.sprite = spriteOpen;
            yield return new WaitForSeconds(animTime);
        }
    }


    IEnumerator EnemyDamaged(Vector2 directionPushed)
    {
        inventory.canBeHurt = false;
        canMove = false;
        rb.velocity = directionPushed * 7;
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = Color.white;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        canMove = true;
        inventory.canBeHurt = true;
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
        utility.OrderDeathPoof(transform.position);
        gameObject.SetActive(false);
    }
}
