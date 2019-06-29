using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManTrigger : MonoBehaviour {

    Rigidbody rb;
    GameObject black;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        black = GameObject.Find("black_pixel");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            StartCoroutine(OldMan());
    }

    IEnumerator OldMan()
    {
        rb.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        black.transform.GetChild(1).gameObject.SetActive(true);
        black.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<OldManSays>().oldman_trigger = true;
    }
}
