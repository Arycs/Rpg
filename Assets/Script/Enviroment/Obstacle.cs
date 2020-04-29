using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 障碍物类
/// </summary>
public class Obstacle : MonoBehaviour,IComparable<Obstacle>
{
    public SpriteRenderer MySpriteRenderer
    {
        get;set;
    }
    //默认颜色和半透明颜色
    private Color defaultColor;
    private Color fadedColor;

    public int CompareTo(Obstacle other)
    {
        if (MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1;
        }
        else if (MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
        {
            return -1;
        }
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = MySpriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 设置为半透明
    /// </summary>
    public void FadeOut()
    {
        MySpriteRenderer.color = fadedColor;
    }
    /// <summary>
    /// 正常颜色
    /// </summary>
    public void FadeIn()
    {
        MySpriteRenderer.color = defaultColor;
    }
}
