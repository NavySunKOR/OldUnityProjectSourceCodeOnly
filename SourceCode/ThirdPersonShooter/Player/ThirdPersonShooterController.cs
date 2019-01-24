using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonShooter
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class ThirdPersonShooterController : MonoBehaviour {

        public float moveSpeed;
        public bool canCover = false;
        public LayerMask cover;
        public Transform coverDetectionObject;

        private CharacterController cc;
        private CapsuleCollider cColider;
        private ThirdPersonShooterCamera trdCamera;
        private float originalHeight;
        private Transform coverHit;
        private Vector3 coverHitPoint;

        private void Awake()
        {
            cc = GetComponent<CharacterController>();
            cColider = GetComponent<CapsuleCollider>();
            trdCamera = GetComponent<ThirdPersonShooterCamera>();


            originalHeight = cc.height;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }

        private void Update()
        {

            CoverCheck();
            InputCheck();
            ApplyInput();
            Move();
        }

        private void CoverCheck()
        {
            if (!PlayerStat.cover)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 5f, cover))
                {
                    canCover = true;
                    coverHit = hit.transform;
                    coverHitPoint = hit.point;

                }
                else
                {
                    canCover = false;
                }
            }

        }

        private void InputCheck()
        {
            bool inputForward = InputManager.GetKey(InputNames.moveForward) && !PlayerStat.crouch && !PlayerStat.cover;
            bool inputBackward = InputManager.GetKey(InputNames.moveBackward) && !PlayerStat.crouch && !PlayerStat.cover;
            bool inputLeft = InputManager.GetKey(InputNames.moveLeft) && !PlayerStat.crouch;
            bool inputRight = InputManager.GetKey(InputNames.moveRight) && !PlayerStat.crouch;
            bool inputCrouch = InputManager.GetKey(InputNames.crouch) && !PlayerStat.run;
            bool inputAim = InputManager.GetKey(InputNames.aim) && !PlayerStat.run;
            bool inputRun = (InputManager.GetKey(InputNames.run) && !inputCrouch && !PlayerStat.cover && !inputAim && !PlayerStat.aim);
            bool inputCover = InputManager.GetKeyDown(InputNames.cover) && canCover;
            
            PlayerStat.crouch = inputCrouch;
            PlayerStat.aim = inputAim;
            PlayerStat.run = inputRun;

            //Movement
            if (inputForward)
            {
                PlayerStat.verticalMove = (inputRun) ? 1f : 0.5f;
            }
            else if(inputBackward)
            {
                PlayerStat.verticalMove = -0.5f;
            }
            else
            {
                PlayerStat.verticalMove = 0f;
            }
            

            if(inputLeft)
            {
                PlayerStat.horizontalMove = -0.5f;
                if (PlayerStat.cover)
                    PlayerStat.coverMovement = -1;
            }
            else if (inputRight)
            {
                PlayerStat.horizontalMove = 0.5f;
                if (PlayerStat.cover)
                    PlayerStat.coverMovement = 1;
            }
            else
            {
                PlayerStat.horizontalMove = 0f;
                PlayerStat.coverMovement = 0;
            }

            if (inputCover)
            {
                if (!PlayerStat.cover)
                {
                    StartCoroutine(SlideToCover());
                }
                PlayerStat.cover = !PlayerStat.cover;
            }

        }

        IEnumerator SlideToCover()
        {
            
            float t = 0;
            while(t < 1)
            {
                t += 1f * Time.deltaTime;
                Vector3 dir = coverHitPoint - transform.position;
                dir.Normalize();
                transform.position = Vector3.Lerp(transform.position, coverHitPoint - dir * 1f, t);
                yield return null;
            }

        }

        private void ApplyInput()
        {
            if (PlayerStat.cover)
            {
                if (PlayerStat.coverMovement > 0)
                {
                    coverDetectionObject.position = transform.position + transform.right * 0.5f;
                }
                else if(PlayerStat.coverMovement < 0)
                {
                    coverDetectionObject.position = transform.position - transform.right * 0.5f;
                }

                Ray ray = new Ray(coverDetectionObject.position, coverDetectionObject.forward);
                RaycastHit hit;
                bool raycast = Physics.Raycast(ray, out hit, 2f, cover);
                if (!raycast)
                {
                    PlayerStat.horizontalMove = 0f;
                    if (InputManager.GetKeyDown(InputNames.aim))
                    {
                        trdCamera.SetOriginalRotation();
                        if(PlayerStat.coverMovement >= 0)
                            transform.position += transform.right * 1f;
                        else if (PlayerStat.coverMovement <= 0)
                            transform.position -= transform.right * 2f;
                    }
                    else if (InputManager.GetKeyUp(InputNames.aim))
                    {
                        trdCamera.BackToOriginalRotation();
                        if (PlayerStat.coverMovement >= 0)
                            transform.position -= transform.right * 1f;
                        else if (PlayerStat.coverMovement <= 0)
                            transform.position += transform.right * 2f;
                    }
                }

                PlayerStat.aim = PlayerStat.aim && !raycast;
            }
            
        }

        private void Move()
        {
            if (!cc.isGrounded)
                cc.Move(transform.forward * PlayerStat.verticalMove * moveSpeed * Time.deltaTime + transform.right * PlayerStat.horizontalMove * moveSpeed * Time.deltaTime + Physics.gravity * Time.deltaTime);
            else
                cc.Move(transform.forward * PlayerStat.verticalMove * moveSpeed * Time.deltaTime + transform.right * PlayerStat.horizontalMove * moveSpeed * Time.deltaTime );
        }
    }
}
