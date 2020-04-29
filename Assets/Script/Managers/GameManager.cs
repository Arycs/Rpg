using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);

public class GameManager : MonoBehaviour
{
    public event KillConfirmed killConfirmedEvent;

    private Camera mainCamera;

    private static GameManager instance;
    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    private HashSet<Vector3Int> blocked = new HashSet<Vector3Int>();

    public HashSet<Vector3Int> Blocked
    {
        get
        {
            return blocked;
        }

        set
        {
            blocked = value;
        }
    }

    [SerializeField]
    private LayerMask clickableLayer, groundLayer;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Enemy currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
    }

    /// <summary>
    /// 鼠标点击,选择目标
    /// </summary>
    private void ClickTarget()
    {
        //双重判断, 第二个判断是否点到UI
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //512 = LayerMask.GetMask("Clickable")
            //射线检测
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
                currentTarget = hit.collider.GetComponent<Enemy>();

                player.MyTarget = currentTarget.Select();

                UIManager.MyInstance.ShowTargetFrame(currentTarget);


            }
            else{

                UIManager.MyInstance.HideTargetFrame();

                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
                currentTarget = null;
                player.MyTarget = null;
            }
        }
        else if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            //射线检测
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayer);

            if (hit.collider != null)
            {
                IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();
                if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && hit.collider.gameObject.GetComponent<IInteractable>() == player.MyInteractable)
                {
                    entity.Interact();
                }
            }
            else
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, groundLayer);
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    Vector3 temp = new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y, 0);
                    player.GetPath(temp);
                }
            }
        }
       
    }

    public void OnKillConfirmed(Character character)
    {
        killConfirmedEvent?.Invoke(character);
    }
}
