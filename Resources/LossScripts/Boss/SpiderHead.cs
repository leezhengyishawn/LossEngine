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
    class SpiderHead : LossBehaviour
    {
        public GameObject Eye0 = null;
        public GameObject Eye1 = null;
        public GameObject Eye2 = null;
        public GameObject Eye3 = null;
        public GameObject Eye4 = null;
        public GameObject Eye5 = null;
        public GameObject Eye6 = null;
        public GameObject Eye7 = null;
        public GameObject Eye8 = null;
        public GameObject Eye9 = null;

        public GameObject EyeSprite0 = null;
        public GameObject EyeSprite1 = null;
        public GameObject EyeSprite2 = null;
        public GameObject EyeSprite3 = null;
        public GameObject EyeSprite4 = null;
        public GameObject EyeSprite5 = null;
        public GameObject EyeSprite6 = null;
        public GameObject EyeSprite7 = null;
        public GameObject EyeSprite8 = null;
        public GameObject EyeSprite9 = null;

        public void SetEyes(bool follow, Vector3 targetPoint)
        {
            if (Eye0 != null)
                Eye0.GetComponent<SpiderEye>().followFrog = follow;
            if (Eye1 != null)
                Eye1.GetComponent<SpiderEye>().followFrog = follow;
            if (Eye2 != null)
                Eye2.GetComponent<SpiderEye>().followFrog = follow;
            if (Eye3 != null)
                Eye3.GetComponent<SpiderEye>().followFrog = follow;
            if (Eye4 != null)
                Eye4.GetComponent<SpiderEye>().followFrog = follow;
            if (Eye5 != null)
                Eye5.GetComponent<SpiderEye>().followFrog = follow;
            if (Eye6 != null)
                Eye6.GetComponent<SpiderEye>().followFrog = follow;
            if (Eye7 != null)
                Eye7.GetComponent<SpiderEye>().followFrog = follow;
            if (Eye8 != null)
                Eye8.GetComponent<SpiderEye>().followFrog = follow;
            if (Eye9 != null)
                Eye9.GetComponent<SpiderEye>().followFrog = follow;

            if (follow == false)
            {
                if (Eye0 != null)
                    Eye0.GetComponent<SpiderEye>().targetPos = targetPoint;
                if (Eye1 != null)
                    Eye1.GetComponent<SpiderEye>().targetPos = targetPoint;
                if (Eye2 != null)
                    Eye2.GetComponent<SpiderEye>().targetPos = targetPoint;
                if (Eye3 != null)
                    Eye3.GetComponent<SpiderEye>().targetPos = targetPoint;
                if (Eye4 != null)
                    Eye4.GetComponent<SpiderEye>().targetPos = targetPoint;
                if (Eye5 != null)
                    Eye5.GetComponent<SpiderEye>().targetPos = targetPoint;
                if (Eye6 != null)
                    Eye6.GetComponent<SpiderEye>().targetPos = targetPoint;
                if (Eye7 != null)
                    Eye7.GetComponent<SpiderEye>().targetPos = targetPoint;
                if (Eye8 != null)
                    Eye8.GetComponent<SpiderEye>().targetPos = targetPoint;
                if (Eye9 != null)
                    Eye9.GetComponent<SpiderEye>().targetPos = targetPoint;
            }
        }

    } //SpiderBoss
} //LossEngine