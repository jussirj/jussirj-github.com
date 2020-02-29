﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cub : MonoBehaviour
{
    public Movin cub;

    public void Start()
    {

        cub = new Movin(transform, "json/cub");
        Debug.Log(cub);
        cub.Play();
    }

    void Update()
    {

    }

    public void Stop()
    {
        cub.Stop();
    }

    public void Play()
    {
        cub.Play();
    }

}
