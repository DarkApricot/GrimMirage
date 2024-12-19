using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AwaredWinningSceneChange : MonoBehaviour
{
    [SerializeField] private GameObject endScreen;
    [SerializeField] private AudioSource playerAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            if (endScreen != null)
            {
                endScreen.SetActive(true);
                playerAudioSource.enabled = false;
            }
        }
    }
}
