using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using LossScriptsTypes;

namespace LossScripts
{
    class Wobble : LossBehaviour
    {
        public float freqX = 5.0f; //how fast it shakes
        public float freqY = 5.0f; //how fast it shakes
        public float ampX  = 0.1f; //how far it can reach
        public float ampY  = 0.1f; //how far it can reach

        private float accumTime = 0.0f;

        private Vector3 startingPos;
        void Start()
        {
            startingPos = gameObject.transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            accumTime += Time.deltaTime;

            float wobbleX = (float)Math.Cos(accumTime * freqX) * ampX;
            float wobbleY = (float)Math.Sin(accumTime * freqY) * ampY;
            this.gameObject.transform.localPosition = new Vector3(
                //this.gameObject.transform.localPosition.x + wobbleX,
                //this.gameObject.transform.localPosition.y + wobbleY,
                startingPos.x + wobbleX,
                startingPos.y + wobbleY,
                startingPos.z);
                
        }
    }
}