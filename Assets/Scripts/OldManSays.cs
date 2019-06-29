using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldManSays : MonoBehaviour {
    string words = "EASTMOST PENNINSULA IS THE SECRET.";
    Text text_component;
    public bool oldman_trigger = false;
    // Use this for initialization
    void Start () {
        text_component = GetComponent<Text>();
    }

    private void Update()
    {
        if(oldman_trigger == true)
        {
            StartCoroutine(WiseWords());
        }
        oldman_trigger = false;
    }
    IEnumerator WiseWords()
    {
        for(int i = 0; i < words.Length; i++)
        {
            text_component.text += words[i].ToString();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
