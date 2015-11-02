using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour{

    public GameObject item;
    public ItemData m_itemData;
    Image itemImage, descriptionBG;
    Text itemCount, itemDescription;
    int stack;
    public bool interactable;
    private Character m_Character;

	// Use this for initialization
	void Start () {
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        itemCount = gameObject.transform.GetChild(1).GetComponent<Text>();
        descriptionBG = gameObject.transform.GetChild(2).GetComponent<Image>();
        itemDescription = gameObject.transform.GetChild(3).GetComponent<Text>();
        stack = 1;
        interactable = false;
        m_Character = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
        if (item != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            itemCount.enabled = true;
            m_itemData = item.GetComponent<ItemData>();
            if (m_itemData.isKey)
            {
                itemCount.text = "";
            }
            else
            {
                itemCount.text = stack.ToString();
            }
            itemDescription.text = m_itemData.description;
        }
        else
        {
            itemImage.enabled = false;
            itemCount.enabled = false;
            //m_itemData = null;
            interactable = false;
            itemDescription.text = "";
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

    public void Clicked()
    {
        if (interactable)
        {
            Debug.Log("Clicked " + item.name );
            stack--;
            m_Character.useItem(item);
            if (stack == 0)
            {
                item = null;
                descriptionBG.enabled = false;
                itemDescription.enabled = false;
                stack = 1;
            }
            if (m_itemData.cookedItem != null)
            {
                m_Character.GetItem(m_itemData.cookedItem);
            }
        }
    }

    public void MouseOver()
    {
        if (item != null)
        {
            descriptionBG.enabled = true;
            itemDescription.enabled = true;
            Debug.Log("MouseOver");
        }
    }

    public void MouseLeft()
    {
        descriptionBG.enabled = false;
        itemDescription.enabled = false;
    }
}
