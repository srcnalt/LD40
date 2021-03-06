﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Image blackScreen;

    private Animator buttonAnimator;
    private float fadeSpeed = 1;

    private void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    public void MouseOver()
    {
        buttonAnimator.SetBool("Selected", true);
        GetComponent<AudioSource>().Play();
    }

    public void MouseExit()
    {
        buttonAnimator.SetBool("Selected", false);
    }

    public void MouseDown(string target)
    {
        if (target == "game")
            StartCoroutine("StartGame");
        else if (target == "menu")
            SceneManager.LoadScene(Scenes.Menu);
        else if (target == "quit")
            Application.Quit();
        else
            SceneManager.LoadScene(Scenes.Credits);
    }

    public IEnumerator StartGame()
    {
        Color color = new Color(0, 0, 0, 0);

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * fadeSpeed;
            blackScreen.color = color;
            yield return null;
        }

        SceneManager.LoadScene(Scenes.Game);
    }
}
