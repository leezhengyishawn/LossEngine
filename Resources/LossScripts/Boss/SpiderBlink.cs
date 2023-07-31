using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using LossScriptsTypes;

namespace LossScripts
{
    class SpiderBlink : LossBehaviour
    {
        public float blinkInterval = 5.0f;
        public float blinkTimer = 5.0f;
        public bool canBlink = true;

        public float backPos = -9.0f;
        public float frontPos = 0.0f;
        private Vector3 shadowColor = new Vector3(0.5f, 0.5f, 0.5f);
        void Update()
        {
            if (canBlink)
            {
                blinkTimer -= Time.deltaTime;
                if (blinkTimer < 0.0f)
                {
                    this.gameObject.GetComponent<Animator>().animateCount = 1;
                    blinkTimer = blinkInterval;
                }

                float diff = 1.0f - shadowColor.x;
                float diffShadow = ((this.gameObject.transform.worldPosition.z - backPos) / (frontPos - backPos)) * diff;

                if (diffShadow + shadowColor.x > 1.0f) //Clamp to 1
                    diffShadow = 1.0f - shadowColor.x;

                this.gameObject.GetComponent<SpriteRenderer>().r = diffShadow;
                this.gameObject.GetComponent<SpriteRenderer>().g = diffShadow;
                this.gameObject.GetComponent<SpriteRenderer>().b = diffShadow;
            }
        }
    }
}