using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RupeeDisplayer : MonoBehaviour {

    public Inventory inventory;
    Text text_component;
	// Use this for initialization
	void Start () {
        text_component = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		if(inventory && text_component)
        {
            text_component.text = "Rupee: " + inventory.GetRupees().ToString() + "\n";
            text_component.text += "Health: " + inventory.GetHealth().ToString() + "\n";
            text_component.text += "Key: " + inventory.GetKeys().ToString() + "\n";
        }
    }
}
