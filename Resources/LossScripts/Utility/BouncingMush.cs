using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class BouncingMush : LossBehaviour
    {
        private Lighting obj = null;
        public int currCount = 0;
        public int maxCount = 3;
        public GameObject lightManager = null;

        void Start()
        {
            obj = lightManager.GetComponent<Lighting>();
        }

        void OnCollisionEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                if (currCount < maxCount)
                {
                    ++currCount;
                    obj.AddCount();
                }
            }
        }
    }
}
