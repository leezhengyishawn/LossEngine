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
    class RotatePoint : LossBehaviour
    {
        public float Speed = 1.0f;
        public float StartRotate = 0.0f;
        public float EndRotate = 0.0f;

        private Transform m_trans;
        private float timer = 0.0f;
        void Start()
        {
            m_trans = gameObject.transform;
            timer = 0.0f;
        }

        void Update()
        {
            timer += Time.deltaTime * Speed;
            Vector3 rot = m_trans.localRotation;
            rot.z = StartRotate + (EndRotate - StartRotate) * (float)Math.Sin(timer);
            m_trans.localRotation = rot;
        }

        void ResetRot()
        {
            timer = 0.0f;
        }
    }
}
