using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class PlayerTongueCollision : LossBehaviour
    {
        //Status
        public bool isCollidingSomething;
        private bool isCollidingHinge;
        private bool isCollidingSling;
        private bool isCollidingDeadEnemy;

        //Object
        private GameObject touchedObject;
        private Vector3 touchedPosition;

        private bool HasIgnoreColliderTag(string tag)
        {
            if (tag != "Player" && tag != "IgnoreCollision" && tag != "Checkpoint" && tag != "CameraZone" && tag != "EventTrigger" && tag != "Bounce" && tag != "CameraOffset" && tag != "HiddenRoom")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void Update()
        {
            CheckCollisionStatus();
        }

        void FixedUpdate()
        {
            CheckCollisionStatus();
        }

        private void CheckCollisionStatus()
        {
            if (touchedObject == null)
            {
                touchedPosition = Vector3.zero;
                isCollidingHinge = false;
                isCollidingSling = false;
                isCollidingDeadEnemy = false;
                isCollidingSomething = false;
            }
        }

        public void ResetCollision()
        {
            isCollidingSomething = false;
            isCollidingHinge = false;
            isCollidingSling = false;
            isCollidingDeadEnemy = false;
        }

        public bool GetCollisionSomething()
        {
            return isCollidingSomething;
        }

        public bool GetCollisionHinge()
        {
            return isCollidingHinge;
        }

        public bool GetCollisionSling()
        {
            return isCollidingSling;
        }

        public bool GetCollisionDeadEnemy()
        {
            return isCollidingDeadEnemy;
        }

        public GameObject GetTouchedObject()
        {
            return touchedObject;
        }

        public Vector3 GetTouchedPosition()
        {
            return touchedPosition;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (HasIgnoreColliderTag(collider.gameObject.tag) == true)
            {
                if (collider.gameObject.tag == "HingePoint")
                {
                    isCollidingHinge = true;
                }
                else if (collider.gameObject.tag == "SlingPoint")
                {
                    isCollidingSling = true;
                }
                else if (collider.gameObject.tag == "EnemyDead")
                {
                    if (collider.gameObject.GetComponent<EnemyBehaviour>() != null)
                    {
                        collider.gameObject.GetComponent<EnemyBehaviour>().EatEnemy(this.gameObject);
                    }
                    isCollidingDeadEnemy = true;
                }
                touchedPosition = collider.gameObject.transform.worldPosition;
                touchedObject = collider.gameObject;
                isCollidingSomething = true;
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (HasIgnoreColliderTag(collider.gameObject.tag) == true)
            {
                if (collider.gameObject.tag == "HingePoint")
                {
                    isCollidingHinge = false;
                }
                else if (collider.gameObject.tag == "SlingPoint")
                {
                    isCollidingSling = false;
                }
                else if (collider.gameObject.tag == "EnemyDead")
                {
                    isCollidingDeadEnemy = false;
                }
                touchedPosition = Vector3.zero;
                touchedObject = null;
                isCollidingSomething = false;
            }
        }
    }
}