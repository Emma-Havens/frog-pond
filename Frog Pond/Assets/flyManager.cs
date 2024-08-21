using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class flyManager : MonoBehaviour
{
    public float age;
    private float birthday;

    private float moveTime = 0;
    private float moveGap = .75f;

    public (int i, int j) pos;

    void Start()
    {
        birthday = Time.time;
    }


    void Update()
    {
        age = Time.time - birthday;
        moveTime += Time.deltaTime;
        if (moveTime > moveGap)
        {
            Move();
            moveTime = 0;
        }

    }

    void Move()
    {
        pond.flyBoard[pos.i, pos.j] = null;
        bool goodPlace = false;
        int i = 0;
        int j = 0;
        while (!goodPlace)
        {
            i = Random.Range(-1, 2);
            j = Random.Range(-1, 2);
            if (canMove(i, j))
            {
                goodPlace = true;
            }
        }
        pos = (pos.i + i, pos.j + j);
        pond.flyBoard[pos.i, pos.j] = this.gameObject;
        Vector2 newPlace = pond.Grid(pos.i, pos.j);
        transform.position = new Vector3(newPlace.x, newPlace.y, -1);
    }

    private bool canMove(int i, int j)
    {
        if (onBoard(pos.i + i, pos.j + j))
        {
            GameObject dest = pond.board[pos.i + i, pos.j + j];
            if (dest != null && dest.tag == "Pad" &&
                    (pos.i + i, pos.j + j) != pond.playerFrog.pos)
            {
                return true;
            }
        }

        return false;
    }

    private bool onBoard(int i, int j)
    {
        bool inBoundsi = 0 <= i && i <= pond.board.GetUpperBound(0);
        bool inBoundsj = 0 <= j && j <= pond.board.GetUpperBound(1);
        return inBoundsi && inBoundsj;
    }
}
