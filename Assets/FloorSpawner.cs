﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    private float happiness = 0;
    private float happinessStep = 0.15f;

    private Camera cameraScript;
    private Transform playerTransform;
    private GameObject floor;

    private float distanceToPlayer = 20f;
    private float zSpeed = 1f;

    private int floorAmount = 30;
    private List<GameObject> floors = new List<GameObject>();
    private int floorIndex = 0;

    private int removedFloorCount = 0;
    private int removedFloorMax = 0;
    private int frames = 0;
    private GameObject lastFloor;
    private bool randomEnabled = false;
    private int noGapsForNextFrames = 0;

    private Vector3 initialPosition;

    
    // Start is called before the first frame update
    void Awake()
    {
        this.floor = GameObject.Find("Cube");
        InstantiateFloors();
        this.playerTransform = GameObject.Find("Player").transform;
        this.cameraScript = GameObject.Find("Main Camera").GetComponent<Camera>();
        this.initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /* TODO deprecated */
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.ChangeHappiness(true);
            print("HAPPINESS: " + this.happiness);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.ChangeHappiness(false);
            print("HAPPINESS: " + this.happiness);
        }

        if (transform.position.z - this.playerTransform.position.z < distanceToPlayer)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + this.happiness, transform.position.z + zSpeed);
            SpawnFloor();    
        }


    }

    void InstantiateFloors()
    {
        for (int i = 0; i < this.floorAmount; ++i)
        {
            GameObject floor = GameObject.Instantiate(this.floor);
            floor.SetActive(false);
            floors.Add(floor);
        }
        this.floor.SetActive(false);
        floors.Add(this.floor);
    }

    void SpawnFloor()
    {
        this.frames++;

        if(noGapsForNextFrames > 0) { 
            this.noGapsForNextFrames--;
        }

        if (this.floorIndex > this.floors.Count - 1)
        {
            this.floorIndex = 0;
        }

        if (this.noGapsForNextFrames == 0 && this.randomEnabled && this.frames > 100 && this.removedFloorCount == 0 && Random.value > 0.95f)
        {
            if(this.happiness > 0)
            {
                this.removedFloorMax = 5;
            } else
            {
                this.removedFloorMax = 8;
            }
        }

        GameObject floor = this.floors[this.floorIndex];
        floor.transform.position = transform.position;
        floor.transform.Find("bone").gameObject.SetActive(true);
        floor.transform.Find("Cube").gameObject.tag = "Floor";
        floor.transform.Find("Cube").GetComponent<Floor>().SetHappiness(this.happiness);
        this.floorIndex++;
        this.lastFloor = floor;

        if (this.removedFloorMax != 0 && this.removedFloorCount <= this.removedFloorMax)
        {
            this.removedFloorCount++;
            floor.transform.Find("bone").gameObject.SetActive(false);

            if (this.removedFloorCount > 2) { 
                floor.transform.Find("Cube").gameObject.tag = "Hole";
                if (this.removedFloorCount == this.removedFloorMax)
                {
                    this.noGapsForNextFrames = 10;
                    this.removedFloorCount = 0;
                    this.removedFloorMax = 0;
                }
            }
        }
        else {
            floor.SetActive(true);
        }
    }

    public GameObject GetLastFloor()
    {
        return this.lastFloor;
    }

    public void ChangeHappiness(bool happiness)
    {
        if (happiness)
        {
            this.happiness += this.happinessStep;
        }
        else
        {
            this.happiness -= this.happinessStep;
        }
        this.cameraScript.UpdateBackground(this.happiness);
    }

    public float GetHappiness()
    {
        return this.happiness;
    }

    public void Reset()
    {
        this.happiness = 0;
        this.frames = 0;
        this.cameraScript.UpdateBackground(this.happiness);
        transform.position = this.initialPosition;
    }

    public void SetRandomEnabled(bool randomEnabled)
    {
        this.randomEnabled = randomEnabled;
    }
}
