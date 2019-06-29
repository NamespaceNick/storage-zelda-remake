using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FetchLight : MonoBehaviour {
    GameObject player, hlight, panel;
    public Sprite h_light;
    public AudioClip fetch_light;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        hlight = GameObject.Find("Light");
        panel = GameObject.Find("ScorePanel");
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<AttackMovementCustom>().have_light = true;
            panel.transform.GetChild(5).gameObject.SetActive(true);
            panel.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = h_light;
            AudioSource.PlayClipAtPoint(fetch_light, Camera.main.transform.position);
            Destroy(hlight);
        }
    }
}
