using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    Vector3 dir;
    // Use this for initialization
    void Start ()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = ( transform.position.z - Camera.main.transform.position.z );
        Vector3 worldPos = Camera.main.ScreenToWorldPoint ( mousePos );

        dir = mousePos - transform.position;
        if( dir != Vector3.zero )
        {
            dir.Normalize ();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        //transform.Translate ( Vector3.forward * Time.deltaTime * 30 ); 
        transform.position += dir * 50 * Time.deltaTime;
    }
}
