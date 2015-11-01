using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    Vector3 pos;
	Vector3 dir;
	public float gravity = 0.02f;
	Quaternion rotateTo;
    public float speed = 5f;
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
    }

    void Update ()
    {
		transform.position += dir * 50 * Time.deltaTime;
		dir.y -= gravity;
		rotateTo.SetFromToRotation(Vector3.right, dir);
		transform.rotation = rotateTo;
	}

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
