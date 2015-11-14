using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
	Vector3 dir;
	public float gravity = 0.02f;
	Quaternion rotateTo;
    public float speed = 50f;
    private float floorY;
    private float origY;
    public bool hasHit = false;
    public bool inMap = false;

    // Use this for initialization
    public void mouseDir ()
    {
		dir = transform.rotation * Vector3.down;

		if( dir != Vector3.zero )
		{
			dir.Normalize ();
		}
        if (!inMap)
        {
            //rotateTo = new Quaternion();
            //rotateTo.SetFromToRotation(Vector3.down, dir);
            //transform.rotation = GameObject.Find ("Bow").transform.rotation;
        }
    }
    void Start()
    {
        origY = transform.position.y;
		int lowerBound = (int)Mathf.Max(GameObject.FindGameObjectWithTag("Char").transform.position.y - 2, Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).y+4);
        floorY = Random.Range(lowerBound, Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).y+4);
        floorY += Random.Range(0, 9) / 10;

    }

    void Update ()
    {
        if (transform.position.y >= floorY && !inMap)
        {
            transform.position += dir * speed * Time.deltaTime;
            dir.y -= gravity;
            rotateTo.SetFromToRotation(Vector3.down, dir);
            transform.rotation = rotateTo;
        }
        else
        {
            GetComponent<Interactable>().enabled = true;
            hasHit = true;
        }
	}


    void OnTriggerEnter2D ( Collider2D other )
    {
        if ( other.gameObject.tag == "Char" && !hasHit )
        {
			//other.gameObject.GetComponent<Character>().TakeDamage(10);
        }
    }

    public void setDir(Vector3 direction)
    {
        dir = direction;
    }
}