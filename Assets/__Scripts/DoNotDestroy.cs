using UnityEngine;
using System.Collections;

public class DoNotDestroy : MonoBehaviour {

    // Make this game object and all its transform children
    // survive when loading a new scene.
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
