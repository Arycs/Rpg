using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Spell : IUseable,IMoveable,IDescribable
{
    [SerializeField]
    private string title;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private GameObject spellPrefab;

    [SerializeField]
    private Color barColor;

    public string MyTitle
    {
        get => title;
    }
    public int MyDamage
    {
        get => damage;
    }
    public Sprite MyIcon
    {
        get => icon;
    }
    public float MySpeed
    {
        get => speed;
    }
    public float MyCastTime
    {
        get => castTime;
    }
    public GameObject MySpellPrefab
    {
        get => spellPrefab;
    }
    public Color MyBarColor
    {
        get => barColor;
    }

    [SerializeField]
    private string description;

    public string GetDescription()
    {
        return string.Format("{0}\n Cast time : {1} seconde(s) \n<color=#ffd111>{2}\n that causes {3} damege</color>", title,castTime,description,damage);
    }

    public void Use()
    {
        Player.MyInstance.CastSpell(MyTitle);
    }
}
