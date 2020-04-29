using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "任务", menuName = "Quests/任务", order = 1)]
public class Quest
{
    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives;

    [SerializeField]
    private KillObjective[] killObjectives;

    [SerializeField]
    private int level;

    [SerializeField]
    private int xp;

    public QuestScript MyQuestScript
    {
        get;set;
    }

    public QuestGiver MyQuestGiver
    {
        get;set;
    }

    public bool IsComplete
    {
        get
        {
            foreach (Objective o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }
            foreach (Objective o in MyKillObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public string MyTitle
    {
        get => title;
        set => title = value;
    }
    public string MyDescription 
    {
        get => description;
        set => description = value;
    }
    public CollectObjective[] MyCollectObjectives
    {
        get => collectObjectives;
    }
    public KillObjective[] MyKillObjectives
    {
        get => killObjectives;
        set => killObjectives = value;
    }
    public int MyLevel
    {
        get => level;
        set => level = value;
    }
    public int MyXp
    {
        get => xp;
        set => xp = value;
    }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;

    private int currentAmount;

    [SerializeField]
    private string type;

    public int MyAmount
    {
        get => amount;
    }
    public int MyCurrentAmount
    {
        get => currentAmount;
        set => currentAmount = value;
    }
    public string MyType    
    {
        get => type;
    }
    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (MyType.ToLower() == item.MyTitle.ToLower())
        {
            MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(item.MyTitle);

            if (MyCurrentAmount <= MyAmount)
            {
                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}:{1}/{2}", item.MyTitle, MyCurrentAmount, MyAmount));
            }
          
            Questlog.MyInstance.UpdateSelected();
            Questlog.MyInstance.CheckCompletion();
        }
    }

    public void UpdateItemCount()
    {
        MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(MyType);

        Questlog.MyInstance.CheckCompletion();
        Questlog.MyInstance.UpdateSelected();
    }

    public void Complete()
    {
        Stack<Item> items = InventoryScript.MyInstance.GetItems(MyType, MyAmount);
        foreach (Item item in items)
        {
            item.Remove();
        }
    }
}

[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(Character character)
    {
        if (MyType == character.MyType)
        {
            if (MyCurrentAmount < MyAmount)
            {
                MyCurrentAmount++;
                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0} :{1}/{2}", MyType, MyCurrentAmount, MyAmount));
                Questlog.MyInstance.CheckCompletion();
                Questlog.MyInstance.UpdateSelected();
            }
        }
    }
}