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
    class DandeParticle : LossBehaviour
    {
        private float lifetimeDurationSet = 5.0f;
        private float lifetimeDurationCurrent = 0.0f;

        private void Update()
        {
            if (lifetimeDurationCurrent < lifetimeDurationSet)
            {
                lifetimeDurationCurrent += Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}