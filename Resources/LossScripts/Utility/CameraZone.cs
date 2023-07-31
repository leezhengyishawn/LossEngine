using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class CameraZone : LossBehaviour
    {
        public bool canChangeBGM;
        public bool canChangeVignette;

        //Facing Left Settings
        public int bgmValueLeft;
        public int facingVisionLeft;
        public float speedVisionLeft;
        public bool vignetteTriggerLeft;


        //Facing Right Settings
        public int bgmValueRight;
        public int facingVisionRight;
        public float speedVisionRight;
        public bool vignetteTriggerRight;

        public int GetPlayerFacingVision(float directionX)
        {
            if (directionX < 0)
            {
                return facingVisionLeft;
            }
            else
            {
                return facingVisionRight;
            }
        }

        public float GetPlayerFacingSpeed(float directionX)
        {
            if (directionX < 0)
            {
                return speedVisionLeft;
            }
            else
            {
                return speedVisionRight;
            }
        }

        public int GetPlayerFacingBGM(float directionX)
        {
            if (directionX < 0)
            {
                return bgmValueLeft;
            }
            else
            {
                return bgmValueRight;
            }
        }

        public bool GetPlayerFacingVignette(float directionX)
        {
            if (directionX < 0)
            {
                return vignetteTriggerLeft;
            }
            else
            {
                return vignetteTriggerRight;
            }
        }

        public bool GetAllowBGMChange()
        {
            return canChangeBGM;
        }

        public bool GetAllowVignetteChange()
        {
            return canChangeVignette;
        }
    }
}