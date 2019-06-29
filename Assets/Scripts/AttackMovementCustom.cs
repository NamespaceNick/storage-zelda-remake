using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMovementCustom : MonoBehaviour {
    Rigidbody rb;
    GameObject[] attacks = new GameObject[5];
    public GameObject bulletPrefab;
    public AudioClip sword_wield, holy_light;
    public Vector3 dir = new Vector3(0, -1, 0);
    public bool boomer_dir = false, sword_hit = false, have_light = false, is_attract = false,
        has_sword = false, played = false;
    public Sprite no_weapon;
    public GameObject panel;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        panel = GameObject.Find("ScorePanel");
        attacks[0] = rb.gameObject.transform.GetChild(0).gameObject;
        attacks[1] = rb.gameObject.transform.GetChild(1).gameObject;
        attacks[2] = rb.gameObject.transform.GetChild(2).gameObject;
        attacks[3] = rb.gameObject.transform.GetChild(3).gameObject;
        attacks[4] = rb.gameObject.transform.GetChild(4).gameObject;
    }

    // Update is called once per frame
    void Update () {

        int facing = rb.gameObject.GetComponent<ArrowKeyMovement>().facing;
        if (Input.GetKeyDown(KeyCode.X) && has_sword)
        {
            has_sword = false;
            panel.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = no_weapon;
            AudioSource.PlayClipAtPoint(sword_wield, Camera.main.transform.position);
            StartCoroutine(Drop(facing));
        } else if (Input.GetKey(KeyCode.Z) && have_light)
        {
            if (!played)
            {
                played = true;
                AudioSource.PlayClipAtPoint(holy_light, Camera.main.transform.position);
            }
            rb.velocity = Vector3.zero;
            rb.gameObject.GetComponent<ArrowKeyMovement>().canWalk = false;
            attacks[4].SetActive(true);
            is_attract = true;
            rb.gameObject.GetComponent<Renderer>().enabled = false;
        }

        if (Input.GetKeyUp(KeyCode.Z) && have_light)
        {
            played = false;
            rb.gameObject.GetComponent<ArrowKeyMovement>().canWalk = true;
            rb.gameObject.GetComponent<Renderer>().enabled = true;
            is_attract = false;
            attacks[4].SetActive(false);
        }
    }

    IEnumerator Drop(int i)
    {
        attacks[i].SetActive(true);
        rb.gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(.3f);

        Vector3 dir = new Vector3(0, -1, 0);
        if (i == 0)
        {
            dir = new Vector3(0, -1, 0);
        }
        else if (i == 1)
        {
            dir = new Vector3(-1, 0, 0);
        }
        else if (i == 2)
        {
            dir = new Vector3(0, 1, 0);
        }
        else if (i == 3)
        {
            dir = new Vector3(1, 0, 0);
        }

        Instantiate(
            bulletPrefab,
            rb.transform.position + dir,
            rb.transform.rotation);

        rb.gameObject.GetComponent<Renderer>().enabled = true;
        attacks[i].SetActive(false);
        yield return new WaitForSeconds(.3f);
    }

    /*IEnumerator Attrack()
    {
        
    }*/
}
