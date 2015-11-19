using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite activeSprite;
    public Sprite gatheredSprite;

	public GameObject gatherableItem;
    private Inventory inventory;
    private GameObject m_char;

    bool checkForOrder;
    public int gathersRemaining = 1;
    public GameObject gatherBar;

    public GameObject StoryItem;

	// Use this for initialization
	void Start () 
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        m_char = GameObject.FindGameObjectWithTag("Char");
        checkForOrder = false;
    }
	
	// Update is called once per frame
	void Update () 
    {
            if (m_char.transform.position.y < transform.position.y)
            {
                GetComponent<SpriteRenderer>().sortingOrder = -1;
            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (this.gameObject.tag == "arrow" && gameObject.GetComponent<Arrow>().hasHit)
		{
			if(coll.gameObject.GetComponent<Character>() != null)
			{
				coll.gameObject.GetComponent<Character>().CollectArrow();
				Destroy(gameObject);
			}
		}
	}

    void OnTriggerStay2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" )
        {
            if ( this.gameObject.tag == "Gatherable" && gathersRemaining > 0 )
            {
                GetComponent<SpriteRenderer>().sprite = activeSprite;
                gatherBar.GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, 
                                                                                                                      transform.position.y+gameObject.GetComponent<SpriteRenderer>().bounds.size.y+1, 
                                                                                                                      transform.position.z));
                coll.gameObject.GetComponent<Character>().gatherBarObj = gatherBar;
				coll.gameObject.GetComponent<Character>().gatherFrom = this.gameObject;
                coll.gameObject.GetComponent<Character>().BeginGather();
			}
            else if ( gathersRemaining > 0 )
            {
                GetComponent<SpriteRenderer>().sprite = activeSprite;
            }
        }
    }

    void OnTriggerExit2D ( Collider2D coll )
    {
        if ( coll.gameObject.tag == "Char" && gathersRemaining > 0 )
        {
            GetComponent<SpriteRenderer> ().sprite = defaultSprite;
			if(this.gameObject.tag == "Gatherable")
            {
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
        inventory.AddItem(gatherableItem);
        gathersRemaining--;
	}

    public bool GetCanGather()
    {
        return ( gathersRemaining > 0 ) ? true : false;
    }

    public void SwitchToGatheredSprite()
    {
        if( gatheredSprite != null )
        {
            GetComponent<SpriteRenderer>().sprite = gatheredSprite;
        }
    }
}