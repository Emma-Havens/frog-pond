using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class score : MonoBehaviour
{

    public Sprite frog_0;
    public Sprite frog_1;
    public Sprite frog_2;
    public Sprite frog_3;

    public AudioClip frogGrow;
    public AudioClip frogShrink;

    static SpriteRenderer frogRenderer;

    static score scoreObject;

    static TMP_Text scoreCount;
    static AudioSource scoreSound;

    static int points = 0;
    static string startText = "Flies Needed: 4/4";
    static int curIndex;

    static int[] thresholdWeights = new int[4] { 0, 4, 12, 24 };

    void Start()
    {
        frogRenderer = FindObjectOfType<frogManager>().GetComponent<SpriteRenderer>();
        scoreObject = FindObjectOfType<score>().GetComponent<score>();
        scoreCount = GetComponent<TMP_Text>();
        scoreSound = GetComponent<AudioSource>();
        curIndex = 0;
        scoreCount.text = startText;
    }

    public static void flyEaten()
    {
        points++;
        if (curIndex == 3) { return; }
        if (points >= thresholdWeights[curIndex + 1])
        {
            curIndex++;
            updateFrog();
            scoreSound.PlayOneShot(scoreObject.frogGrow);
        }
        if (curIndex == 3)
        {
            scoreCount.text = "Frog Supreme!";
            return;
        }
        scoreCount.text = "Flies Needed: " +
                    (thresholdWeights[curIndex + 1] - points) + "/" +
                    (thresholdWeights[curIndex + 1] - thresholdWeights[curIndex]);
    }

    public static void fallenInWater()
    {
        points -= 4;
        if (points < 0)
        {
            points = 0;
            scoreCount.text = startText;
            return;
        }
        if (points < thresholdWeights[curIndex])
        {
            curIndex--;
            updateFrog();
            scoreSound.PlayOneShot(scoreObject.frogShrink);
        }
        if (curIndex == 3) { return; }
        scoreCount.text = "Flies Needed: " +
                    (thresholdWeights[curIndex + 1] - points) + "/" +
                    (thresholdWeights[curIndex + 1] - thresholdWeights[curIndex]);
    }

    static void updateFrog()
    {
        if (curIndex == 0)
        {
            frogRenderer.sprite = scoreObject.frog_0;
            frogRenderer.gameObject.transform.localScale = new Vector3(.2f, .2f);
        }
        if (curIndex == 1)
        {
            frogRenderer.sprite = scoreObject.frog_1;
            frogRenderer.gameObject.transform.localScale = new Vector3(.3f, .3f);
        }
        if (curIndex == 2)
        {
            frogRenderer.sprite = scoreObject.frog_2;
            frogRenderer.gameObject.transform.localScale = new Vector3(.4f, .4f);
        }
        if (curIndex == 3)
        {
            frogRenderer.sprite = scoreObject.frog_3;
        }
        
    }

}
