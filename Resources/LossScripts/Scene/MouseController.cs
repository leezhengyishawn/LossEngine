using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class MouseController : LossBehaviour
    {
        //Sprite Components
        private SpriteRenderer spriteRenderer;
        private string hoverOn = "Aiming_Cursor";
        private string hoverOff = "Normal_Cursor";

        //Getting Game Object
        private GameObject hoverObject;
        private Vector3 hoverPosition;
        private bool isHovering;

        //Position Checkings
        private Vector3 mouseScreenPosition;

        private GameObject player;

        void Start()
        {
            spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.a = 0.0f;

            player = GameObject.GetGameObjectsOfTag("Player")[0];
        }

        void Update()
        {
            CheckPositions();
            MoveMouse();
            CheckHoverStatus();
        }
        
        void FixedUpdate()
        {
            //CheckPositions();
            //MoveMouse();
            //CheckHoverStatus();
        }

        private void CheckPositions()
        {
            mouseScreenPosition = Camera.MouseToWorldPoint();
        }

        private void MoveMouse()
        {
            this.gameObject.transform.localPosition = mouseScreenPosition;
        }

        private void CheckHoverStatus()
        {
            if (hoverObject == null || (hoverObject != null && hoverObject.GetComponent<TongueInteractive>() != null && hoverObject.GetComponent<TongueInteractive>().active == false))
            {
                hoverObject = null;
                hoverPosition = Vector3.zero;
                Mouse.SetCursor(hoverOff, true);
                // spriteRenderer.textureName = hoverOff;
                isHovering = false;
            }
        }

        void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.GetComponent<TongueInteractive>() != null && collider.gameObject.GetComponent<TongueInteractive>().active == true)
            {
                hoverObject = collider.gameObject;
                hoverPosition = hoverObject.transform.worldPosition;
                // spriteRenderer.textureName = hoverOn;
                Mouse.SetCursor(hoverOn, false);
                isHovering = true;
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.GetComponent<TongueInteractive>() != null && collider.gameObject.GetComponent<TongueInteractive>().active == true)
            {
                hoverObject = null;
                hoverPosition = Vector3.zero;
                // spriteRenderer.textureName = hoverOff;
                Mouse.SetCursor(hoverOff, true);
                isHovering = false;
            }
        }

        public GameObject GetHoverObject()
        {
            return hoverObject;
        }

        public Vector3 GetHoverPosition()
        {
            return hoverPosition;
        }

        public bool GetHover()
        {
            return isHovering;
        }
    }
}