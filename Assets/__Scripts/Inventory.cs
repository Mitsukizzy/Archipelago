﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public GameObject slots;
    public List<GameObject> slotList; //use this to know what items we have and what slots they're in
    int initSlotPosx = 40;
    int initSlotPosy = 40;

    private Canvas inventHolder;

    public bool interactable;

	// Use this for initialization
	void Start () {
        for (int i = 1; i < 8; i++)
        {
            GameObject slot = (GameObject)Instantiate(slots);
            slot.transform.parent = this.gameObject.transform;
            slot.name = "Slot " + i; 
            slot.GetComponent<RectTransform>().localPosition = new Vector3(initSlotPosx, initSlotPosy, 0);
            slotList.Add(slot);
            initSlotPosx += 70;
        }
        inventHolder = GameObject.Find("Inventory UI").GetComponent<Canvas>();
        interactable = false;
        inventHolder.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (interactable)
        {
            foreach (GameObject slot in slotList)
            {
                slot.GetComponent<SlotScript>().interactable = true;
            }
        }
        else
        {
            foreach (GameObject slot in slotList)
            {
                slot.GetComponent<SlotScript>().interactable = false;
            }
        }
	}

    public void AddItem(GameObject item)
    {
        //first check if we already have the item
        foreach (GameObject curSlot in slotList)
        {
            SlotScript slotData = curSlot.GetComponent<SlotScript>();
            if (slotData.item != null && slotData.item.name == item.name)
            {
                slotData.increaseStack();
                Debug.Log("increased Stack of " + item.name);
                return;
            }
        }
        //if we dont, add it to a new slot in the inventory
        foreach (GameObject curSlot in slotList)
        {
            SlotScript slotData = curSlot.GetComponent<SlotScript>();
            if (slotData.item == null)
            {
                slotData.item = item;
                break;
            }
        }
    }

    public void ToggleInventory()
    {
        inventHolder.enabled = !inventHolder.enabled;
    }

    public void SetInteractable(bool interact)
    {
        interactable = interact;
    }
}
