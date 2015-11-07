using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    Vector3 pos;
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
		pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = transform.position.z;

		dir = pos - transform.position;

		if( dir != Vector3.zero )
		{
			dir.Normalize ();
		}
        if (!inMap)
        {
            rotateTo = new Quaternion();
            rotateTo.SetFromToRotation(Vector3.right, dir);
            transform.rotation = rotateTo;
        }
    }
    void Start()
    {
        origY = transform.position.y;
        floorY = Random.Range(-19, origY);
        floorY += Random.Range(0, 9) / 10;
    }

    void Update ()
    {
        if (transform.position.y >= floorY && !inMap)
        {
            transform.position += dir * speed * Time.deltaTime;
            dir.y -= gravity;
            rotateTo.SetFromToRotation(Vector3.right, dir);
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
        if ( other.gameObject.tag == "Bird" )
        {
            
        }
    }

    public void setDir(Vector3 direction)
    {
        dir = direction;
    }
}