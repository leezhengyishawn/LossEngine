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
    class TimedDestroy : LossBehaviour
    {
        public float lifetime = 1.0f;

        private void Update()
        {
            lifetime -= Time.deltaTime;
            if (lifetime < 0.0f)
                Destroy(this.gameObject);
        }
    }
}