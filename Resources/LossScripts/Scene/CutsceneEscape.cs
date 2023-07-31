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
        public bool playCutScene = true;

        public GameObject CameraTarget = null;
        public float movementSpeed = 0.0f;
        public float currTime = 0.0f;
        public float timeDelay = 2.0f;

        void Start()
        {
            // Get camera object and its behavior
            m_camera = GameObject.GetGameObjectsOfTag("MainCamera")[0];
            cameraBehaviour = m_camera.GetComponent<CameraBehaviour>();

            // Get player object and its behavior
            mainPlayer = GameObject.GetGameObjectsOfTag("Player")[0];
            mainPlayerBehaviour = mainPlayer.GetComponent<PlayerBehaviour>();

            File.ReadJsonFile("EscapeConfig");
            playCutScene = File.RetrieveDataAsBool("Cutscene");

            if (m_camera != null && CameraTarget != null && playCutScene)
            {
                mainPlayerBehaviour.SetCanControl(false);
                cameraBehaviour.SetDeadLock(true);
                cameraBehaviour.SetCutscene(true);
                m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, CameraTarget.transform.localPosition, movementSpeed);
            }
            else
                reachTarget = true;
        }

        void Update()
        {
            if (playCutScene)
            {
                if (reachTarget)
                    currTime += Time.deltaTime;

                if (m_camera != null && CameraTarget != null && reachTarget == false)
                {
                    // Lddderp camera to curtain
                    m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, CameraTarget.transform.localPosition, movementSpeed);
                    cameraBehaviour.SetShake(0.1f, 0.1f, 0.1f, 0.1f);

                    if (Math.Abs(m_camera.transform.localPosition.x - CameraTarget.transform.localPosition.x) <= 1.0f)
                        reachTarget = true;
                }

                // Check if the camera reached
                if (reachTarget && currTime > timeDelay)
                {
                    File.WriteDataAsBool("Cutscene", false);
                    File.WriteJsonFile("EscapeConfig");
                    mainPlayerBehaviour.SetCanControl(true);
                    cameraBehaviour.SetDeadLock(false);
                    cameraBehaviour.SetCutscene(false);
                    playCutScene = false;
                }
            }
        }

        public bool GetTargetReached()
        {
            return reachTarget;
        }
    }
}
