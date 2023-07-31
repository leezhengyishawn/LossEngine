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
    class SpiderNamesFade : LossBehaviour
    {
        //public GameObject camera = null;   //(0, 2.2, -20)
        //public GameObject spider = null;
        public GameObject name = null;
        public GameObject subtitle = null;
        public float fadeTime = 0.2f;
        //public bool entranceFinished = false;

        //private Vector3 nameMidPos = new Vector3(0, 4.6f, 3.0f);
        //private Vector3 subtitleMidPos = new Vector3(0, -0.65f, 3.0f);

        //public Vector3 nameEndPos = new Vector3(20, 4.6f, 3.0f);
        //public Vector3 subtitleEndPos = new Vector3(-20, -0.65f, 3.0f);

        public bool isFading = false;

        void Start()
        {
            //nameStartPos = name.transform.worldPosition;
            //subtitleStartPos = subtitle.transform.worldPosition;
            name.GetComponent<SpriteRenderer>().a = 0;
            subtitle.GetComponent<SpriteRenderer>().a = 0;
        }

        void Update()
        {
            //sAnimating = name.GetComponent<Tween>().isTranslating;
            if (isFading)
            {
                if (name.GetComponent<SpriteRenderer>().a <= 1.0f)
                {
                    name.GetComponent<SpriteRenderer>().a += Time.deltaTime * fadeTime;
                    subtitle.GetComponent<SpriteRenderer>().a += Time.deltaTime * fadeTime;
                }
            }
            else
            {
                if (name.GetComponent<SpriteRenderer>().a >= 0.0f)
                {
                    name.GetComponent<SpriteRenderer>().a -= Time.deltaTime * fadeTime;
                    subtitle.GetComponent<SpriteRenderer>().a -= Time.deltaTime * fadeTime;
                }
            }
        }
        public void StartAnimation()
        {
            isFading = true;
            name.GetComponent<SpriteRenderer>().a = 0;
            subtitle.GetComponent<SpriteRenderer>().a = 0;
        }

        public void EndAnimation()
        {
            isFading = false;
            name.GetComponent<SpriteRenderer>().a = 1;
            subtitle.GetComponent<SpriteRenderer>().a = 1;
        }
    }
}