using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCollider : MonoBehaviour {

    CustomEnemy parent;
	// Use this for initialization
	void Start () {
        parent = GetComponentInParent<CustomEnemy>();
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (name == "Invulnerable Collider")
        {
            // TODO: Possibly make deflect noise
            Debug.Log("Invulnerable hit");
        }
        if ((name == "Vulnerable Collider") && other.CompareTag("sword"))
        {
            Debug.Log("Vulnerable hit");
            parent.Damaged();
        }
    }
}
