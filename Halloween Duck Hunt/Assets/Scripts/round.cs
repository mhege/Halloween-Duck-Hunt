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


public class round : MonoBehaviour
{

    const float roundLength = 15.0f;//15 second rounds
    const float fadeTime = 5.0f;//ghost fades for 5 seconds
    const float fadeStart = 10.0f;//ghost fades 10 seconds in round

    bool roundDone = false;//check if round is over
   
    public int ghostNumber = 10;//number ghosts per round

    [HideInInspector]
    public int numberMisses;//ghost misses per round

    List<GameObject> ghosts = new List<GameObject>();//list of ghosts
   
    public GameObject ghostPrefab;
    public GameObject dogPrefab;

    GameObject classicDog;
    AudioSource[] dogAudio;

    void Start()
    {
        //Initializations
        for (int i = 0; i < ghostNumber; i++)
        {
            makeGhosts();
        }

        makeDog();

        StartCoroutine(fade(fadeTime, fadeStart));//fade starts in fixed time
        StartCoroutine(roundOver(roundLength));//round is over in fixed time

        dogAudio = classicDog.GetComponents<AudioSource>();

    }

    //Counts when round is over
    IEnumerator roundOver(float wait)
    {
        yield return new WaitForSeconds(wait);
        
        for (int i = 0; i < ghostNumber; i++)
        {

            if (ghosts[i] != null)
            {
                Destroy(ghosts[i]);
                numberMisses += 1;
            }
            
        }

        roundDone = true;

    }

    //fades the ghosts on fixed time
    IEnumerator fade(float fadeTime, float wait)
    {

        float fadeScale = 2.0f;
        yield return new WaitForSeconds(wait);

        for (float t = 0.0f; t < 1.0f; t += 1.0f/(60.0f*fadeTime))
        {
            
            for(int i = 0; i < ghostNumber; i++)
            {
                
                if(ghosts[i] != null)
                {

                    float alpha = ghosts[i].GetComponent<SpriteRenderer>().material.color.a;
                    Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0.0f, t/(fadeTime*fadeScale)));
                    ghosts[i].GetComponent<SpriteRenderer>().material.color = newColor;

                }

            }

            yield return new WaitForSeconds(1.0f/60.0f);

        }
    }

    void Update()
    {
        //plays dog animation and sound at end of round
        if(roundDone)
        {

            if (numberMisses == 0)
            {
                dogAudio[1].Play();
                classicDog.GetComponent<dog>().cheering();
                
            }
            else
            {
                dogAudio[0].Play();
                classicDog.GetComponent<dog>().laughing();
                
            }
                

        }

        roundDone = false;
    }

    //instantiates ghosts
    public void makeGhosts()
    {

        GameObject newGhost = Instantiate(ghostPrefab, new Vector3(0,-5.0f,0), Quaternion.identity) as GameObject;
        ghosts.Add(newGhost);
        
    }

    //instantiates dog
    public void makeDog()
    {
        classicDog = Instantiate(dogPrefab, new Vector3(0, -5.0f, 0), Quaternion.identity) as GameObject;
    }

}
