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

public class score : MonoBehaviour
{
    [HideInInspector]
    public int currentScore = 0;//total score

    //ghost points
    public void addScoreSingle()
    {
        currentScore += 3;
    }

    //add 5 bonus on top of double kill
    public void addScoreDouble()
    {
        currentScore += 11;
    }

    //witch worth 3 shots
    public void addWitch()
    {
        currentScore += 9;
    }

    //missed shot makes you lose 1 point
    public void missedShot()
    {
        if(currentScore > 0)
        {
            currentScore -= 1;
        }
    }

    //during bonus ghosts are 1 point each
    public void bonusHit()
    {
        currentScore += 1;
    }

    //double shot gives no bonus during bonus
    public void bonusHitDouble()
    {
        currentScore += 2;
    }

    //missed targets in bonus makes you lose 2 points each
    public void missedTarget(int misses)
    {
        if (currentScore > 0)
        {
            if (currentScore < 2 * misses)
                currentScore = 0;
            else
                currentScore -= 2 * misses;
        }
        
    }

}
