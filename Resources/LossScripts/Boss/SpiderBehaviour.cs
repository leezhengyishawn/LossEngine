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
    class SpiderBehaviour : LossBehaviour
    {
        private int spiderHealth = 1;

        public enum SpiderState
        {
            IDLE,
            ATTACK_JUMP_SQUATTING,
            ATTACK_JUMP_RISING,
            ATTACK_JUMP_FALLING,
            ATTACK_SWIPE_RAISE_LEFT,
            ATTACK_SWIPE_SWEEP_LEFT,
            ATTACK_SWIPE_RECOVER_LEFT,
            DYING,
            DEAD
        };
        public SpiderState spiderState = SpiderState.IDLE;
        //public SpiderState spiderStatePrevious;

        public float attackPatternTime = 5.0f;
        public float attackPatternTimer = 5.0f; //amount of time before starting attack
        bool hasPerformedSquatAttack = false; //jump first

        public float transitionTimer = 5.0f;
        //public float sceneTimer = 2.0f;

        public float spitTimer = 5.0f;

        //Jump attack variables
        private bool isSquatting = false;
        public float jumpImpulse = 75.0f;

        //Swipe attack variables
        private int swipeAttackCount = 0;
        public float swipeWindupTime = 2.0f;
        public float swipeRecoverTime = 3.0f;
        public float swipeAttackWindupHoldTime = 0.3f;
        public float swipeAttackTime = 0.3f;
        public float swipeAttackHoldTime = 0.4f;
        private float swipeAttackHoldTimer;
        private float dyingTimer = 5.0f;

        public GameObject Camera = null;
        public GameObject DandBottom = null;
        public GameObject DandTop = null;

        public GameObject DeathRock = null;
        public GameObject DamageCollider = null;

        //Animation pivots
        public GameObject FrontLegLeft4 = null;
        public GameObject FrontLegLeft3 = null;
        public GameObject FrontLegLeft2 = null;
        public GameObject FrontLegLeft1 = null;

        public GameObject FrontLegRight4 = null;
        public GameObject FrontLegRight3 = null;
        public GameObject FrontLegRight2 = null;
        public GameObject FrontLegRight1 = null;

        public GameObject BackLegsLeft = null;
        public GameObject BackLegsRight = null;

        public GameObject Head = null;
        public GameObject Thorax = null;
        public GameObject Abdomen = null;

        public GameObject FrontLegLeft4Collider = null;
        public GameObject FrontLegLeft3Collider = null;
        public GameObject FrontLegLeft2Collider = null;
        public GameObject FrontLegLeft1Collider = null;

        public GameObject FrontLegRight4Collider = null;
        public GameObject FrontLegRight3Collider = null;
        public GameObject FrontLegRight2Collider = null;
        public GameObject FrontLegRight1Collider = null;

        void Start()
        {
            ConfineRotationDegrees();

            FrontLegLeft4.GetComponent<RotatePoint>().Speed = 1.0f;
            FrontLegLeft4.GetComponent<RotatePoint>().StartRotate = FrontLegLeft4.transform.localRotation.z - 2;
            FrontLegLeft4.GetComponent<RotatePoint>().EndRotate = FrontLegLeft4.transform.localRotation.z + 2;

            FrontLegLeft3.GetComponent<RotatePoint>().Speed = -1.5f;
            FrontLegLeft3.GetComponent<RotatePoint>().StartRotate = FrontLegLeft3.transform.localRotation.z - 2;
            FrontLegLeft3.GetComponent<RotatePoint>().EndRotate = FrontLegLeft3.transform.localRotation.z + 2;

            FrontLegLeft2.GetComponent<RotatePoint>().Speed = 1.0f;
            FrontLegLeft2.GetComponent<RotatePoint>().StartRotate = FrontLegLeft2.transform.localRotation.z - 2;
            FrontLegLeft2.GetComponent<RotatePoint>().EndRotate = FrontLegLeft2.transform.localRotation.z + 2;

            FrontLegLeft1.GetComponent<RotatePoint>().Speed = -1.5f;
            FrontLegLeft1.GetComponent<RotatePoint>().StartRotate = FrontLegLeft1.transform.localRotation.z - 10;
            FrontLegLeft1.GetComponent<RotatePoint>().EndRotate = FrontLegLeft1.transform.localRotation.z + 10;


            FrontLegRight4.GetComponent<RotatePoint>().Speed = 1.0f;
            FrontLegRight4.GetComponent<RotatePoint>().StartRotate = FrontLegRight4.transform.localRotation.z + 2;
            FrontLegRight4.GetComponent<RotatePoint>().EndRotate = FrontLegRight4.transform.localRotation.z - 2;

            FrontLegRight3.GetComponent<RotatePoint>().Speed = -1.5f;
            FrontLegRight3.GetComponent<RotatePoint>().StartRotate = FrontLegRight3.transform.localRotation.z + 2;
            FrontLegRight3.GetComponent<RotatePoint>().EndRotate = FrontLegRight3.transform.localRotation.z - 2;

            FrontLegRight2.GetComponent<RotatePoint>().Speed = 1.0f;
            FrontLegRight2.GetComponent<RotatePoint>().StartRotate = FrontLegRight2.transform.localRotation.z + 2;
            FrontLegRight2.GetComponent<RotatePoint>().EndRotate = FrontLegRight2.transform.localRotation.z - 2;

            FrontLegRight1.GetComponent<RotatePoint>().Speed = -1.5f;
            FrontLegRight1.GetComponent<RotatePoint>().StartRotate = FrontLegRight1.transform.localRotation.z + 10;
            FrontLegRight1.GetComponent<RotatePoint>().EndRotate = FrontLegRight1.transform.localRotation.z - 10;
        }

        void Update()
        {/*
            if (Input)
            {
            Camera.GetComponent<CameraBehaviour>().FadeOverlayToBlack(true, 0.0f, 1.0f, 0.1f);
            Camera.GetComponent<CameraBehaviour>().FadeVignetteToBlack(true, 0.0f, 1.0f, 0.1f);
            }*/

            ConfineRotationDegrees();
            CheckState();

            if (swipeAttackCount >= 1)
            {
                DandBottom.GetComponent<Lerp>().Translate();
                if (Math.Abs(DandBottom.transform.localPosition.x - DandBottom.GetComponent<Lerp>().wayPoint.transform.localPosition.x) < 1)
                    DandBottom.GetComponent<BoxCollider>().active = true;
            }
            if (swipeAttackCount >= 2)
            {
                DandTop.GetComponent<Lerp>().Translate();
                if (Math.Abs(DandTop.transform.localPosition.x - DandTop.GetComponent<Lerp>().wayPoint.transform.localPosition.x) < 1)
                    DandTop.GetComponent<BoxCollider>().active = true;
            }

            if (DeathRock.transform.worldPosition.y - DamageCollider.transform.worldPosition.y < 0)
            {
                if (spiderHealth > 0)
                {
                    DeathRock.active = false;
                    Audio.PlaySource("SFX_Boss_Spider_Land");
                    Audio.PlaySource("SFX_Boss_Spider_Death");
                    spiderHealth--;
                }
            }
        }

private void CheckState()
{
    switch (spiderState)
    {
        case SpiderState.IDLE:
            if (spiderHealth <= 0)
                spiderState = SpiderState.DYING;

            attackPatternTimer -= Time.deltaTime;

            if (attackPatternTimer <= 0)
            {
                attackPatternTimer = attackPatternTime;
                
                if (!hasPerformedSquatAttack)
                {
                    spiderState = SpiderState.ATTACK_JUMP_SQUATTING;
                }
                else
                {
                    FrontLegLeft4.GetComponent<RotatePoint>().active = false;
                    FrontLegLeft3.GetComponent<RotatePoint>().active = false;
                    FrontLegLeft2.GetComponent<RotatePoint>().active = false;
                    FrontLegLeft1.GetComponent<RotatePoint>().active = false;

                    FrontLegLeft4.GetComponent<Tween>().StartTweenRotate(swipeWindupTime, new Vector3(0,0,280), -1);
                    FrontLegLeft3.GetComponent<Tween>().StartTweenRotate(swipeWindupTime, new Vector3(0,0,80),  1);
                    FrontLegLeft2.GetComponent<Tween>().StartTweenRotate(swipeWindupTime, new Vector3(0,0,130), -1);
                    FrontLegLeft1.GetComponent<Tween>().StartTweenRotate(swipeWindupTime, new Vector3(0,0,30), 1);

                    swipeAttackHoldTimer = swipeAttackWindupHoldTime;
                    spiderState = SpiderState.ATTACK_SWIPE_RAISE_LEFT;
                }

                hasPerformedSquatAttack = !hasPerformedSquatAttack;
            }
            break;

        case SpiderState.ATTACK_JUMP_SQUATTING:
            if (isSquatting == false) //Begin the squat tween
            {
                this.gameObject.GetComponent<Tween>().StartTweenTranslate(0.4f, new Vector3(this.gameObject.transform.localPosition.x, 
                                                                                            -2, 
                                                                                            this.gameObject.transform.localPosition.z));
                BackLegsLeft.GetComponent<Tween>().StartTweenRotate(0.4f, new Vector3(0, 0, 345), -1);
                BackLegsRight.GetComponent<Tween>().StartTweenRotate(0.4f, new Vector3(0, 0, 15), 1);
                isSquatting = true;
            }

            if (this.gameObject.GetComponent<Tween>().isTranslating == false && isSquatting == true) //squat has completed, Perform Jump
            {
                this.gameObject.GetComponent<RigidBody>().active = true;
                this.gameObject.GetComponent<RigidBody>().AddImpulse(new Vector2(0, jumpImpulse), new Vector2(0, 0));

                BackLegsLeft.GetComponent<Tween>().StartTweenRotate(0.15f, new Vector3(0, 0, 45), 1);
                BackLegsRight.GetComponent<Tween>().StartTweenRotate(0.15f, new Vector3(0, 0, 315), -1);
                isSquatting = false; //reset squatting flag
                spiderState = SpiderState.ATTACK_JUMP_RISING;

                Audio.PlaySource("SFX_Boss_Spider_Jump");
            }
            break;


        case SpiderState.ATTACK_JUMP_RISING:
            if (this.gameObject.GetComponent<RigidBody>().velocity.y < 0)
            {
                BackLegsLeft.GetComponent<Tween>().StartTweenRotate(1.0f, new Vector3(0, 0, 0), -1);
                BackLegsRight.GetComponent<Tween>().StartTweenRotate(1.0f, new Vector3(0, 0, 0), 1);
                spiderState = SpiderState.ATTACK_JUMP_FALLING;
            }
            break;

        case SpiderState.ATTACK_JUMP_FALLING:
            if (this.gameObject.transform.localPosition.y <= 0)
            {
                this.gameObject.GetComponent<RigidBody>().velocity = new Vector2(0,0);
                this.gameObject.GetComponent<RigidBody>().active = false;

                Random rnd = new Random();
                for (int i = 0; i < 5; ++i)
                {
                    int randomX = rnd.Next(-6, 7);   // creates a number between -6 and 6
                    GameObject fallingRock = GameObject.InstantiatePrefab("Dropping_Spike_SpiderBoss");
                    fallingRock.transform.localPosition = new Vector3(randomX, 3.5f, 0.01f);
                }
                GameObject fallingRockLeft = GameObject.InstantiatePrefab("Dropping_Spike_SpiderBoss");
                fallingRockLeft.transform.localPosition = new Vector3(-6.5f, 3.5f, 0.01f);

                GameObject fallingRockRight = GameObject.InstantiatePrefab("Dropping_Spike_SpiderBoss");
                fallingRockRight.transform.localPosition = new Vector3(6.5f, 3.5f, 0.01f);

                Camera.GetComponent<ScreenShake>().SetShake();

                if (DeathRock.transform.localPosition.y > 3.9f)
                {
                    DeathRock.transform.localPosition = new Vector3(DeathRock.transform.localPosition.x,
                                                                    DeathRock.transform.localPosition.y - 0.5f,
                                                                    DeathRock.transform.localPosition.z);
                }
                Audio.PlaySource("SFX_Boss_Spider_Land");
                spiderState = SpiderState.IDLE;
            }
            break;

        case SpiderState.ATTACK_SWIPE_RAISE_LEFT:
            if (spiderHealth <= 0)
                spiderState = SpiderState.DYING;
            
            if (!FrontLegLeft4.GetComponent<Tween>().isRotating)
            {
                swipeAttackHoldTimer -= Time.deltaTime;
                if (swipeAttackHoldTimer <= 0)
                {
                    FrontLegLeft4Collider.GetComponent<BoxCollider>().active = true;
                    FrontLegLeft3Collider.GetComponent<BoxCollider>().active = true;
                    FrontLegLeft2Collider.GetComponent<BoxCollider>().active = true;
                    FrontLegLeft1Collider.GetComponent<BoxCollider>().active = true;

                    FrontLegLeft4.GetComponent<Tween>().StartTweenRotate(swipeAttackTime, new Vector3(0,0,15), 1);
                    FrontLegLeft3.GetComponent<Tween>().StartTweenRotate(swipeAttackTime, new Vector3(0,0,200), 1);
                    FrontLegLeft2.GetComponent<Tween>().StartTweenRotate(swipeAttackTime, new Vector3(0,0,250), 1);
                    FrontLegLeft1.GetComponent<Tween>().StartTweenRotate(swipeAttackTime, new Vector3(0,0,320), 1);
                    
                    swipeAttackHoldTimer = swipeAttackHoldTime;
                    Audio.PlaySource("SFX_Boss_Spider_Swipe");
                    spiderState = SpiderState.ATTACK_SWIPE_SWEEP_LEFT;
                }
            }
            break;

        case SpiderState.ATTACK_SWIPE_SWEEP_LEFT:
            if (!FrontLegLeft4.GetComponent<Tween>().isRotating)
            {
                swipeAttackHoldTimer -= Time.deltaTime;

                if (swipeAttackHoldTimer <= 0)
                {
                    FrontLegLeft4Collider.GetComponent<BoxCollider>().active = false;
                    FrontLegLeft3Collider.GetComponent<BoxCollider>().active = false;
                    FrontLegLeft2Collider.GetComponent<BoxCollider>().active = false;
                    FrontLegLeft1Collider.GetComponent<BoxCollider>().active = false;

                    FrontLegLeft4.GetComponent<Tween>().StartTweenRotate(swipeRecoverTime, new Vector3(0,0,0), -1);
                    FrontLegLeft3.GetComponent<Tween>().StartTweenRotate(swipeRecoverTime, new Vector3(0,0,0), -1);
                    FrontLegLeft2.GetComponent<Tween>().StartTweenRotate(swipeRecoverTime, new Vector3(0,0,0), 1);
                    FrontLegLeft1.GetComponent<Tween>().StartTweenRotate(swipeRecoverTime, new Vector3(0,0,0), 1);

                    swipeAttackCount++;

                    spiderState = SpiderState.ATTACK_SWIPE_RECOVER_LEFT;
                }
            }
            break;

        case SpiderState.ATTACK_SWIPE_RECOVER_LEFT:
            if (spiderHealth <= 0)
                spiderState = SpiderState.DYING;
                
            if (!FrontLegLeft4.GetComponent<Tween>().isRotating)
            {
                FrontLegLeft4.GetComponent<RotatePoint>().active = true;
                FrontLegLeft3.GetComponent<RotatePoint>().active = true;
                FrontLegLeft2.GetComponent<RotatePoint>().active = true;
                FrontLegLeft1.GetComponent<RotatePoint>().active = true;

                spiderState = SpiderState.IDLE;
            }
            break;
        
        
        case SpiderState.DYING:
            dyingTimer -= Time.deltaTime;

            FrontLegLeft4.GetComponent<RotatePoint>().Speed *= 1.1f;
            FrontLegLeft3.GetComponent<RotatePoint>().Speed *= 1.1f;
            FrontLegLeft2.GetComponent<RotatePoint>().Speed *= 1.1f;
            FrontLegLeft1.GetComponent<RotatePoint>().Speed *= 1.1f;

            FrontLegRight4.GetComponent<RotatePoint>().Speed *= 1.1f;
            FrontLegRight3.GetComponent<RotatePoint>().Speed *= 1.1f;
            FrontLegRight2.GetComponent<RotatePoint>().Speed *= 1.1f;
            FrontLegRight1.GetComponent<RotatePoint>().Speed *= 1.1f;

            Head.GetComponent<Wobble>().freqX *= 1.1f;
            Head.GetComponent<Wobble>().freqY *= 1.1f;
            Thorax.GetComponent<Wobble>().freqX *= 1.1f;
            Thorax.GetComponent<Wobble>().freqY *= 1.1f;
            Abdomen.GetComponent<Wobble>().freqX *= 1.1f;
            Abdomen.GetComponent<Wobble>().freqY *= 1.1f;

            if (dyingTimer <= 0)
            {
                spiderState = SpiderState.DEAD;
                BackLegsLeft.GetComponent<Tween>().StartTweenRotate(2, new Vector3(0, 0, 345), -1);
                BackLegsRight.GetComponent<Tween>().StartTweenRotate(2, new Vector3(0, 0, 15), 1);
            }
            break;

        case SpiderState.DEAD:
            transitionTimer -= Time.deltaTime;
            FrontLegLeft4.GetComponent<RotatePoint>().Speed = 0;
            FrontLegLeft3.GetComponent<RotatePoint>().Speed = 0;
            FrontLegLeft2.GetComponent<RotatePoint>().Speed = 0;
            FrontLegLeft1.GetComponent<RotatePoint>().Speed = 0;

            FrontLegRight4.GetComponent<RotatePoint>().Speed = 0;
            FrontLegRight3.GetComponent<RotatePoint>().Speed = 0;
            FrontLegRight2.GetComponent<RotatePoint>().Speed = 0;
            FrontLegRight1.GetComponent<RotatePoint>().Speed = 0;

            Head.GetComponent<Wobble>().freqX = 0;
            Head.GetComponent<Wobble>().freqY = 0;
            Thorax.GetComponent<Wobble>().freqX = 0;
            Thorax.GetComponent<Wobble>().freqY = 0;
            Abdomen.GetComponent<Wobble>().freqX = 0;
            Abdomen.GetComponent<Wobble>().freqY = 0;

            this.gameObject.GetComponent<RigidBody>().active = true;
            if (transitionTimer < 0)
            {
                Camera.GetComponent<CameraBehaviour>().FadeOverlayToBlack(true, 0.0f, 1.0f, 0.3f);
                Camera.GetComponent<CameraBehaviour>().FadeVignetteToBlack(true, 0.0f, 1.0f, 0.3f);
                //transitionTimer = 20.0f;
            }
            if (Camera.GetComponent<CameraBehaviour>().GetBlackOverlayAlpha() >= 1.0f)
            {
                Audio.StopAllSource();
                Audio.masterVolume = 1.0f;
                Scene.ChangeScene("MainMenu");
            }
            break;
    };
        }

        void ConfineRotationDegrees() //Sets rotation to be between 0-359 
        {
            Vector3 vec3 = FrontLegLeft4.transform.localRotation;
            FrontLegLeft4.transform.localRotation = new Vector3(vec3.x % 360, vec3.y % 360, vec3.z % 360);
            if (FrontLegLeft4.transform.localRotation.z < 0)
                FrontLegLeft4.transform.localRotation = new Vector3(vec3.x, vec3.y, 360 + vec3.z);
            
            vec3 = FrontLegLeft3.transform.localRotation;
            FrontLegLeft3.transform.localRotation = new Vector3(vec3.x % 360, vec3.y % 360, vec3.z % 360);
            if (FrontLegLeft3.transform.localRotation.z < 0)
                FrontLegLeft3.transform.localRotation = new Vector3(vec3.x, vec3.y, 360 + vec3.z);

            vec3 = FrontLegLeft2.transform.localRotation;
            FrontLegLeft2.transform.localRotation = new Vector3(vec3.x % 360, vec3.y % 360, vec3.z % 360);
            if (FrontLegLeft2.transform.localRotation.z < 0)
                FrontLegLeft2.transform.localRotation = new Vector3(vec3.x, vec3.y, 360 + vec3.z);

            vec3 = FrontLegLeft1.transform.localRotation;
            FrontLegLeft1.transform.localRotation = new Vector3(vec3.x % 360, vec3.y % 360, vec3.z % 360);
            if (FrontLegLeft1.transform.localRotation.z < 0)
                FrontLegLeft1.transform.localRotation = new Vector3(vec3.x, vec3.y, 360 + vec3.z);

            vec3 = FrontLegRight4.transform.localRotation;
            FrontLegRight4.transform.localRotation = new Vector3(vec3.x % 360, vec3.y % 360, vec3.z % 360);
            if (FrontLegRight4.transform.localRotation.z < 0)
                FrontLegRight4.transform.localRotation = new Vector3(vec3.x, vec3.y, 360 + vec3.z);

            vec3 = FrontLegRight3.transform.localRotation;
            FrontLegRight3.transform.localRotation = new Vector3(vec3.x % 360, vec3.y % 360, vec3.z % 360);
            if (FrontLegRight3.transform.localRotation.z < 0)
                FrontLegRight3.transform.localRotation = new Vector3(vec3.x, vec3.y, 360 + vec3.z);
            
            vec3 = FrontLegRight2.transform.localRotation;
            FrontLegRight2.transform.localRotation = new Vector3(vec3.x % 360, vec3.y % 360, vec3.z % 360);
            if (FrontLegRight2.transform.localRotation.z < 0)
                FrontLegRight2.transform.localRotation = new Vector3(vec3.x, vec3.y, 360 + vec3.z);
            
            vec3 = FrontLegRight1.transform.localRotation;
            FrontLegRight1.transform.localRotation = new Vector3(vec3.x % 360, vec3.y % 360, vec3.z % 360);
            if (FrontLegRight1.transform.localRotation.z < 0)
                FrontLegRight1.transform.localRotation = new Vector3(vec3.x, vec3.y, 360 + vec3.z);
        }

    }//spider behaviour
}//lossengine
