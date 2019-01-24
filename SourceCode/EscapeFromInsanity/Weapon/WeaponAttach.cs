using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon {

    [System.Serializable]
    public class Weapon
    {
        public float rpm;
        public float damage;
        public float range;
        public float attackInterval;
        public float attackTimer;
        public int currentMagazine;
        public int maxMagazine;
        public int currentCapacity;
        public int maxCapacity;
    }

	public class WeaponAttach : MonoBehaviour {

        public Transform cameraTr;
        public Transform playerTr;
        public Weapon weaponInfo;
        public AudioClip fireSound;
        public AudioClip reloadSound;
        public GameObject bulletHole;
        public GameObject muzzleFlash;
        public GameObject bloodEffect;
        public LayerMask enemyLayer;
        private AudioSource audioSource;


        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
       
            weaponInfo.attackInterval = 60f / weaponInfo.rpm;
            audioSource = GetComponent<AudioSource>();

        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && weaponInfo.currentMagazine > 0)
            {
                if(Time.time - weaponInfo.attackTimer > weaponInfo.attackInterval)
                {
                    weaponInfo.attackTimer = Time.time;
                    FireWeapon();
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && weaponInfo.currentCapacity >= weaponInfo.maxMagazine - weaponInfo.currentMagazine && weaponInfo.currentMagazine < weaponInfo.maxMagazine)
            {
                Reload();
            }
        }

        private void FireWeapon()
        {
            Ray ray = new Ray(cameraTr.position + cameraTr.forward * 1f, cameraTr.forward);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, weaponInfo.range, enemyLayer))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.transform.SendMessage("TookHit", weaponInfo.damage);
                    GameObject go = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(playerTr.position - hit.point));
                    Destroy(go, 1.5f);
                }
            }
            weaponInfo.currentMagazine--;
            StartCoroutine(MuzzleFlash());

        }

        IEnumerator MuzzleFlash()
        {
            audioSource.clip = fireSound;
            audioSource.Play();
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.01f);
            muzzleFlash.SetActive(false);
        }


        private void Reload()
        {
            audioSource.clip = reloadSound;
            audioSource.Play();
            weaponInfo.currentMagazine += weaponInfo.maxMagazine - weaponInfo.currentMagazine;
            playerTr.SendMessage("Reload");
        }
    }
}
