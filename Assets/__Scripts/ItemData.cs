using UnityEngine;
using System.Collections;

public class ItemData : MonoBehaviour
{

    public string description = "An Item";
    public int healthIncrease = 10;
    public int hungerIncrease = 10;
    public bool healsHunger = true;
    public bool healsSickness = false;
    public bool healsTired = false;
    public bool isInstant = false;
    public GameObject cookedItem;


    // Use this for initialization
    void Awake ()
    {
    }

    // Update is called once per frame
    void Update ()
    {

    }
}
