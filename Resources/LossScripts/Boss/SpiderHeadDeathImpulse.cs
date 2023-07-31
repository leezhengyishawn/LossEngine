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
    class SpiderHeadDeathImpulse : LossBehaviour
    {
        public GameObject spiderMainBody = null;
        public GameObject frog = null;
        //public GameObject camera = null;
        public Vector3 targetPos = new Vector3(7.0f, -1.0f, 1.0f);
        public float yAddedHeight = 2.5f;                           //How much additional height the head reaches
        private bool impulsed = false;
        public void DeathImpulse()
        {
            spiderMainBody.GetComponent<SpiderBoss>().TeethLeftPivot.GetComponent<RotatePoint>().active = false;
            spiderMainBody.GetComponent<SpiderBoss>().TeethRightPivot.GetComponent<RotatePoint>().active = false;
            spiderMainBody.GetComponent<SpiderBoss>().TeethLeftPivot.transform.localRotation = new Vector3(0.0f, 0.0f, -7.0f);
            spiderMainBody.GetComponent<SpiderBoss>().TeethRightPivot.transform.localRotation = new Vector3(0.0f, 0.0f, 20.0f);
            this.gameObject.transform.parent = null;
            this.gameObject.GetComponent<RigidBody>().AddImpulse(new Vector2(0.0f, 40.0f), new Vector2(0, 0));

            impulsed = true;
        }

        void Update()
        {
            if (this.gameObject.transform.worldPosition.y < targetPos.y)
            {
                this.gameObject.transform.localPosition = new Vector3(targetPos.x, targetPos.y, targetPos.z);
                this.gameObject.GetComponent<RigidBody>().active = false;

                //Change the frog sprite into fainting
                frog.GetComponent<SpriteRenderer>().textureName = "Character_Death";
                frog.GetComponent<SpriteRenderer>().currentFrameX = 1;
                frog.GetComponent<SpriteRenderer>().currentFrameY = 1;

                frog.GetComponent<Animator>().fileName = "PlayerDeathAnim";
                frog.GetComponent<Animator>().animateCount = 1;

                spiderMainBody.GetComponent<SpiderBoss>().camera.GetComponent<ScreenShake>().SetShake(0.5f, 2.2f, 2.0f, 2.0f);

                //Shockwave
                spiderMainBody.GetComponent<Shockwave>().objToTrigger = this.gameObject;
                //spiderMainBody.GetComponent<Shockwave>().waveAmplitude = 5000.0f;
                //spiderMainBody.GetComponent<Shockwave>().waveWidth = 0.1f;
                //spiderMainBody.GetComponent<Shockwave>().waveSpeed = 0.8f;
                //spiderMainBody.GetComponent<Shockwave>().timer = 1.6f;
                spiderMainBody.GetComponent<Shockwave>().Pulse();

                frog.GetComponent<PlayerBehaviour>().active = false;
                frog.GetComponent<RigidBody>().AddImpulse(-25.0f, 0.0f, 0.0f, 0.0f);
                Audio.PlaySource("SFX_Boss_Spider_Land");
            }
            else if (this.gameObject.transform.worldPosition.y > 15.0f && impulsed == true)
            {
                this.gameObject.transform.localPosition = new Vector3(targetPos.x, 15.0f, targetPos.z);
                this.gameObject.transform.localRotation = new Vector3(0.0f, 0.0f, 230.0f);
                this.gameObject.GetComponent<RigidBody>().gravityScale = 10.0f;
                spiderMainBody.GetComponent<SpiderBoss>().TeethLeftPivot.transform.localRotation = new Vector3(0.0f, 0.0f, -7.0f);
                spiderMainBody.GetComponent<SpiderBoss>().TeethRightPivot.transform.localRotation = new Vector3(0.0f, 0.0f, 20.0f);
            }
        }
    } //SpiderBoss
} //LossEngine