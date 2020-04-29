﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{

    public Quest MyQuest
    {
        get; set;
    }
    /// <summary>
    /// 任务是否完成
    /// </summary>
    public bool markedComplete
    {
        get; set;
    }

    public void Select()
    {
        GetComponent<Text>().color = Color.red;
        Questlog.MyInstance.ShowDescription(MyQuest);
    }

    public void DeSelect()
    {
        GetComponent<Text>().color = Color.white;
    }

    public void IsComplete()
    {
        if (MyQuest.IsComplete && !markedComplete)
        {
            markedComplete = true;
            GetComponent<Text>().text += "(Complete)";
            MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}(C)", MyQuest.MyTitle));
        }
        else if (!MyQuest.IsComplete)
        {
            markedComplete = false;
            GetComponent<Text>().text = "[" + MyQuest.MyLevel + "]" + MyQuest.MyTitle;
        }
    }
}
