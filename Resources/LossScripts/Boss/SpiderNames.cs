using System;
using System.Collections.Generic;
using LossScriptsTypes;
//------------------------------------------------------------------------------
//All content © 2020 DigiPen Institute of Technology Singapore. 
//All Rights Reserved
//Authors: 
//Purpose: 
//------------------------------------------------------------------------------
namespace LossScripts
{
    class SpiderNames : LossBehaviour
    {
        public GameObject camera = null;   //(0, 2.2, -20)
        public GameObject spider = null;
        public GameObject name = null;
        public GameObject subtitle = null;
        //public bool entranceFinished = false;

        private Vector3 nameStartPos = new Vector3(0, 4.6f, 3.0f);    //(0, 4.6, 3)
        private Vector3 subtitleStartPos = new Vector3(0, - 0.65f, 3.0f);  //(0, -0.65, 3)

        public Vector3 nameMidPos = new Vector3(0, 4.6f, 3.0f);
        public Vector3 subtitleMidPos = new Vector3(0, -0.65f, 3.0f);

        public Vector3 nameEndPos = new Vector3(20, 4.6f, 3.0f);
        public Vector3 subtitleEndPos = new Vector3(-20, -0.65f, 3.0f);

        public bool isAnimating = false;

        void Start()
        {
            nameStartPos = name.transform.worldPosition;
            subtitleStartPos = subtitle.transform.worldPosition;
        }

        void Update()
        {
            isAnimating = name.GetComponent<Tween>().isTranslating;
        }

        public void StartAnimation()
        {
            name.GetComponent<Tween>().StartTweenTranslate(1.0f, nameMidPos);
            subtitle.GetComponent<Tween>().StartTweenTranslate(1.0f, subtitleMidPos);
        }

        public void EndAnimation()
        {
            name.GetComponent<Tween>().StartTweenTranslate(1.0f, nameEndPos);
            subtitle.GetComponent<Tween>().StartTweenTranslate(1.0f, subtitleEndPos);
        }
    }
}