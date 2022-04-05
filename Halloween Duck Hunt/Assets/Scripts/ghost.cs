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

public class ghost : MonoBehaviour
{
    //speeds
    [HideInInspector]
    public float horizSpeed = 1.0f;

    [HideInInspector]
    public float vertSpeed = 1.0f;
    
    //for animations
    bool isMovingHoriz = false;
    bool isMovingVertDown = false;
    bool isMovingVertUp = false;

    //boundaries
    float fixedX1 = -6.5f;
    float fixedX2 = 6.5f;
    float fixedY1 = -1.5f;
    float fixedY2 = 4.5f;
    //direction values
    float xDirection = 0.0f;
    float yDirection = 0.0f;
    float directionTimer = 3.0f;//every 3 seconds, ghosts change direction
    float timer = 0.0f;

    int location;
    
    Vector3 directionVector;
    Vector3 locationVector;

    private SpriteRenderer sprite;
    Animator animator;
    

    private void Start()
    {
        //Initializations
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ghostLocation();
        direction();
    }

    void Update()
    {
        //makes ghost change directions on timer
        timer += Time.deltaTime;

        if (timer >= directionTimer)
        {
            direction();
            timer = 0.0f;
        }
            
        ghostMove();

        animator.SetBool("isMovingInX", isMovingHoriz);
        animator.SetBool("isMovingPosY", isMovingVertUp);
        animator.SetBool("isMovingNegY", isMovingVertDown);

    }

    //direction is calculated full 360 and animation is update accordingly
    //Ex: more left than up,down and right = left animation, etc
    public void direction()
    {

        xDirection = Random.Range(-1.0f, 1.0f);
        yDirection = Random.Range(-1.0f, 1.0f);

        directionVector = new Vector3(xDirection, yDirection, 0.0f);
        directionVector = directionVector.normalized;

        if( (directionVector.y > directionVector.x) && (Mathf.Abs(directionVector.y) > Mathf.Abs(directionVector.x)))
        {
            isMovingVertUp = true;
            isMovingHoriz = false;
            isMovingVertDown = false;
        }else if((directionVector.y < directionVector.x) && (Mathf.Abs(directionVector.y) > Mathf.Abs(directionVector.x)))
        {
            isMovingVertUp = false;
            isMovingHoriz = false;
            isMovingVertDown = true;
        }else if ((directionVector.y < directionVector.x) && (Mathf.Abs(directionVector.y) < Mathf.Abs(directionVector.x)))
        {
            isMovingVertUp = false;
            isMovingHoriz = true;
            isMovingVertDown = false;
            sprite.flipX = true;
        }
        else if ((directionVector.y > directionVector.x) && (Mathf.Abs(directionVector.y) < Mathf.Abs(directionVector.x)))
        {
            isMovingVertUp = false;
            isMovingHoriz = true;
            isMovingVertDown = false;
            sprite.flipX = false;
        }

    }

    //ghost spawn locations
    public void ghostLocation()
    {
        int locationTemp = Random.Range(1, 4);
        location = locationTemp;

        switch (locationTemp)
        {
            case 1:
                Vector3 positionLeft = new Vector3(fixedX1, Random.Range(fixedY1, fixedY2));
                locationVector = positionLeft;
                break;
            case 2:
                Vector3 positionTop = new Vector3(Random.Range(fixedX1, fixedX2), fixedY2);
                locationVector = positionTop;
                break;
            case 3:
                Vector3 positionRight = new Vector3(fixedX2, Random.Range(fixedY1, fixedY2));
                locationVector = positionRight;
                break;
        }

        transform.position = locationVector;

    }

    //collision detection and ghost movement
    public void ghostMove()
    {

        transform.Translate(directionVector.x * horizSpeed * Time.deltaTime, directionVector.y * vertSpeed * Time.deltaTime, 0.0f);

        //collision on boundaries are mediated by nudging ghost off and changing directions immediately
        if ((transform.position.x < fixedX1))
        {
            transform.Translate(0.1f, 0.0f, 0.0f);
            direction();
        }
        else if(transform.position.x > fixedX2)
        {
            transform.Translate(-0.1f, 0.0f, 0.0f);
            direction();
        }
        else if (transform.position.y > fixedY2)
        {
            transform.Translate(0.0f, -0.1f, 0.0f);
            direction();
        }
        else if (transform.position.y < fixedY1)
        {
            transform.Translate(0.0f, 0.1f, 0.0f);
            direction();
        }

    }

}
