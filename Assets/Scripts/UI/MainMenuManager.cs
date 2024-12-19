using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private Animator animator;
    private MainMenuButton[] menuButtons;
    void Start()
    {
        animator = GetComponent<Animator>();
        menuButtons = GetComponentsInChildren<MainMenuButton>();
    }

    void Update()
    {
    }

    public void StartGame()
    {

    }

    public void RollCredits()
    {
        animator.enabled = true;
        animator.SetTrigger("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        animator.SetTrigger("Credits");
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }
}
