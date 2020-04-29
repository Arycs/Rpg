using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour
{

    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }
            return instance;
        }
    }

    private SlotScript fromSlot;

    /// <summary>
    /// 背包列表
    /// </summary>
    private List<Bag> bags = new List<Bag>();

    public bool CanAddBag
    {
        get
        {
            return MyBags.Count < 5;
        }
    }

    public SlotScript FromSlot
    {
        get => fromSlot;
        set
        {
            fromSlot = value;
            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }

    /// <summary>
    /// 背包总体空格子数量
    /// </summary>
    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (Bag bag in MyBags)
            {
                count += bag.MyBagScript.MyEmptySlotCount;
            }
            return count;
        }
    }

    /// <summary>
    /// 背包总体格子数量
    /// </summary>
    public int MyTotalSlotCount
    {
        get
        {
            int count = 0;
            foreach (Bag bag in MyBags)
            {
                count += bag.MyBagScript.MySlots.Count;
            }

            return count;
        }
    }

    /// <summary>
    /// 背包中已满的格子数量
    /// </summary>
    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }

    public List<Bag> MyBags
    {
        get => bags;
    }

    [Header("背包栏的按钮")]
    [SerializeField]
    private BagButton[] bagButtons;

    [Header("物品")]
    [SerializeField]
    private Item[] items;

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(16);
        bag.Use();
    }

    private void Update()
    {
        //Debug测试
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(8);
            bag.Use();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(16);
            AddItem(bag);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            AddItem((Armor)Instantiate(items[2]));
            AddItem((Armor)Instantiate(items[3]));
            AddItem((Armor)Instantiate(items[4]));
            AddItem((Armor)Instantiate(items[5]));
            AddItem((Armor)Instantiate(items[6]));
            AddItem((Armor)Instantiate(items[7]));
            AddItem((Armor)Instantiate(items[8]));
            AddItem((Armor)Instantiate(items[9]));
            AddItem((Armor)Instantiate(items[10]));
        }
    }

    /// <summary>
    /// 移除背包
    /// </summary>
    /// <param name="bag"></param>
    public void RemoveBag(Bag bag)
    {
        MyBags.Remove(bag);
        Destroy(bag.MyBagScript.gameObject);
    }


    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (MyTotalSlotCount - oldBag.MySlotCount) + newBag.MySlotCount;

        if (newSlotCount - MyFullSlotCount >=0)
        {
            //旧背包中的物品
            List<Item> bagItems = oldBag.MyBagScript.GetItems();

            RemoveBag(oldBag);

            newBag.MyBagButton = oldBag.MyBagButton;

            newBag.Use();

            foreach (Item item in bagItems)
            {
                if (item != newBag) 
                {
                    AddItem(item);
                }
            }

            AddItem(oldBag);

            HandScript.MyInstance.Drop();
            MyInstance.fromSlot = null;
        }
    }


    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                MyBags.Add(bag);
                bag.MyBagButton = bagButton;
                bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
                break;
            }
        }
    }

    public void AddBag(Bag bag,BagButton bagButton)
    {
        MyBags.Add(bag);
        bagButton.MyBag = bag;
        bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
    }

    public void AddBag(Bag bag, int bagIndex)
    {
        bag.SetupScript();
        MyBags.Add(bag);
        bag.MyBagButton = bagButtons[bagIndex];
        bagButtons[bagIndex].MyBag = bag;
    }

    public bool AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return true;
            }
        }
        return PlaceInEmpty(item);
    }

    /// <summary>
    /// 判断是否有空位置添加物品
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool PlaceInEmpty(Item item)
    {
        foreach (Bag bag in MyBags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 判断是否有可以叠加的物品
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slots in bag.MyBagScript.MySlots)
            {
                if (slots.StackItem(item))
                {
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 保存物品的具体位置
    /// </summary>
    /// <param name="item">具体物品</param>
    /// <param name="slotIndex">具体物品所在格子的索引</param>
    /// <param name="bagIndex">具体所在背包索引</param>
    public void PlaceInSpecific(Item item, int slotIndex, int bagIndex)
    {
        bags[bagIndex].MyBagScript.MySlots[slotIndex].AddItem(item);
    }

    public void OpenClose()
    {
        bool closeBag = MyBags.Find(x => !x.MyBagScript.IsOpen);
        //If closed bag == true, then open all closed bags;
        //If close bag == false, then close all open bags;
        foreach (Bag bag in MyBags)
        {
            if (bag.MyBagScript.IsOpen != closeBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    public List<SlotScript> GetAllItems()
    {
        List<SlotScript> slots = new List<SlotScript>();

        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty)
                {
                    slots.Add(slot);
                }
            }
        }
        return slots;
    }


    public Stack<IUseable> GetUseable(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
                {
                    foreach (Item item in slot.MyItems)
                    {
                        useables.Push(item as IUseable);
                    }
                }
            }
        }
        return useables;
    }

    public IUseable GetUseable(string type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    foreach (Item item in slot.MyItems)
                    {
                        return (slot.MyItem as IUseable);
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 得到物品数量
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int GetItemCount(string type)
    {
        int itemCount = 0;
        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    itemCount += slot.MyItems.Count;
                }
            }
        }
        return itemCount;
    }

    /// <summary>
    /// 得到物品
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public Stack<Item> GetItems(string type,int count)
    {
        Stack<Item> items = new Stack<Item>();
        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    foreach (Item item in slot.MyItems)
                    {
                        items.Push(item);
                        if (items.Count == count)
                        {
                            return items;
                        }
                    }
                }
            }
        }
        return items;
    }

    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }
}
