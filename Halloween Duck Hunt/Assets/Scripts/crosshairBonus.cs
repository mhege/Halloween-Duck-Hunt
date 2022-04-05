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

public class crosshairBonus : MonoBehaviour
{
    public Texture2D cursor;//cursor is texture
    private bool mouseLeftDown = false;//check player input
    private bool witchShot = false;//check if witch

    private Vector2 clickedPos;//worldspace click coordinate

    AudioSource[] audio;

    //score script
    score scoreScript;
    GameObject scoreObj;

    //overlapping ghost kill
    int doubleKill = 0;

    void Start()
    {
        //Initializations
        scoreObj = GameObject.FindWithTag("Score");
        scoreScript = scoreObj.GetComponent<score>();
        Cursor.SetCursor(cursor, new Vector2(cursor.width, cursor.height), CursorMode.Auto);
        audio = GetComponents<AudioSource>();
    }

    void Update()
    {
        //held, not clicked
        mouseLeftDown = Input.GetMouseButton(0);

        if (mouseLeftDown)
        {
            //where in z is player clicking
            Vector3 screenPosDepth = Input.mousePosition;
            screenPosDepth.z = 10.0f;
            clickedPos = Camera.main.ScreenToWorldPoint(screenPosDepth);

            //first shot raytrace
            RaycastHit2D hitOne = Physics2D.Raycast(clickedPos, clickedPos, 0.0f);
            if (hitOne && (hitOne.collider.gameObject.layer == LayerMask.NameToLayer("Ghost")))
            {

                if (hitOne.collider.gameObject.tag == "Witch")
                {
                    witchShot = true;
                    hitOne.collider.SendMessage("lostHealth", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    Destroy(hitOne.collider.gameObject);
                    audio[1].Play();
                    doubleKill += 1;
                }

            }

            //second shot raytrace
            RaycastHit2D hitTwo = Physics2D.Raycast(clickedPos, clickedPos, 0.0f);
            if (hitTwo && (hitTwo.collider.gameObject.layer == LayerMask.NameToLayer("Ghost")))
            {

                if (!witchShot && hitTwo.collider.gameObject.tag == "Witch")
                {
                    hitTwo.collider.SendMessage("lostHealth", SendMessageOptions.DontRequireReceiver);
                }
                else if (hitTwo.collider.gameObject.tag == "Witch")
                {
                    //Do Nothing;
                }
                else
                {
                    Destroy(hitTwo.collider.gameObject);
                    audio[1].Play();
                    doubleKill += 1;
                }

            }

            //check overlap kill
            if (doubleKill == 1)
            {
                scoreScript.bonusHit();
            }
            else if (doubleKill == 2)
            {
                scoreScript.bonusHitDouble();
            }
            else if (witchShot)
            {
                //Do Nothing
            }

        }

        witchShot = false;
        doubleKill = 0;
    }
}
