using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 技能栏按键
/// </summary>
public class ActionButton : MonoBehaviour, IPointerClickHandler,IClickable, IPointerEnterHandler, IPointerExitHandler
{
    public IUseable MyUseable
    {
        get;set;
    }

    [SerializeField]
    private Text stackSize;

    private Stack<IUseable> useables =new Stack<IUseable>();

    private int count;

    public Button MyButton
    {
        get;private set;
    }

    public Image MyIcon
    {
        get => icon;
        set => icon = value;
    }

   
    [SerializeField]
    private Image icon;

    public int MyCount
    {
        get
        {
            return count;
        }
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    public Stack<IUseable> MyUseables
    {
        get => useables;
        set
        {
            if (value.Count > 0)
            {
                MyUseable = value.Peek();

            }
            else
            {
                MyUseable = null;
            }
            useables = value;
        }
    }

    public void OnClick()
    {
        if (HandScript.MyInstance.MyMoveable == null)
        {
            if (MyUseable != null)
            {
                MyUseable.Use();
            }
            else if (MyUseables != null && MyUseables.Count > 0)
            {
                MyUseables.Peek().Use();
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);

        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUseable)
            {
                SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
            }
        }
    }

    public void SetUseable(IUseable useable)
    {
        if (useable is Item)
        {
            MyUseables = InventoryScript.MyInstance.GetUseable(useable);
            //count = useables.Count;
            if (InventoryScript.MyInstance.FromSlot)
            {
                InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
                InventoryScript.MyInstance.FromSlot = null;
            }
        }
        else
        {
            MyUseables.Clear();
            this.MyUseable = useable;
        }

        count = MyUseables.Count;

        UpdateVisual(useable as IMoveable);
        UIManager.MyInstance.RefreshToolTip(MyUseable as IDescribable);
    }

    public void UpdateVisual(IMoveable moveable)
    {

        if (HandScript.MyInstance.MyMoveable != null)
        {
            HandScript.MyInstance.Drop();
        }

        MyIcon.sprite = moveable.MyIcon;
        MyIcon.color = Color.white;

        if (count > 1)
        {
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    public void UpdateItemCount(Item item)
    {
        //If item is the same items as we have or this button
        if (item is IUseable && MyUseables.Count > 0)
        {
            if (MyUseables.Peek().GetType() == item.GetType())
            {
                MyUseables = InventoryScript.MyInstance.GetUseable(item as IUseable);
                count = MyUseables.Count;
                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable tmp = null;

        if (MyUseable != null && MyUseable is IDescribable)
        {
            tmp = (IDescribable)MyUseable ;
            //UIManager.MyInstance.ShowToolTip(transform.position);
        }
        else if (MyUseables.Count > 0)
        {
            //UIManager.MyInstance.ShowToolTip(transform.position);
        }
        if (tmp != null)
        {
            UIManager.MyInstance.ShowToolTip(new Vector2 (1,0),transform.position, tmp);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideToolTip();
    }
}
