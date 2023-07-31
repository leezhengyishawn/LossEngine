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
    class Dandelion : LossBehaviour
    {
        public bool isCave = false;

        public void SpawnParticle()
        {
            GameObject danteParticle;
            if (!isCave)
            {
                danteParticle = GameObject.InstantiatePrefab("DandeParticle");
            }
            else
            {
                danteParticle = GameObject.InstantiatePrefab("MushParticle");
            }
            if (danteParticle != null)
            {
                danteParticle.transform.localPosition = this.gameObject.transform.worldPosition;
            }
            Destroy(this.gameObject);
        }

        private bool HasIgnoreColliderTag(string tag)
        {
            if (tag != "IgnoreCollision" && tag != "HingePoint" && tag != "SlingPoint" && tag != "Checkpoint" && tag != "HiddenRoom" && tag != "CameraOffset" && tag != "CameraZone")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnTriggerStay(Collider collider)
        {
            if (HasIgnoreColliderTag(collider.gameObject.tag) == true)
            {
                SpawnParticle();
            }
        }
    }
}