using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour {

    public GameObject item;
    Image itemImage;
    int stack;

	// Use this for initialization
	void Start () {
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        stack = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (item != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            itemImage.enabled = false;
        }
	}

    public void increaseStack()
    {
        stack++;
    }
    public void decreaseStack()
    {
        stack--;
    }
}
