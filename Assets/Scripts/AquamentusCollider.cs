using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquamentusCollider : MonoBehaviour
{

    Aquamentus parent;

    void Start()
    {
        parent = GetComponentInParent<Aquamentus>();
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO: Is it called door?
        if ((other.CompareTag("sword") && name == "Head") || other.CompareTag("bullet"))
        {
            parent.Damaged();
        }
    }
}
