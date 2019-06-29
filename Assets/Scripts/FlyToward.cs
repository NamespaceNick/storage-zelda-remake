using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyToward : MonoBehaviour {
    GameObject player, panel;
    Rigidbody rb;
    public Transform target;
    float speed, MAX_DISTANCE = 15;

    public Sprite has_weapon;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        panel = GameObject.Find("ScorePanel");
        //transform.forward = Vector3.down;
	}

    void FixedUpdate()
    {
        if (player.GetComponent<AttackMovementCustom>().is_attract)
        {
            // transform.up = transform.position - player.transform.position;
            transform.up = Vector3.Lerp(transform.up, (transform.position - player.transform.position), 0.01f);
            float distance = Vector2.Distance(player.transform.position,transform.position);
            speed = Mathf.Abs(MAX_DISTANCE - distance);
            rb.AddForce((player.transform.position - transform.position).normalized * speed);
        } else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<AttackMovementCustom>().has_sword = true;
            panel.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = has_weapon;
            Destroy(this.gameObject);
        }
    }
}
