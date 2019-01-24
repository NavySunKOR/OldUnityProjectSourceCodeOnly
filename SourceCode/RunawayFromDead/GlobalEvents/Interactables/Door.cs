using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class Door : MonoBehaviour {
        public string doorOpenRequire;
        public float openingSpeed;
        private bool isOpen;
        private bool doorTurning;
        private float minAngle;
        private float maxAngle;
        private float angle;
        private Player player;
        private GlobalEventManager ge;

        private void Awake()
        {
            minAngle = 0f;
            maxAngle = 90f;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            ge = GameObject.FindGameObjectWithTag("GlobalEventManager").GetComponent<GlobalEventManager>();
        }

        private void Update()
        {
            if (doorTurning)
            {
                MovingDoor();
            }
        }

        public void OpenDoor()
        {
            if(PropertyName.IsNullOrEmpty(doorOpenRequire))
            {
                doorTurning = true;
                isOpen = !isOpen;
            }
            else
            {
                if (ge.HasKey(doorOpenRequire))
                {
                    if (doorOpenRequire.Equals("hallwayKey"))
                    {
                        player.CallPlayerGameOverEvent();
                    }
                    else
                    {
                        doorTurning = true;
                        isOpen = !isOpen;
                    }
                }
                //check global events for open...
            }
        }

        public void MovingDoor()
        {
            if (isOpen)
            {
                Debug.Log(angle + " closing");
                angle = Mathf.LerpAngle(maxAngle, minAngle, Time.deltaTime);
                if (angle >= maxAngle - 10f)
                {
                    doorTurning = false;
                }
            }
            else
            {
                Debug.Log(angle + " opening");
                angle = Mathf.LerpAngle(minAngle, maxAngle, Time.deltaTime);
                if (angle <= minAngle + 10f)
                {
                    doorTurning = false;
                }
            }
            

            transform.localEulerAngles = new Vector3(0, angle, 0);
        }


        
	}
}
