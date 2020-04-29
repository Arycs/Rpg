using System;
using UnityEngine;

/// <summary>
/// 挡板,射线挡板,用来判断敌人是否在可以攻击的范围
/// </summary>
[Serializable]
public class Block
{
    [SerializeField]
    private GameObject first, second;

    public void Deactivate()
    {
        first.SetActive(false);
        second.SetActive(false);
    }

    public void Activate()
    {
        first.SetActive(true);
        second.SetActive(true);
    }
}
