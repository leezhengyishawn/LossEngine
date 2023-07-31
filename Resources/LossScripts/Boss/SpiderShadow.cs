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
    class SpiderShadow : LossBehaviour
    {
        public GameObject spider; //follow the spider
        Vector3 startPos;
        void Start()
        {
            startPos = new Vector3(this.gameObject.transform.localPosition.x,
            this.gameObject.transform.localPosition.y,
            this.gameObject.transform.localPosition.z + 9);
        }
        void Update()
        {
            this.gameObject.transform.localPosition = new Vector3(spider.transform.localPosition.x + startPos.x,
                                                                  this.gameObject.transform.localPosition.y,
                                                                  spider.transform.localPosition.z + startPos.z);
        }
    } //SpiderBoss
} //LossEngine