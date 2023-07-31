using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class SpikeTrigger : LossBehaviour
    {
        private bool isActivated = false;

        public GameObject spike1 = null;
        public GameObject spike2 = null;
        public GameObject spike3 = null;

        void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                if (!isActivated)
                {
                    if (spike1 != null)
                        spike1.GetComponent<SpikeShake>().SetSpikeShake(true);

                    if (spike2 != null)
                        spike2.GetComponent<SpikeShake>().SetSpikeShake(true);

                    if (spike3 != null)
                        spike3.GetComponent<SpikeShake>().SetSpikeShake(true);

                    isActivated = true;
                }
            }
        }
    }
}
