using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class BeingAttackedCustom : MonoBehaviour {
    Rigidbody rb;
    Inventory inventory;
    AttackMovement attMovement;
    GameObject[] attackeds = new GameObject[4];
    public GameObject knock_down, knock_left, knock_up, knock_right;
    public AnimationClip run_down, run_left, run_up, run_right;
    bool god_mode = false;
    public AudioClip link_death_sound, health_danger;
    public bool is_captured, attacked_once;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<Inventory>();
        attMovement = GetComponent<AttackMovement>();
        attackeds[0] = rb.gameObject.transform.GetChild(5).gameObject;
        attackeds[1] = rb.gameObject.transform.GetChild(6).gameObject;
        attackeds[2] = rb.gameObject.transform.GetChild(7).gameObject;
        attackeds[3] = rb.gameObject.transform.GetChild(8).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Debug.Log("1");
            god_mode = !god_mode;
            if (god_mode)
            {
                inventory.AddHealth(3);
                inventory.AddRupee(4);
                inventory.AddKey(1);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: IF TIME: Goriya boomerang does 1 damage, maybe other damage differences?
        if ((other.CompareTag("wallmaster") || other.CompareTag("enemy")) && !god_mode && !attacked_once)
        {
            Debug.Log(other.name);
            inventory.DamageHealth(0.5f);
            rb.velocity = Vector3.zero;
            if (inventory.GetHealth() <= 0.0f)
            {
                StartCoroutine(BeDead(rb.gameObject.GetComponent<ArrowKeyMovement>().facing));
                return;
            }
            else if (inventory.GetHealth() <= 1)
            {
                StartCoroutine(Beeping());
            }
            if (other.CompareTag("wallmaster"))
            {
                attacked_once = true;
                is_captured = true;
                StartCoroutine(BeCaptured(rb.gameObject.GetComponent<ArrowKeyMovement>().facing));
            }
            else
            {
                StartCoroutine(BeAttacked(rb.gameObject.GetComponent<ArrowKeyMovement>().facing));
            }
        }
    }

    IEnumerator BeAttacked(int i)
    {
        // rb.gameObject.GetComponent<AttackMovement>().canAttack = false;
        rb.gameObject.GetComponent<ArrowKeyMovement>().canWalk = false;

        Vector3 dir = new Vector3(0, -1, 0);
        if (i == 0)
        {
            dir = new Vector3(0, 1, 0);
        }
        else if (i == 1)
        {
            dir = new Vector3(1, 0, 0);
        }
        else if (i == 2)
        {
            dir = new Vector3(0, -1, 0);
        }
        else if (i == 3)
        {
            dir = new Vector3(-1, 0, 0);
        }

        rb.velocity = rb.transform.TransformDirection(dir) * 7;
        attackeds[i].SetActive(true);
        rb.gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(.1f);
        attackeds[i].GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(.1f);
        attackeds[i].GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(.1f);
        attackeds[i].GetComponent<Renderer>().enabled = true;
        rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(.1f);
        attackeds[i].GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(.1f);
        attackeds[i].GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(.1f);
        rb.gameObject.GetComponent<Renderer>().enabled = true;
        attackeds[i].SetActive(false);
        yield return new WaitForSeconds(.1f);

        // rb.gameObject.GetComponent<AttackMovement>().canAttack = true;
        rb.gameObject.GetComponent<ArrowKeyMovement>().canWalk = true;
    }

    IEnumerator BeDead(int i)
    {
        rb.gameObject.GetComponent<ArrowKeyMovement>().canWalk = false;
        // rb.gameObject.GetComponent<AttackMovement>().canAttack = false;
        AudioSource.PlayClipAtPoint(link_death_sound, Camera.main.transform.position);
        attackeds[i].SetActive(true);
        rb.gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(.1f);
        attackeds[i].GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(.1f);
        attackeds[i].GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(.1f);
        attackeds[i].GetComponent<Renderer>().enabled = true;
        rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(.1f);
        attackeds[i].GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(.1f);
        attackeds[i].GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(.1f);

        
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Beeping()
    {
        while (inventory.GetHealth() <= 1)
        {
            AudioSource.PlayClipAtPoint(health_danger, Camera.main.transform.position);
            yield return new WaitForSeconds(1.2f);
        }
    }

    IEnumerator BeCaptured(int i)
    {
        while (is_captured)
        {
            rb.gameObject.GetComponent<AttackMovement>().canAttack = false;
            rb.gameObject.GetComponent<ArrowKeyMovement>().canWalk = false;
            yield return null;
        }
        attacked_once = false;
        rb.gameObject.GetComponent<AttackMovement>().canAttack = true;
        rb.gameObject.GetComponent<ArrowKeyMovement>().canWalk = true;
    }
}
