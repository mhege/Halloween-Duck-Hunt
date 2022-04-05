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

public class witch : MonoBehaviour
{

    public int health = 3; //3 Click to kill
    int location;//spawn location
    int xDirection = 0;//horizontal move direction

    [HideInInspector]
    public float horizSpeed = 2.5f;//horizontal speed

    //spawn coordinates boundaries
    float fixedX1 = -6.5f;
    float fixedX2 = 6.5f;
    float fixedY1 = -1.5f;
    float fixedY2 = 4.5f;

    const float maxWitchTime = 10.0f;//latest witch can spawn in round
    float randomTime;//time between 0 and maxWitchTime

    bool moving = false;//for animation start
    bool destroyCheck = false;//know when to destroy gameobject

    Vector3 locationVector;//where the witch spawned in world space

    private SpriteRenderer sprite;//witch sprite

    Animator animator;//animation handle
    AudioSource[] audio;//audio handle

    score scoreScript;//Talk to score script
    GameObject scoreObj;

    void Start()
    {
        //Initializations on start
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audio = GetComponents<AudioSource>();

        scoreObj = GameObject.FindWithTag("Score");
        scoreScript = scoreObj.GetComponent<score>();

        randomTime = Random.Range(0.0f, maxWitchTime);
        
        //witch pops up at randomtime and gets destroyed bas on that time
        StartCoroutine(witchLocationDelay(randomTime));
        StartCoroutine(witchMoveDelay(randomTime));
        StartCoroutine(witchDelayDestroy(randomTime+1.0f));
    }

    void Update()
    {
        animator.SetBool("isMoving", moving);//update animation bool

        if (moving)
        {
            witchMove();//move gameobject
        }

        if (destroyCheck)
        {
            if( (transform.position.x > (fixedX2+1.0f)) || (transform.position.x < (fixedX1 - 1.0f)))
            {
                Destroy(gameObject);//destroy if out of bounds
            }
        }
        
    }

    //go out of screen before destroying
    IEnumerator witchDelayDestroy(float wait)
    {
        yield return new WaitForSeconds(wait);
        destroyCheck = true;
    }

    //spawns based on random time
    IEnumerator witchLocationDelay(float wait)
    {
        yield return new WaitForSeconds(wait);
        witchLocation();
        direction();
    }

    //moves based on random time
    IEnumerator witchMoveDelay(float wait)
    {
        yield return new WaitForSeconds(wait);

        moving = true;
        audio[0].Play();
    }

    //laughs based on random time
    IEnumerator witchaudioDelay(float wait)
    {
        yield return new WaitForSeconds(wait);

        Destroy(gameObject);
    }

    //crosshair talks to this method to signal a loss of health
    public void lostHealth()
    {
 
        health -= 1;

        if (health == 0)
        {
            scoreScript.addWitch();
            audio[1].Play();
            transform.position = new Vector3(0.0f, -5.0f, 0.0f);
            horizSpeed = 0.0f;
            StartCoroutine(witchaudioDelay(2.0f));
        }
            
    }

    //calculate spawn location
    public void witchLocation()
    {
        int locationTemp = Random.Range(1, 3);
        location = locationTemp;

        switch (locationTemp)
        {
            case 1:
                Vector3 positionLeft = new Vector3(fixedX1, Random.Range(fixedY1, fixedY2));
                locationVector = positionLeft;
                break;
            case 2:
                Vector3 positionRight = new Vector3(fixedX2, Random.Range(fixedY1, fixedY2));
                locationVector = positionRight;
                break;
              
        }

        transform.position = locationVector;
    }

    //give direction
    public void direction()
    {
        
        if (location == 1)
        {
            xDirection = 1;
        }
        else
        {
            xDirection = -1;
            sprite.flipX = true;
        }
            
    }

    //moves witch
    public void witchMove()
    {

        transform.Translate(xDirection * horizSpeed * Time.deltaTime, 0.0f, 0.0f);

    }

}
