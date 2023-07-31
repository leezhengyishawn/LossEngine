using System;
using System.Collections.Generic;
using LossScriptsTypes;
//------------------------------------------------------------------------------
//All content © 2020 DigiPen Institute of Technology Singapore. 
//All Rights Reserved
//Authors: Lee Zheng Yi Shawn
//Purpose: Control the spider boss and misc components
//------------------------------------------------------------------------------
namespace LossScripts
{
    class SpiderBoss : LossBehaviour
    {
        public enum SpiderState
        {
            START,
            START_FALL,
            START_NAMES,
            START_NAMES_WAIT,
            START_ROAR,
            START_ROAR_WAIT,

            IDLE,
            VULNERABLE,
            DAMAGED,

            RECOVERY_SQUATTING,
            RECOVERY_JUMP,
            RECOVERY_FALLING,

            ATTACK_JUMP_SQUATTING,
            ATTACK_JUMP_RISING,
            ATTACK_JUMP_FALLING,

            ATTACK_STAB_APPROACH,
            ATTACK_STAB_ATTACK,

            ATTACK_SPIT_WINDUP,
            ATTACK_SPIT_RECOVERY,

            ATTACK_SLIDE_SQUATTING,
            ATTACK_SLIDE_JUMP,
            ATTACK_SLIDE_TELL_AND_ATTACK,

            ATTACK_SWIPE_SQUATTING,
            ATTACK_SWIPE_JUMP,

            ATTACK_ROCK_TOSS_SQUATTING,
            ATTACK_ROCK_TOSS_JUMP,
            ATTACK_ROCK_TOSS_STAB,
            ATTACK_ROCK_TOSS_TELEGRAPH,
            ATTACK_ROCK_TOSS_SWIPE,

            DYING,
            DEAD
        };

        public int spiderHealth = 2;
        public SpiderState spiderState = SpiderState.START;
        public bool canDamage = false;
        public Vector3 damageImpulse = new Vector3(0f,0f,0f);
        private bool isDamaged = false;
        private bool isDead = false;
        private float attackPattern = 0.0f;
        private float attackPatternTimer = 0.0f;

        private float nameWaitIntroTimer = 2.0f;
        private float nameLingerTimer = 2.0f;       //

        private float dyingTimer = 5.0f;
        private float explodedLimbTimer = 1.0f;
        private int explodedLimbCount = 0;

        //Initial positions and rotations
        private Vector3 startPos;
        private float startPivot4;
        private float startPivot3;
        private float startPivot2;
        private float roarTimer = 1.0f;


        public GameObject frog = null;
        public GameObject camera = null;

        private float tell = 1.0f;
        private float tellTimer = 0.0f;
        private float vulnerable = 3.0f;
        private float vulnerableTimer = 0.0f;

        //Animation pivots
        public GameObject FrontLegRightPivot4 = null;
        public GameObject FrontLegRightPivot3 = null;
        public GameObject FrontLegRightPivot2 = null;
        public GameObject FrontLegRightPivot1 = null;

        public GameObject FrontLegLeftPivot4 = null;
        public GameObject FrontLegLeftPivot3 = null;
        public GameObject FrontLegLeftPivot2 = null;
        public GameObject FrontLegLeftPivot1 = null;

        public GameObject BackLegsRightPivot4 = null;
        public GameObject BackLegsRightPivot3 = null;
        public GameObject BackLegsRightPivot2 = null;
        public GameObject BackLegsRightPivotMaster = null;

        public GameObject BackLegsLeftPivot4 = null;
        public GameObject BackLegsLeftPivot3 = null;
        public GameObject BackLegsLeftPivot2 = null;
        public GameObject BackLegsLeftPivotMaster = null;

        public GameObject TeethLeftPivot = null;
        public GameObject TeethRightPivot = null;
        
        public GameObject BigDustParticleLeft = null;
        public GameObject BigDustParticleRight = null;

        public GameObject SlideDustParticleLeft = null;
        public GameObject SlideDustParticleRight = null;

        //These GOs are both the pivots and sprites
        public GameObject Head = null;
        public GameObject Thorax = null;
        public GameObject Abdomen = null;

        //Sprites
        public GameObject FrontLegRightSprite4 = null;
        public GameObject FrontLegRightSprite3 = null;
        public GameObject FrontLegRightSprite2 = null;

        public GameObject FrontLegLeftSprite4 = null;
        public GameObject FrontLegLeftSprite3 = null;
        public GameObject FrontLegLeftSprite2 = null;
        
        public GameObject BackLegsRightSprite4 = null;
        public GameObject BackLegsRightSprite3 = null;
        public GameObject BackLegsRightSprite2 = null;
        
        public GameObject BackLegsLeftSprite4 = null;
        public GameObject BackLegsLeftSprite3 = null;
        public GameObject BackLegsLeftSprite2 = null;

        public GameObject TeethLeftSprite = null;
        public GameObject TeethRightSprite = null;

        //Colliders
        public GameObject FrontLegRightCollider = null;
        public GameObject FrontLegLeftCollider = null;

        //Escape Scene GameObjects
        public GameObject EscapeFrogPos = null;
        
        public GameObject SpiderNames = null;
        public GameObject SpiderNamesFade = null;
        public GameObject CameraTarget = null;

        //General Movement variables
        private float skitterSpeed = 200.0f;
        private float bodyWobbleFreq = 1.0f;
        private float backLegRotateSpeed = 1.0f;
        
        private float teethRotateSpeed = 1.0f;
        private float tellTeethSpeed = 75.0f;

        private Vector3 prevPos = new Vector3(0,0,0);

        //Shadow variables
        public float backPos = -9.0f;
        public float frontPos = 0.0f;
        private Vector3 shadowColor = new Vector3(0.5f, 0.5f, 0.5f);

        //Spawn rock
        private int ceilingRockCount = 4;

        //Jump attack variables
        private bool isSquatting = false;
        private float squatSpeed = 0.75f;
        private float jumpImpulse = 1.0f;

        //Stab attack variables
        private bool isApproaching = false;
        private bool isStabbing = false;
        private float approachTime = 0.5f;
        private float stabTime = 0.05f;
        private float stabRockImpulse = 5.0f;
        private float retreatTime = 2.0f;
        private float stabTelegraphHoldTime = 0.3f;

        //Slide attack variables
        private bool slideAttackFromLeft = true; //Refers to where the attack is STARTING FROM
        private bool isFalling = false;
        private float timeAscending = 0.0f;
        public bool isSliding = false;
        private float slideTime = 0.0f;
        private float slideTimer = 0.0f;
        private float slideAttackPosLeft = -3.5f;
        private float slideAttackPosRight = 3.5f;
        private float slideAttackTargetLeft = 8.5f;
        private float slideAttackTargetRight = -8.5f;

        //Rock toss attack variables
        private bool rockTossAttackFromLeft = false;
        private float rockTossAttackPosLeft = -3.5f;
        private float rockTossAttackPosRight = 3.5f;
        private float rockTossTelegraphTime = 1.0f;
        private float rockTossTelegraphHoldTime = 1.0f;
        private float rockTossSwipeTime = 0.2f;
        private bool isSwiping = false;

        //General attack pattern variables
        private int stabs = 0;
        private float transitionTimer = 5.0f;
        private bool isTelegraphing = false;
        
        private int attackType = 0; //Var to track the cycling attack patterns
        public int damageLimit = 2; //How many times the boss can take damage before temporary invincibility

        private bool isCutscene = false;

        public bool isFadeIntro = false;
        void Start()
        {
            attackType = 0;
            spiderHealth = 8;
            damageLimit = 2;

            spiderState = SpiderState.START;
            canDamage = false;
            isDamaged = false;
            skitterSpeed = 200.0f;
            bodyWobbleFreq = 1.0f;
            backLegRotateSpeed = 1.0f;
            teethRotateSpeed = 1.0f;
            tellTeethSpeed = 75.0f;

            attackPattern = 2.0f;
            attackPatternTimer = 0.0f;

            explodedLimbCount = 0;

            nameWaitIntroTimer = 2.0f;
            nameLingerTimer = 2.0f;

            //Initial positions and rotations
            startPos = new Vector3(0.0f, 
                                   2.25f, 
                                   this.gameObject.transform.localPosition.z);
            prevPos = new Vector3(0,0,0);
            startPivot4 = FrontLegRightPivot4.transform.localRotation.z;
            startPivot3 = FrontLegRightPivot4.transform.localRotation.z;
            startPivot2 = FrontLegRightPivot4.transform.localRotation.z;

            tell = 2.0f;
            tellTimer = 0.0f;
            vulnerable = 3.5f;
            vulnerableTimer = 0.0f;

            //Shadow variables
            backPos = -9.0f;
            frontPos = 0.0f;
            shadowColor = new Vector3(0.15f, 0.15f, 0.15f);

            ceilingRockCount = 4;

            //Jump attack variables
            isSquatting = false;
            squatSpeed = 0.75f;
            jumpImpulse = 125.0f;

            //Stab attack variables
            isApproaching = false;
            isStabbing = false;
            approachTime = 0.4f;
            stabTime = 0.2f;
            stabRockImpulse = 25.0f;
            retreatTime = 2.0f;

            //Slide attack variables
            slideAttackFromLeft = true; //If false, attacking right
            isFalling = false;
            timeAscending = 0.0f;
            isSliding = false;
            slideTime = 3.75f;
            slideTimer = 0.0f;
            slideAttackPosLeft = -3.5f;
            slideAttackPosRight = 3.5f;
            slideAttackTargetLeft = 9.5f;
            slideAttackTargetRight = -9.5f;

            //General attack pattern variables
            rockTossAttackFromLeft = false;
            rockTossAttackPosLeft = -3.5f;
            rockTossAttackPosRight = 3.5f;
            rockTossTelegraphTime = 1.5f;
            rockTossSwipeTime = 0.1f;
            isSwiping = false;

            //Transition to fade out black
            transitionTimer = 3.0f;

            //Turn on particles
            BigDustParticleLeft.GetComponent<ParticleEmitter>().active = true;
            BigDustParticleRight.GetComponent<ParticleEmitter>().active = true;
            SlideDustParticleLeft.GetComponent<ParticleEmitter>().active = true;
            SlideDustParticleRight.GetComponent<ParticleEmitter>().active = true;
            
            ToggleFrontLegColliders(false);
            InitBodyWobble();
            InitFrontLegsRotate();
            InitBackLegsRotate();
        }


