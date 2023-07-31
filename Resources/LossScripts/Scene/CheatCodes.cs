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
    class CheatCodes : LossBehaviour
    {
        void Update()
        {
            if (Input.GetKey(KEYCODE.KEY_1))
            {
                Audio.StopAllSource();
                Audio.masterVolume = 1f;
                Scene.ChangeScene("03_FatherCutscene");
            }
            if (Input.GetKey(KEYCODE.KEY_2))
            {
                Audio.StopAllSource();
                Audio.masterVolume = 1f;
                Scene.ChangeScene("05_Cavern");
            }
            if (Input.GetKey(KEYCODE.KEY_3))
            {
                Audio.StopAllSource();
                Audio.masterVolume = 1f;
                Scene.ChangeScene("06_SecretCave");
            }            
            if (Input.GetKey(KEYCODE.KEY_4))
            {
                Audio.StopAllSource();
                Audio.masterVolume = 1f;
                Scene.ChangeScene("07_Boss");
            }
            if (Input.GetKey(KEYCODE.KEY_5))
            {
                Audio.StopAllSource();
                Audio.masterVolume = 1f;
                Scene.ChangeScene("08_Escape");
            }
            if (Input.GetKey(KEYCODE.KEY_6))
            {
                Audio.StopAllSource();
                Audio.masterVolume = 1f;
                Scene.ChangeScene("09_SecretForest");
            }
        }
    }
}