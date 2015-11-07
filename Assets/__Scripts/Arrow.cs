using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    Vector3 pos;
	Vector3 dir;
	public float gravity = 0.02f;
	Quaternion rotateTo;
    public float speed = 5f;
    private float floorY;
    private float origY;

    // Use this for initialization
    void Start ()
    {
		pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = transform.position.z;

		dir = pos - transform.position;

		if( dir != Vector3.zero )
		{
			dir.Normalize ();
		}

		rotateTo = new Quaternion();
		rotateTo.SetFromToRotation(Vector3.right, dir);
		transform.rotation = rotateTo;

        origY = transform.position.y;
        floorY = Random.Range(-19, origY);
        floorY += Random.Range(0, 9) / 10;
    }

    void Update ()
    {
        if (transform.position.y >= floorY)
        {
            transform.position += dir * 50 * Time.deltaTime;
            dir.y -= gravity;
            rotateTo.SetFromToRotation(Vector3.right, dir);
            transform.rotation = rotateTo;
        }
        else
        {
            GetComponent<Interactable>().enabled = true;
        }
	}


    void OnTriggerEnter2D ( Collider2D other )
    {
        if ( other.gameObject.tag == "Char" )
        {
            //GameObject.FindGameObjectWithTag("Char").GetComponent<Character>().TakeDamage ( 10 );
            //Destroy ( gameObject );
        }
    }
}