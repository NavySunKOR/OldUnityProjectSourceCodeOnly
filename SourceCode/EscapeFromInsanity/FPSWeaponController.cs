using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreDevelopment {
    [System.Serializable]
    public class Weapon {
        public int damage;
        public float weaponRange;
        public float rpm;
        public float fireInterval;
        public float recoilStatusVertical;
        public float recoilStatusHorizontal;
        public float recoilDecrease;
        public float fireTimer;
        //used in NormalDistribution
        public float fRandR;
        //used in NormalDistribution
        public float fRandT;
        public float verticalRecoil;
        public float horizontalRecoil;
        //used in LinearEquation
        public float recoilDirection;
    }

	public class FPSWeaponController : MonoBehaviour {

        public Weapon weaponInfo;
        public Transform cameraTr;
        public GameObject bulletHole;
        private RaycastHit hit;

        private Vector3 usedPos;
        private Vector3 originalPos;

        private void Awake()
        {
            weaponInfo.fireInterval = 60f / weaponInfo.rpm;
            InitNormalDistribution();

            usedPos = cameraTr.position;
            originalPos = cameraTr.position;

        }

        private void Update()
        {
            //back to original recoil.
            if(usedPos.y > originalPos.y)
            {
                usedPos -= new Vector3(0, weaponInfo.recoilDecrease * Time.deltaTime, 0);
                cameraTr.rotation = Quaternion.LookRotation(usedPos + new Vector3(0, 0, 3f)); // LookUp Forward
            }
            if (Input.GetMouseButton(0))
            {
                if(Time.time - weaponInfo.fireTimer > weaponInfo.fireInterval)
                {
                    weaponInfo.fireTimer = Time.time;

                    //bullet raycast hit.
                    Ray ray = new Ray(usedPos, Vector3.forward);
                    if(Physics.Raycast(ray,out hit, weaponInfo.weaponRange))
                    {
                        GameObject bullet = Instantiate(bulletHole, hit.point, Quaternion.identity);
                        Destroy(bullet, 4f);
                    }


                    //spread pattern calc
                    //normal distribution
                    //RecoilCalcByNormalDistribution();
                    RecoilCalcByLinearEquation();
                    //

                    //calc corrdinates.
                    Vector3 rot = new Vector3(weaponInfo.horizontalRecoil, weaponInfo.verticalRecoil, 0);
                    usedPos += rot; // add to used pos
                    cameraTr.rotation = Quaternion.Lerp(cameraTr.rotation, Quaternion.LookRotation(usedPos + new Vector3(0,0,3f)), 1f*Time.deltaTime); // 앞을 바라보댜.
                }
            }
        }

        void InitNormalDistribution()
        {
            weaponInfo.fRandR = Mathf.Sqrt(-2f * Mathf.Log(Random.Range(0f, 1f)));
            weaponInfo.fRandT = 2f * Mathf.PI * Random.Range(0f, 1f);

            // set initial normal distribution
            weaponInfo.horizontalRecoil = (weaponInfo.fRandR * Mathf.Cos(weaponInfo.fRandT)) * (weaponInfo.recoilStatusHorizontal);
            weaponInfo.verticalRecoil = weaponInfo.recoilStatusVertical;
        }

        //spread pattern in circular sector shape. (aka left-right pattern)
        void RecoilCalcByNormalDistribution()
        {
            weaponInfo.fRandR = Mathf.Sqrt(-2f * Mathf.Log(Random.Range(0f, 1f)));
            weaponInfo.fRandT = 2f * Mathf.PI * Random.Range(0f, 1f);
            weaponInfo.horizontalRecoil = (weaponInfo.fRandR * Mathf.Cos(weaponInfo.fRandT)) * (weaponInfo.recoilStatusHorizontal);
        }
        
        //spread pattern in curved shape.
        void RecoilCalcByLinearEquation()
        {
            weaponInfo.horizontalRecoil += weaponInfo.recoilStatusHorizontal  * weaponInfo.recoilDirection ;
            weaponInfo.verticalRecoil = Mathf.Sqrt(weaponInfo.horizontalRecoil * 10f + weaponInfo.recoilStatusVertical);
        }

    }
}
