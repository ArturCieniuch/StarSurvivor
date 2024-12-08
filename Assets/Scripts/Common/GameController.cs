using System.Collections;
using Redcode.Moroutines;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public static bool IsPaused;

    public bool isDead;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject levelUpMenu;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource deathMusic;


    private InputAction pauseAction;

    public static Random Rand;

    private void Awake()
    {
        Instance = this;
        Rand = new Random(1312);
        pauseAction = InputSystem.actions.FindAction("Pause");
        music.ignoreListenerPause = true;
        deathMusic.ignoreListenerPause = true;
    }

    public void SetPause(bool pause)
    {
        IsPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
        AudioListener.pause = pause;
    }

    public void TogglePauseMenu(bool enable)
    {
        SetPause(enable);
        pauseMenu.SetActive(enable);
    }

    private void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            TogglePauseMenu(!IsPaused);
        }
    }

    public void OnLevelUp()
    {
        SetPause(true);
        levelUpMenu.SetActive(true);
    }

    public void OnPlayerDeath()
    {
        IsPaused = false;
        isDead = true;
        music.Stop();
        deathMusic.Play();
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
