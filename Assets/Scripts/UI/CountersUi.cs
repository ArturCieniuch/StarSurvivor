using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountersUi : MonoBehaviour
{
    public Image hpBar;
    public Image exBar;
    public TextMeshProUGUI levelText;

    private void Start()
    {
        Player.Instance.onHpChange.AddListener(UpdateHp);
        Player.Instance.onExChange.AddListener(UpdateEx);
        Player.Instance.onLevelChange.AddListener(UpdateLevel);
    }

    private void UpdateHp(int hp, int maxHp)
    {
        hpBar.fillAmount = (float)hp/maxHp;
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
