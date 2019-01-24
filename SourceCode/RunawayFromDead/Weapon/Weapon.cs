using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class Weapon : MonoBehaviour {

        //TODO:fix spread problem.

        public int damage;
        public float rpm;
        public float range;
        public WeaponType type;
        [Tooltip("maximum capacity of mag and current loaded.")]
        public WeaponMagazine weaponMagazine;
        public GameObject gunFireEffect;
        public GameObject bulletPoint;
        public float verticalRecoil;
        public float horizontalRecoil;
        public float spreadCurveLimit;
        [Tooltip("-1 is left, 1 is right.")]
        [Range(-1,1)]
        public float spreadCurveDirection;
        public Transform cameraObj;

        private WeaponEvent weaponEvent;
        private float weaponDelay;
        private float fireTime = 0f;
        private float shotgunSpread;
        private int shotgunPalletCount = 8;
        [SerializeField]
        private float currentVerticalRecoil = 0f;
        [SerializeField]
        private float currentHorizontalRecoil = 0f;



        private void OnEnable () {
            shotgunSpread = 1f;
            currentVerticalRecoil = 0f;
            currentHorizontalRecoil = 0f;
            weaponEvent = GetComponent<WeaponEvent>();
            weaponEvent.WeaponFireEvent += FireWeapon;
            weaponEvent.WeaponAimEvent += AimWeapon;
            weaponEvent.WeaponReloadEvent += ReloadWeapon;
            weaponDelay = 60f / rpm; 
        }

		private void OnDisable () {
            weaponEvent.WeaponFireEvent -= FireWeapon;
            weaponEvent.WeaponAimEvent -= AimWeapon;
            weaponEvent.WeaponReloadEvent -= ReloadWeapon;
        }

		private void Update () {
            CheckReload();
            CheckAim();
            CheckFire();
            //ReduceSpread();
        }

        private void CheckReload()
        {
            //add condition.....
            if (InputManager.GetKeyDown(InputNames.reload) && weaponMagazine.ammoAmount < weaponMagazine.maximumAmount)
            {
                weaponEvent.CallWeaponReloadEvent(weaponMagazine.maximumAmount - weaponMagazine.ammoAmount);
            }
        }
        private void CheckAim()
        {
            //add condition.....
            if (InputManager.GetKey(InputNames.aim))
            {
                weaponEvent.CallWeaponAimEvent();
            }
        }
        private void CheckFire()
        {
            //add condition.....
            
            if (InputManager.GetKey(InputNames.fire) && Time.time - fireTime > weaponDelay && weaponMagazine.ammoAmount > 0)
            {
                Debug.Log("FireOnCheckFire");
                weaponEvent.CallWeaponFireEvent();
                fireTime = Time.time;
            }
        }

        private void FireWeapon()
        {

            if(type == WeaponType.sg)
            {
                Ray[] rayArray = new Ray[shotgunPalletCount];
                RaycastHit[] hitArray = new RaycastHit[shotgunPalletCount];
                for (int i = 0; i < rayArray.Length; i++)
                {
                    rayArray[i] = new Ray(cameraObj.transform.position + Random.insideUnitSphere * shotgunSpread, cameraObj.forward);
                }

                for(int i = 0; i < shotgunPalletCount; i++)
                {
                    if (Physics.Raycast(rayArray[i],out hitArray[i], range))
                    {
                        if (hitArray[i].transform.CompareTag("Enemy"))
                        {
                            //Call Damage - Add hit.point to display bloodeffect (later)
                            hitArray[i].transform.GetComponent<NPCEvents>().CallNPCTookHitEvent(damage);
                        }
                        else
                        {
                            GameObject bullet = Instantiate(bulletPoint, hitArray[i].point + new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity) as GameObject;
                            Destroy(bullet, 7f);
                        }
                    }
                }
                StartCoroutine(FireEffect());
                Debug.Log("FireOnGlobalFire1");
            }
            else
            {
                Ray ray = new Ray(cameraObj.transform.position, cameraObj.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, range))
                {
                    if (hit.transform.CompareTag("Enemy"))
                    {
                        //Call Damage - Add hit.point to display bloodeffect (later)
                        hit.transform.GetComponent<NPCEvents>().CallNPCTookHitEvent(damage);
                    }
                    else
                    {
                        GameObject bullet = Instantiate(bulletPoint, hit.point + new Vector3(0.1f,0.1f,0.1f), Quaternion.FromToRotation(hit.point,hit.normal)) as GameObject;
                        Destroy(bullet, 7f);
                    }
                }
                StartCoroutine(FireEffect());
            }
            //MakeSpread();
            weaponMagazine.ammoAmount--;
        }

        IEnumerator FireEffect()
        {
            gunFireEffect.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            gunFireEffect.SetActive(false);
        }

        private void ReduceSpread()
        {
            Vector3 originRotation = cameraObj.transform.eulerAngles;

            currentVerticalRecoil = originRotation.x;
            currentHorizontalRecoil = originRotation.y;

            currentVerticalRecoil += (currentVerticalRecoil > 0f) ? verticalRecoil * Time.deltaTime : 0f;
            currentHorizontalRecoil -= (currentHorizontalRecoil > 0f) ? horizontalRecoil/2 : 0f;

            originRotation.x = currentVerticalRecoil;
            originRotation.y = currentHorizontalRecoil;

            cameraObj.transform.eulerAngles = originRotation;

        }

        private void MakeSpread()
        {
            currentVerticalRecoil += verticalRecoil;
            if (currentVerticalRecoil > spreadCurveLimit)
            {
                currentHorizontalRecoil += horizontalRecoil * spreadCurveDirection;
            }
            Vector3 originRotation = cameraObj.transform.eulerAngles;
            originRotation.x -= currentVerticalRecoil;
            originRotation.y += currentHorizontalRecoil;
            cameraObj.transform.eulerAngles = originRotation;
        }

        private void AimWeapon()
        {

        }

        private void ReloadWeapon(int amount)
        {
            weaponMagazine.ammoAmount += amount;
        }
	}
}
