using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathState : IState
{
    private Stack<Vector3> path;

    private Vector3 destination;

    private Vector3 current;

    private Vector3 goal;

    private Transform transform;

    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;

        this.transform = parent.transform;

        path = parent.MyAstar.Algorithm(parent.transform.parent.position, parent.MyTarget.position);

        current = path.Pop();
        destination = path.Pop();

        this.goal = parent.MyTarget.parent.position;

    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (path!=null)
        {

            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, 2 * Time.deltaTime);

            Vector3Int dest = parent.MyAstar.MyTilemap.WorldToCell(destination);
            Vector3Int cur = parent.MyAstar.MyTilemap.WorldToCell(current);

            float distance = Vector2.Distance(destination, transform.parent.position);

            if (cur.y > dest.y)
            {
                parent.Direction = Vector2.down;
            }
            else if (cur.y < dest.y)
            {
                parent.Direction = Vector2.up;
            }
            if (cur.y == dest.y)
            {
                if (cur.x > dest.x)
                {
                    parent.Direction = Vector2.left;
                }
                else if (cur.x < dest.x)
                {
                    parent.Direction = Vector2.right;
                }
            }

            if (distance <=0f)
            {
                if (path.Count > 0)
                {
                    current = destination;
                    destination = path.Pop();
                }
                else
                {
                    path = null;
                }
            }
        }
    }
}