        void Update()
        {
            UpdateColor();

            UpdateBackLegsRotate();

            LerpCamera(CameraTarget.transform.localPosition);

            UpdateState();

            //Update the z
            if (this.gameObject.transform.localPosition.x < -3.5 || this.gameObject.transform.localPosition.x > 3.5)
                this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x,
                                                                      this.gameObject.transform.localPosition.y,
                                                                      -0.1f);
            //targetX = -3.5f;
            //        else if (targetX > 3.5f)
            //    targetX = 3.5f;

            if (Input.GetKey(KEYCODE.KEY_1))
                spiderState = SpiderState.ATTACK_ROCK_TOSS_SQUATTING;
            if (Input.GetKey(KEYCODE.KEY_2))
                spiderState = SpiderState.ATTACK_STAB_APPROACH;
            if (Input.GetKey(KEYCODE.KEY_3))
                spiderState = SpiderState.ATTACK_SLIDE_SQUATTING;

            if (Input.GetKey(KEYCODE.KEY_0))
                spiderState = SpiderState.DYING;
        } //Update



        void UpdateState()
        {
            if (spiderState == SpiderState.START)
            {
                canDamage = false;
            }
            else if (spiderState == SpiderState.START_FALL)
            {
                isCutscene = true;
                if (this.gameObject.transform.localPosition.y <= startPos.y)
                {
                    SpiderLand();
                    this.gameObject.GetComponent<RigidBody>().active = false;
                    if (isFadeIntro)
                        SpiderNamesFade.GetComponent<SpiderNamesFade>().StartAnimation();
                    else
                        SpiderNames.GetComponent<SpiderNames>().StartAnimation();
                    spiderState = SpiderState.START_NAMES;
                }
            }
            else if (spiderState == SpiderState.START_NAMES)
            {
                if (isFadeIntro)
                {
                    if (SpiderNamesFade.GetComponent<SpiderNamesFade>().name.GetComponent<SpriteRenderer>().a >= 1.0f)
                    {
                        spiderState = SpiderState.START_ROAR;
                        Audio.PlaySource("SFX_Boss_Spider_Land");
                    }
                }
                else
                {
                    if (SpiderNames.GetComponent<SpiderNames>().isAnimating == false)
                    {
                        spiderState = SpiderState.START_ROAR;
                        SpiderLand();
                        Audio.PlaySource("SFX_Boss_Spider_Land");
                    }
                }
            }
            else if (spiderState == SpiderState.START_NAMES_WAIT)
            {

            }
            else if (spiderState == SpiderState.START_ROAR)
            {
                nameLingerTimer -= Time.deltaTime;
                if (nameLingerTimer < 0)
                {
                    if (isFadeIntro)
                        SpiderNamesFade.GetComponent<SpiderNamesFade>().EndAnimation();
                    else
                        SpiderNames.GetComponent<SpiderNames>().EndAnimation();

                }
                roarTimer -= Time.deltaTime;
                if (roarTimer < 0)
                {
                    Audio.PlaySource("SFX_Boss_Spider_Roar");
                    camera.GetComponent<ScreenShake>().SetShake(3.0f, 1.0f, 0.2f, 0.2f);
                    roarTimer = 1.0f;
                    spiderState = SpiderState.START_ROAR_WAIT;
                    isCutscene = false;
                }
            }
            else if (spiderState == SpiderState.START_ROAR_WAIT)
            {
                nameLingerTimer -= Time.deltaTime;
                if (nameLingerTimer < 0)
                {
                    if (isFadeIntro)
                        SpiderNamesFade.GetComponent<SpiderNamesFade>().EndAnimation();
                    else
                        SpiderNames.GetComponent<SpiderNames>().EndAnimation();
                }
                roarTimer -= Time.deltaTime;
                if (roarTimer < 0)
                {
                    camera.GetComponent<CameraBehaviour>().SetBlackBarSpeed(0.2f);
                    camera.GetComponent<CameraBehaviour>().SetCutscene(false);
                    camera.GetComponent<CameraBehaviour>().SetDeadLock(false);
                    frog.GetComponent<PlayerBehaviour>().SetCanControl(true);
                    Audio.PlaySource("BGM_BossSpider");
                    spiderState = SpiderState.IDLE;
                }
            }
            else if (spiderState == SpiderState.IDLE)
            {
                canDamage = false;
                ToggleFrontLegColliders(false);
                attackPatternTimer += Time.deltaTime;
                if (attackPatternTimer >= attackPattern)
                {
                    if (attackType == 0)
                        spiderState = SpiderState.ATTACK_ROCK_TOSS_SQUATTING;
                    else if (attackType == 1)
                        spiderState = SpiderState.ATTACK_STAB_APPROACH;
                    else if (attackType == 2)
                        spiderState = SpiderState.ATTACK_SLIDE_SQUATTING;

                    if (++attackType > 2)
                        attackType = 0;

                    attackPatternTimer = 0.0f;
                }
            }
            else if (spiderState == SpiderState.VULNERABLE)
            {
                vulnerableTimer += Time.deltaTime;
                if (vulnerableTimer >= vulnerable)
                {
                    vulnerableTimer = 0.0f;
                    spiderState = SpiderState.RECOVERY_SQUATTING;
                }
            }
            else if (spiderState == SpiderState.DAMAGED)
            {
                ToggleFrontLegColliders(false);
                isSquatting = isSliding = isStabbing = isApproaching = isFalling = isTelegraphing = isSwiping = false; //Set all possible flags to false
                rockTossTelegraphHoldTime = 1.0f;
                if (isDamaged == false) //Send the spider flying
                {
                    camera.GetComponent<CameraBehaviour>().SetShake(-1.0f, 0f, 0f, 0f);
                    this.gameObject.GetComponent<Tween>().isTranslating = false;
                    this.gameObject.GetComponent<Tween>().isRotating = false;


                    this.gameObject.GetComponent<RigidBody>().active = true;

                    if (spiderHealth > 0)
                        this.gameObject.GetComponent<RigidBody>().AddImpulse(new Vector2(damageImpulse.x, damageImpulse.y), new Vector2(0, 0));

                    isDamaged = true;
                    Audio.PlaySource("SFX_Boss_Spider_Hurt");

                }

                DamageShake();

                if (this.gameObject.GetComponent<RigidBody>().velocity.y < 0 && isDamaged) //Check is descending
                {
                    if (this.gameObject.transform.localPosition.y <= startPos.y) //Once landed
                    {
                        SpiderLand();
                        isDamaged = false;
                        spiderState = SpiderState.RECOVERY_SQUATTING;
                    }
                }

                if (spiderHealth <= 0)
                {
                    Audio.PlaySource("SFX_Boss_Spider_Death");
                    spiderState = SpiderState.DYING;
                }
            }
            else if (spiderState == SpiderState.RECOVERY_SQUATTING)
            {
                SpiderSquat(jumpImpulse * 1.8f, SpiderState.RECOVERY_JUMP);
            }
            else if (spiderState == SpiderState.RECOVERY_JUMP)
            {
                canDamage = false;
                ToggleFrontLegColliders(false);
                Head.GetComponent<SpiderHead>().SetEyes(true, new Vector3(0f, 0f, 0f));

                //Out of camera frame
                if (this.gameObject.GetComponent<RigidBody>().velocity.y < 0)
                {
                    //Teleport spider backwards
                    this.gameObject.transform.localPosition = new Vector3(startPos.x, this.gameObject.transform.localPosition.y, startPos.z);
                    ResetSpider();
                    //Return back legs to normal
                    BackLegsLeftPivotMaster.GetComponent<Tween>().StartTweenRotate(0.2f, new Vector3(0, 0, 0), -1);
                    BackLegsRightPivotMaster.GetComponent<Tween>().StartTweenRotate(0.2f, new Vector3(0, 0, 0), 1);
                    isFalling = false;
                    ToggleFrontLegColliders(false);
                    spiderState = SpiderState.RECOVERY_FALLING;

                }
            }
            else if (spiderState == SpiderState.RECOVERY_FALLING)
            {
                ToggleFrontLegColliders(false);
                if (this.gameObject.transform.localPosition.y <= startPos.y)
                {
                    SpiderLand();
                    ToggleFrontLegColliders(false);
                    SpawnCeilingRocks();
                    spiderState = SpiderState.IDLE;
                    damageLimit = 2;
                }
            }


            ////////////SQUAT ATTACK///////////////////
            else if (spiderState == SpiderState.ATTACK_JUMP_SQUATTING)
            {
                SpiderSquat(jumpImpulse, SpiderState.ATTACK_JUMP_SQUATTING);
            }
            else if (spiderState == SpiderState.ATTACK_JUMP_RISING)
            {
                //If starting to fall, transition to falling state
                if (this.gameObject.GetComponent<RigidBody>().velocity.y < 0)
                {
                    //Return back legs to normal
                    BackLegsLeftPivotMaster.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, 0), -1);
                    BackLegsRightPivotMaster.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, 0), 1);
                    spiderState = SpiderState.ATTACK_JUMP_FALLING;
                }
            }
            else if (spiderState == SpiderState.ATTACK_JUMP_FALLING)
            {
                if (this.gameObject.transform.localPosition.y <= startPos.y)
                {
                    SpiderLand();
                    SpawnCeilingRocks();
                    spiderState = SpiderState.IDLE;
                }
            }


            ////////////////STAB ATTACK////////////////
            else if (spiderState == SpiderState.ATTACK_STAB_APPROACH)
            {
                ToggleFrontLegColliders(false);
                if (isApproaching == false)
                {
                    //Bind the x value so the front legs stay within bounds
                    float targetX = frog.transform.localPosition.x;
                    if (targetX < -3.5f)
                        targetX = -3.5f;
                    else if (targetX > 3.5f)
                        targetX = 3.5f;

                    this.gameObject.GetComponent<Tween>().StartTweenTranslate(approachTime,
                                                                              new Vector3(targetX,
                                                                                          this.gameObject.transform.localPosition.y,
                                                                                          frontPos));

                    ToggleFrontLegsIdleRotate(false);

                    FrontLegRightPivot4.GetComponent<Tween>().StartTweenRotate(approachTime * 0.5f, new Vector3(0, 0, 120), 1);
                    FrontLegRightPivot3.GetComponent<Tween>().StartTweenRotate(approachTime * 0.5f, new Vector3(0, 0, -90), -1);
                    FrontLegRightPivot2.GetComponent<Tween>().StartTweenRotate(approachTime * 0.5f, new Vector3(0, 0, -135), 1);

                    FrontLegLeftPivot4.GetComponent<Tween>().StartTweenRotate(approachTime * 0.5f, new Vector3(0, 0, -120), -1);
                    FrontLegLeftPivot3.GetComponent<Tween>().StartTweenRotate(approachTime * 0.5f, new Vector3(0, 0, 90), 1);
                    FrontLegLeftPivot2.GetComponent<Tween>().StartTweenRotate(approachTime * 0.5f, new Vector3(0, 0, 135), -1);
                    isApproaching = true;
                }
                //Condition to move on to next state
                if (this.gameObject.GetComponent<Tween>().isTranslating == false && isApproaching == true)
                {
                    spiderState = SpiderState.ATTACK_STAB_ATTACK;
                    isApproaching = false;
                }
            }
            else if (spiderState == SpiderState.ATTACK_STAB_ATTACK)
            {
                canDamage = true;
                ToggleFrontLegColliders(true);

                stabTelegraphHoldTime -= Time.deltaTime;

                if (isStabbing == false && stabTelegraphHoldTime < 0.0f)
                {
                    FrontLegRightPivot4.GetComponent<Tween>().StartTweenRotate(stabTime, new Vector3(0, 0, 80.0f - (float)(stabs * 7.0f)), -1);
                    FrontLegRightPivot3.GetComponent<Tween>().StartTweenRotate(stabTime, new Vector3(0, 0, -60.0f - (float)(stabs * 2.5f)), 1);
                    FrontLegRightPivot2.GetComponent<Tween>().StartTweenRotate(stabTime * 0.75f, new Vector3(0, 0, 0.0f - (float)(stabs * 7.5f)), -1);

                    FrontLegLeftPivot4.GetComponent<Tween>().StartTweenRotate(stabTime, new Vector3(0, 0, -80.0f + (float)(stabs * 7.0f)), 1);
                    FrontLegLeftPivot3.GetComponent<Tween>().StartTweenRotate(stabTime, new Vector3(0, 0, 60.0f + (float)(stabs * 2.5f)), -1);
                    FrontLegLeftPivot2.GetComponent<Tween>().StartTweenRotate(stabTime * 0.75f, new Vector3(0, 0, 0.0f + (float)(stabs * 7.5f)), 1);
                    Audio.PlaySource("SFX_Boss_Spider_Swipe");
                    isStabbing = true;
                    if (++stabs >= 3)
                        stabs = 0;
                }

                //Condition to move on to next state
                if (FrontLegLeftPivot4.GetComponent<Tween>().isRotating == false && isStabbing == true)
                {
                    isStabbing = false;
                    spiderState = SpiderState.VULNERABLE;
                    stabTelegraphHoldTime = 0.3f;

                    BigDustParticleLeft.GetComponent<ParticleEmitter>().Restart();
                    BigDustParticleRight.GetComponent<ParticleEmitter>().Restart();

                    camera.GetComponent<ScreenShake>().SetShake(0.2f, 3.0f, 0.2f, 0.2f);
                    Audio.PlaySource("SFX_Boss_Spider_Land");

                    //Spawn rocks
                    Random rnd = new Random();
                    for (int i = 0; i < 2; ++i)
                    {
                        GameObject fallingRock = GameObject.InstantiatePrefab("Debris_Spike_SpiderBoss");
                        fallingRock.GetComponent<Breakable>().particleEffectName = "DustBoss";
                        if (i < 1)
                        {
                            //Spawn rocks from left leg
                            fallingRock.transform.localPosition = new Vector3(FrontLegRightPivot1.transform.worldPosition.x,
                                                                              FrontLegRightPivot1.transform.worldPosition.y + 0.3f,
                                                                              0.2f);
                            float xComp = (float)rnd.NextDouble() * -1.0f;
                            float yComp = (float)Math.Sqrt(1.0f - (xComp * xComp));
                            fallingRock.GetComponent<RigidBody>().AddImpulse(new Vector2(xComp * stabRockImpulse,
                                                                                         yComp * stabRockImpulse),
                                                                             new Vector2(0, 0));
                            fallingRock.GetComponent<Breakable>().sfx = "SFX_RockDrop";
                        }
                        else
                        {
                            //Spawn rocks from right leg
                            fallingRock.transform.localPosition = new Vector3(FrontLegLeftPivot1.transform.worldPosition.x,
                                                                              FrontLegLeftPivot1.transform.worldPosition.y + 0.3f,
                                                                              0.2f);
                            float xComp = (float)rnd.NextDouble();
                            float yComp = (float)Math.Sqrt(1.0f - (xComp * xComp));
                            fallingRock.GetComponent<RigidBody>().AddImpulse(new Vector2(xComp * stabRockImpulse,
                                                                                         yComp * stabRockImpulse),
                                                                             new Vector2(0, 0));
                            fallingRock.GetComponent<Breakable>().sfx = "SFX_RockDrop";
                        }
                    }
                }
            }

            ////////////////SPIT ATTACK////////////////
            else if (spiderState == SpiderState.ATTACK_SPIT_WINDUP)
            {

            }
            else if (spiderState == SpiderState.ATTACK_SPIT_RECOVERY)
            {

            }

            ////////////////SLIDE ATTACK////////////////
            else if (spiderState == SpiderState.ATTACK_SLIDE_SQUATTING)
            {
                ToggleFrontLegColliders(false);
                if (SpiderSquat(jumpImpulse * 1.8f, SpiderState.ATTACK_SLIDE_JUMP))
                    slideAttackFromLeft = !slideAttackFromLeft;
            }
            else if (spiderState == SpiderState.ATTACK_SLIDE_JUMP)
            {
                if (this.gameObject.GetComponent<RigidBody>().velocity.y < 0 && isFalling == false)
                {
                    canDamage = true;

                    ToggleFrontLegsIdleRotate(false);

                    BackLegsLeftPivotMaster.GetComponent<Tween>().StartTweenRotate(timeAscending, new Vector3(0, 0, 0), -1);
                    BackLegsRightPivotMaster.GetComponent<Tween>().StartTweenRotate(timeAscending, new Vector3(0, 0, 0), 1);

                    if (slideAttackFromLeft == true)
                    {
                        this.gameObject.transform.localPosition = new Vector3(slideAttackPosLeft,
                                                                              this.gameObject.transform.localPosition.y,
                                                                              frontPos);

                        FrontLegLeftPivot4.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, -30), -1);
                        FrontLegLeftPivot3.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, 0), 1);
                        FrontLegLeftPivot2.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, 0), 1);

                        FrontLegRightPivot4.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, 90), 1);
                        FrontLegRightPivot3.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, -60), -1);
                        FrontLegRightPivot2.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, 60), 1);

                        Random rnd = new Random();
                    }
                    else
                    {
                        this.gameObject.transform.localPosition = new Vector3(slideAttackPosRight,
                                                                              this.gameObject.transform.localPosition.y,
                                                                              frontPos);

                        FrontLegRightPivot4.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, 30), -1);
                        FrontLegRightPivot3.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, 0), 1);
                        FrontLegRightPivot2.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, 0), -1);

                        FrontLegLeftPivot4.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, -90), 1);
                        FrontLegLeftPivot3.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, 60), -1);
                        FrontLegLeftPivot2.GetComponent<Tween>().StartTweenRotate(0.5f, new Vector3(0, 0, -60), 1);
                    }
                    //timeAscending = 0.0f;
                    SpawnSlideAttackRocks(slideAttackFromLeft);
                    isFalling = true;
                }

                if (this.gameObject.transform.localPosition.y <= startPos.y && isFalling == true)
                {
                    camera.GetComponent<ScreenShake>().SetShake(0.3f, 4.0f, 0.5f, 0.5f);
                    SpiderLand();
                    ToggleFrontLegColliders(true);
                    isFalling = false;
                    canDamage = true;
                    spiderState = SpiderState.ATTACK_SLIDE_TELL_AND_ATTACK;
                }

            }
            else if (spiderState == SpiderState.ATTACK_SLIDE_TELL_AND_ATTACK)
            {
                if (isSliding == false)
                    tellTimer += Time.deltaTime;

                if (tellTimer > tell * 0.25f && isSliding == false) //Make eyes look in direction
                {
                    if (slideAttackFromLeft)
                        Head.GetComponent<SpiderHead>().SetEyes(false, new Vector3(Head.transform.worldPosition.x + 500.0f,
                                                                                   Head.transform.worldPosition.y,
                                                                                   Head.transform.worldPosition.z));
                    else
                        Head.GetComponent<SpiderHead>().SetEyes(false, new Vector3(Head.transform.worldPosition.x - 500.0f,
                                                                                   Head.transform.worldPosition.y,
                                                                                   Head.transform.worldPosition.z));
                    SpiderMouthTwitch(true);
                    teethRotateSpeed = tellTeethSpeed;
                    TeethRightPivot.GetComponent<RotatePoint>().Speed = -teethRotateSpeed;
                    TeethLeftPivot.GetComponent<RotatePoint>().Speed = teethRotateSpeed;

                }

                if (tellTimer >= tell && isSliding == false)
                {
                    tellTimer = 0.0f;
                    isSliding = true;
                    //SpiderMouthTwitch(false);
                    if (slideAttackFromLeft)
                        this.gameObject.GetComponent<Tween>().StartTweenTranslate(slideTime, new Vector3(slideAttackTargetLeft,
                                                                                                        this.gameObject.transform.localPosition.y,
                                                                                                        this.gameObject.transform.localPosition.z));
                    else
                        this.gameObject.GetComponent<Tween>().StartTweenTranslate(slideTime, new Vector3(slideAttackTargetRight,
                                                                                                        this.gameObject.transform.localPosition.y,
                                                                                                        this.gameObject.transform.localPosition.z));
                    teethRotateSpeed = 1.0f;
                    TeethRightPivot.GetComponent<RotatePoint>().Speed = -teethRotateSpeed;
                    TeethLeftPivot.GetComponent<RotatePoint>().Speed = teethRotateSpeed;
                    camera.GetComponent<CameraBehaviour>().SetShake(slideTime - 0.25f, 0.5f, 0.15f, 0.15f);
                }
                if (isSliding == true)
                {
                    if (slideAttackFromLeft)
                        SlideDustParticleLeft.GetComponent<ParticleEmitter>().Play();
                    else
                        SlideDustParticleRight.GetComponent<ParticleEmitter>().Play();

                }
                if (this.gameObject.GetComponent<Tween>().isTranslating == false && isSliding == true)
                {
                    tellTimer = 0.0f;
                    isSliding = false;
                    Head.GetComponent<SpiderHead>().SetEyes(true, new Vector3(0f, 0f, 0f));
                    //slideAttackFromLeft = !slideAttackFromLeft;
                    spiderState = SpiderState.VULNERABLE;
                }

            }
            else if (spiderState == SpiderState.ATTACK_ROCK_TOSS_SQUATTING)
            {
                SpiderSquat(jumpImpulse * 1.8f, SpiderState.ATTACK_ROCK_TOSS_JUMP);
            }
            else if (spiderState == SpiderState.ATTACK_ROCK_TOSS_JUMP)
            {
                if (this.gameObject.GetComponent<RigidBody>().velocity.y < 0 && isFalling == false)
                {
                    isFalling = true;
                    ToggleFrontLegsIdleRotate(false);

                    //Return back legs to normal
                    BackLegsLeftPivotMaster.GetComponent<Tween>().StartTweenRotate(0.2f, new Vector3(0, 0, 0), -1);
                    BackLegsRightPivotMaster.GetComponent<Tween>().StartTweenRotate(0.2f, new Vector3(0, 0, 0), 1);

                    //Raise the claws
                    FrontLegRightPivot4.GetComponent<Tween>().StartTweenRotate(approachTime, new Vector3(0, 0, 120), 1);
                    FrontLegRightPivot3.GetComponent<Tween>().StartTweenRotate(approachTime, new Vector3(0, 0, -90), -1);
                    FrontLegRightPivot2.GetComponent<Tween>().StartTweenRotate(approachTime, new Vector3(0, 0, -135), 1);

                    FrontLegLeftPivot4.GetComponent<Tween>().StartTweenRotate(approachTime, new Vector3(0, 0, -120), -1);
                    FrontLegLeftPivot3.GetComponent<Tween>().StartTweenRotate(approachTime, new Vector3(0, 0, 90), 1);
                    FrontLegLeftPivot2.GetComponent<Tween>().StartTweenRotate(approachTime, new Vector3(0, 0, 135), -1);

                    if (rockTossAttackFromLeft == true)
                    {
                        this.gameObject.transform.localPosition = new Vector3(rockTossAttackPosLeft,
                                                                              this.gameObject.transform.localPosition.y,
                                                                              frontPos);
                    }
                    else
                    {
                        this.gameObject.transform.localPosition = new Vector3(rockTossAttackPosRight,
                                                                              this.gameObject.transform.localPosition.y,
                                                                              frontPos);
                    }
                }
                if (this.gameObject.transform.localPosition.y <= startPos.y && isFalling == true)
                {
                    SpiderLand();
                    spiderState = SpiderState.ATTACK_ROCK_TOSS_STAB;
                }
            }
            else if (spiderState == SpiderState.ATTACK_ROCK_TOSS_STAB)
            {
                canDamage = true;
                ToggleFrontLegColliders(true);

                if (isStabbing == false)
                    stabTelegraphHoldTime -= Time.deltaTime;

                if (isStabbing == false && stabTelegraphHoldTime < 0.0f)
                {
                    FrontLegRightPivot4.GetComponent<Tween>().StartTweenRotate(stabTime, new Vector3(0, 0, 80.0f - (float)(2.0f * 7.5f)), -1);
                    FrontLegRightPivot3.GetComponent<Tween>().StartTweenRotate(stabTime, new Vector3(0, 0, -60.0f - (float)(2.0f * 2.5f)), 1);
                    FrontLegRightPivot2.GetComponent<Tween>().StartTweenRotate(stabTime * 0.75f, new Vector3(0, 0, 0.0f - (float)(2.0f * 7.5f)), -1);

                    FrontLegLeftPivot4.GetComponent<Tween>().StartTweenRotate(stabTime, new Vector3(0, 0, -80.0f + (float)(2.0f * 7.5f)), 1);
                    FrontLegLeftPivot3.GetComponent<Tween>().StartTweenRotate(stabTime, new Vector3(0, 0, 60.0f + (float)(2.0f * 2.5f)), -1);
                    FrontLegLeftPivot2.GetComponent<Tween>().StartTweenRotate(stabTime * 0.75f, new Vector3(0, 0, 0.0f + (float)(2.0f * 7.5f)), 1);
                    Audio.PlaySource("SFX_Boss_Spider_Swipe");
                    isStabbing = true;
                }

                //Condition to move on to next state
                if (FrontLegLeftPivot4.GetComponent<Tween>().isRotating == false && isStabbing == true)
                {
                    isStabbing = false;
                    stabTelegraphHoldTime = 0.3f;
                    spiderState = SpiderState.ATTACK_ROCK_TOSS_TELEGRAPH;
                    BigDustParticleLeft.GetComponent<ParticleEmitter>().Restart();
                    BigDustParticleRight.GetComponent<ParticleEmitter>().Restart();

                    ToggleFrontLegColliders(false); //Turn off collider for windup

                    camera.GetComponent<ScreenShake>().SetShake(0.2f, 3.0f, 0.2f, 0.2f);
                    Audio.PlaySource("SFX_Boss_Spider_Land");
                }
            }
            else if (spiderState == SpiderState.ATTACK_ROCK_TOSS_TELEGRAPH)
            {
                if (rockTossAttackFromLeft == true)
                {
                    if (isTelegraphing == false) //Begin telegraph rotation
                    {
                        FrontLegRightPivot4.GetComponent<Tween>().StartTweenRotate(rockTossTelegraphTime, new Vector3(0, 0, -45), -1);
                        FrontLegRightPivot3.GetComponent<Tween>().StartTweenRotate(rockTossTelegraphTime, new Vector3(0, 0, -160), -1);
                        FrontLegRightPivot2.GetComponent<Tween>().StartTweenRotate(rockTossTelegraphTime, new Vector3(0, 0, 45), 1);
                        isTelegraphing = true;
                    }

                    if (FrontLegRightPivot2.GetComponent<Tween>().isRotating == false) //Hold for sec
                        rockTossTelegraphHoldTime -= Time.deltaTime;

                    if (isTelegraphing == true && rockTossTelegraphHoldTime < 0.0f) //Swipe Down
                    {
                        isTelegraphing = false;
                        isSwiping = true;
                        ToggleFrontLegColliders(true);
                        FrontLegRightPivot4.GetComponent<Tween>().StartTweenRotate(rockTossSwipeTime, new Vector3(0, 0, 90), 1);
                        FrontLegRightPivot3.GetComponent<Tween>().StartTweenRotate(rockTossSwipeTime * 1.25f, new Vector3(0, 0, -60), 1);
                        FrontLegRightPivot2.GetComponent<Tween>().StartTweenRotate(rockTossSwipeTime * 1.50f, new Vector3(0, 0, 90), -1);
                        rockTossTelegraphHoldTime = 1.0f;
                        Audio.PlaySource("SFX_Boss_Spider_Swipe");
                        spiderState = SpiderState.ATTACK_ROCK_TOSS_SWIPE;
                    }
                }
                else //attack from right
                {
                    if (isTelegraphing == false) //Begin telegraph rotation
                    {
                        FrontLegLeftPivot4.GetComponent<Tween>().StartTweenRotate(rockTossTelegraphTime, new Vector3(0, 0, 45), 1);
                        FrontLegLeftPivot3.GetComponent<Tween>().StartTweenRotate(rockTossTelegraphTime, new Vector3(0, 0, 160), 1);
                        FrontLegLeftPivot2.GetComponent<Tween>().StartTweenRotate(rockTossTelegraphTime, new Vector3(0, 0, -45), -1);
                        isTelegraphing = true;
                    }

                    if (FrontLegLeftPivot2.GetComponent<Tween>().isRotating == false) //Hold for sec
                        rockTossTelegraphHoldTime -= Time.deltaTime;

                    if (FrontLegLeftPivot2.GetComponent<Tween>().isRotating == false && isTelegraphing == true && rockTossTelegraphHoldTime < 0.0f) //Swipe Down
                    {
                        isTelegraphing = false;
                        isSwiping = true;
                        ToggleFrontLegColliders(true);
                        FrontLegLeftPivot4.GetComponent<Tween>().StartTweenRotate(rockTossSwipeTime, new Vector3(0, 0, -90), -1);
                        FrontLegLeftPivot3.GetComponent<Tween>().StartTweenRotate(rockTossSwipeTime * 1.25f, new Vector3(0, 0, 60), -1);
                        FrontLegLeftPivot2.GetComponent<Tween>().StartTweenRotate(rockTossSwipeTime * 1.50f, new Vector3(0, 0, -90), -1);
                        rockTossTelegraphHoldTime = 1.0f;
                        Audio.PlaySource("SFX_Boss_Spider_Swipe");
                        spiderState = SpiderState.ATTACK_ROCK_TOSS_SWIPE;
                    }
                }

            }
            else if (spiderState == SpiderState.ATTACK_ROCK_TOSS_SWIPE)
            {
                if (rockTossAttackFromLeft == true)
                {
                    if (FrontLegRightPivot2.GetComponent<Tween>().isRotating == false && isSwiping == true)
                    {
                        spiderState = SpiderState.VULNERABLE;
                        rockTossAttackFromLeft = !rockTossAttackFromLeft;
                    }
                }
                else
                {
                    if (FrontLegLeftPivot2.GetComponent<Tween>().isRotating == false && isSwiping == true)
                    {
                        spiderState = SpiderState.VULNERABLE;
                        rockTossAttackFromLeft = !rockTossAttackFromLeft;
                    }
                }
            }
            else if (spiderState == SpiderState.DYING)
            {
                //Start slow-mo
                if (isDead == false)
                {
                    ToggleFrontLegsIdleRotate(true);
                    this.gameObject.GetComponent<RigidBody>().active = false;

                    camera.GetComponent<CameraBehaviour>().SetCutscene(true);
                    camera.GetComponent<CameraBehaviour>().SetDeadLock(true);
                    frog.GetComponent<PlayerBehaviour>().SetCanControl(false);
                    //camera.GetComponent<ScreenShake>().SetShake(dyingTimer - 0.3f, 2.2f, 0.3f, 0.3f);
                    frog.GetComponent<PlayerBehaviour>().HealthFullRestore();
                    isCutscene = true;
                    isDead = true;
                }
                dyingTimer -= Time.deltaTime;
                explodedLimbTimer -= Time.deltaTime;
                if (explodedLimbTimer < 0.0f)
                {
                    ++explodedLimbCount;
                    if (explodedLimbCount == 1)
                    {
                        FrontLegLeftPivot2.GetComponent<RotatePoint>().active = false;
                        ExplodeLimb(FrontLegLeftSprite2);
                        ExplodeBloodSplatter(FrontLegLeftPivot2);
                        explodedLimbTimer = 0.75f;
                    }
                    else if (explodedLimbCount == 2)
                    {
                        FrontLegRightPivot2.GetComponent<RotatePoint>().active = false;
                        ExplodeLimb(FrontLegRightSprite2);
                        ExplodeBloodSplatter(FrontLegRightPivot2);
                        explodedLimbTimer = 0.75f;
                    }
                    else if (explodedLimbCount == 3)
                    {
                        FrontLegLeftPivot3.GetComponent<RotatePoint>().active = false;
                        FrontLegRightPivot3.GetComponent<RotatePoint>().active = false;

                        ExplodeLimb(FrontLegLeftSprite3);
                        ExplodeLimb(FrontLegRightSprite3);
                        ExplodeBloodSplatter(FrontLegLeftPivot3);
                        ExplodeBloodSplatter(FrontLegRightPivot3);
                        explodedLimbTimer = 0.5f;
                    }
                    else if (explodedLimbCount == 4)
                    {
                        FrontLegLeftPivot4.GetComponent<RotatePoint>().active = false;
                        FrontLegRightPivot4.GetComponent<RotatePoint>().active = false;

                        ExplodeLimb(FrontLegLeftSprite4);
                        ExplodeLimb(FrontLegRightSprite4);
                        ExplodeBloodSplatter(FrontLegLeftPivot4);
                        ExplodeBloodSplatter(FrontLegRightPivot4);
                    }
                }

                if (FrontLegLeftPivot4.GetComponent<RotatePoint>().Speed < 50.0f)
                    FrontLegLeftPivot4.GetComponent<RotatePoint>().Speed *= 1.01f;
                if (FrontLegLeftPivot3.GetComponent<RotatePoint>().Speed < 50.0f)
                    FrontLegLeftPivot3.GetComponent<RotatePoint>().Speed *= 1.01f;
                if (FrontLegLeftPivot2.GetComponent<RotatePoint>().Speed < 50.0f)
                    FrontLegLeftPivot2.GetComponent<RotatePoint>().Speed *= 1.01f;

                if (FrontLegRightPivot4.GetComponent<RotatePoint>().Speed < 50.0f)
                    FrontLegRightPivot4.GetComponent<RotatePoint>().Speed *= 1.01f;
                if (FrontLegRightPivot3.GetComponent<RotatePoint>().Speed < 50.0f)
                    FrontLegRightPivot3.GetComponent<RotatePoint>().Speed *= 1.01f;
                if (FrontLegRightPivot2.GetComponent<RotatePoint>().Speed < 50.0f)
                    FrontLegRightPivot2.GetComponent<RotatePoint>().Speed *= 1.01f;

                if (Head.GetComponent<Wobble>().freqX < 90.0f)
                    Head.GetComponent<Wobble>().freqX *= 1.01f;
                if (Head.GetComponent<Wobble>().freqY < 90.0f)
                    Head.GetComponent<Wobble>().freqY *= 1.01f;

                if (Thorax.GetComponent<Wobble>().freqX < 90.0f)
                    Thorax.GetComponent<Wobble>().freqX *= 1.01f;
                if (Thorax.GetComponent<Wobble>().freqY < 90.0f)
                    Thorax.GetComponent<Wobble>().freqY *= 1.01f;

                if (Abdomen.GetComponent<Wobble>().freqX < 90.0f)
                    Abdomen.GetComponent<Wobble>().freqX *= 1.01f;
                if (Abdomen.GetComponent<Wobble>().freqY < 90.0f)
                    Abdomen.GetComponent<Wobble>().freqY *= 1.01f;

                if (backLegRotateSpeed < 100.0f)
                    backLegRotateSpeed *= 1.05f;

                if (dyingTimer <= 0) //Final Death Explosion
                {
                    spiderState = SpiderState.DEAD;

                    Head.GetComponent<Wobble>().active = false;
                    Thorax.GetComponent<Wobble>().active = false;
                    Abdomen.GetComponent<Wobble>().active = false;

                    Head.GetComponent<SpiderHeadDeathImpulse>().DeathImpulse();
                    ExplodeLimb(Thorax);
                    ExplodeLimb(Abdomen);
                    ExplodeBloodSplatter(Head, 5.0f);
                    ExplodeBloodSplatter(Thorax, 5.0f);

                    BackLegsLeftPivot2.GetComponent<RotatePoint>().active = false;
                    BackLegsLeftPivot3.GetComponent<RotatePoint>().active = false;
                    BackLegsLeftPivot4.GetComponent<RotatePoint>().active = false;
                    BackLegsRightPivot2.GetComponent<RotatePoint>().active = false;
                    BackLegsRightPivot3.GetComponent<RotatePoint>().active = false;
                    BackLegsRightPivot4.GetComponent<RotatePoint>().active = false;
                    ExplodeLimb(BackLegsLeftSprite2);
                    ExplodeLimb(BackLegsLeftSprite3);
                    ExplodeLimb(BackLegsLeftSprite4);
                    ExplodeLimb(BackLegsRightSprite2);
                    ExplodeLimb(BackLegsRightSprite3);
                    ExplodeLimb(BackLegsRightSprite4);
                    ExplodeBloodSplatter(BackLegsLeftPivot2, 2.0f);
                    ExplodeBloodSplatter(BackLegsLeftPivot3, 2.0f);
                    ExplodeBloodSplatter(BackLegsLeftPivot4, 2.0f);
                    ExplodeBloodSplatter(BackLegsRightPivot2, 2.0f);
                    ExplodeBloodSplatter(BackLegsRightPivot3, 2.0f);
                    ExplodeBloodSplatter(BackLegsRightPivot4, 2.0f);

                    camera.GetComponent<ScreenShake>().SetShake(0.2f, 3.0f, 0.5f, 0.5f);
                }
            }
            else if (spiderState == SpiderState.DEAD)
            {
                transitionTimer -= Time.deltaTime;

                if (transitionTimer < 0)
                {
                    camera.GetComponent<CameraBehaviour>().FadeOverlayToBlack(true, 0.0f, 1.0f, 0.3f);
                    camera.GetComponent<CameraBehaviour>().FadeVignetteToBlack(true, 0.0f, 1.0f, 0.3f);
                }

                if (camera.GetComponent<CameraBehaviour>().GetBlackOverlayAlpha() >= 1.0f)
                {
                    //Music continues to keep playing into this scene
                    Audio.StopAllSource();
                    Scene.ClearScenes("08_Escape");
                    File.WriteDataAsBool("Cutscene", true);
                    File.WriteJsonFile("EscapeConfig");
                }
            }
        }



        void UpdateColor()
        {
            float diff = 1.0f - shadowColor.x;
            float diffShadow = ((this.gameObject.transform.localPosition.z - backPos) / (frontPos - backPos)) * diff;

            if (diffShadow + shadowColor.x > 1.0f) //Clamp to 1
                diffShadow = 1.0f - shadowColor.x;
            
            SetColor(new Vector3(shadowColor.x + diffShadow, shadowColor.y + diffShadow, shadowColor.z + diffShadow));
        }

        //Set the whole body of spider to certain color
        void SetColor(Vector3 newColor)
        {
            //Front Leg Right
            FrontLegRightSprite4.GetComponent<SpriteRenderer>().r = newColor.x;
            FrontLegRightSprite4.GetComponent<SpriteRenderer>().g = newColor.y;
            FrontLegRightSprite4.GetComponent<SpriteRenderer>().b = newColor.z;

            FrontLegRightSprite3.GetComponent<SpriteRenderer>().r = newColor.x;
            FrontLegRightSprite3.GetComponent<SpriteRenderer>().g = newColor.y;
            FrontLegRightSprite3.GetComponent<SpriteRenderer>().b = newColor.z;

            FrontLegRightSprite2.GetComponent<SpriteRenderer>().r = newColor.x;
            FrontLegRightSprite2.GetComponent<SpriteRenderer>().g = newColor.y;
            FrontLegRightSprite2.GetComponent<SpriteRenderer>().b = newColor.z;

            //Front Leg Left
            FrontLegLeftSprite4.GetComponent<SpriteRenderer>().r = newColor.x;
            FrontLegLeftSprite4.GetComponent<SpriteRenderer>().g = newColor.y;
            FrontLegLeftSprite4.GetComponent<SpriteRenderer>().b = newColor.z;

            FrontLegLeftSprite3.GetComponent<SpriteRenderer>().r = newColor.x;
            FrontLegLeftSprite3.GetComponent<SpriteRenderer>().g = newColor.y;
            FrontLegLeftSprite3.GetComponent<SpriteRenderer>().b = newColor.z;

            FrontLegLeftSprite2.GetComponent<SpriteRenderer>().r = newColor.x;
            FrontLegLeftSprite2.GetComponent<SpriteRenderer>().g = newColor.y;
            FrontLegLeftSprite2.GetComponent<SpriteRenderer>().b = newColor.z;

            //Back Leg Right
            BackLegsRightSprite4.GetComponent<SpriteRenderer>().r = newColor.x * 0.6f;
            BackLegsRightSprite4.GetComponent<SpriteRenderer>().g = newColor.y * 0.6f;
            BackLegsRightSprite4.GetComponent<SpriteRenderer>().b = newColor.z * 0.6f;

            BackLegsRightSprite3.GetComponent<SpriteRenderer>().r = newColor.x * 0.65f;
            BackLegsRightSprite3.GetComponent<SpriteRenderer>().g = newColor.y * 0.65f;
            BackLegsRightSprite3.GetComponent<SpriteRenderer>().b = newColor.z * 0.65f;

            BackLegsRightSprite2.GetComponent<SpriteRenderer>().r = newColor.x * 0.7f;
            BackLegsRightSprite2.GetComponent<SpriteRenderer>().g = newColor.y * 0.7f;
            BackLegsRightSprite2.GetComponent<SpriteRenderer>().b = newColor.z * 0.7f;

            //Back Leg Right
            BackLegsLeftSprite4.GetComponent<SpriteRenderer>().r = newColor.x * 0.6f;
            BackLegsLeftSprite4.GetComponent<SpriteRenderer>().g = newColor.y * 0.6f;
            BackLegsLeftSprite4.GetComponent<SpriteRenderer>().b = newColor.z * 0.6f;

            BackLegsLeftSprite3.GetComponent<SpriteRenderer>().r = newColor.x * 0.65f;
            BackLegsLeftSprite3.GetComponent<SpriteRenderer>().g = newColor.y * 0.65f;
            BackLegsLeftSprite3.GetComponent<SpriteRenderer>().b = newColor.z * 0.65f;

            BackLegsLeftSprite2.GetComponent<SpriteRenderer>().r = newColor.x * 0.7f;
            BackLegsLeftSprite2.GetComponent<SpriteRenderer>().g = newColor.y * 0.7f;
            BackLegsLeftSprite2.GetComponent<SpriteRenderer>().b = newColor.z * 0.7f;

            //Body
            Head.GetComponent<SpriteRenderer>().r = newColor.x;
            Head.GetComponent<SpriteRenderer>().g = newColor.y;
            Head.GetComponent<SpriteRenderer>().b = newColor.z;

            TeethLeftSprite.GetComponent<SpriteRenderer>().r = newColor.x;
            TeethLeftSprite.GetComponent<SpriteRenderer>().g = newColor.y;
            TeethLeftSprite.GetComponent<SpriteRenderer>().b = newColor.z;

            TeethRightSprite.GetComponent<SpriteRenderer>().r = newColor.x;
            TeethRightSprite.GetComponent<SpriteRenderer>().g = newColor.y;
            TeethRightSprite.GetComponent<SpriteRenderer>().b = newColor.z;

            Thorax.GetComponent<SpriteRenderer>().r = newColor.x * 0.8f;
            Thorax.GetComponent<SpriteRenderer>().g = newColor.y * 0.8f;
            Thorax.GetComponent<SpriteRenderer>().b = newColor.z * 0.8f;

            Abdomen.GetComponent<SpriteRenderer>().r = newColor.x * 0.7f;
            Abdomen.GetComponent<SpriteRenderer>().g = newColor.y * 0.7f;
            Abdomen.GetComponent<SpriteRenderer>().b = newColor.z * 0.7f;
        } //SetColor

        void ToggleFrontLegsIdleRotate(bool set)
        {
            FrontLegRightPivot4.GetComponent<RotatePoint>().active = set;
            FrontLegRightPivot3.GetComponent<RotatePoint>().active = set;
            FrontLegRightPivot2.GetComponent<RotatePoint>().active = set;
            FrontLegLeftPivot4.GetComponent<RotatePoint>().active = set;
            FrontLegLeftPivot3.GetComponent<RotatePoint>().active = set;
            FrontLegLeftPivot2.GetComponent<RotatePoint>().active = set;
        }

        void UpdateBackLegsRotate()
        {   
            Vector3 diff = this.gameObject.transform.localPosition - prevPos;
            int random;
            Random rnd = new Random();

            if (spiderState != SpiderState.DYING)
                backLegRotateSpeed = 1.0f + (diff.magnitude * skitterSpeed);

            random = rnd.Next(1, 6);
            BackLegsRightPivot4.GetComponent<RotatePoint>().Speed = backLegRotateSpeed * (random / 2.5f);
            random = rnd.Next(1, 6);
            BackLegsRightPivot3.GetComponent<RotatePoint>().Speed = -backLegRotateSpeed * (random / 3.0f);
            random = rnd.Next(1, 6);
            BackLegsRightPivot2.GetComponent<RotatePoint>().Speed = backLegRotateSpeed * (random / 4.5f);
            random = rnd.Next(1, 6);
            BackLegsLeftPivot4.GetComponent<RotatePoint>().Speed = -backLegRotateSpeed * (random / 2.0f);
            random = rnd.Next(1, 6);
            BackLegsLeftPivot3.GetComponent<RotatePoint>().Speed = backLegRotateSpeed * (random / 3.5f);
            random = rnd.Next(1, 6);
            BackLegsLeftPivot2.GetComponent<RotatePoint>().Speed = -backLegRotateSpeed * (random / 4.0f);

            prevPos = this.gameObject.transform.localPosition;
        }


        /////////////////////////////
        //Init Functions
        ////////////////////////////
        void InitFrontLegsRotate()
        {
            FrontLegLeftPivot4.GetComponent<RotatePoint>().Speed = 1.0f;
            FrontLegLeftPivot4.GetComponent<RotatePoint>().StartRotate = FrontLegLeftPivot4.transform.localRotation.z - 2;
            FrontLegLeftPivot4.GetComponent<RotatePoint>().EndRotate = FrontLegLeftPivot4.transform.localRotation.z + 2;

            FrontLegLeftPivot3.GetComponent<RotatePoint>().Speed = -1.5f;
            FrontLegLeftPivot3.GetComponent<RotatePoint>().StartRotate = FrontLegLeftPivot3.transform.localRotation.z - 2;
            FrontLegLeftPivot3.GetComponent<RotatePoint>().EndRotate = FrontLegLeftPivot3.transform.localRotation.z + 2;

            FrontLegLeftPivot2.GetComponent<RotatePoint>().Speed = 1.0f;
            FrontLegLeftPivot2.GetComponent<RotatePoint>().StartRotate = FrontLegLeftPivot2.transform.localRotation.z - 2;
            FrontLegLeftPivot2.GetComponent<RotatePoint>().EndRotate = FrontLegLeftPivot2.transform.localRotation.z + 2;

            FrontLegRightPivot4.GetComponent<RotatePoint>().Speed = 1.0f;
            FrontLegRightPivot4.GetComponent<RotatePoint>().StartRotate = FrontLegRightPivot4.transform.localRotation.z + 2;
            FrontLegRightPivot4.GetComponent<RotatePoint>().EndRotate = FrontLegRightPivot4.transform.localRotation.z - 2;

            FrontLegRightPivot3.GetComponent<RotatePoint>().Speed = -1.5f;
            FrontLegRightPivot3.GetComponent<RotatePoint>().StartRotate = FrontLegRightPivot3.transform.localRotation.z + 2;
            FrontLegRightPivot3.GetComponent<RotatePoint>().EndRotate = FrontLegRightPivot3.transform.localRotation.z - 2;

            FrontLegRightPivot2.GetComponent<RotatePoint>().Speed = 1.0f;
            FrontLegRightPivot2.GetComponent<RotatePoint>().StartRotate = FrontLegRightPivot2.transform.localRotation.z + 2;
            FrontLegRightPivot2.GetComponent<RotatePoint>().EndRotate = FrontLegRightPivot2.transform.localRotation.z - 2;
        }

        void InitBackLegsRotate()
        {
            BackLegsRightPivot4.GetComponent<RotatePoint>().Speed = backLegRotateSpeed;
            BackLegsRightPivot4.GetComponent<RotatePoint>().StartRotate = 5.0f;
            BackLegsRightPivot4.GetComponent<RotatePoint>().EndRotate = -5.0f;

            BackLegsRightPivot3.GetComponent<RotatePoint>().Speed = -backLegRotateSpeed;
            BackLegsRightPivot3.GetComponent<RotatePoint>().StartRotate = 2.5f;
            BackLegsRightPivot3.GetComponent<RotatePoint>().EndRotate = -5.0f;

            BackLegsRightPivot2.GetComponent<RotatePoint>().Speed = backLegRotateSpeed;
            BackLegsRightPivot2.GetComponent<RotatePoint>().StartRotate = 2.5f;
            BackLegsRightPivot2.GetComponent<RotatePoint>().EndRotate = -5.0f;

            BackLegsLeftPivot4.GetComponent<RotatePoint>().Speed = -backLegRotateSpeed;
            BackLegsLeftPivot4.GetComponent<RotatePoint>().StartRotate = 5.0f;
            BackLegsLeftPivot4.GetComponent<RotatePoint>().EndRotate = -5.0f;

            BackLegsLeftPivot3.GetComponent<RotatePoint>().Speed = backLegRotateSpeed;
            BackLegsLeftPivot3.GetComponent<RotatePoint>().StartRotate = 2.5f;
            BackLegsLeftPivot3.GetComponent<RotatePoint>().EndRotate = -5.0f;

            BackLegsLeftPivot2.GetComponent<RotatePoint>().Speed = -backLegRotateSpeed;
            BackLegsLeftPivot2.GetComponent<RotatePoint>().StartRotate = 2.5f;
            BackLegsLeftPivot2.GetComponent<RotatePoint>().EndRotate = -5.0f;

            TeethRightPivot.GetComponent<RotatePoint>().Speed = -teethRotateSpeed;
            TeethRightPivot.GetComponent<RotatePoint>().StartRotate = -1.5f;
            TeethRightPivot.GetComponent<RotatePoint>().EndRotate = 1.5f;

            TeethLeftPivot.GetComponent<RotatePoint>().Speed = teethRotateSpeed;
            TeethLeftPivot.GetComponent<RotatePoint>().StartRotate = -1.5f;
            TeethLeftPivot.GetComponent<RotatePoint>().EndRotate = 1.5f;
        }

        void InitBodyWobble()
        {
            Head.GetComponent<Wobble>().freqX = 2.0f * bodyWobbleFreq;
            Head.GetComponent<Wobble>().freqY = 2.0f * bodyWobbleFreq;
            Head.GetComponent<Wobble>().ampX = 0.05f;
            Head.GetComponent<Wobble>().ampY = 0.05f;

            Thorax.GetComponent<Wobble>().freqX = -1.5f * bodyWobbleFreq;
            Thorax.GetComponent<Wobble>().freqY = -1.5f * bodyWobbleFreq;
            Thorax.GetComponent<Wobble>().ampX = 0.04f;
            Thorax.GetComponent<Wobble>().ampY = 0.04f;

            Abdomen.GetComponent<Wobble>().freqX = 1.0f * bodyWobbleFreq;
            Abdomen.GetComponent<Wobble>().freqY = 1.0f * bodyWobbleFreq;
            Abdomen.GetComponent<Wobble>().ampX = 0.03f;
            Abdomen.GetComponent<Wobble>().ampY = 0.03f;
        }

        public void ToggleFrontLegColliders(bool state)
        {
            FrontLegRightCollider.GetComponent<RigidBody>().active = state;
            FrontLegLeftCollider.GetComponent<RigidBody>().active = state;

            FrontLegRightCollider.GetComponent<BoxCollider>().active = state;
            FrontLegLeftCollider.GetComponent<BoxCollider>().active = state;

            FrontLegRightCollider.active = state;
            FrontLegLeftCollider.active = state;
        }

        void ResetSpider() //Reset to spider's default transforms
        {
            tellTimer = 0.0f;

            FrontLegRightPivot4.transform.localRotation = new Vector3 (0.0f, 0.0f, startPivot4);
            FrontLegRightPivot3.transform.localRotation = new Vector3 (0.0f, 0.0f, startPivot3);
            FrontLegRightPivot2.transform.localRotation = new Vector3 (0.0f, 0.0f, startPivot2);

            FrontLegLeftPivot4.transform.localRotation = new Vector3 (0.0f, 0.0f, -startPivot4);
            FrontLegLeftPivot3.transform.localRotation = new Vector3 (0.0f, 0.0f, -startPivot3);
            FrontLegLeftPivot2.transform.localRotation = new Vector3 (0.0f, 0.0f, -startPivot2);

            ToggleFrontLegsIdleRotate(true);
            ToggleFrontLegColliders(false);

            InitBodyWobble();
            InitBackLegsRotate();

            teethRotateSpeed = 1.0f;
            TeethRightPivot.GetComponent<RotatePoint>().Speed = -teethRotateSpeed;
            TeethLeftPivot.GetComponent<RotatePoint>().Speed = teethRotateSpeed;

            stabTelegraphHoldTime = 0.3f;
            rockTossTelegraphHoldTime = 1.0f;

            Head.GetComponent<SpiderHead>().SetEyes(true, new Vector3(0.0f, 0.0f, 0.0f));
        }

        void DamageShake()
        {
            ToggleFrontLegsIdleRotate(true);

            FrontLegLeftPivot4.GetComponent<RotatePoint>().Speed = 5.0f;
            FrontLegLeftPivot3.GetComponent<RotatePoint>().Speed = -5.0f;
            FrontLegLeftPivot2.GetComponent<RotatePoint>().Speed = 5.0f;
                
            FrontLegRightPivot4.GetComponent<RotatePoint>().Speed = 5.0f;
            FrontLegRightPivot3.GetComponent<RotatePoint>().Speed = -5.0f;
            FrontLegRightPivot2.GetComponent<RotatePoint>().Speed = 5.0f;

            Head.GetComponent<Wobble>().freqX = 50.0f;
            Head.GetComponent<Wobble>().freqY = 50.0f;

            Thorax.GetComponent<Wobble>().freqX = 50.0f;
            Thorax.GetComponent<Wobble>().freqY = 50.0f;
            
            Abdomen.GetComponent<Wobble>().freqX = 50.0f;
            Abdomen.GetComponent<Wobble>().freqY = 50.0f;
            
            backLegRotateSpeed *= 1.5f;
            if (backLegRotateSpeed > 50.0f)
                backLegRotateSpeed = 50.0f;
        }

        public void SpawnCeilingRocks()
        {
            Random rnd = new Random();
            //int randomSound = rnd.Next(0, 3);

            for (int i = 0; i < ceilingRockCount + ((8 - spiderHealth)); ++i)
            {
                int randomSound = rnd.Next(0, 3);

                int randomX = rnd.Next(0, 21);          // creates a number between 0 and 30
                int randomYOffset = rnd.Next(0, 6);     
                GameObject fallingRock = GameObject.InstantiatePrefab("Dropping_Spike_SpiderBoss");
                fallingRock.transform.localPosition = new Vector3((randomX * 0.75f) - 7.5f, 
                                                                  (randomYOffset * 0.25f) + 8.0f, 
                                                                  0.01f);
                fallingRock.GetComponent<Breakable>().particleEffectName = "DustBoss";
                if (randomSound == 0)
                    fallingRock.GetComponent<Breakable>().sfx = "SFX_RockDrop";
                else if (randomSound == 1)
                    fallingRock.GetComponent<Breakable>().sfx = "SFX_RockDropCave";
                else 
                    fallingRock.GetComponent<Breakable>().sfx = "SFX_RockDropForest";
            }
        } 

        public void SpawnSlideAttackRocks(bool fromLeft)
        {
            int dirModifier;
            if (fromLeft)
                dirModifier = 1;
            else
                dirModifier = -1;

            Random rnd = new Random();
            //int randomSound = rnd.Next(0, 3);
            for (int j = 0; j < 2; ++j) //Layers of rock
            {
                for (int i = 0; i < 10; ++i) //Rows of rock
                {
                    int randomSound = rnd.Next(0, 3);
                    int randomYOffset = rnd.Next(0, 6);
                    GameObject fallingRock = GameObject.InstantiatePrefab("Dropping_Spike_SpiderBoss");
                    fallingRock.transform.localPosition = new Vector3((7.50f * dirModifier) - ((i * 0.9f)*dirModifier), 
                                                                      8.00f + (i * 1.0f) + (j * 2.5f) + (randomYOffset * 0.2f), 
                                                                      0.01f);
                    fallingRock.GetComponent<Breakable>().particleEffectName = "DustBoss";
                    if (randomSound == 0)
                        fallingRock.GetComponent<Breakable>().sfx = "SFX_RockDrop";
                    else if (randomSound == 1)
                        fallingRock.GetComponent<Breakable>().sfx = "SFX_RockDropCave";
                    else
                        fallingRock.GetComponent<Breakable>().sfx = "SFX_RockDropForest";
                }
            }
        } 

        public bool SpiderSquat(float jumpImpulsePassedIn, SpiderState stateToGoTo)
        {
            //Begin the squat tween
            if (isSquatting == false) 
            {
                this.gameObject.GetComponent<Tween>().StartTweenTranslate(0.4f, new Vector3(this.gameObject.transform.localPosition.x, 
                                                                                            startPos.y - 1, 
                                                                                            this.gameObject.transform.localPosition.z));

                BackLegsLeftPivotMaster.GetComponent<Tween>().StartTweenRotate(0.4f, new Vector3(0, 0, 345), -1);
                BackLegsRightPivotMaster.GetComponent<Tween>().StartTweenRotate(0.4f, new Vector3(0, 0, 15), 1);
                isSquatting = true;   
            }
            
            //Transition to next state
            if (BackLegsLeftPivotMaster.GetComponent<Tween>().isRotating == false && isSquatting == true)
            {
                //Turn on rigid body and apply jump pulse
                this.gameObject.GetComponent<RigidBody>().active = true;
                this.gameObject.GetComponent<RigidBody>().AddImpulse(new Vector2(0, jumpImpulsePassedIn), new Vector2(0, 0));

                //Rotate tween legs to up form
                BackLegsLeftPivotMaster.GetComponent<Tween>().StartTweenRotate(0.15f, new Vector3(0, 0, 45), 1);
                BackLegsRightPivotMaster.GetComponent<Tween>().StartTweenRotate(0.15f, new Vector3(0, 0, 315), -1);

                //Reset squatting flag and transition
                isSquatting = false; 
                spiderState = stateToGoTo;

                if (stateToGoTo == SpiderState.ATTACK_ROCK_TOSS_JUMP)
                    rockTossAttackFromLeft = !rockTossAttackFromLeft;

                Audio.PlaySource("SFX_Boss_Spider_Jump");
                return true; //Can move to next state
            }

            return false; //Not ready to move on to next state
        }

        public void SpiderLand() //Screen shake and sound effects for landing on ground
        {
            //Set position 
            this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, 
                                                                startPos.y, 
                                                                this.gameObject.transform.localPosition.z);
            //Deactivate rigid body
            this.gameObject.GetComponent<RigidBody>().velocity = new Vector2(0,0);
            this.gameObject.GetComponent<RigidBody>().active = false;
            
            camera.GetComponent<ScreenShake>().SetShake();
            
            Audio.PlaySource("SFX_Boss_Spider_Land");
        }

        public void ExplodeLimb(GameObject limb)
        {
            float explodeMag = 5.0f;
            float torqueMag = 0.001f;
            Random rnd = new Random();
            limb.transform.parent = null; //Unparent into the root of the world
            limb.GetComponent<RigidBody>().active = true;
            limb.GetComponent<RigidBody>().timeScale = 2.0f;
            limb.GetComponent<RigidBody>().gravityScale = 1.0f;
            limb.GetComponent<RigidBody>().inertia = 2.0f;
            int dir = rnd.Next(0, 2);
            if (dir == 0)
                dir = -1;
            else
                dir = 1;
            float xImpulse = ((float)rnd.NextDouble() * 2.0f * explodeMag) + explodeMag;
            limb.GetComponent<RigidBody>().AddImpulse(new Vector2(xImpulse * dir, explodeMag), 
                                                      new Vector2((float)rnd.NextDouble() * 2.0f + 1.0f, (float)rnd.NextDouble() * 2.0f + 1.0f)); //rand (-1,-1) to (1,1)
            /*
            if (xImpulse < 0)
                limb.GetComponent<RigidBody>().AddTorque(torqueMag);
            else
                limb.GetComponent<RigidBody>().AddTorque(-torqueMag);*/
        }

        public void ExplodeBloodSplatter(GameObject pivotPoint, float scale = 3.0f)
        {
            GameObject blood = GameObject.InstantiatePrefab("SpiderBossBlood");
            blood.transform.localPosition = new Vector3(pivotPoint.transform.worldPosition.x,
                                                        pivotPoint.transform.worldPosition.y,
                                                        pivotPoint.transform.worldPosition.z - 0.01f);
            blood.transform.localScale = new Vector3(scale, scale, 1.0f);
            Audio.PlaySource("SFX_EnemyDeath");
        }

        public void LerpCamera(Vector3 target)
        {
            if (isCutscene)
            {
                camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, target, 0.1f);
            }
        }

        public void SpiderMouthTwitch(bool twitch)
        {
            if (twitch)
            {
                TeethLeftPivot.GetComponent<RotatePoint>().Speed = 30.0f;
                TeethRightPivot.GetComponent<RotatePoint>().Speed = 30.0f;
            }
            else
            {
                TeethLeftPivot.GetComponent<RotatePoint>().Speed = 1.0f;
                TeethRightPivot.GetComponent<RotatePoint>().Speed = 1.0f;
            }
        }
    } //SpiderBoss
} //LossEngine