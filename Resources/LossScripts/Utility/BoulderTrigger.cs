using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class BoulderTrigger : LossBehaviour
    {
        private RigidBody m_boulderRB = null;
        public GameObject m_boulder = null;

        void Start()
        {
            if (m_boulder != null)
                m_boulderRB = m_boulder.GetComponent<RigidBody>();
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                m_boulderRB.timeScale = 1.0f;
            }
        }
    }
}
