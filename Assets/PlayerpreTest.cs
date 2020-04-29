using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerpreTest : MonoBehaviour
{
    private string playerName = "刘能";
    private string playerJob = "程序员";

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("玩家", playerName+"-"+playerJob);
        string playerdata =  PlayerPrefs.GetString("玩家");
        string[] playerdata1 =  playerdata.Split('-');
        foreach (var item in playerdata1)
        {
            Debug.Log(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
