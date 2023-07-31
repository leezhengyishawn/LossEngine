using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class Lighting : LossBehaviour
    {
        private LightSource light1 = null;
        private LightSource light2 = null;
        private LightSource light3 = null;
        private LightSource light4 = null;
        private LightSource light5 = null;
        private LightSource light6 = null;
        private LightSource light7 = null;
        private LightSource light8 = null;
        private LightSource light9 = null;
        private LightSource light10 = null;
        private LightSource light11 = null;
        private LightSource light12 = null;

        private bool turnOn = false;

        public GameObject lightGO1 = null;
        public GameObject lightGO2 = null;
        public GameObject lightGO3 = null;
        public GameObject lightGO4 = null;
        public GameObject lightGO5 = null;
        public GameObject lightGO6 = null;
        public GameObject lightGO7 = null;
        public GameObject lightGO8 = null;
        public GameObject lightGO9 = null;
        public GameObject lightGO10 = null;
        public GameObject lightGO11 = null;
        public GameObject lightGO12 = null;

        public int counter = 0;
        public int maxCounter = 0;
        public int colourMode = 0;

        void Start()
        {
            light1 = lightGO1.GetComponent<LightSource>();
            light2 = lightGO2.GetComponent<LightSource>();
            light3 = lightGO3.GetComponent<LightSource>();
            light4 = lightGO4.GetComponent<LightSource>();
            light5 = lightGO5.GetComponent<LightSource>();
            light6 = lightGO6.GetComponent<LightSource>();
            light7 = lightGO7.GetComponent<LightSource>();
            light8 = lightGO8.GetComponent<LightSource>();
            light9 = lightGO9.GetComponent<LightSource>();
            light10 = lightGO10.GetComponent<LightSource>();
            light11 = lightGO11.GetComponent<LightSource>();
            light12 = lightGO12.GetComponent<LightSource>();
        }

        void Update()
        {
            if (!turnOn)
            {
                if (counter >= maxCounter)
                {
                    if (colourMode == 0)
                    {
                        light1.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light2.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light3.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light4.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light5.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light6.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light7.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light8.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light9.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light10.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light11.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light12.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                    }
                    else
                    {
                        light1.lightColor = new Vector4(0.8f, 0.0f, 0.0f, 1.0f);
                        light2.lightColor = new Vector4(0.0f, 0.8f, 0.0f, 1.0f);
                        light3.lightColor = new Vector4(0.0f, 0.0f, 0.8f, 1.0f);
                        light4.lightColor = new Vector4(0.8f, 0.8f, 0.0f, 1.0f);
                        light5.lightColor = new Vector4(0.0f, 0.8f, 0.8f, 1.0f);
                        light6.lightColor = new Vector4(0.8f, 0.0f, 0.8f, 1.0f);

                        light7.lightColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                        light8.lightColor = new Vector4(1.0f, 0.5f, 0.0f, 1.0f);
                        light9.lightColor = new Vector4(1.0f, 0.4f, 0.78f, 1.0f);
                        light10.lightColor = new Vector4(0.45f, 0.45f, 0.45f, 1.0f);
                        light11.lightColor = new Vector4(1.0f, 0.8f, 0.0f, 1.0f);
                        light12.lightColor = new Vector4(0.86f, 0.07f, 0.23f, 1.0f);
                    }

                    turnOn = true;
                }
            }
        }

        public void AddCount()
        {
            ++counter;
        }
    }
}
