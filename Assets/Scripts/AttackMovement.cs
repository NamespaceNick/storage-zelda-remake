using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMovement : MonoBehaviour
{
    Rigidbody rb;
    GameObject[] attacks = new GameObject[12];
    GameObject boomer;
    public bool canAttack = true, isAttacking = false;
    public bool boomer_dir = false, sword_hit = false, sword_out = false;
    Inventory inventory;
    public GameObject bullet_down, bullet_left, bullet_up, bullet_right;
    public GameObject arrow_down, arrow_left, arrow_up, arrow_right;
    public GameObject boomerang, bombPrefab;
    public Vector3 dir = new Vector3(0, -1, 0);
    public AudioClip sword_wield, bomb_explode, sword_explode;
    public GameObject player;

    BeingAttacked beingAttacked;
    BoxCollider col;
    //public AnimationCurve ease_curve;
    // Use this for initialization
    void Start()
    {
        inventory = GetComponent<Inventory>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        beingAttacked = GetComponent<BeingAttacked>();
        attacks[0] = rb.gameObject.transform.GetChild(0).gameObject;
        attacks[1] = rb.gameObject.transform.GetChild(1).gameObject;
        attacks[2] = rb.gameObject.transform.GetChild(2).gameObject;
        attacks[3] = rb.gameObject.transform.GetChild(3).gameObject;
        attacks[4] = rb.gameObject.transform.GetChild(8).gameObject;
        attacks[5] = rb.gameObject.transform.GetChild(9).gameObject;
        attacks[6] = rb.gameObject.transform.GetChild(10).gameObject;
        attacks[7] = rb.gameObject.transform.GetChild(11).gameObject;
        attacks[8] = rb.gameObject.transform.GetChild(12).gameObject;
        attacks[9] = rb.gameObject.transform.GetChild(13).gameObject;
        attacks[10] = rb.gameObject.transform.GetChild(14).gameObject;
        attacks[11] = rb.gameObject.transform.GetChild(15).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) && canAttack)
        {
            inventory.SwitchWeapon();
        }

        int facing = rb.gameObject.GetComponent<ArrowKeyMovement>().facing;

        if (Input.GetKeyDown(KeyCode.X) && canAttack)
        {
            rb.gameObject.GetComponent<ArrowKeyMovement>().canWalk = false;
            rb.velocity = Vector3.zero;
            AudioSource.PlayClipAtPoint(sword_wield, Camera.main.transform.position);
            StartCoroutine(Attack(facing));
            if (inventory.GetHealth() == 3 && !sword_out)
            {
                sword_out = true;
                StartCoroutine(Fire(facing));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z) && canAttack) { 
            if (inventory.GetWeapon() == "Bow")
            {
                if (inventory.GetRupees() <= 0 && !beingAttacked.god_mode) inventory.SwitchWeapon();
                else
                {
                    if (!beingAttacked.god_mode)
                    {
                        inventory.DeductRupee();
                    }
                    StartCoroutine(Shoot(facing));
                    Bow(facing);
                }
            }
            else if (inventory.GetWeapon() == "Boomerang")
            {
                boomer_dir = false;
                StartCoroutine(Boomer(facing));
            }
            else if (inventory.GetWeapon() == "Bomb")
            {
                StartCoroutine(Bomb(facing));
            }
        }
        //Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        //Debug.DrawRay(boomer.transform.position, boomer.transform.TransformDirection(Vector3.forward), Color.green);
    }

    IEnumerator Attack(int i)
    {
        canAttack = false;
        isAttacking = true;
        attacks[i].SetActive(true);
        rb.gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(.3f);
        rb.gameObject.GetComponent<Renderer>().enabled = true;
        attacks[i].SetActive(false);
        canAttack = true;
        isAttacking = false;
        rb.gameObject.GetComponent<ArrowKeyMovement>().canWalk = true;
    }

    IEnumerator Shoot(int i)
    {
        canAttack = false;
        attacks[i+4].SetActive(true);
        yield return new WaitForSeconds(.3f);
        attacks[i+4].SetActive(false);
        yield return new WaitForSeconds(.3f);
        canAttack = true;
    }

    IEnumerator Fire(int facing)
    {
        canAttack = false;
        Vector3 dir = new Vector3(0, -1, 0);
        GameObject bulletPrefab = bullet_down;
        if (facing == 0)
        {
            bulletPrefab = bullet_down;
            dir = new Vector3(0, -1, 0);
        }
        else if (facing == 1)
        {
            bulletPrefab = bullet_left;
            dir = new Vector3(-1, 0, 0);
        }
        else if (facing == 2)
        {
            bulletPrefab = bullet_up;
            dir = new Vector3(0, 1, 0);
        }
        else if (facing == 3)
        {
            bulletPrefab = bullet_right;
            dir = new Vector3(1, 0, 0);
        }

        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            rb.transform.position,
            rb.transform.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.TransformDirection(dir) * 10;
        sword_hit = false;
        while (!sword_hit) {
            yield return null;
        }
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        AudioSource.PlayClipAtPoint(sword_explode, Camera.main.transform.position);

        bullet.gameObject.GetComponent<Renderer>().enabled = false;
        bullet.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        bullet.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        bullet.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        bullet.gameObject.transform.GetChild(3).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        sword_hit = false;
        Destroy(bullet);
        sword_out = false;
        canAttack = true;
    }

    void Bow(int facing)
    {
        Vector3 dir = new Vector3(0, -1, 0);
        GameObject arrowPrefab = arrow_down;
        if (facing == 0)
        {
            arrowPrefab = arrow_down;
            dir = new Vector3(0, -1, 0);
        }
        else if (facing == 1)
        {
            arrowPrefab = arrow_left;
            dir = new Vector3(-1, 0, 0);
        }
        else if (facing == 2)
        {
            arrowPrefab = arrow_up;
            dir = new Vector3(0, 1, 0);
        }
        else if (facing == 3)
        {
            arrowPrefab = arrow_right;
            dir = new Vector3(1, 0, 0);
        }

        var arrow = (GameObject)Instantiate(
            arrowPrefab,
            rb.transform.position,
            rb.transform.rotation);

        arrow.GetComponent<Rigidbody>().velocity = arrow.transform.TransformDirection(dir) * 15;
        Destroy(arrow, 2.0f);
    }

    IEnumerator Boomer(int facing)
    {
        canAttack = false;
        rb.velocity = Vector3.zero;
        //rb.gameObject.GetComponent<ArrowKeyMovement>().canWalk = false;

        dir = new Vector3(0, -1, 0);
        GameObject boomerPrefab = boomerang;
        if (facing == 0)
        {
            dir = new Vector3(0, -1, 0);
        }
        else if (facing == 1)
        {
            dir = new Vector3(-1, 0, 0);
        }
        else if (facing == 2)
        {
            dir = new Vector3(0, 1, 0);
        }
        else if (facing == 3)
        {
            dir = new Vector3(1, 0, 0);
        }

        boomer = (GameObject)Instantiate(
            boomerPrefab,
            rb.transform.position,
            rb.transform.rotation);
        
        float time = 2;
        float width = 0;
        float dist = 2;

        Vector3 pos = transform.position;
        float height = transform.position.z;
        Quaternion q = Quaternion.FromToRotation(Vector3.forward, dir);
        float timer = 0.0f;
        boomer.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 15);
        Debug.Log(boomer_dir);
        while (timer < time / 2 && !boomer_dir)
        {
            float t = Mathf.PI * 2.0f * timer / time - Mathf.PI / 2.0f;
            float x = width * Mathf.Cos(t);
            float y = dist * Mathf.Sin(t);
            Vector3 v = new Vector3(x, height, y + dist);
            boomer.GetComponent<Rigidbody>().MovePosition(pos + (q * v));
            timer += Time.deltaTime;
            yield return null;
        }
        Debug.Log(boomer_dir);
        while (timer < time && Vector3.Magnitude(rb.transform.position - boomer.GetComponent<Rigidbody>().position) > 0.4f)
        {
            Vector3 d = rb.transform.position - boomer.GetComponent<Rigidbody>().position;
            float speed = Mathf.Max(Vector3.Magnitude(d) / (time - timer), 6);
            boomer.GetComponent<Rigidbody>().MovePosition(boomer.GetComponent<Rigidbody>().position + Vector3.Normalize(d) * speed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        boomer_dir = false;
        Destroy(boomer);
        canAttack = true;
    }

    IEnumerator Bomb(int i)
    {
        canAttack = false;
        Vector3 pos = new Vector3(0, 5, 0);
        canAttack = false;

        attacks[i+8].SetActive(true);
        rb.gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(.3f);
        rb.gameObject.GetComponent<Renderer>().enabled = true;
        attacks[i+8].SetActive(false);

        canAttack = true;
        float distance = 0.8f;
        if (i == 0)
        {
            pos = new Vector3(0.0f, -distance, 0.0f);
        }
        else if (i == 1)
        {
            pos = new Vector3(-distance, 0.0f, 0.0f);
        }
        else if (i == 2)
        {
            pos = new Vector3(0.0f, distance, 0.0f);
        }
        else if (i == 3)
        {
            pos = new Vector3(distance, 0.0f, 0.0f);
        }
        Debug.Log(dir);
        var bomb = (GameObject)Instantiate(
            bombPrefab, 
            rb.transform.position + pos, 
            rb.transform.rotation);

        canAttack = true;
        yield return new WaitForSeconds(0.5f);
        AudioSource.PlayClipAtPoint(bomb_explode, Camera.main.transform.position);

        bomb.gameObject.GetComponent<Renderer>().enabled = false;
        bomb.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        bomb.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        bomb.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        bomb.gameObject.transform.GetChild(3).gameObject.SetActive(true);
        bomb.gameObject.transform.GetChild(4).gameObject.SetActive(true);
        bomb.gameObject.transform.GetChild(5).gameObject.SetActive(true);
        bomb.gameObject.transform.GetChild(6).gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        Destroy(bomb);
    }


}
