using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Entering Scene Transition Area");
        Application.LoadLevel("Wetlands2");
    }
}
