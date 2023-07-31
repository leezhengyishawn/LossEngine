using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class BoulderHole : LossBehaviour
    {
        private Transform HoleTopTransform = null;
        private Transform HoleBtmTransform = null;

        public bool startAnimation = false;
        public float startSize = 0.0f;
        public float endSize = 0.0f;
        public float incrementSpeed = 0.0f;
        public GameObject HoleTop = null;
        public GameObject HoleBtm = null;
        public GameObject DebrisParticleEmitter = null;
        public GameObject DebrisParticleEmitter2 = null;
        public GameObject DustParticleEmitter = null;

        void Start()
        {
            if (HoleTop != null)
            {
                HoleTopTransform = HoleTop.transform;
                HoleTopTransform.localScale = new Vector3(startSize, startSize, 1.0f);
            }

            if (HoleBtm != null)
            {
                HoleBtmTransform = HoleBtm.transform;
                HoleBtmTransform.localScale = new Vector3(startSize, startSize, 1.0f);
            }

            if (DebrisParticleEmitter != null)
                DebrisParticleEmitter.GetComponent<ParticleEmitter>().Stop();

            if (DebrisParticleEmitter2 != null)
                DebrisParticleEmitter2.GetComponent<ParticleEmitter>().Stop();

            if (DustParticleEmitter != null)
                DustParticleEmitter.GetComponent<ParticleEmitter>().Stop();
        }

        void Update()
        {
            if (startAnimation)
            {
                DebrisParticleEmitter.GetComponent<ParticleEmitter>().Play();
                DebrisParticleEmitter2.GetComponent<ParticleEmitter>().Play();
                DustParticleEmitter.GetComponent<ParticleEmitter>().Play();

                HoleTopTransform.localScale = new Vector3(HoleTopTransform.localScale.x + incrementSpeed, 
                                                          HoleTopTransform.localScale.y + incrementSpeed,
                                                          1.0f);
                HoleBtmTransform.localScale = new Vector3(HoleBtmTransform.localScale.x + incrementSpeed, 
                                                          HoleBtmTransform.localScale.y + incrementSpeed, 
                                                          1.0f);

                if (HoleTopTransform.localScale.x >= endSize)
                {
                    HoleTopTransform.localScale = new Vector3(endSize,endSize, 1.0f);
                    HoleBtmTransform.localScale = new Vector3(endSize, endSize, 1.0f);
                    startAnimation = false;
                }
            }
        }

        public void SetAnimation(bool startAni)
        {
            startAnimation = startAni;
        }

        public bool GetAnimation()
        {
            return startAnimation;
        }
    }
}
