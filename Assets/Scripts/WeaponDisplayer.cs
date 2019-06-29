using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplayer : MonoBehaviour {
    private Image image;
    public Sprite bow_image, boomerang_image, bomb_image;
    public Inventory inventory;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = bow_image;
    }
    // Update is called once per frame
    void Update () {
        string curr = inventory.GetWeapon();

        if (curr == "Bow")
        {
            image.sprite = bow_image;
        }
        else if (curr == "Boomerang")
        {
            image.sprite = boomerang_image;
        }
        else
        {
            image.sprite = bomb_image;
        }
    }
}
