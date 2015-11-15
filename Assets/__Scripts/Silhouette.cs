using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Silhouette : MonoBehaviour {

	RectTransform SilhouetteBG;
	private GameObject Boat;
	public Canvas SilhouetteCanvas;
	private GameManager mGame;

	// Use this for initialization
	void Start () {
		SilhouetteBG = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
		Boat = GameObject.Find("Boat");
		mGame = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
		//SilhouetteCanvas = SilhouetteCanvasObj.GetComponent<
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowItems()
	{
		SilhouetteCanvas.enabled = true; 
		SilhouetteBG.anchoredPosition = Camera.main.WorldToScreenPoint(new Vector3(Boat.transform.position.x, 
		                                                                           Boat.transform.position.y + Boat.GetComponent<SpriteRenderer>().bounds.size.y + 3, 
		                                                                           Boat.transform.position.z));
		if(!mGame.CheckItem("wood"))
		{
			GameObject.Find("WoodIcon").GetComponent<Image>().color = new Color32(255,255,255,255);
		}
		if(!mGame.CheckItem("rope"))
		{
			GameObject.Find ("RopeIcon").GetComponent<Image>().color = new Color32(255,255,255,255);
		}
		if(!mGame.CheckItem("hammer"))
		{
			GameObject.Find ("HammerIcon").GetComponent<Image>().color = new Color32(255,255,255,255);
		}
	}

	public void Close()
	{
		SilhouetteCanvas.enabled = false;
	}

}
