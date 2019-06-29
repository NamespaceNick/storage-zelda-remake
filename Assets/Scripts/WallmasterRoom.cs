using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallmasterRoom : MonoBehaviour
{
    public List<Wallmaster> WallmasterList;

    public float cooldownTime;
    public int maxNumWallmasters; 


    private bool onCooldown = false;

    Coroutine cooldown;


    public void SpawnWallmaster(Vector3 linkLocation)
    {
        if (CanSpawn())
        {
            cooldown = StartCoroutine(SpawnCooldown());
            foreach (Wallmaster wm in WallmasterList)
            {
                if (!wm.isBusy)
                {
                    wm.isBusy = true;
                    StartCoroutine(wm.GrabLink(linkLocation));
                    break;
                }
            }
        }
    }

    // Checks if necessary conditions are met to spawn a Wallmaster
    bool CanSpawn()
    {
        int numActive = 0;
        int numBusy = 0;
        foreach(Wallmaster wm in WallmasterList)
        {
            if (wm.gameObject.activeInHierarchy)
            {
                numActive++;
            }
            if (wm.isBusy)
            {
                numBusy++;
            }
        }
        if ((!onCooldown) && (numActive > 0) && (numBusy < numActive))
        {
            return true;
        }
        return false;
    }

    IEnumerator SpawnCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }
}
