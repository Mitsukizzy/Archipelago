using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite activeSprite;

	public GameObject gatherableItem;
    private Inventory inventory;

	// Use this for initialization
	void Start () 
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}


    void OnTriggerEnter2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {
            GetComponent<SpriteRenderer> ().sprite = activeSprite;
			if(this.gameObject.tag == "Gatherable"){
				coll.gameObject.GetComponent<Character>().gatherFrom = this.gameObject;
			}
        }
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetBool("inRange", true);
        }
    }

    void OnTriggerExit2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {
            GetComponent<SpriteRenderer> ().sprite = defaultSprite;
			if(this.gameObject.tag == "Gatherable"){
				coll.gameObject.GetComponent<Character>().gatherFrom = null;
			}
		}
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetBool("inRange", false);
        }
    }

	public void ReceiveItem()
	{
		//Replace this with better code once we actually implement a UI
		//gatherableItem.SetActive(true);
        inventory.AddItem(gatherableItem);
	}

}
