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

public class dog : MonoBehaviour
{

    //animations variables
    Animator animator;

    bool isLaughing = false;
    bool isCheering = false;

    
    void Start()
    {
        //initialization
        animator = GetComponent<Animator>();
        
    }

    
    void Update()
    {
        //for animations
        animator.SetBool("laugh", isLaughing);
        animator.SetBool("cheer", isCheering);    
    }

    //laugh
    public void laughing()
    {
        isLaughing = true;
        isCheering = false;
        
    }

    //cheer
    public void cheering()
    {
        isCheering = true;
        isLaughing = false;
        
    }

    //idle
    public void idle()
    {
        isCheering = false;
        isLaughing = false;
    }

}
