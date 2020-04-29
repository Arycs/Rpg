using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;

/// <summary>
/// 关卡控制器(地图生成)
/// </summary>
public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// 生成地图的父物体
    /// </summary>
    [SerializeField]
    private Transform map;
    /// <summary>
    /// 由于根据图片的颜色来生成地图,因此该数组存储的是地图简图
    /// </summary>
    [SerializeField]
    private Texture2D[] mapData;
    /// <summary>
    /// 地图色块
    /// </summary>
    [SerializeField]
    private MapElement[] mapElements;
    /// <summary>
    /// 默认色块,根据这个的大小等属性,依次排列其他色块
    /// </summary>
    [SerializeField]
    private Sprite defaultTile;
    /// <summary>
    /// 水的色块字典
    /// </summary>
    private Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();

    /// <summary>
    /// 水的图集
    /// </summary>
    [SerializeField]
    private SpriteAtlas waterAtlas;


    /// <summary>
    /// 地图起始点
    /// </summary>
    private Vector3 WorldStartPos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        }
    }

    private void Start()
    {
        
        GenerateMap();
    }

    /// <summary>
    /// 创建地图
    /// </summary>
    private void GenerateMap()
    {
        //根据位置给不同色块添加 layer 防止遮挡
        int height = mapData[0].height;
        int width = mapData[0].width;

        for (int i = 0; i < mapData.Length; i++)
        {
            for (int x = 0; x < mapData[i].width; x++)
            {
                for (int y = 0; y < mapData[i].height; y++)
                {
                    Color c = mapData[i].GetPixel(x, y);

                    MapElement newElement = Array.Find(mapElements, e => e.MyColor == c);
                    if (newElement != null)
                    {
                        float xPos = WorldStartPos.x + (defaultTile.bounds.size.x * x);
                        float yPos = WorldStartPos.y + (defaultTile.bounds.size.y * y) ;

                        GameObject go = Instantiate(newElement.MyElementPrefab);
                        go.transform.position = new Vector2(xPos,yPos);

                        if (newElement.MyTileTag == "Water")
                        {
                            waterTiles.Add(new Point(x,y), go);
                        }


                        if (newElement.MyTileTag == "Tree")
                        {
                            go.GetComponent<SpriteRenderer>().sortingOrder = height * 2 - y * 2;
                        }

                        go.transform.parent = map;
                    }
                }
            }
        }

        CheckWater();
    }

    private void CheckWater()
    {
        foreach (KeyValuePair<Point,GameObject> tile in waterTiles)
        {
            string composition = TileCheck(tile.Key);

            if (composition[1] == 'E' && composition[3] == 'W'&&composition[4] == 'E' &&composition[6] =='W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("0");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("1");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("2");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("3");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("4");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("5");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("6");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("7");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("8");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("9");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("10");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("11");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("12");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("13");
            }


            if (composition[3] == 'W' && composition[5] == 'E' && composition[6] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("14");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[2] == 'E' && composition[4] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("15");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("16");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("17");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }






            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                int randomChance = UnityEngine.Random.Range(0, 100);
                if (randomChance < 15)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("19");
                }
            }
            if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W')
            {
                int randomChance = UnityEngine.Random.Range(0, 100);
                if (randomChance < 10)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("20");
                }
            }
        }
    }

    public string TileCheck(Point currentPoint)
    {
        string composition = string.Empty;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0)
                {
                    if (waterTiles.ContainsKey(new Point(currentPoint.MyX + x, currentPoint.MyY + y)))
                    {
                        composition += "W";
                    }
                    else
                    {
                        composition += "E";
                    }
                }
            }
        }
        return composition;
    }
}

/// <summary>
/// 地图色块类,包括Tag,对应mapdata的颜色,以及生成的预制体
/// </summary>
[Serializable]
public class MapElement
{
    [SerializeField]
    private string tileTag;

    [SerializeField]
    private Color color;

    [SerializeField]
    private GameObject elementPrefab;

    public GameObject MyElementPrefab
    {
        get => elementPrefab;
    }

    public Color MyColor
    {
        get => color;
    }
    public string MyTileTag
    {
        get => tileTag;
    }
}

/// <summary>
/// 地图上坐标点
/// </summary>
public struct Point
{
    public int MyX
    {
        get;set;
    }
    public int MyY
    {
        get; set;
    }

    public Point(int x ,int y)
    {
        this.MyX = x;
        this.MyY = y;
    }
}