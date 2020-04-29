using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField]
    private Quest[] quests;

    [SerializeField]
    private Sprite question, questionSliver, exclemation;

    [SerializeField]
    private SpriteRenderer statusRenderer;

    [SerializeField]
    private int questGiverID;

    public Quest[] MyQuests
    {
        get => quests;
    }
    public int MyQuestGiverID
    {
        get => questGiverID;
        set => questGiverID = value;
    }
    public List<string> MyCompltedQuests
    {
        get => compltedQuests;
        set {
            compltedQuests = value;
            foreach (string title in compltedQuests)
            {
                for (int i = 0; i < quests.Length; i++)
                {
                    if (quests[i] != null &&quests[i].MyTitle == title)
                    {
                        quests[i] = null;
                    }
                }
            }
        }
    }

    private List<string> compltedQuests = new List<string>();

    private void Start()
    {
        foreach (Quest quest in quests)
        {
            quest.MyQuestGiver = this;
        }
    }

    public void UpdateQuestStatus()
    {
        int count = 0;

        foreach (Quest quest in quests)
        {
            if (quest != null)
            {
                if (quest.IsComplete && Questlog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    break;
                }
                else if (!Questlog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = exclemation;
                    break;
                }
                else if (!quest.IsComplete && Questlog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSliver;
                }
            }
            else
            {
                count++;
                if (count == quests.Length)
                {
                    statusRenderer.enabled = false;
                }
            }
        }
    }

}
