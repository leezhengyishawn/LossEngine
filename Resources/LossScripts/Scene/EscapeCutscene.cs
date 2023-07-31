using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class EscapeCutscene : LossBehaviour
    {
        // Camera
        private GameObject m_camera = null;
        private CameraBehaviour cameraBehaviour;

        // Player
        private GameObject mainPlayer;
        private PlayerBehaviour mainPlayerBehaviour;

        private bool reachTarget = false;

        public GameObject CameraTarget = null;
        public float movementSpeed = 0.0f;
        
        void Start()
        {
            // Get camera object and its behavior
            m_camera = GameObject.GetGameObjectsOfTag("MainCamera")[0];
            cameraBehaviour = m_camera.GetComponent<CameraBehaviour>();

            // Get player object and its behavior
            mainPlayer = GameObject.GetGameObjectsOfTag("Player")[0];
            mainPlayerBehaviour = mainPlayer.GetComponent<PlayerBehaviour>();

            mainPlayerBehaviour.SetCanControl(false);
            cameraBehaviour.SetDeadLock(true);

            if (m_camera != null && CameraTarget != null)
            {
                m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, CameraTarget.transform.localPosition, movementSpeed);
            }
        }

        void Update()
        {
            if (m_camera != null && CameraTarget != null && reachTarget == false)
            {
                m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, CameraTarget.transform.localPosition, movementSpeed);

                if (m_camera.transform.localPosition.x -  CameraTarget.transform.localPosition.x < 0.0f)
                    reachTarget = true;
            }

            if (reachTarget)
            {
                m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, mainPlayer.transform.localPosition, movementSpeed);

                mainPlayerBehaviour.SetCanControl(true);
                cameraBehaviour.SetDeadLock(false);
            }
        }
    }
}
