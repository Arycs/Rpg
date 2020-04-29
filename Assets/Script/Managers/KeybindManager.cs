using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{

    private static KeybindManager instance;

    public static KeybindManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
            }
            return instance;
        }
    }
    /// <summary>
    /// 操作按键
    /// </summary>
    public Dictionary<string, KeyCode> KeyBinds
    {
        get; private set;
    }
    /// <summary>
    /// 技能按键
    /// </summary>
    public Dictionary<string, KeyCode> ActionBinds
    {
        get;private set;
    }

    private string bindName;

    // Start is called before the first frame update
    void Start()
    {
        KeyBinds = new Dictionary<string, KeyCode>();

        ActionBinds = new Dictionary<string, KeyCode>();

        BindKey("UP", KeyCode.W);
        BindKey("LEFT", KeyCode.A);
        BindKey("DOWN", KeyCode.S);
        BindKey("RIGHT", KeyCode.D);

        BindKey("SPELLBOOK", KeyCode.P);
        BindKey("BAG", KeyCode.B);

        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);


    }

    public void BindKey(string key,KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = KeyBinds;

        if (key.Contains("ACT"))
        {
            currentDictionary = ActionBinds;
        }

        if (!currentDictionary.ContainsKey(key))
        {
            currentDictionary.Add(key, keyBind);
            UIManager.MyInstance.UpdateKeyText(key, keyBind);
        }
        if (currentDictionary.ContainsValue(keyBind))
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            currentDictionary[myKey] = KeyCode.None;
            UIManager.MyInstance.UpdateKeyText(myKey, KeyCode.None);
        }

        currentDictionary[key] = keyBind;
        UIManager.MyInstance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }

    private void OnGUI()
    {
        if (bindName != string.Empty)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
