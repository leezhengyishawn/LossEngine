using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class CameraTrigger : LossBehaviour
    {
        // Camera
        private GameObject m_camera = null;
        private CameraBehaviour cameraBehaviour;
        private AudioSource[] rumble = null;

        public bool camShake = false;
        public GameObject otherTrigger = null;
        public GameObject sfxObject = null;

        void Start()
        {
            // Get camera object and its behavior
            m_camera = GameObject.GetGameObjectsOfTag("MainCamera")[0];
            cameraBehaviour = m_camera.GetComponent<CameraBehaviour>();

            rumble = sfxObject.GetComponents<AudioSource>();
        }

        void Update()
        {
            if (camShake)
                cameraBehaviour.SetShake(0.1f, 0.1f, 0.1f, 0.1f);
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player" && this.gameObject.tag == "SafetyCheckpoint")
            {
                for (uint i = 0; i < rumble.Length; ++i)
                {
                    if (rumble[i].sourceName == "SFX_Rumble")
                    {
                        //Audio.StopSource(rumble[i].id);
                        //Audio.PauseSource(rumble[i].id, true);
                        rumble[i].volume = 0.0f;
                        break;
                    }
                }
                otherTrigger.GetComponent<CameraTrigger>().camShake = false;
                camShake = false;
            }
        }

        void OnTriggerStay(Collider collider)
        { 
            if (collider.gameObject.tag == "Player" && this.gameObject.tag == "CamShake")
                camShake = true;
        }
    }
}
