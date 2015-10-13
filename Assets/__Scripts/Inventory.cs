using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public GameObject slots;
    public List<GameObject> slotList; //use this to know what items we have and what slots they're in
    int initSlotPosx = 40;
    int initSlotPosy = 40;

    private Canvas inventHolder;

    private GameObject bag;
    SpriteState initBagSprites;

    private Animator Success;
    private Animator Fail;

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
        inventHolder.enabled = false;
        bag = GameObject.Find("Bag");
        initBagSprites = bag.GetComponent<Button>().spriteState;

        Success = GameObject.Find("Success").GetComponent<Animator>();
        Fail = GameObject.Find("Fail").GetComponent<Animator>();



	}

	void Awake() {
		//the inventory will now not be destroyed between scenes
		//DontDestroyOnLoad(GameObject.Find ("Inventory UI") );
	}

	// Update is called once per frame
	void Update () {
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
                //add some kind of success noise
                Success.SetTrigger("becameActive");
                if (item.GetComponent<ItemData>().isInstant)
                {
                    slotData.interactable = true;
                }
                return;
            }
        }
        //if we dont, add it to a new slot in the inventory if we have a slot avaliable
        foreach (GameObject curSlot in slotList)
        {
            SlotScript slotData = curSlot.GetComponent<SlotScript>();
            if (slotData.item == null)
            {
                slotData.item = item;
                //add some kind of success noise
                Success.SetTrigger("becameActive");
                if (item.GetComponent<ItemData>().isInstant)
                {
                    slotData.interactable = true;
                }
                return;
            }
        }

        //couldn't gather the item :/
        //add some kind of fail noise?
        Fail.SetTrigger("becameActive");


    }

    public void ToggleInventory()
    {
        inventHolder.enabled = !inventHolder.enabled;
        if (inventHolder.enabled)
        {
            bag.GetComponent<Image>().sprite = initBagSprites.pressedSprite;
            SpriteState openBagSpriteStates = new SpriteState();
            openBagSpriteStates.highlightedSprite = initBagSprites.pressedSprite;
            openBagSpriteStates.pressedSprite = initBagSprites.disabledSprite;
            
            bag.GetComponent<Button>().spriteState = openBagSpriteStates;
        }
        else
        {
            bag.GetComponent<Image>().sprite = initBagSprites.disabledSprite;
            bag.GetComponent<Button>().spriteState = initBagSprites;
        }
    }

    public void OpenInventory()
    {
        inventHolder.enabled = true;
        bag.GetComponent<Image>().sprite = initBagSprites.pressedSprite;
        SpriteState openBagSpriteStates = new SpriteState();
        openBagSpriteStates.highlightedSprite = initBagSprites.pressedSprite;
        openBagSpriteStates.pressedSprite = initBagSprites.disabledSprite;

        bag.GetComponent<Button>().spriteState = openBagSpriteStates;
    }

    public void CloseInventory()
    {
        inventHolder.enabled = false;
        bag.GetComponent<Image>().sprite = initBagSprites.disabledSprite;
        bag.GetComponent<Button>().spriteState = initBagSprites;
        SetInteractable(false);
    }

    public void SetInteractable(bool interact)
    {
        foreach (GameObject slot in slotList)
        {
            SlotScript slotData = slot.GetComponent<SlotScript>();
            slotData.interactable = interact;
            if (slotData.item != null && slotData.m_itemData.isInstant)
            {
                slotData.interactable = true;
            }
        }

    }
}
