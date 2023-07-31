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
    class EventTrigger : LossBehaviour
    {
        //Getting Camera
        private GameObject camera;
        private CameraBehaviour cameraBehaviour;

        //Getting Watchout Object
        private string watchoutSheet = "In_Game_Dad_Dialogue";
        public GameObject watchoutObject;
        private SpriteRenderer watchoutRender;
        private Animator watchoutAnimator;

        //Duration Setting
        public float eventDurationSet;
        private float eventDurationCurrent;

        //Event Sequence
        public int eventSequence;
        public bool isActivated;

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
                watchoutRender = watchoutObject.GetComponent<SpriteRenderer>();
                watchoutAnimator = watchoutObject.GetComponent<Animator>();
                watchoutObject.active = false;
                hasInit = true;
            }
        }

        private void CheckEvent()
        {
            if (isActivated == true)
            {
                if (eventDurationCurrent < eventDurationSet)
                {
                    eventDurationCurrent += Time.deltaTime;
                }
                else
                {
                    switch (eventSequence)
                    {
                        case 0: //Falling Cutscene, fading to black
                            cameraBehaviour.SetXYSpeed(0.2f);
                            cameraBehaviour.FadeVignetteToBlack(true, 0.0f, 1.0f, 0.5f);
                            cameraBehaviour.FadeOverlayToBlack(true, 0.0f, 3.0f, 0.7f);
                            ++eventSequence;
                            break;

                        case 1: //Falling Cutscene, stop camera
                            if (cameraBehaviour.GetBlackOverlayAlpha() >= 0.5f)
                            {
                                cameraBehaviour.SetXYSpeed(0.15f);
                                cameraBehaviour.SetDeadLock(true);
                                ++eventSequence;
                            }
                            break;

                        case 2: //Falling Cutscene, stop all audio
                            if (cameraBehaviour.GetBlackOverlayAlpha() >= 1.0f)
                            {
                                ++eventSequence;
                            }
                            if (Audio.masterVolume > 0)
                            {
                                Audio.masterVolume -= Time.deltaTime * 1f;
                            }
                            break;

                        case 3: //Falling Cutscene, play drop audio
                            if (cameraBehaviour.GetBlackOverlayAlpha() >= 1.5f)
                            {
                                Audio.StopAllSource();
                                Audio.masterVolume = 1f;
                                Audio.PlaySource("SFX_Crash");
                                ++eventSequence;
                            }
                            break;

                        case 4: //Falling Cutscene, play drop audio
                            if (cameraBehaviour.GetBlackOverlayAlpha() >= 2.2f)
                            {
                                Audio.PlaySource("SFX_RockDropCave");
                                ++eventSequence;
                            }
                            break;

                        case 5: //Falling Cutscene, change scene
                            if (cameraBehaviour.GetBlackOverlayAlpha() >= 2.8f)
                            {
                                Scene.ChangeScene("03_FatherCutscene");
                                ++eventSequence;
                                isActivated = false;
                            }
                            break;
                    }
                }
            }
        }

        public bool GetIsActivated()
        {
            return isActivated;
        }

        public void ActivateEvent()
        {
            isActivated = true;
            cameraBehaviour.SetCutscene(true);
            //cameraBehaviour.DisplayHealthUI(false);
            cameraBehaviour.SetXYSpeed(0.2f);
            watchoutObject.active = true;
            if (watchoutRender.textureName != watchoutSheet)
            {
                watchoutRender.textureName = watchoutSheet;
                watchoutRender.currentFrameX = 1;
                watchoutRender.currentFrameY = 1;
            }
            watchoutAnimator.animateCount = 1;
            if (!(Audio.IsSourcePlaying("SFX_Talk_WatchOut")))
            {
                Audio.PlaySource("SFX_Talk_WatchOut");
            }
        }
    }
}