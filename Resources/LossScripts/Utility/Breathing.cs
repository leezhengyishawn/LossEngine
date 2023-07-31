using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class Breathing : LossBehaviour
    {
        private LightSource light = null;
        private float timer = 0.0f;
        private bool pause = false;

        public float min = 0.0f;
        public float max = 0.0f;
        public float increment = 0.0f;
        public float holdPeakTimer = 0.0f;

        void Start()
        {
            light = this.gameObject.GetComponent<LightSource>();
        }

        void FixedUpdate()
        {
            if (!pause)
            {
                if (light != null)
                    light.diffuse += increment;

                if (light.diffuse > max)
                {
                    pause = true;
                    increment *= -1.0f;
                }

                if (light.diffuse < min)
                {
                    pause = true;
                    increment *= -1.0f;
                }
            }
            else
                timer += Time.deltaTime;

            if (timer > holdPeakTimer)
            {
                timer = 0.0f;
                pause = false;
            }
        }
    }
}
