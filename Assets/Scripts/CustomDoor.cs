using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDoor : MonoBehaviour
{
    public Sprite unlockedSprite, lockedSprite;
    public AudioClip doorUnlock;
    public Door partnerDoor;
    public bool isLocked;
    public bool thisDoor = false;

    private SpriteRenderer rend;
    private BoxCollider col;
    // Use this for initialization
    void Start()
    {
        if (isLocked && !unlockedSprite)
        {
            Debug.LogWarning("This locked door does not have an unlockedSprite");
        }
        col = GetComponent<BoxCollider>();
        rend = GetComponent<SpriteRenderer>();
        if (!isLocked)
        {
            gameObject.layer = 13;
        }
        col.isTrigger = false;
    }

    public void Unlock()
    {
        isLocked = false;
        rend.sprite = unlockedSprite;
        gameObject.layer = 13;
        if (partnerDoor)
        {
            partnerDoor.Unlock();
        }
        AudioSource.PlayClipAtPoint(doorUnlock, Camera.main.transform.position);
    }

    public void Lock()
    {
        isLocked = true;
        rend.sprite = lockedSprite;
        gameObject.layer = 0;
        AudioSource.PlayClipAtPoint(doorUnlock, Camera.main.transform.position);
    }
}
