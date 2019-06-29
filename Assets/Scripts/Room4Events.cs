using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room4Events : MonoBehaviour
{
    public Door door;
    public List<EnemyInventory> enemies;
    public int numRequired;
    public AudioClip doorLock;

    private UnityAction DeathAction;
    private int numDead = 0;

	// Use this for initialization
	void Start ()
    {
        DeathAction = EnemyDied;
        foreach (EnemyInventory e in enemies)
        {
            e.RegisterDeathCallbacks(DeathAction);

        }
	}

    void EnemyDied()
    {
        ++numDead;
        if (numDead >= numRequired)
        {
            door.Unlock();
            AudioSource.PlayClipAtPoint(doorLock, Camera.main.transform.position);
        }
    }
}
