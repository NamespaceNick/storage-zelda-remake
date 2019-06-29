using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room0Events : MonoBehaviour
{
    public List<EnemyInventory> enemies;
    public GameObject key;
    public AudioClip keyDropped;
    public int numDeadReq;

    int numDead = 0;
    UnityAction DeathAction;


	// Use this for initialization
	void Start ()
    {
        key.SetActive(false);
        DeathAction = EnemyDied;
        foreach (EnemyInventory enemy in enemies)
            enemy.RegisterDeathCallbacks(DeathAction);
	}
	
    void EnemyDied()
    {
        ++numDead;
        if (numDead >= numDeadReq)
        {
            key.SetActive(true);
            AudioSource.PlayClipAtPoint(keyDropped, Camera.main.transform.position);
        }
    }
}
