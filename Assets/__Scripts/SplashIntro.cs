using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashIntro : MonoBehaviour
{
    public Image logoUSC;
    public Image logoBerklee;
    public GameObject splashBG;

    // Use this for initialization
    void Start ()
    {
        // Only load on first playthrough
        Debug.Log(GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().GetPreviousSceneIndex());
        if ( GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().GetPreviousSceneIndex() < 1 )
        {
            splashBG.SetActive(true);
            logoUSC.canvasRenderer.SetAlpha(0.0f);
            logoBerklee.canvasRenderer.SetAlpha(0.0f);
            StartCoroutine(FadeInThenOut());
        }
    }

    // Update is called once per frame
    void Update ()
    {

    }

    IEnumerator FadeInThenOut()
    {
        logoUSC.CrossFadeAlpha ( 1.0f, 2.0f, false );
        yield return new WaitForSeconds ( 4 );
        logoUSC.CrossFadeAlpha ( 0.0f, 1.5f, false );
        yield return new WaitForSeconds ( 1.5f );
        logoBerklee.CrossFadeAlpha ( 1.0f, 2.0f, false );
        yield return new WaitForSeconds ( 4 );
        logoBerklee.CrossFadeAlpha ( 0.0f, 1.5f, false );
        yield return new WaitForSeconds ( 1.5f );
        splashBG.GetComponent<Image> ().CrossFadeAlpha ( 0.0f, 3.0f, false );
        yield return new WaitForSeconds ( 3 );
        gameObject.SetActive ( false );
    }
}
