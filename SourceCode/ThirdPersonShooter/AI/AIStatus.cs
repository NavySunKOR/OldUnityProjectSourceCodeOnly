using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonShooter
{
    [System.Serializable]
	public class AIStatus {
        public int health;
        public int damage;
        public float movementSpeed;
        public bool tracking;
        public bool die;
        public float attackInterval;
        public float attackTimer;
        public float attackRange;
        public float detectionRange;
	}
}
