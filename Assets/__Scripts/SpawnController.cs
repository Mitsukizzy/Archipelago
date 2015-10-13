using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject m_char = GameObject.Find("Character");
		m_char.transform.position = transform.position;
        m_char.GetComponent<Character>().SetPlayerState(Character.PlayerState.Idle);
	}
}
