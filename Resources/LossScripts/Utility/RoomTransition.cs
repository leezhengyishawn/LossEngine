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
    class RoomTransition : LossBehaviour
    {
        //Getting Camera
        private GameObject camera;
        private CameraBehaviour cameraBehaviour;

        //Event Sequence
        public int eventSequence;
        public bool isActivated;
        public bool cutsceneEnd;
        public bool level1Start;
        public bool level1End;
        public bool secretStart;
        public bool secretEnd;
        public bool escapeEnd;
        public bool secretCaveBack;

        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        void Update()
        {
            CheckInit();
            CheckEvent();
        }

        private void CheckInit()
        {
            if (hasInit == false)
            {
                camera = GameObject.GetGameObjectsOfTag("MainCamera")[0];
                cameraBehaviour = camera.GetComponent<CameraBehaviour>();
                hasInit = true;
            }
        }

        private void CheckEvent()
        {
            if (isActivated == true)
            {
                switch (eventSequence)
                {
                    case 0:
                        cameraBehaviour.FadeVignetteToBlack(true, 0.0f, 3.0f, 0.5f);
                        cameraBehaviour.FadeOverlayToBlack(true, 0.0f, 3.0f, 0.7f);
                        ++eventSequence;
                        break;

                    case 1:
                        if (cameraBehaviour.GetBlackOverlayAlpha() >= 0.1f)
                        {
                            ++eventSequence;
                        }
                        break;

                    case 2:
                        if (Audio.masterVolume > 0)
                        {
                            Audio.masterVolume -= Time.deltaTime * 1f;
                        }
                        else
                        {
                            Audio.StopAllSource();
                            Audio.masterVolume = 1f;
                            ++eventSequence;
                        }
                        break;

                    case 3:
                       
                        if (cameraBehaviour.GetBlackOverlayAlpha() >= 1.0f)
                        {
                            if (cutsceneEnd == true)
                            {
                                Scene.ChangeScene("05_Cavern");
                            }
                            else if (level1Start == true)
                            {
                                Scene.ChangeScene("04_FatherCave_Back");
                            }
                            else if (secretEnd == true)
                            {
                                Scene.ChangeScene("07_Boss");
                            }
                            else if (level1End == true)
                            {
                                Scene.ChangeScene("06_SecretCave");
                            }
                            else if (secretStart == true)
                            {
                                Scene.ChangeScene("05_Cavern_Back");
                            }
                            else if (escapeEnd == true)
                            {
                                Scene.ChangeScene("09_SecretForest");
                            }
                            else if (secretCaveBack == true)
                            {
                                Scene.ChangeScene("06_SecretCave_Back");
                            }
                            else
                            {
                                Audio.StopAllSource();
                                Scene.ClearScenes("10_Ending");
                            }
                        }
                        break;
                }
            }
        }

        public bool GetIsActivated()
        {
            return isActivated;
        }

        public void ActivateTransition()
        {
            isActivated = true;
            cameraBehaviour.SetDeadLock(true);
            cameraBehaviour.DisplayHealthUI(false);
        }
    }
}