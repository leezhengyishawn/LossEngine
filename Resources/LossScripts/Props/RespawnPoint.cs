using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class RespawnPoint : LossBehaviour
    {
        public GameObject spawnPoint;

        public GameObject SpawnPointObject()
        {
            return spawnPoint;
        }

        public Vector3 SpawnPointPosition()
        {
            return spawnPoint.transform.worldPosition;
        }
    }
}