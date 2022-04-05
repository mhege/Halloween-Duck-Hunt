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

public class levelGroup : MonoBehaviour
{

    public GameObject levelPrefab; //level group is composed of levels
    bool appTermination = false;//checks if app to be closed
    bool bonusKeyPressed = false;//is bonus triggered
    bool bonusTime = false;//is it bonus time

    int numberLevels = 10;//levels in game. Can be tuned. each level has a fixed number of rounds
    int maxLevels;// max number levels (utility)

    int roundmiss = 0; // number of ghosts missed
    int deltaRoundMissA = 0;//counts change in misses per round
    int deltaRoundMissB = 0;

    public float ghostSpeedInc = 0.5f;//how much do ghosts speed up per level
    float ghostBaseHSpeed;//ghosts speeds
    float ghostBaseVSpeed;

    GameObject level;

    //UI images for misses
    Image img1;
    Image img2;
    Image img3;

    //UI gameobjects for misses
    GameObject missGO1;
    GameObject missGO2;
    GameObject missGO3;
    //UI gameobjects for text
    GameObject textGO;
    GameObject textLevelGO;
    GameObject textGameOverGO;
    //bonus
    GameObject roundBonusGO;
    GameObject crosshair;

    //text for lvl and gameover
    Text textLevel;
    Text textGameOver;

    //various prefabs needed 
    public GameObject ghostPrefab;
    public GameObject bonusRoundPrefab;
    public GameObject crosshairMain;
    public GameObject crosshairBonus;
    
    //game over message
    IEnumerator endMessage(float wait)
    {
        textGameOver.text = "Game Over";
        ghostPrefab.GetComponent<ghost>().horizSpeed = ghostBaseHSpeed;
        ghostPrefab.GetComponent<ghost>().vertSpeed = ghostBaseVSpeed;
        yield return new WaitForSeconds(wait);
        //reset ghost speed 
        QuitGame();

    }

    //Display flashing bonus text
    IEnumerator bonusBonus(float waitflash)
    {
        textGameOver.text = "Bonus!!!";
        yield return new WaitForSeconds(waitflash);
        textGameOver.text = "";
        yield return new WaitForSeconds(waitflash);
        textGameOver.text = "Bonus!!!";
        yield return new WaitForSeconds(waitflash);
        textGameOver.text = "";
        yield return new WaitForSeconds(waitflash);
        textGameOver.text = "Bonus!!!";
        yield return new WaitForSeconds(waitflash);
        textGameOver.text = "";
    }

    
    void Start()
    {

        //Initialization

        makeMainCrosshair();

        ghostBaseHSpeed = ghostPrefab.GetComponent<ghost>().horizSpeed;
        ghostBaseVSpeed = ghostPrefab.GetComponent<ghost>().vertSpeed;

        makeLevel();
        maxLevels = numberLevels;
        numberLevels -= 1;

        missGO1 = GameObject.FindGameObjectWithTag("gMiss1");
        missGO2 = GameObject.FindGameObjectWithTag("gMiss2");
        missGO3 = GameObject.FindGameObjectWithTag("gMiss3");

        img1 = missGO1.GetComponent<Image>();
        img2 = missGO2.GetComponent<Image>();
        img3 = missGO3.GetComponent<Image>();

        textLevelGO = GameObject.FindGameObjectWithTag("levelText");
        textLevel = textLevelGO.GetComponent<Text>();

        textGameOverGO = GameObject.FindGameObjectWithTag("gameover");
        textGameOver = textGameOverGO.GetComponent<Text>();

        textLevel.text = "lvl " + (maxLevels - numberLevels);
        textGO = GameObject.FindGameObjectWithTag("roundText");
    }

    
    void Update()
    {
        if (!appTermination)
        {

            //check if bonus key triggered
            if (Input.GetKeyDown("b"))
            {
                if (bonusKeyPressed)
                {
                    //Do Nothing
                }   
                else
                {
                    bonusKeyPressed = true;
                    bonusTime = true;
                }
                    
            }
                
            //get round misses and load into delta checker
            if(level != null)
                deltaRoundMissA = level.GetComponent<level>().roundMisses;

            //Calculate actual total misses
            if(deltaRoundMissA != deltaRoundMissB)
            {
                if ( (deltaRoundMissA == 0) )
                    deltaRoundMissB = 0;
                else if (deltaRoundMissA < deltaRoundMissB)
                {
                    deltaRoundMissB = 0;
                    roundmiss += deltaRoundMissA - deltaRoundMissB;
                    spriteMisses(roundmiss);
                }
                else
                {
                    roundmiss += deltaRoundMissA - deltaRoundMissB;
                    spriteMisses(roundmiss);
                }

                deltaRoundMissB = deltaRoundMissA;
               
            }

            //level over check
            if (level.GetComponent<level>().levelOver)
            {
                //if bonus, enter
                if (bonusTime || bonusKeyPressed)
                {
                    //run bonus components
                    if (bonusTime)
                    {
                        StartCoroutine(bonusBonus(.2f));
                        makeBonusCrosshair();
                        makeBonus();
                    }

                    bonusTime = false;

                    if (roundBonusGO.GetComponent<bonusRound>().roundDelay)
                    {
                        bonusKeyPressed = false;
                        makeMainCrosshair();
                        destroyBonusElements();
                    }
                   
                }
                else
                {   //check if there's another level to load
                    if (numberLevels > 0 && (roundmiss <= 3))
                    {
                        destroyGameElements();
                        makeLevel();
                        numberLevels -= 1;
                        textLevel.text = "lvl " + (maxLevels - numberLevels);
                        ghostPrefab.GetComponent<ghost>().horizSpeed = ghostPrefab.GetComponent<ghost>().horizSpeed + ghostSpeedInc;
                        ghostPrefab.GetComponent<ghost>().vertSpeed = ghostPrefab.GetComponent<ghost>().vertSpeed + ghostSpeedInc;
                    }
                    else
                    {
                        appTermination = true;
                        Destroy(crosshair);
                        StartCoroutine(endMessage(3.0f));
                    }
                }
 
            }else if(roundmiss >= 3)//terminate if player needs to giTgUd
            {
                appTermination = true;
                Destroy(textGO);
                destroyGameElements();
                Destroy(crosshair);
                StartCoroutine(endMessage(3.0f));
            }

        }
            
    }

