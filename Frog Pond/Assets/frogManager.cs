using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogManager : MonoBehaviour
{
    Transform trans;

    static AudioSource frogAudio;

    public AudioClip eatFlySound;
    public AudioClip waterSplash;

    public (int i, int j) pos;

    (int, int) startPos = (6, 3);

    //frog 1: .2
    //frog 2: .3
    
    void Start()
    {
        trans = this.GetComponent<Transform>();
        pos = startPos;
        frogAudio = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        Move();
    }

    void Move()
    {
        bool changed = false;
        if (Input.GetKeyDown(KeyCode.UpArrow) && canMove(0, 1))
        {
            pos = (pos.i, pos.j + 1);
            changed = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && canMove(0, -1))
        {
            pos = (pos.i, pos.j - 1);
            changed = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && canMove(-1, 0))
        {
            pos = (pos.i - 1, pos.j);
            changed = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && canMove(1, 0))
        {
            pos = (pos.i + 1, pos.j);
            changed = true;
        }

        if (changed)
        {
            Vector2 newPlace = pond.Grid(pos.i, pos.j);
            trans.position = new Vector3(newPlace.x, newPlace.y, -1);

            if (pond.board[pos.i, pos.j] == null)
            {
                fallInWater();
            }
            if (pond.flyBoard[pos.i, pos.j] != null)
            {
                eatFly(pos.i, pos.j);
            }
        }
    }

    private bool canMove(int i, int j)
    {
        if (onBoard(pos.i + i, pos.j + j))
        {
            GameObject dest = pond.board[pos.i + i, pos.j + j];
            if (dest == null || dest.tag == "Pad")
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

    public void fallInWater()
    {
        frogAudio.PlayOneShot(waterSplash);
        score.fallenInWater();
        bool goodPlace = false;
        int i = -1;
        int j = -1;
        Vector2 newPlace = new Vector2();
        while (!goodPlace)
        {
            i = Random.Range(0, 10);
            j = Random.Range(0, 5);
            newPlace = pond.Grid(i, j);
            if (pond.board[i, j] != null && pond.board[i, j].tag == "Pad")
            {
                goodPlace = true;
            }
        }
        pos = (i, j);
        trans.position = new Vector3(newPlace.x, newPlace.y, -1);
    }

    public void eatFly(int i, int j)
    {
        pond.flyCount--;
        Destroy(pond.flyBoard[i, j]);
        frogAudio.PlayOneShot(eatFlySound);
        score.flyEaten();
    }
}
