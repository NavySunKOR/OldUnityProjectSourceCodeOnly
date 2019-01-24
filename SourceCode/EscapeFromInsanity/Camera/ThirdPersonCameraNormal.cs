using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Camera {
	public class ThirdPersonCameraNormal : MonoBehaviour {
        
        public Transform target;
        public float distance;
        public float mouseSensivity;
        public bool invertMouse;
        public float pitch;
        private float yaw;
        private float invertMouseValue;
        private Vector3 currentRotation;
        

        private void Update()
        {
            invertMouseValue = (!invertMouse) ? -1f : 1f;
            float horizontal = Input.GetAxis("Mouse X") * invertMouseValue * mouseSensivity * Time.deltaTime;
            float vertical = Input.GetAxis("Mouse Y") * invertMouseValue * mouseSensivity * Time.deltaTime;
            yaw -= horizontal;
            pitch += vertical;
            pitch = Mathf.Clamp(pitch, -40f, 85f);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(pitch, yaw), 1.2f);
            transform.position = target.position - transform.forward * distance;

        }


    }
}
