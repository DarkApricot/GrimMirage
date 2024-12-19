using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    private bool pauseMenuState = false;

    [SerializeField] private AudioSource audioTest;

    private OverlayShaderHandler camShaderScript;

    private Coroutine timeStopCor;
    private Coroutine timeStopTimerCor;
    private Coroutine forceBackTimeCor;

    [SerializeField] private float timePassedWhilePaused;
    
    void Start()
    {
        camShaderScript = Camera.main.GetComponent<OverlayShaderHandler>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPauseMenu();
        }
    }

    private void SetPauseMenu()
    {
        pauseMenuState = !pauseMenuState;
        FindObjectOfType<PlayerMovement>()._GamePaused = pauseMenuState;
        pauseScreen.SetActive(pauseMenuState);

        if (pauseMenuState)
        {
            SetTime(1, 0, 1, 0.2f);
            camShaderScript.isForcingBack = 1;

            timeStopTimerCor = StartCoroutine(TimePausedTimer());
            forceBackTimeCor = StartCoroutine(ForceBackTime());
        }
        else
        {
            if (timeStopTimerCor != null) { StopCoroutine(timeStopTimerCor); }
            if (forceBackTimeCor != null) { StopCoroutine(forceBackTimeCor); timePassedWhilePaused = 0; }

            SetTime(0, 1, 0.2f, 1);

            camShaderScript.isForcingBack = 0;
        }
    }

    private void SetTime(float _startNr, float _endNr, float _audioStartSpeed, float _audioEndSpeed)
    {
        if (timeStopCor != null) { StopCoroutine(timeStopCor); }

        timeStopCor = StartCoroutine(TimeStop(_startNr, _endNr, _audioStartSpeed, _audioEndSpeed));
    }

    private IEnumerator TimeStop(float _startNr, float _endNr, float _audioStartSpeed, float _audioEndSpeed)
    {
        float _timeTracker = 0;

        while (_timeTracker < 1.2f)
        {
            Time.timeScale = Mathf.Lerp(_startNr, _endNr, _timeTracker / 1.2f); // Stops time
            audioTest.pitch = Mathf.Lerp(_audioStartSpeed, _audioEndSpeed, _timeTracker / 1.2f); // Decreases / increases speed ( Don't question this.)
            audioTest.spatialBlend = Mathf.Lerp(0, _startNr, _timeTracker / 1.2f); // Increases volume ( Also don't question this.. )
            _timeTracker += Time.unscaledDeltaTime;

            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = _endNr;
        audioTest.spatialBlend = _startNr; // Set "additional volume" to max
    }

    private IEnumerator TimePausedTimer()
    {
        while (true)
        {
            timePassedWhilePaused += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ForceBackTime()
    {
        yield return new WaitUntil(() => timePassedWhilePaused >= 4f);

        camShaderScript.isForcingBack = 2;
        float _timeTracker = 0;

        while (_timeTracker < 0.3f)
        {
            Time.timeScale = Mathf.Lerp(0, 0.2f, _timeTracker / 0.3f); // Stops time
            audioTest.pitch = Mathf.Lerp(0.2f, 0.6f, _timeTracker / 0.3f); // Decreases / increases speed ( Don't question this.)
            _timeTracker += Time.unscaledDeltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.2f);

        _timeTracker = 0;

        while (_timeTracker < 0.3f)
        {
            Time.timeScale = Mathf.Lerp(0.2f, 0f, _timeTracker / 0.3f); // Stops time
            audioTest.pitch = Mathf.Lerp(0.6f, 0.2f, _timeTracker / 0.3f); // Decreases / increases speed ( Don't question this.)
            _timeTracker += Time.unscaledDeltaTime;

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();

        camShaderScript.isForcingBack = 1;
        Time.timeScale = 0;
        audioTest.pitch = 0.2f;
    }
}