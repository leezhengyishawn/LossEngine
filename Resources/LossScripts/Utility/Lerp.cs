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
    class Lerp : LossBehaviour
    {
        private Vector3 transformLerp;
        private float transformX;
        private float transformY;

        public GameObject wayPoint;

        public void Translate()
        {
            Vector3 moveTowards = new Vector3(wayPoint.transform.worldPosition.x, wayPoint.transform.worldPosition.y, wayPoint.transform.worldPosition.z);
            transformLerp = Vector3.Lerp(this.gameObject.transform.worldPosition, moveTowards, 0.01f);
            transformX = transformLerp.x;
            transformY = transformLerp.y;

            this.gameObject.transform.localPosition = new Vector3(transformX, transformY, this.gameObject.transform.localPosition.z);
        }

    }
}
