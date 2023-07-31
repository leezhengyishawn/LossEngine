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
    class ScreenShake : LossBehaviour
    {
        //public float shakeDuration = 0.2f;
        public float trauma = 2.0f;
        public float maxOffsetX = 0.25f;
        public float maxOffsetY = 0.25f;

        private Vector3 oldPos;
        private float shakeTimer = 0;

        void FixedUpdate()
        {
            if (shakeTimer > 0)
            {
                Random rnd = new Random();

                this.gameObject.transform.localPosition = new Vector3(oldPos.x + (maxOffsetX * trauma * rnd.Next(-1,2)),
                                                                      oldPos.y + (maxOffsetY * trauma * rnd.Next(-1,2)),
                                                                      this.gameObject.transform.localPosition.z);

                trauma *= 0.9f; //Quadratically scale down the trauma

                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0)
                    this.gameObject.transform.localPosition = new Vector3(oldPos.x, oldPos.y, oldPos.z);
            }
        }

        public void SetShake(float time = 0.2f, float traumaSet = 1.5f, float offsetX = 0.25f, float offsetY = 0.25f)
        {
            //If already shaking we do not add a new shake
            if (shakeTimer > 0)
                return;

            oldPos = this.gameObject.transform.localPosition;
            //shakeDuration = time;
            shakeTimer = time;
            trauma = traumaSet;
            maxOffsetX = offsetX;
            maxOffsetY = offsetY;
        }
    }
}
