using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public GameObject slots;
    public List<GameObject> slotList; //use this to know what items we have and what slots they're in
    int initSlotPosx = 40;
    int initSlotPosy = 40;
    private bool isKeepingBagOpen = false;

    private Canvas inventHolder;

    private GameObject bag;
    SpriteState initBagSprites;

    private Animator Success;
    private Animator Fail;
    private AudioManager mAudio;
    private Character mChar;

	// Use this for initialization
	void Start () {
        for (int i = 1; i < 8; i++)
        {
            GameObject slot = (GameObject)Instantiate(slots);
            slot.transform.SetParent( this.gameObject.transform );
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

        mAudio = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<AudioManager> ();
        mChar = GameObject.FindGameObjectWithTag("Char").GetComponent<Character>();
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
                StartCoroutine( KeepInventoryOpen () ); // Opens the inventory for a few seconds then closes it
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
                mAudio.PlayOnce ( "newItem" );
                Success.SetTrigger("becameActive");
                if (item.GetComponent<ItemData>().isInstant)
                {
                    slotData.interactable = true;
                }
                StartCoroutine( KeepInventoryOpen () ); // Opens the inventory for a few seconds then closes it
                return;
            }
        }

        //couldn't gather the item :/
        mAudio.PlayOnce ( "failed" );
        Fail.SetTrigger("becameActive");


    }

    IEnumerator KeepInventoryOpen( int numSeconds = 3 )
    {
        isKeepingBagOpen = true;
        OpenInventory ();
        yield return new WaitForSeconds ( numSeconds );
        isKeepingBagOpen = false;
        if ( mChar.GetPlayerState() != Character.PlayerState.Interact )
        {
            CloseInventory();
        }
    }

    public void ToggleInventory()
    {
        mAudio.PlayOnce ( "newItem" );
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
        if ( !isKeepingBagOpen )
        {
            inventHolder.enabled = false;
            bag.GetComponent<Image> ().sprite = initBagSprites.disabledSprite;
            bag.GetComponent<Button> ().spriteState = initBagSprites;
            SetInteractable ( false );
        }
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
