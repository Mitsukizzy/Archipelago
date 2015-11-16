using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinFade : MonoBehaviour
{
    public GameObject blackOverlay;

    public void StartOverlay()
    {
        StartCoroutine(runOverlay());
    }

    IEnumerator runOverlay()
    {
        blackOverlay.SetActive(true);
        blackOverlay.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        blackOverlay.GetComponent<Image>().CrossFadeAlpha(1.0f, 2.0f, false);
        //insert building sound effects here??
        yield return new WaitForSeconds(4);
    }
}
