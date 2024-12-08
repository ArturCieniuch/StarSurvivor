using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountersUi : MonoBehaviour
{
    public Image hpBar;
    public TextMeshProUGUI hpText;
    public Image exBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;

    private void Start()
    {
        Player.Instance.onHpChange += UpdateHp;
        Player.Instance.onExChange += UpdateEx;
        Player.Instance.onLevelChange += UpdateLevel;
    }

    private void Update()
    {
        timeText.text = TimeSpan.FromSeconds(Time.timeSinceLevelLoad).ToString(@"mm\:ss");
    }

    private void UpdateHp(int hp, int maxHp)
    {
        hpBar.fillAmount = (float)hp/maxHp;
        hpText.text = $"{hp}/{maxHp}";
    }

    private void UpdateEx(int points, int pointToLevel)
    {
        exBar.fillAmount = (float)points / pointToLevel;
    }

    private void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
    }
}
