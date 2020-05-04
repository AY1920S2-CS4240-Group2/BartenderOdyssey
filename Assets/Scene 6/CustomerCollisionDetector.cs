﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.BartenderOdyssey;

public class CustomerCollisionDetector : MonoBehaviour
{
    public GameObject customer1;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            customer1.GetComponent<Scene6_Customer1>().getHit();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
