using System;
using System.Collections.Generic;
using LossScriptsTypes;
//------------------------------------------------------------------------------
//All content © 2020 DigiPen Institute of Technology Singapore. 
//All Rights Reserved
//Authors: 
//Purpose: 
//------------------------------------------------------------------------------
namespace LossScripts
{

    class BossStartCollider : LossBehaviour
    {
        public GameObject frog;
        public GameObject spider;
        public GameObject camera;

        void OnCollisionEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                spider.GetComponent<SpiderBoss>().spiderState = LossScripts.SpiderBoss.SpiderState.START_FALL;
                spider.GetComponent<RigidBody>().active = true;
                camera.GetComponent<CameraBehaviour>().SetCutscene(true);
                camera.GetComponent<CameraBehaviour>().SetDeadLock(true);
                frog.GetComponent<PlayerBehaviour>().SetCanControl(false);
                camera.GetComponent<CameraBehaviour>().SetBlackBarSpeed(0.05f);
                Destroy(gameObject);
            }
        }

    } //SpiderBoss
} //LossEngine