    //makes level
    public void makeLevel()
    {
        level = Instantiate(levelPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
    }

    //makes bonus
    public void makeBonus()
    {
        roundBonusGO = Instantiate(bonusRoundPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
    }

    //makes standard crosshair
    public void makeMainCrosshair()
    {
        if (crosshair != null)
            Destroy(crosshair);

        crosshair = Instantiate(crosshairMain, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
    }

    //makes bonus crosshair
    public void makeBonusCrosshair()
    {
        if (crosshair != null)
            Destroy(crosshair);

        crosshair = Instantiate(crosshairBonus, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
    }

    //quit game
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    //pops up number of sprites based on number of total misses
    public void spriteMisses(int roundMisses)
    {
        if (roundMisses == 1)
        {
            Color cTemp1 = img3.color;
            cTemp1.a = 255.0f;
            img3.color = cTemp1;
        }
        else if (roundMisses == 2)
        {
            Color cTemp1 = img3.color;
            cTemp1.a = 255.0f;
            img3.color = cTemp1;

            Color cTemp2 = img2.color;
            cTemp2.a = 255.0f;
            img2.color = cTemp2;
        }
        else if (roundMisses >= 3)
        {
            Color cTemp1 = img3.color;
            cTemp1.a = 255.0f;
            img3.color = cTemp1;

            Color cTemp2 = img2.color;
            cTemp2.a = 255.0f;
            img2.color = cTemp2;

            Color cTemp3 = img1.color;
            cTemp3.a = 255.0f;
            img1.color = cTemp3;
        }
    }

    //clears memory for next level
    public void destroyGameElements()
    {

        //Ghosts
        var clones1 = GameObject.FindGameObjectsWithTag("Ghost");

        for (int i = 0; i < clones1.Length; i++)
        {
            Destroy(clones1[i]);
        }

        //dog
        var clones2 = GameObject.FindGameObjectsWithTag("dog");

        for (int i = 0; i < clones2.Length; i++)
        {
            Destroy(clones2[i]);
        }

        //round
        var clones3 = GameObject.FindGameObjectsWithTag("round");

        for (int i = 0; i < clones3.Length; i++)
        {
            Destroy(clones3[i]);
        }

        //level
        Destroy(level);

    }

    //clears bonus elements
    public void destroyBonusElements()
    {
        //Ghosts
        var clones1 = GameObject.FindGameObjectsWithTag("bonusGhost");

        for (int i = 0; i < clones1.Length; i++)
        {
            Destroy(clones1[i]);
        }

        Destroy(roundBonusGO);

    }

    //reset ghost speed since prefab is changed
    void OnApplicationQuit()
    {
        //reset ghost speed prefab
        ghostPrefab.GetComponent<ghost>().horizSpeed = ghostBaseHSpeed;
        ghostPrefab.GetComponent<ghost>().vertSpeed = ghostBaseVSpeed;
    }

}
