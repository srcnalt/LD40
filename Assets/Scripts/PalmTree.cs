﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmTree : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> coconuts;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);

        if (coconuts.Count > 0)
        {
            coconuts[0].isKinematic = false;
            coconuts.RemoveAt(0);
        }
    }
}
