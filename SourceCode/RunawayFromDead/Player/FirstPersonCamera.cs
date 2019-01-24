using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class FirstPersonCamera : MonoBehaviour {
        [Range(0,88)]
        public float upperAngleLimit;
        [Range(-88, 0)]
        public float underAngleLimit;
        public float mouseSensivity;
        [Range(-1,1)]
        public float invertMouseY;
        public Transform cameraTr;
        private float horizontal;
        private float vertical;
        private Vector3 rotation;
        
        

		private void Update () {
            horizontal += Input.GetAxis("MouseX") * mouseSensivity;
            vertical += Input.GetAxis("MouseY") * -invertMouseY * mouseSensivity;

            if(vertical >= upperAngleLimit)
            {
                vertical = upperAngleLimit;
            }
            else if(vertical <= underAngleLimit)
            {
                vertical = underAngleLimit;
            }

            rotation = new Vector3(vertical, horizontal, 0);
            cameraTr.rotation = Quaternion.Euler(rotation);
            transform.forward = cameraTr.forward;
        }
	}
}
