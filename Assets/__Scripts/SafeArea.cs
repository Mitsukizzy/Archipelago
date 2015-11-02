using UnityEngine;
using System.Collections;

public class SafeArea : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Char")
        {
            GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
            foreach (GameObject b in birds)
            {
                b.GetComponent<BirdAI>().StopAttacking();
            }
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Char")
        {
            GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
            foreach (GameObject b in birds)
            {
                b.GetComponent<BirdAI>().enterSafeArea();
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Char")
        {
            GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
            foreach (GameObject b in birds)
            {
                b.GetComponent<BirdAI>().exitedSafeArea();
            }
        }
    }
}
