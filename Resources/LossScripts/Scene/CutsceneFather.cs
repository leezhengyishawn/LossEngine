using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class CutsceneFather : LossBehaviour
    {
        //Getting Camera
        private GameObject camera;
        private CameraBehaviour cameraBehaviour;

        //Getting Characters
        public GameObject fatherObject;

        //Getting Sprite
        private string frameFirst = "Dad_kidnap_01";
        private string frameSecond = "Dad_kidnap_02";

        //Getting Sprite Renderer and Animator
        private SpriteRenderer fatherRenderer;
        private Animator fatherAnimator;

        //Duration Setting
        private float animDurationSet;
        private float animDurationCurrent;

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
                camera = GameObject.GetGameObjectsOfTag("MainCamera")[0];
                cameraBehaviour = camera.GetComponent<CameraBehaviour>();
                fatherRenderer = fatherObject.GetComponent<SpriteRenderer>();
                fatherAnimator = fatherObject.GetComponent<Animator>();
                Audio.StopAllSource();
                fatherRenderer.textureName = frameFirst;
                eventSequence = 0;
                hasInit = true;
            }
        }

        private void CheckEventSequence()
        {
            switch (eventSequence)
            {
                case 0:
                    Audio.StopAllSource();
                    cameraBehaviour.FadeOverlayToBlack(false, 0.0f, 1.0f, 0.4f);
                    cameraBehaviour.SetFieldOfVision(-1, 1.0f);
                    ++eventSequence;
                    break;

                case 1:
                    if (cameraBehaviour.GetBlackOverlayAlpha() < 0.4f)
                    {
                        cameraBehaviour.FadeVignetteToBlack(false, 0.7f, 1.0f, 0.4f);
                        cameraBehaviour.SetFieldOfVision(0, 0.02f);
                        Audio.PlaySource("SFX_EnemyWalk");
                        ++eventSequence;
                    }
                    break;

                case 2:
                    if (cameraBehaviour.GetVignetteAlpha() < 0.75f)
                    {
                        cameraBehaviour.SetFieldOfVision(1, 0.2f);
                        ++eventSequence;
                    }
                    break;

                case 3:
                    if (cameraBehaviour.GetTargetFieldOfVision() > 12.5f)
                    {
                        fatherAnimator.active = true;
                        ++eventSequence;
                    }
                    break;

                case 4:
                    if (animDurationCurrent < animDurationSet)
                    {
                        animDurationCurrent += Time.deltaTime;
                    }
                    else
                    {
                        if (fatherRenderer.textureName != frameSecond)
                        {
                            fatherRenderer.textureName = frameSecond;
                            fatherRenderer.currentFrameX = 1;
                            fatherRenderer.currentFrameY = 1;
                        }
                        fatherAnimator.animateCount = 1;
                        animDurationSet = 1.0f;
                        animDurationCurrent = 0.0f;

                        Audio.PlaySource("SFX_Boss_Spider_Swipe");
                        ++eventSequence;
                    }
                    break;

                case 5:
                    if (animDurationCurrent < animDurationSet)
                    {
                        animDurationCurrent += Time.deltaTime;
                    }
                    else
                    {
                        fatherObject.active = false;
                        cameraBehaviour.FadeOverlayToBlack(true, 0.0f, 1.1f, 0.6f);
                        cameraBehaviour.FadeVignetteToBlack(true, 0.5f, 1.0f, 0.8f);
                        animDurationSet = 1.0f;
                        animDurationCurrent = 0.0f;
                        ++eventSequence;
                    }
                    break;

                case 6:
                    if (Audio.masterVolume > 0)
                    {
                        Audio.masterVolume -= Time.deltaTime * 1f;
                    }
                    else
                    {
                        if (cameraBehaviour.GetBlackOverlayAlpha() >= 1.0f)
                        {
                            Audio.StopAllSource();
                            Audio.masterVolume = 1f;
                            Scene.ChangeScene("04_FatherCave");
                            ++eventSequence;
                        }
                    }
                    break;
            }
        }
    }
}