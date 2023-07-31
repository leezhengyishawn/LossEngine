using System;
using LossScriptsTypes;
using LossScriptsAttributes;

namespace LossScripts
{
    class StressTestScriptIndividual : LossBehaviour
    {
        public float distSq = 9.0f;
        private SpriteRenderer renderer;
        public GameObject go;
        public GameObject go2;
        public GameObject go3;

        void Start()
        {
            renderer = gameObject.GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (renderer != null)
            {
                Vector3 mousePos = Camera.MouseToWorldPoint();

                if ((gameObject.transform.worldPosition - mousePos).magnitudeSq < distSq)
                    renderer.r = 0.0f;
                else
                    renderer.r = 1.0f;
            }
        }
    }
}
