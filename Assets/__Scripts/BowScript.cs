using UnityEngine;
using System.Collections;

public class BowScript : MonoBehaviour {

    public Sprite hasArrow;
    public Sprite noArrows;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void swapSprite()
    {
        if (GetComponent<SpriteRenderer>().sprite == hasArrow)
        {
            GetComponent<SpriteRenderer>().sprite = noArrows;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = hasArrow;
        }
    }
}
