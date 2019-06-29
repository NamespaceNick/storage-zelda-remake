using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtilities : MonoBehaviour {
    public bool isCustom = false;
    public bool aquamentusDead = false;
    public bool oldManRoomEntered = false;
    public AudioClip itemPickup;
    public AudioClip rupeePickup;
    public AudioClip roomDrop;
    public AudioClip mysteryDiscovered;
    public AudioClip linkDeath;
    public AudioClip doorUnlock;
    public AudioClip aquamentusHurt;
    public AudioClip aquamentusRoar;
    public AudioClip enemyHurt;
    public AudioClip enemyKilled;

    public GameObject bomb;
    public GameObject boomerang;
    public GameObject fireball;
    public GameObject key;
    public GameObject rupee;
    public GameObject appearFX, disappearFX;

    public List<RoomEvents> rooms;

    public void WallmasterReset()
    {
        foreach (RoomEvents room in rooms)
        {
            room.RespawnCreatures();
        }
    }

    public void OrderDeathPoof(Vector3 deathLocation)
    {
        StartCoroutine(DeathPoof(deathLocation));
    }

    // Plays death sparkle animation at `deathLocation`
    IEnumerator DeathPoof(Vector3 deathLocation)
    {
        // TODO: IF TIME: This function
        yield break;
    }


}
