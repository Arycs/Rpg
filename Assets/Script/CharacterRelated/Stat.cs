using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 血量,蓝量
/// </summary>
public class Stat : MonoBehaviour
{
    private Image content;

    [SerializeField]
    private Text statValue;

    /// <summary>
    /// 变少的速度
    /// </summary>
    [SerializeField]
    private float lerpSpeed = 1;

    /// <summary>
    /// 当前值占最大值的百分比
    /// </summary>
    private float currentFil;

    private float overflow;

    public float MyMaxValue
    {
        get;set;
    }

    private float currentValue;

    public float MyCurrentValue
    {
        get => currentValue;
        set {
            if (value > MyMaxValue)
            {
                overflow = value - MyMaxValue;
                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }
            currentFil = currentValue / MyMaxValue;

            if (statValue != null)
            {
                statValue.text = currentValue + "/" + MyMaxValue;
            }

        }
    } 

    public float MyOverflow
    {
        get
        {
            float tmp = overflow;
            overflow = 0;
            return tmp;
        }
    }

    public bool IsFull
    {
        get
        {
            return content.fillAmount == 1;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFil != content.fillAmount)
        {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFil, Time.deltaTime * lerpSpeed);
        }
    }

    /// <summary>
    /// 初始化 最大值和当前值
    /// </summary>
    /// <param name="currentValue"></param>
    /// <param name="maxValue"></param>
    public void Initialize(float currentValue,float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    public void Reset()
    {
        content.fillAmount = 0;
    }
}
