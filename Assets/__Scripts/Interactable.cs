using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite activeSprite;

	public GameObject gatherableItem;
    private Inventory inventory;
    private GameObject m_char;

    bool checkForOrder;

	// Use this for initialization
	void Start () 
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        m_char = GameObject.Find("Character");
        checkForOrder = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (checkForOrder)
        {
            if (m_char.transform.position.y < transform.position.y + 1)
            {
                GetComponent<SpriteRenderer>().sortingOrder = -1;
            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
	}



    void OnTriggerStay2D ( Collider2D coll )
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
        checkForOrder = true;
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
        checkForOrder = false;
    }

	public void ReceiveItem()
	{
        inventory.AddItem(gatherableItem);
	}

}
