using System;
using System.Collections.Generic;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class DandeSpawn : LossBehaviour
    {
        public float spawnDurationSet;
        public bool isCave = false;

        private float spawnDurationCurrent;

        private void Update()
        {
            if (spawnDurationCurrent < spawnDurationSet)
            {
                spawnDurationCurrent += Time.deltaTime;
            }
            else
            {
                spawnDurationCurrent = 0.0f;
                SpawnDande();
            }
        }

        private void SpawnDande()
        {
            GameObject mySpawnedDande;
            if (!isCave)
            {
                mySpawnedDande = GameObject.InstantiatePrefab("DandeObject");
            }
            else
            {
                mySpawnedDande = GameObject.InstantiatePrefab("MushObject");
            }
            if (mySpawnedDande != null)
            {
                mySpawnedDande.transform.localPosition = this.gameObject.transform.worldPosition;
            }
        }
    }
}