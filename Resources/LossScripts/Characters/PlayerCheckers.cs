using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class PlayerCheckers : LossBehaviour
    {
        //Status
        private bool isCollidingGround;
        private bool isCollidingBounce;
        private bool isCollidingWall;
        private bool isCollidingJumpWall;

        //Checkings
        public bool checkGround;
        public bool checkBounce;
        public bool checkWall;
        public bool checkJumpWall;

        //Touched Game Object
        private GameObject touchedGroundObject;
        private GameObject touchedWallObject;
        private GameObject touchedBounceObject;
        private GameObject touchedJumpWallObject;

        //Collision Ignore
        private string[] listOfCollisionIgnore;

        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        void Update()
        {
            if (hasInit == false)
            {
                listOfCollisionIgnore = new string[15];
                listOfCollisionIgnore[0] = "Player";
                listOfCollisionIgnore[1] = "IgnoreCollision";
                listOfCollisionIgnore[2] = "HingePoint";
                listOfCollisionIgnore[3] = "SlingPoint";
                listOfCollisionIgnore[4] = "Checkpoint";
                listOfCollisionIgnore[5] = "CameraZone";
                listOfCollisionIgnore[6] = "EventTrigger";
                listOfCollisionIgnore[7] = "HiddenRoom";
                listOfCollisionIgnore[8] = "RoomTransition";
                listOfCollisionIgnore[9] = "Thorn";
                listOfCollisionIgnore[10] = "Spike";
                listOfCollisionIgnore[11] = "CamShake";
                listOfCollisionIgnore[12] = "SafetyCheckpoint";
                listOfCollisionIgnore[13] = "CameraOffset";
                listOfCollisionIgnore[14] = "Father";
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
            if (touchedBounceObject == null)
            {
                isCollidingBounce = false;
            }
            if (touchedJumpWallObject == null)
            {
                isCollidingJumpWall = false;
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

                case "BOUNCE":
                    return isCollidingBounce;

                case "WALL":
                    return isCollidingWall;

                case "JUMPWALL":
                    return isCollidingJumpWall;

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
                if (checkBounce == true && collider.gameObject.tag == "Bounce")
                {
                    touchedBounceObject = collider.gameObject;
                    if (collider.gameObject.GetComponent<Mushroom>() != null) { collider.gameObject.GetComponent<Mushroom>().isActivated = true; }
                    if (collider.gameObject.GetComponent<EnemyMushroom>() != null) { collider.gameObject.GetComponent<EnemyMushroom>().isActivated = true; }
                    isCollidingBounce = true;
                }
                if (checkWall == true)
                {
                    touchedWallObject = collider.gameObject;
                    isCollidingWall = true;
                }
                if (checkJumpWall == true && collider.gameObject.tag == "JumpWall")
                {
                    touchedJumpWallObject = collider.gameObject;
                    isCollidingJumpWall = true;
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
                if (checkBounce == true && collider.gameObject.tag == "Bounce")
                {
                    touchedBounceObject = null;
                    isCollidingBounce = false;
                }
                if (checkWall == true)
                {
                    touchedWallObject = null;
                    isCollidingWall = false;
                }
                if (checkJumpWall == true && collider.gameObject.tag == "JumpWall")
                {
                    touchedJumpWallObject = null;
                    isCollidingJumpWall = false;
                }
            }
        }
    }
}