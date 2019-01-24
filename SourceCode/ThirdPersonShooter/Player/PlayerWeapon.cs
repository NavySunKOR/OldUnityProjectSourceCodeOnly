using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonShooter
{
    [System.Serializable]
    public class Weapon
    {
        public float rpm;
        public float fireInterval;
        public float fireTimer;
        public int damage;
        public float recoil;
        public int magazine;
        public int maxMagazine;
        public int carryingCapacity;
        public int maxCapacity;
    }

	public class PlayerWeapon : MonoBehaviour {

        public GameObject rifle;
        public GameObject pistol;
        public GameObject bulletHole;
        public GameObject rifleMuzzleFlash;
        public GameObject pistolMuzzleFlash;
        public AudioClip shotfire;
        public AudioSource audioSource;
        public Weapon rifleInfo;
        public Weapon pistolInfo;
        private IActionable anim;

        private void Awake()
        {
            anim = (IActionable)GetComponentInChildren<PlayerAnimatorController>();
            rifleInfo.fireInterval = 60f / rifleInfo.rpm;
            pistolInfo.fireInterval = 60f / pistolInfo.rpm;
        }

        private void Update()
        {
            WeaponEquip();
            FireWeapon();
            ReloadWeapon();
        }

        private void FireWeapon()
        {
            if (PlayerStat.rifleWield)
            {
                if (InputManager.GetKey(InputNames.fire)
                    && PlayerStat.aim
                    && Time.time - rifleInfo.fireTimer > rifleInfo.fireInterval
                    && rifleInfo.magazine > 0)
                {
                    rifleInfo.fireTimer = Time.time;
                    rifleInfo.magazine--;
                    Ray ray = new Ray(Camera.main.transform.position + Camera.main.transform.forward * 2f, Camera.main.transform.forward);
                    RaycastHit hit;
                    if(Physics.Raycast(ray,out hit, 200f))
                    {
                        if (hit.transform.CompareTag("Enemy"))
                        {
                            hit.transform.SendMessage("TookHit", rifleInfo.damage);
                        }
                        else
                        {
                            GameObject hole = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.transform.position - hit.normal));
                            Destroy(hole, 3f);
                        }
                    }
                    //prefab and recoil
                    audioSource.clip = shotfire;
                    audioSource.Play();
                    rifleMuzzleFlash.SetActive(true);
                }
            }
            else if (PlayerStat.pistolWield)
            {
                if (InputManager.GetKey(InputNames.fire)
                    && PlayerStat.aim
                    && Time.time - pistolInfo.fireTimer > pistolInfo.fireInterval
                    && pistolInfo.magazine > 0)
                {
                    pistolInfo.fireTimer = Time.time;
                    pistolInfo.magazine--;
                    Ray ray = new Ray(Camera.main.transform.position + Camera.main.transform.forward * 2f, Camera.main.transform.forward);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 200f))
                    {
                        if (hit.transform.CompareTag("Enemy"))
                        {
                            hit.transform.SendMessage("TookHit", pistolInfo.damage);
                        }
                        else
                        {
                            GameObject hole = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.transform.position - hit.normal));
                            Destroy(hole, 3f);
                        }
                    }
                    //prefab and recoil
                    audioSource.clip = shotfire;
                    audioSource.Play();
                    pistolMuzzleFlash.SetActive(true);
                }
            }
        }

        private void WeaponEquip()
        {
            if (InputManager.GetKeyDown(InputNames.equipRifle))
            {
                rifle.SetActive(true);
                pistol.SetActive(false);
                anim.RifleWield();
            }
            else if (InputManager.GetKeyDown(InputNames.equipPistol))
            {
                rifle.SetActive(false);
                pistol.SetActive(true);
                anim.PistolWield();
            }
        }

        private void ReloadWeapon()
        {
            if (InputManager.GetKeyDown(InputNames.reload) && (PlayerStat.pistolWield || PlayerStat.rifleWield))
            {
                if(PlayerStat.rifleWield)
                {
                    if(rifleInfo.magazine < rifleInfo.maxMagazine)
                    {
                        if(rifleInfo.carryingCapacity > rifleInfo.maxMagazine - rifleInfo.magazine || rifleInfo.carryingCapacity > 0)
                        {
                            anim.Reload();
                            WaitCoroutine(3f);
                            if(rifleInfo.carryingCapacity > rifleInfo.maxMagazine - rifleInfo.magazine)
                            {
                                rifleInfo.magazine += rifleInfo.maxMagazine - rifleInfo.magazine;
                                rifleInfo.carryingCapacity -= rifleInfo.maxMagazine - rifleInfo.magazine;
                            }
                            else
                            {
                                rifleInfo.magazine += rifleInfo.carryingCapacity;
                                rifleInfo.carryingCapacity -= rifleInfo.carryingCapacity;
                            }
                        }
                    }
                }
                else if(PlayerStat.pistolWield)
                {
                    if(pistolInfo.magazine < pistolInfo.maxMagazine)
                    {

                        anim.Reload();
                        WaitCoroutine(2f);
                        if (pistolInfo.carryingCapacity > pistolInfo.maxMagazine - pistolInfo.magazine)
                        {
                            pistolInfo.magazine += pistolInfo.maxMagazine - pistolInfo.magazine;
                            pistolInfo.carryingCapacity -= pistolInfo.maxMagazine - pistolInfo.magazine;
                        }
                        else
                        {
                            pistolInfo.magazine += pistolInfo.carryingCapacity;
                            pistolInfo.carryingCapacity -= pistolInfo.carryingCapacity;
                        }
                    }
                }
            }
        }

        IEnumerator WaitCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
        }

    }
}
