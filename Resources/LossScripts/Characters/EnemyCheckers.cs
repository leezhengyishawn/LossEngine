using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class EnemyCheckers : LossBehaviour
    {
        //Status
        private bool isCollidingGround;
        private bool isCollidingWall;

        //Checkings
        public bool checkGround;
        public bool checkWall;

        //Touched Game Object
        private GameObject touchedGroundObject;
        private GameObject touchedWallObject;

        //Collision Ignore
        private string[] listOfCollisionIgnore;
        
        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        void Update()
        {
            if (hasInit == false)
            {
                listOfCollisionIgnore = new string[11];
                listOfCollisionIgnore[0] = "Enemy";
                listOfCollisionIgnore[1] = "IgnoreCollision";
                listOfCollisionIgnore[2] = "HingePoint";
                listOfCollisionIgnore[3] = "SlingPoint";
                listOfCollisionIgnore[4] = "Checkpoint";
                listOfCollisionIgnore[5] = "CameraZone";
                listOfCollisionIgnore[6] = "EventTrigger";
                listOfCollisionIgnore[7] = "HiddenRoom";
                listOfCollisionIgnore[8] = "RoomTransition";
                listOfCollisionIgnore[9] = "Bounce";
                listOfCollisionIgnore[10] = "CameraOffset";
                hasInit = true;
            }
            CheckCollisionStatus();
        }

        void FixedUpdate()
        {
            CheckCollisionStatus();
        }

        private void CheckCollisionStatus()
        {
            if (touchedGroundObject == null)
            {
                isCollidingGround = false;
            }
            if (touchedWallObject == null)
            {
                isCollidingWall = false;
            }
        }

        private bool HasIgnoreColliderTag(string tag)
        {
            for (int i = 0; i < listOfCollisionIgnore.Length; i++)
            {
                if (tag == listOfCollisionIgnore[i])
                {
                    return false;
                }
            }
            return true;
        }

        public bool GetCollisionStatus(string checkString)
        {
            switch (checkString)
            {
                case "GROUND":
                    return isCollidingGround;

                case "WALL":
                    return isCollidingWall;

                default:
                    return false;
            }
        }

        void OnTriggerStay(Collider collider)
        {
            if (HasIgnoreColliderTag(collider.gameObject.tag) == true)
            {
                if (checkGround == true)
                {
                    touchedGroundObject = collider.gameObject;
                    isCollidingGround = true;
                }
                if (checkWall == true)
                {
                    touchedWallObject = collider.gameObject;
                    isCollidingWall = true;
                }
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (HasIgnoreColliderTag(collider.gameObject.tag) == true)
            {
                if (checkGround == true)
                {
                    touchedGroundObject = null;
                    isCollidingGround = false;
                }
                if (checkWall == true)
                {
                    touchedWallObject = null;
                    isCollidingWall = false;
                }
            }
        }
    }
}