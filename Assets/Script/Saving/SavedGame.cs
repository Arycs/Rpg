using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
    [SerializeField]
    private Text dateTime;

    [SerializeField]
    private Image health;

    [SerializeField]
    private Image mana;

    [SerializeField]
    private Image xp;

    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text manaText;
    [SerializeField]
    private Text xpText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private GameObject visuals;

    [SerializeField]
    private int index;

    public int MyIndex
    {
        get => index;
        set => index = value;
    }

    private void Awake()
    {
        visuals.SetActive(false);
    }

    public void ShowInfo(SaveData saveData)
    {
        visuals.SetActive(true);

        dateTime.text = "日期:" + saveData.MyDateTime.ToString("dd/MM/yy") + "- 时间:" + saveData.MyDateTime.ToString("H:mm");

        health.fillAmount = saveData.MyPlayerData.MyHealth / saveData.MyPlayerData.MyMaxHealth;
        healthText.text = saveData.MyPlayerData.MyHealth + "/" + saveData.MyPlayerData.MyMaxHealth;

        mana.fillAmount = saveData.MyPlayerData.MyMana / saveData.MyPlayerData.MyMaxMana;
        manaText.text = saveData.MyPlayerData.MyMana + "/"+ saveData.MyPlayerData.MyMaxMana;

        xp.fillAmount = saveData.MyPlayerData.MyXp / saveData.MyPlayerData.MyMaxXp;
        xpText.text = saveData.MyPlayerData.MyXp + "/" + saveData.MyPlayerData.MyMaxXp;

        levelText.text = saveData.MyPlayerData.MyLevel.ToString();
    }

    public void HideVisuals()
    {
        visuals.SetActive(false);
    }
}
