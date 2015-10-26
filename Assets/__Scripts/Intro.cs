﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Intro : MonoBehaviour
{

    public Image screenFadeFill;
	public GameObject screenCover;

    public Tutorial tutorial;

    private float fadeTime = 0.5f;
    public bool isFadingIn;

    public bool beginGame = false;

    private Character m_Char;

	public bool playIntro = true;

    void Awake()
    {
        screenFadeFill.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }


    void Start()
    {
        m_Char = GameObject.Find("Character").GetComponent<Character>();
        m_Char.SetPlayerState(Character.PlayerState.Interact);
        if(playIntro){
			StartCoroutine(TimedDialogueShow());
		}
		else{
			screenFadeFill.color = Color.clear;
			screenCover.SetActive(false);
			isFadingIn = false;
			m_Char.SetPlayerState(Character.PlayerState.Idle);
			tutorial.SetHint("wasd");
		}
    }

    void Update()
    {
        if (isFadingIn)
        {
            FadeIn();
        }
    }


    private void FadeIn()
    {
        // Fades to clear
        screenFadeFill.color = Color.Lerp(screenFadeFill.color, Color.clear, fadeTime * Time.deltaTime);
        m_Char.SetPlayerState(Character.PlayerState.Interact);
        if (screenFadeFill.color.a <= 0.15f)
        {
            screenFadeFill.color = Color.clear;
			screenCover.SetActive(false);
            isFadingIn = false;
            m_Char.SetPlayerState(Character.PlayerState.Idle);
            tutorial.SetHint("wasd");
        }
    }

    IEnumerator TimedDialogueShow(int seconds = 5)
    {
        yield return new WaitForSeconds(seconds);
        tutorial.ShowDialogue(11, 13);
    }
}
