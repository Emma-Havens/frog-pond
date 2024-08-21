using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pond : MonoBehaviour
{
    public GameObject lilyPrefab;
    public GameObject reedPrefab;
    public GameObject flyPrefab;

    private int[,] reedMap =
        new int[13, 2] { { 0, 0 },
                         { 0, 3 },
                         { 2, 2 },
                         { 3, 1 },
                         { 3, 4 },
                         { 4, 3 },
                         { 5, 3 },
                         { 7, 3 },
                         { 7, 4 },
                         { 8, 1 },
                         { 8, 3 },
                         { 8, 4 },
                         { 9, 4 } };
    private int[,] padMap =
        new int[23, 2] { { 0, 2 },
                         { 1, 1 },
                         { 1, 2 },
                         { 1, 4 },
                         { 2, 0 },
                         { 3, 0 },
                         { 3, 2 },
                         { 3, 3 },
                         { 4, 1 },
                         { 4, 4 },
                         { 5, 0 },
                         { 5, 1 },
                         { 5, 4 },
                         { 6, 0 },
                         { 6, 1 },
                         { 6, 2 },
                         { 6, 3 },
                         { 6, 4 },
                         { 7, 0 },
                         { 7, 2 },
                         { 8, 2 },
                         { 9, 0 },
                         { 9, 3 } };

    public static GameObject[,] board = new GameObject[10, 10];
    public static GameObject[,] flyBoard = new GameObject[10, 10];

    static SpriteRenderer pad;
    public static frogManager playerFrog;

    private int spawnGap = 3;
    private int spawnTime = 1;
    public static int flyCount = 0;
    private int flyLimit = 4;
    private int maxFlyTime = 15;

    // Start is called before the first frame update
    void Start()
    {
        pad = lilyPrefab.GetComponent<SpriteRenderer>();
        playerFrog = FindObjectOfType<frogManager>();
        for (int i = 0; i <= board.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= board.GetUpperBound(1); j++)
            {
                board[i, j] = null;
            }
        }
        for (int k = 0; k <= padMap.GetUpperBound(0); k++) 
        {
            int i = padMap[k, 0];
            int j = padMap[k, 1];
            Vector2 pos = Grid(i, j);
            board[i, j] = Instantiate(lilyPrefab, pos, Quaternion.identity);
        }
        for (int k = 0; k <= reedMap.GetUpperBound(0); k++)
        {
            int i = reedMap[k, 0];
            int j = reedMap[k, 1];
            Vector2 pos = Grid(i, j);
            board[i, j] = Instantiate(reedPrefab, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //every so often, sink and surface lily pads
        if (Time.time > spawnTime)
        {
            spawnPad();
            StartCoroutine(destroyPad());
            if (flyCount < flyLimit)
            {
                spawnFly();
            }
            spawnTime += spawnGap;
        }
        foreach (var fly in GameObject.FindGameObjectsWithTag("Fly"))
        {
            if (fly.GetComponent<flyManager>().age > maxFlyTime)
            {
                destroyFly(fly);
            }
        }
    }

    //returns a position corresponding to the pond grid
    // i <= 10, j <= 5
    public static Vector2 Grid(float i, float j)
    {
        Vector2 pos = new Vector2 ((i+1) / 10, (j+1) / 5);
        //Debug.Log(i + ", " + j);
        Vector2 world = Camera.main.ViewportToWorldPoint(pos);
        world.x -= pad.sprite.bounds.extents.x;
        world.y -= pad.sprite.bounds.extents.y;
        return world;
    }

    void spawnPad()
    {
        bool goodPlace = false;
        int i = -1;
        int j = -1;
        Vector2 pos = new Vector2();
        while (!goodPlace)
        {
            i = Random.Range(0, 10);
            j = Random.Range(0, 5);
            pos = Grid(i, j);
            if (board[i, j] == null)
            {
                goodPlace = true;
            }
        }
        board[i, j] = Instantiate(lilyPrefab, pos, Quaternion.identity);
    }

    IEnumerator destroyPad()
    {
        bool goodPlace = false;
        int i = -1;
        int j = -1;
        while (!goodPlace)
        {
            i = Random.Range(0, 10);
            j = Random.Range(0, 5);
            if (board[i, j] != null && board[i, j].tag == "Pad" && flyBoard[i, j] == null)
            {
                goodPlace = true;
            }
        }
        GameObject padToDestroy = board[i, j];
        Vector3 scale = padToDestroy.transform.localScale;
        padToDestroy.transform.localScale =
                new Vector3(scale.x * .7f, scale.y * .7f, scale.z * .7f);
        yield return new WaitForSecondsRealtime(3);
        Destroy(board[i, j]);

        if (playerFrog.pos == (i, j))
        {
            playerFrog.fallInWater();
        }
    }

    void spawnFly()
    {
        bool goodPlace = false;
        int i = -1;
        int j = -1;
        Vector2 pos = new Vector2 ();
        while (!goodPlace)
        {
            i = Random.Range(0, 10);
            j = Random.Range(0, 5);
            pos = Grid(i, j);
            if (board[i, j] != null && board[i, j].tag == "Pad" && flyBoard[i, j] == null)
            {
                goodPlace = true;
            }
        }
        Vector3 position = new Vector3(pos.x, pos.y, -1);
        var fly = Instantiate(flyPrefab, position, Quaternion.identity);
        flyBoard[i, j] = fly;
        fly.GetComponent<flyManager>().pos = (i, j);
        flyCount++;

        if (playerFrog.pos == (i, j))
        {
            playerFrog.eatFly(i, j);
        }
    }

    public static void destroyFly(GameObject fly)
    {
        Destroy(fly);
        flyCount--;
    }
}
