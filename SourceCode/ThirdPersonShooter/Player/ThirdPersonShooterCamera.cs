using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonShooter
{
	public class ThirdPersonShooterCamera : MonoBehaviour {

        public Transform cameraTr;
        public Transform characterTr;
        public Transform headTr;
        public Vector3 cameraDistance;
        public Vector3 aimCameraDistance;
        public float sensivity;
        private float cameraTransition = 6f;
        private Vector3 originalHeadPosition;
        private Quaternion originalRotation;
        private float rotationHorizontal = 0f;
        private float rotationVertical = 0f;
        

        private void Update()
        {
            if (PlayerStat.aim)
            {
                if(PlayerStat.cover && PlayerStat.coverMovement < 0)
                {
                    cameraTr.position = Vector3.Lerp(cameraTr.position
                        ,headTr.position + (headTr.right * -aimCameraDistance.x + headTr.up * aimCameraDistance.y + headTr.forward * aimCameraDistance.z)
                        ,10f * Time.deltaTime);
                }
                else
                {
                    cameraTr.position = Vector3.Lerp(cameraTr.position
                        ,headTr.position + (headTr.right * aimCameraDistance.x + headTr.up * aimCameraDistance.y + headTr.forward * aimCameraDistance.z)
                        ,10f * Time.deltaTime);
                }
            }
            else
            {
                if (PlayerStat.cover && PlayerStat.coverMovement < 0)
                {
                    cameraTr.position = Vector3.Lerp(cameraTr.position
                        , headTr.position + (headTr.right * -cameraDistance.x + headTr.up * cameraDistance.y + headTr.forward * cameraDistance.z)
                        , 10f * Time.deltaTime);
                }
                else
                {
                    cameraTr.position = Vector3.Lerp(cameraTr.position
                        , headTr.position + (headTr.right * cameraDistance.x + headTr.up * cameraDistance.y + headTr.forward * cameraDistance.z)
                        , 10f * Time.deltaTime);
                }

                    
            }

            

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            if (!PlayerStat.cover && PlayerStat.aim)
            {
                rotationHorizontal += mouseX * sensivity * Time.deltaTime;
                rotationVertical += mouseY * -sensivity * Time.deltaTime;
                rotationVertical = Mathf.Clamp(rotationVertical, -88, 88);
                characterTr.rotation = Quaternion.Euler(rotationVertical, rotationHorizontal, 0);
            }
            else if(PlayerStat.cover && PlayerStat.aim)
            {
                rotationHorizontal += mouseX * sensivity * Time.deltaTime;
                rotationVertical += mouseY * -sensivity * Time.deltaTime;
                rotationVertical = Mathf.Clamp(rotationVertical, -88, 88);
                characterTr.rotation = Quaternion.Euler(rotationVertical, rotationHorizontal, 0);
            }
            else if (!PlayerStat.cover)
            {
                rotationHorizontal += mouseX * sensivity * Time.deltaTime;
                characterTr.rotation = Quaternion.Euler(0, rotationHorizontal, 0);
            }
            cameraTr.rotation = Quaternion.Lerp(cameraTr.rotation,characterTr.rotation, cameraTransition * Time.deltaTime);

            if (PlayerStat.crouch)
            {
                headTr.localPosition = new Vector3(0, 2.5f, 0);
            }
            else 
            {
                headTr.localPosition = new Vector3(0, 3.5f, 0);
            }
        }

        public void SetOriginalRotation()
        {
            originalRotation = characterTr.rotation;
        }

        public void BackToOriginalRotation()
        {
            characterTr.rotation = originalRotation;
        }
        
    }
}
