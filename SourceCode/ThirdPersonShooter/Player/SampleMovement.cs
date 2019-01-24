using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPersonShooter;

namespace ThirdPersonShooter {
	public class SampleMovement : MonoBehaviour {

        private Animator anim;
        private bool aim;
        private bool crouch;
        private bool run;
        private bool pistolWield;
        private bool rifleWield;
        private bool cover;
        private float vertical;
        private float horizontal;
        private int coverMovement;
        



        private void Awake()
        {
            anim = GetComponent<Animator>();
            pistolWield = false; // for now...
            rifleWield = false;// for now...
            anim.SetBool("PistolWield", pistolWield);
            anim.SetBool("RifleWield", rifleWield);
        }

        private void Update()
        {
            vertical = (run)? Mathf.Clamp(Input.GetAxis("Vertical"),0,1f) : Mathf.Clamp(Input.GetAxis("Vertical"), -0.5f, 0.5f);
            horizontal = Input.GetAxis("Horizontal");
            if (cover)
            {
                if(horizontal > 0)
                {
                    coverMovement = 1;
                }
                else
                {
                    coverMovement = -1;
                }
            }
            

            if (Input.GetMouseButton(1))
            {
                aim = true;
            }
            else if(Input.GetMouseButtonUp(1))
            {
                aim = false;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                anim.SetTrigger("Reloading");
            }


            if(Input.GetKey(KeyCode.LeftControl))
            {
                crouch = true;
            }
            else if(Input.GetKeyUp(KeyCode.LeftControl))
            {
                crouch = false;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                run = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                run = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                cover = !cover;
            }

            anim.SetFloat("HorizontalMove", horizontal);
            anim.SetFloat("VerticalMove", vertical);
            anim.SetBool("Crouch", crouch);
            anim.SetBool("Aim", aim);
            anim.SetBool("Run", run);
            anim.SetBool("Cover", cover);
            anim.SetInteger("CoverMovement", coverMovement);
        }

    }
}
