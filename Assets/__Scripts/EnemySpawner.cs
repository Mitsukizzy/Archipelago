using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyToSpawn;
    public int minAmount = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int enemiesInScene = GameObject.FindGameObjectsWithTag(enemyToSpawn.tag).Length;
        if (enemiesInScene < minAmount)
        {
            GameObject newEnemy = (GameObject)Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }
	}
}
