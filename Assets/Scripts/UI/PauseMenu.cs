using System;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public TextMeshProUGUI modsText;

    private void OnEnable()
    {
        modsText.text = $"{ModType.MAX_VELOCITY}: {Player.GetMod(ModType.MAX_VELOCITY) * 100}% \n" +
                        $"{ModType.ACCELERATION}: {Player.GetMod(ModType.ACCELERATION) * 100}% \n" +
                        $"{ModType.ROTATION}: {Player.GetMod(ModType.ROTATION) * 100}% \n" +
                        $"{ModType.PICK_UP_RANGE}: {Player.GetMod(ModType.PICK_UP_RANGE) * 100}% \n" +
                        $"{ModType.TURRETS_DAMAGE}: {Player.GetMod(ModType.TURRETS_DAMAGE) * 100}% \n" +
                        $"{ModType.TURRETS_FIRE_RATE}: {Player.GetMod(ModType.TURRETS_FIRE_RATE) * 100}% \n";
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        GameController.Instance.TogglePauseMenu(false);
    }
}
