using System;
using System.Collections;
using Redcode.Moroutines;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public static bool IsPaused;

    public bool isDead;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;

    private InputAction pauseAction;

    private void Awake()
    {
        Instance = this;
        pauseAction = InputSystem.actions.FindAction("Pause");
    }

    public void SetPause(bool pause)
    {
        IsPaused = pause;
        Time.timeScale = pause ? 0f : 1f;

        if (!deathMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(pause);
        }

        AudioListener.pause = pause;
    }

    private void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            SetPause(!IsPaused);
        }
    }

    public void OnPlayerDeath()
    {
        IsPaused = false;
        isDead = true;
        Moroutine.Run(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        float timer = 0;

        while (timer < 2f)
        {
            Time.timeScale = 1 - (timer / 2f);
            timer += Time.unscaledDeltaTime;

            yield return null;
        }

        Time.timeScale = 0f;
        deathMenu.SetActive(true);
    }
}
