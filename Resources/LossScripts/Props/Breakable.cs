using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class Breakable : LossBehaviour
    {
        private GameObject spawnerObject;
        private GameObject particle;
        public String sfx;
        public String particleEffectName = "Dust";
        private bool HasIgnoreColliderTag(string tag)
        {
            if (tag != "IgnoreCollision" && tag != "HingePoint" && tag != "SlingPoint" && tag != "Checkpoint" && tag != "Thorn" && tag != "CameraOffset")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void OnTriggerStay(Collider collider)
        {
            if (HasIgnoreColliderTag(collider.gameObject.tag) == true && collider.gameObject != spawnerObject)
            {
                if (collider.gameObject.tag == "Enemy")
                {
                    collider.gameObject.GetComponent<EnemyBehaviour>().HealthDecrease();
                }
                else if (collider.gameObject.tag == "Player")
                {
                    collider.gameObject.GetComponent<PlayerBehaviour>().HealthDecrease(true, this.gameObject.transform.worldPosition.x);
                }

                //if (this.gameObject.name != "Dropping_Spike_Escape")
                Audio.PlaySource(sfx);

                particle = GameObject.InstantiatePrefab(particleEffectName);

                if (particle != null && this != null)
                    particle.transform.localPosition = this.gameObject.transform.worldPosition;

                Destroy(this.gameObject);
            }
        }

        public void BreakableSettings(GameObject spawner, string newSheet)
        {
            spawnerObject = spawner;
            this.gameObject.GetComponent<SpriteRenderer>().textureName = newSheet;
        }
    }
}