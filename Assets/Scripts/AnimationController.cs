using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator anim;

    void Update()
    {
        AnimationControl();
    }
    void AnimationControl()
    {

        if(!PauseMenu.gamePaused)
        {

            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("isAlive", false);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }

            if (Input.GetButtonDown("Jump"))
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isJumping", false);
            }
        }
    }
}
