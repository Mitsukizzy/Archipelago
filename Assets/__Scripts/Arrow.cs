using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    Vector3 pos;
    float speed = 5f;
    public float dist = 10f;
    // Use this for initialization
    void Start ()
    {
        pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);

    }

    // Update is called once per frame
    void Update ()
    {
        Vector3.MoveTowards(transform.position, pos, speed*Time.deltaTime);
    }
}
