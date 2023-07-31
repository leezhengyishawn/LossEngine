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
    class GodRays : LossBehaviour
    {
        SpriteRenderer SR;
        public float speed;
        private float timePassed = 0.0f;
        private bool fadeOut = true;
        void Start()
        {
            SR = this.gameObject.GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (SR != null)
            {
                if (fadeOut)
                {
                    SR.a -= Time.deltaTime * speed;
                    if (SR.a <= 0.0f)
                    {
                        fadeOut = false;
                        SR.a = 0.0f;
                    }
                }
                else
                {
                    SR.a += Time.deltaTime * speed;
                    if (SR.a >= 1.0f)
                    {
                        fadeOut = true;
                        SR.a = 1.0f;
                    }
                }
            }
        }

    }
}
