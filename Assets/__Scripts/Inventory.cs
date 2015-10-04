using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public GameObject slots;
    int initSlotPosx = 40;
    int initSlotPosy = 40;

	// Use this for initialization
	void Start () {
        for (int i = 1; i < 8; i++)
        {
            GameObject slot = (GameObject)Instantiate(slots);
            slot.transform.parent = this.gameObject.transform;
            slot.name = "Slot " + i; 
            slot.GetComponent<RectTransform>().localPosition = new Vector3(initSlotPosx, initSlotPosy, 0);
            initSlotPosx += 70;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
