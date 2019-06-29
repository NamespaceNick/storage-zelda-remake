using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int rupee_count = 0;
    public int max_rupee = 4;
    private float health = 3;
    private int key_count = 0;
    public int max_key = 1;
    private int curr_weapon = 0;
    private int bomb_count = 0;

    GameUtilities utility;
    BeingAttacked beingAttacked;
    void Start()
    {
        utility = Camera.main.GetComponent<GameUtilities>();
        beingAttacked = GetComponent<BeingAttacked>();
        
    }
    public void AddRupee(int num_rupees)
    {
        rupee_count += num_rupees;
        rupee_count = Mathf.Min(rupee_count, max_rupee);
    }

    public int GetRupees()
    {
        return rupee_count;
    }

    public void DeductRupee()
    {
        rupee_count--;
    }

    public void AddHealth(float h)
    {
        health += h;
        health = Mathf.Min(3, health);
    }

    public float GetHealth()
    {
        return health;
    }

    public void DamageHealth(float damage)
    {
        health -= damage;
        health = Mathf.Max(0, health);
    }

    public void AddKey(int num_keys)
    {
        key_count += num_keys;
    }

    public int GetKeys()
    {
        return key_count;
    }

    public void DeductKey()
    {
        if (!utility.isCustom && beingAttacked.god_mode)
        {
            return;
        }
        key_count--;
    }
    public void AddBomb()
    {
        bomb_count++;
    }
    public void DeductBomb()
    {
        bomb_count--;
    }
    public int GetBombs()
    {
        return bomb_count;
    }

    public void SwitchWeapon()
    {
        Debug.Log(curr_weapon);
        curr_weapon = (curr_weapon + 1) % 3;

        // If no rupee avaiable, the bow weapon cannot be selected
        if(rupee_count == 0 && curr_weapon == 0 && !beingAttacked.god_mode)
        {
            curr_weapon++;
        }
    }

    public string GetWeapon()
    {
        if (curr_weapon == 0)
        {
            return "Bow";
        }
        else if (curr_weapon == 1)
        {
            return "Boomerang";
        }
        else
        {
            return "Bomb";
        }
    }
}
