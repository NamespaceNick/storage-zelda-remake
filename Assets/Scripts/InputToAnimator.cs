using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputToAnimator : MonoBehaviour {
    Animator animator;
    public GameObject player;
    bool custom = false;
    bool isCaptured = false;
    BeingAttacked beingAttacked;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        beingAttacked = GetComponent<BeingAttacked>();
        if (beingAttacked == null)
        {
            custom = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        float h_input = Input.GetAxisRaw("Horizontal");
        float v_input = Input.GetAxisRaw("Vertical");
        // Had to implement this because of the custom level
        if (!custom)
        {
            isCaptured = beingAttacked.is_captured;
        }
        else
        {
            isCaptured = false;
        }

        if (h_input == 0.0f && v_input == 0.0f && !isCaptured)
        {
            animator.speed = 0.0f;
        } else
        {
            animator.speed = 1.0f;
        }

        /*if (Mathf.Abs(h_input) > 0.0f)
        {
            v_input = 0.0f;
        }*/
        animator.SetFloat("h_input", h_input);
        animator.SetFloat("v_input", v_input);
	}
}
