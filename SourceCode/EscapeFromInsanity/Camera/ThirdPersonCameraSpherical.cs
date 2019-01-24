using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera {
	public class ThirdPersonCameraSpherical : MonoBehaviour {

        public Transform targetTr;
        public Transform cameraTr;
        public float azimuth;
        public float elevation;
        public float radius;
        public float rotationSpeed;
        public bool invertMouse;
        private float invertMouseValue;

        private void Awake()
        {
            invertMouseValue = -1f;
        }


        private void Update()
        {
            invertMouseValue = (invertMouse) ? 1f : -1f;

            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed * invertMouseValue * Time.deltaTime;
            float vertical = Input.GetAxis("Mouse Y") * rotationSpeed * invertMouseValue * Time.deltaTime;

            azimuth += horizontal;
            elevation += vertical;

            elevation = elevation % 180f;
            azimuth = azimuth % 360f;

            elevation = Mathf.Clamp(elevation, -90f, 90f);
            cameraTr.position =  targetTr.position + new Vector3(radius * Mathf.Cos(azimuth * Mathf.Deg2Rad) * Mathf.Cos(elevation * Mathf.Deg2Rad)
                , radius * Mathf.Sin(elevation * Mathf.Deg2Rad)
                , radius * Mathf.Sin(azimuth * Mathf.Deg2Rad) * Mathf.Cos(elevation * Mathf.Deg2Rad));
            cameraTr.LookAt(targetTr);

        }


    }
}
