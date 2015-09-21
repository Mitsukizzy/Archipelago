using UnityEngine;
using System.Collections;

public class AttackMovement : MonoBehaviour {

	public GameObject moveBack;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, moveBack.transform.position, Time.deltaTime*10);
	}
}
