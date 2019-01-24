using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Controller {
    [RequireComponent(typeof(CharacterController))]
	public class ThirdPersonControllerNormal : MonoBehaviour {

        public Transform cameraTr;
        public float movementSpeed;
        public PlayerStat stat;
        private CharacterController cc;

        

        private void Awake()
        {
            cc = GetComponent<CharacterController>();
            stat = GetComponent<PlayerStatus>().stat;
        }

        private void Update()
        {
            float horizontal = (stat.aim)?Input.GetAxis("Horizontal") / 4f : Input.GetAxis("Horizontal") / 2f;
            float vertical = (stat.aim)? Input.GetAxis("Vertical") / 4f : Input.GetAxis("Vertical") / 2f;
            stat.isRunning = Input.GetKey(KeyCode.LeftShift);
            vertical = (stat.isRunning) ? vertical * 2f : vertical;

            stat.horizontal = horizontal;
            stat.vertical = vertical;

            Vector3 movement = transform.forward * vertical * movementSpeed * Time.deltaTime + transform.right * horizontal * movementSpeed * Time.deltaTime;
            if (!cc.isGrounded)
            {
                movement += Physics.gravity * Time.deltaTime;
            }
            
            if(horizontal * horizontal > Mathf.Epsilon || vertical * vertical > Mathf.Epsilon || stat.aim)
                transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,cameraTr.eulerAngles.y, 0),10f * Time.deltaTime);

            cc.Move(movement);

            /*
            Quaternion rotationAngle = 
            transform.rotation = Mathf.SmoothDampAngle(transform.rotation, )
            */
        }

    }
}
