using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowKeyMovement : MonoBehaviour {
    public int speed = 3;
    Rigidbody rb;
    public int facing = 0;
    public bool canWalk = true;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (canWalk)
        {
            Vector2 current_input = GetInput();
            rb.velocity = current_input * speed;
        }
	}

    Vector2 GetInput()
    {
        float h_input = Input.GetAxisRaw("Horizontal");
        float v_input = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(h_input) > 0.0f)
        {
            v_input = 0.0f;
        }

        if (v_input != 0.0f)
        {
            float dist = ClosedHorizontalGridline(rb);
            if(Mathf.Round(Mathf.Abs(dist) * 10f) / 10f != 0.0f && Mathf.Round(Mathf.Abs(dist) * 10f) / 10f != 0.5)
            {
                if (dist < 0) {
                    facing = 3;
                    h_input = Mathf.Abs(v_input);
                } else
                {
                    facing = 1;
                    h_input = -Mathf.Abs(v_input);
                }
                v_input = 0.0f;
            }
        }
        else if (h_input != 0.0f)
        {
            float dist = ClosedVerticalGridline(rb);
            if (Mathf.Round(Mathf.Abs(dist) * 10f) / 10f != 0.0f && Mathf.Round(Mathf.Abs(dist) * 10f) / 10f != 0.5)
            {
                if (dist < 0)
                {
                    v_input = Mathf.Abs(h_input);
                }
                else
                {
                    v_input = -Mathf.Abs(h_input);
                }
                h_input = 0.0f;
            }
        }

        if(v_input < 0.0f)
        {
            facing = 0;
        } else if(h_input < 0.0f)
        {
            facing = 1;
        } else if(v_input > 0.0f)
        {
            facing = 2;
        } else if(h_input > 0.0f)
        {
            facing = 3;
        }

        return new Vector2(h_input, v_input);
    }

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
}
