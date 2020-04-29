using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [Header("键位菜单")]
    [SerializeField]
    private CanvasGroup keybindMenu;

    [Header("技能书")]
    [SerializeField]
    private CanvasGroup spellBook;

    [Header("提示")]
    [SerializeField]
    private GameObject toolTip;

    [Header("角色面板")]
    [SerializeField]
    private CharacterPanel charPanel;

    [SerializeField]
    private RectTransform tooltipRect;

    private Text toolTipText;

    /// <summary>
    /// 按键按钮组
    /// </summary>
    private GameObject[] keybindButtons;

    /// <summary>
    /// 按钮的集合,让按钮也相应映射到快捷键上
    /// </summary>
    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    [Header("所有界面")]
    private CanvasGroup[] menus;

    [Header("目标单位的画布")]
    [SerializeField]
    private GameObject targetFrame;

    [SerializeField]
    private Text levelText;

    private Stat healthStat;

    [Header("目标单位的头像")]
    [SerializeField]
    private Image portraitFrame;

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        toolTipText = toolTip.GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(menus[0]);
        }
       
        if (Input.GetKeyDown(KeybindManager.MyInstance.KeyBinds["SPELLBOOK"]))
        {
            OpenClose(menus[1]);
        }

        if (Input.GetKeyDown(KeybindManager.MyInstance.KeyBinds["BAG"]))
        {
            InventoryScript.MyInstance.OpenClose();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenClose(menus[2]);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OpenClose(menus[3]);
        }
    }

    /// <summary>
    /// 在屏幕中显示敌人血条,头像等
    /// </summary>
    /// <param name="target"></param>
    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);
        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);
        portraitFrame.sprite = target.MyPortrait;
        levelText.text = target.MyLevel.ToString();
        target.healthChanged += new HealthChanged(UpdateTargetFrame);
        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

        if (target.MyLevel >= Player.MyInstance.MyLevel + 5)
        {
            levelText.color = Color.red;
        }
        else if (target.MyLevel == Player.MyInstance.MyLevel + 3 || target.MyLevel == Player.MyInstance.MyLevel + 4)
        {
            levelText.color = new Color(255, 124, 0, 255);
        }
        else if (target.MyLevel >= Player.MyInstance.MyLevel - 2 && target.MyLevel <= Player.MyInstance.MyLevel + 2)
        {
            levelText.color = Color.yellow;
        }
        else if (target.MyLevel <= Player.MyInstance.MyLevel - 3 && target.MyLevel > Player.MyInstance.MyLevel - 10)
        {
            levelText.color = Color.green;
        }
        else
        {
            levelText.color = Color.grey;
        }
    }
    /// <summary>
    /// 隐藏敌人血条头像
    /// </summary>
    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }
    /// <summary>
    /// 更新敌人血条
    /// </summary>
    /// <param name="health"></param>
    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }

    /// <summary>
    /// 更新按钮绑定显示
    /// </summary>
    /// <param name="key"></param>
    /// <param name="code"></param>
    public void UpdateKeyText(string key,KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    /// <summary>
    /// 点击ActionButton
    /// </summary>
    /// <param name="buttonName"></param>
    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = (canvasGroup.blocksRaycasts == true ? false : true);
    }

    public void OpenSingle(CanvasGroup canvasGroup)
    {
        foreach (CanvasGroup canvas in menus)
        {
            CloseSingle(canvas);
        }

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void CloseSingle(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }


    /// <summary>
    /// 更新快捷栏上的物品数量
    /// </summary>
    /// <param name="clickable"></param>
    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = Color.white;
        }

        if (clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }

    public void ShowToolTip(Vector2 pivot, Vector3 position,IDescribable description)
    {
        tooltipRect.pivot = pivot;
        toolTip.SetActive(true);
        toolTip.transform.position = position;
        toolTipText.text = description.GetDescription();
    }
    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }

    public void RefreshToolTip(IDescribable description)
    {
        toolTipText.text = description.GetDescription();
    }
}
