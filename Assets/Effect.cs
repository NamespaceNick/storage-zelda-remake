using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {
    public Sprite[] appear;
    public Sprite[] disappear;

    public float appearTime;

    Coroutine appearing, disappearing;
    SpriteRenderer rend;
	// Use this for initialization
	void Start () {
	}
    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public void OrderAppear(GameObject objectToActivate)
    {
        appearing = StartCoroutine(Appear(objectToActivate));
    }

    IEnumerator Appear(GameObject toActivate)
    {
        for (int i = 0; i < 3; ++i)
        {
            if (rend == null)
                Debug.Log("Rend is null");
            if (toActivate == null)
                Debug.Log("gameobject ot activate is null");
            rend.sprite = appear[i];
            yield return new WaitForSeconds(appearTime);
        }
        toActivate.SetActive(true);
        Destroy(gameObject);
    }
	
}
