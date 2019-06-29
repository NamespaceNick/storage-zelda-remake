using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RoomEvents : MonoBehaviour
{
    public List<EnemyInventory> enemies;
    public List<GameObject> roomDrops;
    public SpecialWall specialWall;
    public Door lockDoor;
    public Door oldManDoor;
    public float appearTime;
    public bool droppingItems;
    public bool unlockingDoor;
    public bool aquamentusRoar = false;
    public bool containsAquamentus = false;
    public bool gelMysteryRoom;
    public bool oldManRoom;
    public bool trapDoor = false;
    public bool inAquaRoom = false;
    public bool haveCustom = false;
    public int numDeadReq;

    int numDead;
    bool eventTriggered = false;
    bool hasUnlocked = false;
    GameUtilities utility;
    UnityAction DeathAction;
    Coroutine roars, spawning;


    // Use this for initialization
    void Start()
    {
        foreach(GameObject item in roomDrops)
        {
            item.SetActive(false);
        }
        foreach(EnemyInventory enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
        utility = Camera.main.GetComponent<GameUtilities>();
        DeathAction = EnemyDied;
        foreach (EnemyInventory enemy in enemies)
        {
            enemy.RegisterDeathCallbacks(DeathAction);
        }
        numDeadReq = enemies.Count;
    }

    public void RoomEntered()
    {
        if (containsAquamentus || aquamentusRoar)
        {
            inAquaRoom = true;
        }
        if (oldManRoom)
        {
            utility.oldManRoomEntered = true;
        }
        if (gelMysteryRoom && utility.oldManRoomEntered)
        {
            oldManDoor.Lock();
        }
        if (trapDoor && !hasUnlocked)
        {
            lockDoor.Lock();
        }
        foreach (EnemyInventory enemy in enemies)
        {
            if (!enemy.isDead)
            {
                GameObject appear = (GameObject)Instantiate(utility.appearFX,
                    enemy.transform.position, Quaternion.identity);
                appear.GetComponent<Effect>().OrderAppear(enemy.gameObject);
            }
        }

        if ((aquamentusRoar || containsAquamentus) && !utility.aquamentusDead)
        {
            if (roars == null)
            {
                roars = StartCoroutine(Roars());
            }
        }
    }

    public void RoomExited()
    {
        if (roars != null)
        {
            StopCoroutine(roars);
        }
        if (containsAquamentus || aquamentusRoar)
        {
            inAquaRoom = false;
        }
        foreach (EnemyInventory enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    public void BeginTrap()
    {
        if (!eventTriggered)
        {
            eventTriggered = true;
            lockDoor.Lock();
        }
    }


    public void MysteryDoor()
    {
        if (!eventTriggered)
        {
            eventTriggered = true;
            lockDoor.Unlock();
            AudioSource.PlayClipAtPoint(utility.mysteryDiscovered, Camera.main.transform.position);
        }
    }
    
    
    public void CloseMysteryDoor()
    {
        lockDoor.Lock();
    }


    public void RespawnCreatures()
    {
        if (!containsAquamentus)
        {
            numDead = 0;
            foreach (EnemyInventory enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
                enemy.Respawned();
                enemy.gameObject.SetActive(false);
            }
            hasUnlocked = false;
            if (trapDoor)
            {
                lockDoor.MasterUnlock();
            }

        }
    }


    void EnemyDied()
    {
        // TODO: Call the dissapear effect on the enemy location (need to know which died tho)
        ++numDead;
        if (numDead >= numDeadReq)
        {
            ExecuteActions();
        }
    }

    // Executes all actions to be done for the particular room
    void ExecuteActions()
    {
        if (gelMysteryRoom)
        {
            specialWall.canMove = true;
        }
        if (droppingItems)
        {
            foreach (GameObject item in roomDrops)
            {
                item.SetActive(true);
            }
            AudioSource.PlayClipAtPoint(utility.roomDrop, Camera.main.transform.position);
        }
        if (unlockingDoor && !hasUnlocked)
        {
            hasUnlocked = true;
            lockDoor.MasterUnlock();
        }
        if (containsAquamentus)
        {
            utility.aquamentusDead = true;
            if (roars != null)
            {
                StopCoroutine(roars);
            }
        }
        if (haveCustom)
        {
            StartCoroutine(EndGame());
        }
    }


    // Creates faux appearance animation poof in desired location
    // Activates gameobject after animation

    IEnumerator Roars()
    {
        while (!utility.aquamentusDead && inAquaRoom)
        {
            AudioSource.PlayClipAtPoint(utility.aquamentusRoar, Camera.main.transform.position);
            yield return new WaitForSeconds(Random.Range(14.0f, 17.0f));
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
