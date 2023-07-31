using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using LossScriptsTypes;

namespace LossScripts
{
    class TutorialCheatCodes : LossBehaviour
    {
        public GameObject player;
        public GameObject cp1; //checkpoint 1
        public GameObject cp2;
        public GameObject cp3;
        public GameObject cp4;

        void Update()
        {
            if (Input.GetKey(KEYCODE.KEY_CONTROL) && Input.GetKeyPress(KEYCODE.KEY_1))
            {
                player.transform.localPosition = new Vector3(cp1.transform.worldPosition.x,
                                                             cp1.transform.worldPosition.y,
                                                             cp1.transform.worldPosition.z);
            }
            if (Input.GetKey(KEYCODE.KEY_CONTROL) && Input.GetKeyPress(KEYCODE.KEY_2))
            {
                player.transform.localPosition = new Vector3(cp2.transform.worldPosition.x,
                                                             cp2.transform.worldPosition.y,
                                                             cp2.transform.worldPosition.z);
            }
            if (Input.GetKey(KEYCODE.KEY_CONTROL) && Input.GetKeyPress(KEYCODE.KEY_3))
            {
                player.transform.localPosition = new Vector3(cp3.transform.worldPosition.x,
                                                             cp3.transform.worldPosition.y,
                                                             cp3.transform.worldPosition.z);
            }
            if (Input.GetKey(KEYCODE.KEY_CONTROL) && Input.GetKeyPress(KEYCODE.KEY_4))
            {
                player.transform.localPosition = new Vector3(cp4.transform.worldPosition.x,
                                                             cp4.transform.worldPosition.y,
                                                             cp4.transform.worldPosition.z);
            }
            if (Input.GetKey(KEYCODE.KEY_CONTROL) && Input.GetKey(KEYCODE.KEY_B))
            {
                Audio.StopAllSource();
                Audio.masterVolume = 1f;
                Scene.ChangeScene("07_Boss");
            }
        }
    }
}