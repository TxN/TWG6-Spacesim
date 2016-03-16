/**
 * Author: Sander Homan
 * Copyright 2012
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BezierController : MonoBehaviour
{
    public BezierPath path = null;
    public float speed = 1;
    public bool byDist = false;

    private float t = 0;

    void Start()
    {
    }

    void Update()
    {
        t += speed*Time.deltaTime;
        if (!byDist)
            transform.position = path.GetPositionByT(t);
        else
            transform.position = path.GetPositionByDistance(t);
    }
}

