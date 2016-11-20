﻿using UnityEngine;
using System.Collections;
using System.Linq;
public class PickUp : MonoBehaviour
{
    public const string DROP_BUTTON = "E";
    public Transform handPlaceholder;
    public Animation animation;

    private const string PICKUP_TAG = "PickUp";
    private const string DoorTrigger_TAG = "DoorTrigger";
    private const string DoorTrigger1_TAG = "DoorTrigger1";
    private const string UnCondDoorTrigger_TAG = "unconditionDoor";
    private const string BookTrigger = "Book";
    private const string Key0_TAG = "Key0";
    private const string Key1_TAG = "Key1";

    private bool isDoorOpen = false;
    private bool isDoor1Open = false;

    private bool holding = false;
    private bool firstTimeEnteringExitTrigger = false;
    GameObject attachedObject;
    Memories memoriesScript;

    // createMemory > number

    // Use this for initialization
    void Start()
    {
        memoriesScript = GetComponent<Memories>();
    }

    void Update()
    {
        if (holding)
        {
            if (!isDoorOpen)
            {
                attachedObject.transform.position = handPlaceholder.transform.position;
                attachedObject.transform.rotation = handPlaceholder.transform.rotation;
            }
            else
            {

            }
        }
        checkDropItem();
    }

    void OnTriggerEnter(Collider other)
    {
        //log("gameObject.tag =  " + gameObject.tag);
        if (!holding && other.gameObject.tag.Contains(PICKUP_TAG))
        {
            holding = true;
            attachedObject = other.gameObject;
        }
        doorTrigger(other);
        bookTrigger(other);
    }


    void checkDropItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && holding)
        {
            //log(attachedObject.name);
            attachedObject.transform.parent = null;
            attachedObject.transform.localPosition = transform.position;
            holding = false;
        }
    }

    void doorTrigger(Collider other)
    {
        if (isDoorOpen) return;
        if (other.gameObject.CompareTag(DoorTrigger1_TAG) && holding && attachedObject.tag.Contains(Key1_TAG))
        {
            log("doorTrigger");
            animation.Play();
            attachedObject.transform.position = other.gameObject.transform.position + new Vector3(-3.05f, 0, 1.45f);
            isDoorOpen = true;
            holding = false;
        }
    }

    void doorTrigger1(Collider other)
    {
        if (isDoor1Open) return;
        if (other.gameObject.CompareTag(DoorTrigger_TAG) && holding && attachedObject.tag.Contains(Key1_TAG))
        {
            log("doorTrigger");
            GetComponentInParent<Animation>().Play();
            attachedObject.transform.position = other.gameObject.transform.position + new Vector3(-3.05f, 0, 1.45f);
            isDoorOpen = true;
            holding = false;
        }
    }



    void bookTrigger(Collider other)
    {
        if (other.gameObject.CompareTag(BookTrigger) && holding && attachedObject.tag.Contains(Key1_TAG))
        {
            log("books opens");
            memoriesScript.createMemory(7);
            holding = false;
            Object.Destroy(attachedObject.gameObject);
        }
    }

    // Remove this for debug
    void log(string text)
    {
        Debug.Log(text);
    }
}