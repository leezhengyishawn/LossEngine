using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class CutsceneEnding : LossBehaviour
    {
        //Getting Camera
        private GameObject camera;
        private CameraBehaviour cameraBehaviour;

        //Event Sequence
        private int eventSequence;

        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        void Update()
        {
            CheckInit();
            CheckEventSequence();
        }

        private void CheckInit()
        {
            if (hasInit == false)
            {
                cameraBehaviour = camera.GetComponent<CameraBehaviour>();
                eventSequence = 0;
                hasInit = true;
            }
        }

        private void CheckEventSequence()
        {
            switch (eventSequence)
            {
                case 0:
                    cameraBehaviour.FadeOverlayToBlack(false, 0.0f, 1.0f, 0.6f);
                    ++eventSequence;
                    break;

                case 1:
                    if (cameraBehaviour.GetBlackOverlayAlpha() < 0.1f)
                    {
                        cameraBehaviour.FadeOverlayToBlack(true, 0.0f, 1.3f, 0.4f);
                        cameraBehaviour.SetFieldOfVision(-1, 0.002f);
                        ++eventSequence;
                    }
                    break;

                case 2:
                    if (cameraBehaviour.GetBlackOverlayAlpha() >= 1.3f)
                    {
                        ++eventSequence;
                    }
                    break;

                case 3:
                    Audio.StopAllSource();
                    Audio.masterVolume = 1f;
                    Scene.ClearScenes("01_MainMenu");
                    break;
            }
        }
    }
}