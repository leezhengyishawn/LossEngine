using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class EnemyWebBehaviour : LossBehaviour
    {
        //Game Object Component
        private RigidBody rb2D;
        private GameObject spawnerObject;
        private Vector3 movementDirection;
        private float movementSpeed;
        private float lifetimeTimerSet;
        private float lifetimeTimerCurrent;

        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        void Update()
        {
            if (hasInit == false)
            {
                rb2D = this.gameObject.GetComponent<RigidBody>();
                hasInit = true;
            }
            else
            {
                rb2D.velocity = new Vector2(movementDirection.x * movementSpeed, movementDirection.y * movementSpeed);
                if (lifetimeTimerCurrent < lifetimeTimerSet)
                {
                    lifetimeTimerCurrent += Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private bool HasIgnoreColliderTag(string tag)
        {
            if (tag != "IgnoreCollision" && tag != "HingePoint" && tag != "SlingPoint" && tag != "Checkpoint" && tag != "CameraZone" && tag != "EventTrigger" && tag != "HiddenRoom" && tag != "RoomTransition")
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
                if (collider.gameObject.tag == "Player")
                {
                    collider.gameObject.GetComponent<PlayerBehaviour>().StunPlayer(true, this.gameObject.transform.worldPosition.x);
                }
                Destroy(gameObject);
            }
        }

        public void WebSettings(GameObject spawner, Vector3 direction, float speed, float lifetime)
        {
            spawnerObject = spawner;
            movementDirection = direction;
            movementSpeed = speed;
            lifetimeTimerSet = lifetime;
        }
    }
}