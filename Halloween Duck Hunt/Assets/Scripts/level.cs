/*
 * Author: Marc Hegedus (26242219)
 * COMP376: INTRODUCTION TO GAME DEVELOPMENT
 * Assignment 2 (2D Game)
 * 
 * References (copyrights):
 * Background - Nihongamer.tumbler.com 
 * Dog & dog sounds - Nintendo
 * Kamek sounds - Nintendo
 * Cloud, grass, ground Sprites - Nintendo
 * Tree Sprite - COCEFI @ chickysprout
 * 
 * All other Assests are license free or created by the developer of this code
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class level : MonoBehaviour
{

    [HideInInspector]
    public bool levelOver = false;//check if level is over

    [HideInInspector]
    public int roundMisses = 0;//total misses in rounds

    const int numberRounds = 3;//number rounds per level
    int roundsLeft = 3;//counts if round needs to be made
    
    //witch round appearance
    int witchAppear1 = 0;
    int witchAppear2 = 0;

    const float roundDelay = 1.5f;//for round text
    const float roundLength = 20.0f;//round length
    float timer = 0.0f;//counts to roundlength

    AudioSource audio;

    //gets necessary gameobjects, named appropriately
    GameObject round;
    GameObject textGO;
    GameObject textScoreGO;
    GameObject scoreGO;

    //for round display and score
    Text text;
    Text textScore;

    //talks to score
    score scoreScript;

    //witch list
    List<GameObject> witches = new List<GameObject>();

    public GameObject witchPrefab;
    public GameObject roundPrefab;
    public GameObject ghostPrefab;

    void Start()
    {
        
        //initializations
        witchAppear1 = Random.Range(1, numberRounds + 1);
        witchAppear2 = Random.Range(1, numberRounds + 1);

        textGO = GameObject.FindGameObjectWithTag("roundText");
        text = textGO.GetComponent<Text>();

        textScoreGO = GameObject.FindGameObjectWithTag("points");
        textScore = textScoreGO.GetComponent<Text>();

        scoreGO = GameObject.FindGameObjectWithTag("Score");
        scoreScript = scoreGO.GetComponent<score>();

        audio = GetComponent<AudioSource>();

        roundsLeft -= 1;

        StartCoroutine(displayRound(roundDelay));
        StartCoroutine(startRound(roundDelay));

        
    }

    //text to display round
    IEnumerator displayRound(float wait)
    {

        Color cTemp = text.color;
        text.text = "Round " + (numberRounds - roundsLeft);
        cTemp.a = 255.0f;
        text.color = cTemp;

        yield return new WaitForSeconds(wait);

        cTemp.a = 0.0f;
        text.color = cTemp;

    }

    //makes the round
    IEnumerator startRound(float wait)
    {

        yield return new WaitForSeconds(wait);

        makeRound();

        if (( (numberRounds - roundsLeft) == witchAppear1) && ( (numberRounds - roundsLeft) == witchAppear2))
        {
            makeWitch();
            makeWitch();
        }
        else if (( (numberRounds - roundsLeft) == witchAppear1) || ( (numberRounds - roundsLeft) == witchAppear2))
        {
            makeWitch();
        }
            

        audio.Play();
    }

    void Update()
    {
        timer += Time.deltaTime;

        textScore.text = "" + scoreScript.currentScore;

        //checks if level is over
        if (!levelOver)
        {
            if(timer >= roundLength && roundsLeft >= 1)
            {

                roundMisses += round.GetComponent<round>().numberMisses;
                //stops if missed more than 3 ghosts in level
                if(roundMisses >= 3)
                {
                    roundsLeft = 0;
                }
                else//cotinues
                {
                    roundsLeft -= 1;

                    StartCoroutine(displayRound(roundDelay));
                    StartCoroutine(startRound(roundDelay));

                    timer = 0.0f;
                }
            //level is over
            }else if(timer >= roundLength && roundsLeft == 0)
            {
                roundMisses += round.GetComponent<round>().numberMisses;
                
                levelOver = true;

            }

        }

    }

    //makes round
    public void makeRound()
    {

        round = Instantiate(roundPrefab, new Vector3(0, -5.0f, 0), Quaternion.identity) as GameObject;

    }

    //makes witch
    public void makeWitch()
    {
        GameObject theWitch = Instantiate(witchPrefab, new Vector3(0, -5.0f, 0), Quaternion.identity) as GameObject;
        witches.Add(theWitch);
    }

}
