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
    class Tween : LossBehaviour
    {
        public bool isTranslating = false;
        public bool isRotating = false;

        private float timeToTweenTranslate = 1.0f;
        private float timeToTweenRotate = 1.0f;

        private float timeTranslateLeft = 1.0f;
        private float timeRotateLeft = 1.0f;

        public Vector3 endTranslate;
        public Vector3 endRotate;

        private int dir = 1; //+ve is clockwise, use only 1 or -1

        private Vector3 translateDifference;
        private Vector3 rotationDifference;

        public bool isTranslatingX = false;
        public bool isTranslatingY = false;
        public bool isTranslatingZ = false;

        private float timeTranslateLeftX = 1.0f;
        private float timeTranslateLeftY = 1.0f;
        private float timeTranslateLeftZ = 1.0f;

        void Update()
        {
            if (isTranslating)
            {
                this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x + ((translateDifference.x / timeToTweenTranslate) * Time.deltaTime), 
                                                                      this.gameObject.transform.localPosition.y + ((translateDifference.y / timeToTweenTranslate) * Time.deltaTime), 
                                                                      this.gameObject.transform.localPosition.z + ((translateDifference.z / timeToTweenTranslate) * Time.deltaTime));
                
                timeTranslateLeft -= Time.deltaTime;

                if (timeTranslateLeft <= 0)
                {
                    this.gameObject.transform.localPosition = new Vector3(endTranslate.x, endTranslate.y, endTranslate.z);
                    isTranslating = false;
                }
            }

            if (isRotating)
            {
                this.gameObject.transform.localRotation = new Vector3(0, 
                                                                      0, 
                                                                      this.gameObject.transform.localRotation.z + ((rotationDifference.z / timeToTweenRotate) * dir) * Time.deltaTime);
                
                timeRotateLeft -= Time.deltaTime;

                if (timeRotateLeft <= 0)
                {
                    this.gameObject.transform.localRotation = new Vector3(endRotate.x, endRotate.y, endRotate.z);
                    isRotating = false;
                }
            }
        }

        void FixedUpdate()
        {
            if (isTranslatingX)
            {
                this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x + ((translateDifference.x / timeToTweenTranslate) * Time.deltaTime), 
                                                                      this.gameObject.transform.localPosition.y, 
                                                                      this.gameObject.transform.localPosition.z);
                
                timeTranslateLeftX -= Time.deltaTime;

                if (timeTranslateLeftX <= 0)
                {
                    this.gameObject.transform.localPosition = new Vector3(endTranslate.x, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
                    isTranslatingX = false;
                }
            }
        }

        public void StartTweenTranslate(float time, Vector3 target)
        {
            timeToTweenTranslate = time;
            timeTranslateLeft = time;
            endTranslate = target;

            translateDifference = new Vector3(
                target.x - this.gameObject.transform.localPosition.x,
                target.y - this.gameObject.transform.localPosition.y,
                target.z - this.gameObject.transform.localPosition.z
            );

            isTranslating = true;
        }

        public void StartTweenTranslateX(float time, float newX)
        {
            timeTranslateLeftX = time;
            endTranslate = new Vector3(newX, endTranslate.y, endTranslate.z);

            translateDifference = new Vector3(
                newX - this.gameObject.transform.localPosition.x,
                translateDifference.y,
                translateDifference.z
            );

            isTranslatingX = true;
        }

        public void StartTweenRotate(float time, Vector3 target, int direction)
        {
            timeToTweenRotate = time;
            timeRotateLeft = time;
            endRotate = target;

            if (direction > 0) //anticlockwise
            {
                if (target.z < this.gameObject.transform.localRotation.z)
                    target.z += 360;
                rotationDifference = new Vector3(0,
                                                 0,
                                                 target.z - this.gameObject.transform.localRotation.z);
            }
            else
            {
                if (target.z > this.gameObject.transform.localRotation.z)
                    target.z -= 360;
                rotationDifference = new Vector3(0,
                                                 0,
                                                 target.z - this.gameObject.transform.localRotation.z );
            }
            isRotating = true;
        }
    }
}
