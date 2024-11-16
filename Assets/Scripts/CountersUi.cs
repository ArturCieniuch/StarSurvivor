using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountersUi : MonoBehaviour
{
    public Image hpBar;
    public TextMeshProUGUI pointText;

    private void Start()
    {
        Player.Instance.onHpChange.AddListener(UpdateHp);
        Player.Instance.onPointsChange.AddListener(UpdatePoints);
    }

    private void UpdateHp(int hp, int maxHp)
    {
        hpBar.fillAmount = (float)hp/maxHp;
    }

    private void UpdatePoints(int points)
    {
        pointText.text = points.ToString();
    }
}
