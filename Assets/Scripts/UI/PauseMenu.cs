using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        GameController.Instance.SetPause(false);
    }
}
