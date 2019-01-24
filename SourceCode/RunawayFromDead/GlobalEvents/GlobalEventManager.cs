using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunawayFromDead {
	public class GlobalEventManager : MonoBehaviour {

        public string[] keyOwn;
        public Transform[] finalSpawnPoint;
        public GameObject enemy;

        private void Awake()
        {
            finalSpawnPoint = GameObject.Find("FinalSpawnPoints").GetComponentsInChildren<Transform>();
        }

        public bool HasKey(string key)
        {
            for(int i = 0; i < keyOwn.Length; i++)
            {
                if(keyOwn[i] == key)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddKey(string key)
        {
            for(int i=0; i < keyOwn.Length; i++)
            {
                if (PropertyName.IsNullOrEmpty(keyOwn[i]))
                {
                    if (key.Equals("hallwayKey"))
                        SpawnFinalEnemies();
                    keyOwn[i] = key;
                    return;
                }
            }
        }

        public void SpawnFinalEnemies()
        {
            for(int i = 0; i < finalSpawnPoint.Length; i++)
            {
                Instantiate(enemy, finalSpawnPoint[i].position, Quaternion.identity);
            }
        }
	}
}
