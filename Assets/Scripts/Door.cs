using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public Sprite unlockedSprite, lockedSprite;
    public Door partnerDoor;
    public bool isLocked;
    public bool canUseKey = true;

    GameObject player;
    SpriteRenderer rend;
    BoxCollider col;
    GameUtilities utility;


	void Start ()
    {
        utility = Camera.main.GetComponent<GameUtilities>();
        if (utility == null)
        {
            Debug.LogError("Could not find GameUtilities");
        }
        player = GameObject.Find("Player");
        if (isLocked && !unlockedSprite)
        {
            Debug.LogWarning("This locked door does not have an unlockedSprite");
        }
        col = GetComponent<BoxCollider>();
        rend = GetComponent<SpriteRenderer>();
        if(!isLocked)
        {
            gameObject.layer = 13;
        }
        col.isTrigger = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isLocked && 
            player.GetComponent<Inventory>().GetKeys() >= 1 && isLocked)
        {
            player.GetComponent<Inventory>().DeductKey();
            Unlock();
            if (partnerDoor)
            {
                partnerDoor.Unlock();
            }
        }
    }
    public void MasterUnlock()
    {
        isLocked = false;
        rend.sprite = unlockedSprite;
        gameObject.layer = 13;
        AudioSource.PlayClipAtPoint(utility.doorUnlock, Camera.main.transform.position);
    }

    public void Unlock()
    {
        if (!canUseKey)
        { return; }
        isLocked = false;
        rend.sprite = unlockedSprite;
        gameObject.layer = 13;
        AudioSource.PlayClipAtPoint(utility.doorUnlock, Camera.main.transform.position);
    }

    public void Lock()
    {
        isLocked = true;
        rend.sprite = lockedSprite;
        gameObject.layer = 0;
        AudioSource.PlayClipAtPoint(utility.doorUnlock, Camera.main.transform.position);
    }
}
