using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Camera {
	public class ThirdPersonCameraAim : MonoBehaviour {
        public Transform upperBody;
        private float rotationYaw;
        private float rotationPitch;
        private ThirdPersonCameraNormal cameraScript;
        private Transform target;
        private float originalPitchAngle;
        private float pitchAngle;
        private float yawAngle;
        private float cameraDistance;
        private PlayerStat stat;

        private void Awake()
        {
            cameraScript = GetComponent<ThirdPersonCameraNormal>();
            cameraDistance = cameraScript.distance;
            target = cameraScript.target;
            stat = target.GetComponentInParent<PlayerStatus>().stat;
        }

        private void LateUpdate()
        {

            if (Input.GetMouseButtonDown(1))
            {
                originalPitchAngle = upperBody.eulerAngles.x;
                pitchAngle = 0f;
            }
            else if(Input.GetMouseButtonUp(1))
            {
                upperBody.eulerAngles = new Vector3(originalPitchAngle, upperBody.eulerAngles.y,upperBody.eulerAngles.z);
            }

            bool aim = Input.GetMouseButton(1);
            stat.aim = aim;
            if (stat.aim)
            {

                cameraScript.distance = Mathf.Lerp(cameraScript.distance,cameraDistance / 2f,1f*Time.deltaTime);
                float yaw = Input.GetAxis("Mouse X") * cameraScript.mouseSensivity * Time.deltaTime;
                float pitch = Input.GetAxis("Mouse Y") * cameraScript.mouseSensivity * Time.deltaTime;

                yawAngle += yaw;
                pitchAngle -= pitch;

                pitchAngle = Mathf.Clamp(pitchAngle, -70f, 70f);
                //upperBody x coord
                target.eulerAngles = new Vector3(0, yawAngle, 0);
                upperBody.eulerAngles = new Vector3(pitchAngle + cameraScript.pitch, upperBody.eulerAngles.y, upperBody.eulerAngles.z);
                //target y coord
            }
            else
            {
                cameraScript.distance = Mathf.Lerp(cameraScript.distance, cameraDistance, 1f * Time.deltaTime);
            }
        }
    }
}
