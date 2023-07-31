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
    class BGMController : LossBehaviour
    {
        private AudioSource[] sourceArray;
        private Vector2[] volumePitchArray;
        private int myBGMValue;
        public bool forestBGM = false;
        public bool undergroundBGM = false;
        public bool caveBGM = false;
        public bool caveSecretBGM = false;
        public bool bossBGM = false;

        void Start()
        {
            sourceArray = this.gameObject.GetComponents<AudioSource>();
            volumePitchArray = new Vector2[sourceArray.Length];
            for (int i = 0; i < volumePitchArray.Length; i++)
            {
                volumePitchArray[i] = new Vector2(0, 1);
            }
            for (int i = 0; i < sourceArray.Length; i++)
            {
                sourceArray[i].volume = volumePitchArray[i].x;
                sourceArray[i].pitch = volumePitchArray[i].y;
            }
        }

        void Update()
        {
            PlayBGM();
        }

        private void PlayBGM()
        {
            if (forestBGM)
            {
                ChangeBGMPitch("BGM_Forest", 0.7f, 1f);
                ChangeBGMPitch("BGM_FrogAmbience", 0.3f, 0.8f);
                ChangeBGMPitch("BGM_ForestAmbience", 0.1f, 0.8f);
                ChangeBGMPitch("BGM_WhaleAmbience", 0.0f, 0.8f);
                ChangeBGMPitch("BGM_Escape", 0.0f, 1.0f);
                undergroundBGM = false;
                caveBGM = false;
                caveSecretBGM = false;
                bossBGM = false;
            }
            else if (undergroundBGM)
            {
                ChangeBGMPitch("BGM_Forest", 0.7f, 0.9f);
                ChangeBGMPitch("BGM_FrogAmbience", 0.2f, 0.7f);
                ChangeBGMPitch("BGM_ForestAmbience", 0.1f, 0.3f);
                ChangeBGMPitch("BGM_WhaleAmbience", 0.0f, 0.8f);
                ChangeBGMPitch("BGM_Escape", 0.0f, 1.0f);
                forestBGM = false;
                caveBGM = false;
                caveSecretBGM = false;
                bossBGM = false;
            }
            else if (caveBGM)
            {
                ChangeBGMPitch("BGM_Forest", 0.6f, 0.85f);
                ChangeBGMPitch("BGM_FrogAmbience", 0.1f, 0.4f);
                ChangeBGMPitch("BGM_ForestAmbience", 0.1f, 0.1f);
                ChangeBGMPitch("BGM_WhaleAmbience", 0.2f, 0.5f);
                ChangeBGMPitch("BGM_Escape", 0.0f, 1.0f);
                undergroundBGM = false;
                forestBGM = false; 
                caveSecretBGM = false;
                bossBGM = false;
            }
            else if (caveSecretBGM)
            {
                ChangeBGMPitch("BGM_Main_Game", 0.6f, 0.7f);
                ChangeBGMPitch("BGM_FrogAmbience", 0.0f, 0.4f);
                ChangeBGMPitch("BGM_ForestAmbience", 0.0f, 0.1f);
                ChangeBGMPitch("BGM_WhaleAmbience", 0.5f, 0.5f);
                ChangeBGMPitch("BGM_Escape", 0.0f, 1.0f);
                forestBGM = false;
                undergroundBGM = false;
                caveBGM = false;
                bossBGM = false;
            }
            else if (bossBGM)
            {
                ChangeBGMPitch("BGM_Main_Game", 0.0f, 0.8f);
                ChangeBGMPitch("BGM_FrogAmbience", 0.0f, 0.4f);
                ChangeBGMPitch("BGM_ForestAmbience", 0.0f, 0.1f);
                ChangeBGMPitch("BGM_WhaleAmbience", 0.2f, 0.6f);
                ChangeBGMPitch("BGM_Escape", 0.6f, 0.8f);
                forestBGM = false;
                undergroundBGM = false;
                caveBGM = false;
                caveSecretBGM = false;
            }
        }

        private void ChangeBGMPitch(string soundName, float volumeValue, float pitchValue)
        {
            for (int i = 0; i < sourceArray.Length; i++)
            {
                if (sourceArray[i].sourceName == soundName)
                {
                    volumePitchArray[i] = Vector2.Lerp(volumePitchArray[i], new Vector2(volumeValue, volumePitchArray[i].y), 0.01f);
                    sourceArray[i].volume = volumePitchArray[i].x;
                    volumePitchArray[i] = Vector2.Lerp(volumePitchArray[i], new Vector2(volumePitchArray[i].x, pitchValue), 0.0005f);
                    sourceArray[i].pitch = volumePitchArray[i].y;
                }
            }
        }

        public void SetBGM(int bgmValue)
        {
            myBGMValue = bgmValue;
            switch (bgmValue)
            {
                case 0:
                    //Forest BGM
                    forestBGM = true;
                    undergroundBGM = false;
                    caveBGM = false;
                    caveSecretBGM = false;
                    bossBGM = false;
                    break;

                case 1:
                    //Underground BGM
                    forestBGM = false;
                    undergroundBGM = true;
                    caveBGM = false;
                    caveSecretBGM = false;
                    bossBGM = false;
                    break;

                case 2:
                    //Cave BGM
                    forestBGM = false;
                    undergroundBGM = false;
                    caveBGM = true;
                    caveSecretBGM = false;
                    bossBGM = false;
                    break;

                case 3:
                    //Cave BGM
                    forestBGM = false;
                    undergroundBGM = false;
                    caveBGM = false;
                    caveSecretBGM = true;
                    bossBGM = false;
                    break;

                case 4:
                    //Cave BGM
                    forestBGM = false;
                    undergroundBGM = false;
                    caveBGM = false;
                    caveSecretBGM = false;
                    bossBGM = true;
                    break;
            }
        }

        public int GetBGM()
        {
            return myBGMValue;
        }
    }
}