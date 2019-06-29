using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour {

    Inventory inventory;
    GameUtilities utility;
    AttackMovement attMovement;
    public AudioClip rupee_collecting_sound, heart_collecting_sound;
    void Start()
    {
        inventory = GetComponent<Inventory>();
        if (!inventory)
        {
            Debug.LogWarning("WARNING: No inventory to store things!");
        }
        utility = Camera.main.GetComponent<GameUtilities>();
        attMovement = GetComponent<AttackMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!utility.isCustom && attMovement.isAttacking)
        {
            return;
        }
        GameObject thing = other.gameObject;

        if(thing.tag == "rupee")
        {
            if (inventory) inventory.AddRupee(1);
            Debug.Log(inventory.GetRupees());
            Destroy(thing);
            AudioSource.PlayClipAtPoint(rupee_collecting_sound, Camera.main.transform.position);
        } else if(thing.tag == "heart")
        {
            if (inventory) inventory.AddHealth(1);
            Destroy(thing);
            AudioSource.PlayClipAtPoint(heart_collecting_sound, Camera.main.transform.position);
        } else if(thing.tag == "key")
        {
            if (inventory) inventory.AddKey(1);
            Destroy(thing);
            AudioSource.PlayClipAtPoint(heart_collecting_sound, Camera.main.transform.position);
        } else if(thing.tag == "bomb")
        {
            if (inventory) inventory.AddBomb();
            Destroy(thing);
            AudioSource.PlayClipAtPoint(heart_collecting_sound, Camera.main.transform.position);
        }
    }
}
