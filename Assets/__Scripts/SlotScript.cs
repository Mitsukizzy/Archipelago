using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour {

    public GameObject item;
    Image itemImage;
    Text itemCount;
    int stack;

	// Use this for initialization
	void Start () {
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        itemCount = gameObject.transform.GetChild(1).GetComponent<Text>();
        stack = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (item != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            itemCount.enabled = true;
            itemCount.text = stack.ToString();
        }
        else
        {
            itemImage.enabled = false;
            itemCount.enabled = false;
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
