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
    class PauseController : LossBehaviour
    {
        //Getting Player
        private GameObject mainPlayer;
        private PlayerBehaviour mainPlayerBehaviour;

        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        void Update()
        {
            CheckInit();
            CheckPauseInput();
        }

        private void CheckInit()
        {
            if (hasInit == false)
            {
                int playerCount = GameObject.GetGameObjectsOfTag("Player").Length;
                if (playerCount > 0) //There is a player in the scene
                {
                    mainPlayer = GameObject.GetGameObjectsOfTag("Player")[0];
                    mainPlayerBehaviour = mainPlayer.GetComponent<PlayerBehaviour>();
                }
                hasInit = true;
            }
        }

        private void CheckPauseInput()
        {
            if (Input.GetKeyPress(KEYCODE.KEY_ESCAPE) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_START, 0))
            {
                if (mainPlayerBehaviour != null && mainPlayerBehaviour.GetCanControl() == true)
                {
                    Scene.PushScene("01_PauseMenu");
                }
            }
        }
    }
}