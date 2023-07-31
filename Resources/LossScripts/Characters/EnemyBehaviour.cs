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
    class EnemyBehaviour : LossBehaviour
    {
        //States
        private EnemyState enemyState;
        private EnemyState enemyStatePrevious;
        private enum EnemyState
        {
            IDLE,
            MOVE,
            JUMP,
            JUMPING,
            CHASE,
            MELEE,
            SHOOT,
            DEFY,
            FALL,
            LAND,
            DAMAGED,
            DEAD,
            BOUNCE,
            RECOVER,
            EATEN
        }

        //Components
        private SpriteRenderer spriteRenderer;
        private Animator spriteAnimator;
        private RigidBody rb2D;
        private BoxCollider col2D;
        private TongueInteractive tongueInteractive;

        //Sprite Strings
        private string sheetIdle = "Enemy_Idle";
        private string sheetMove = "Enemy_Walk";
        private string sheetAttack = "Enemy_Shoot";
        private string sheetUpDown = "Enemy_UpDown";
        private string sheetDeath = "Enemy_Death";

        private string sheetIdleMushroom = "Enemy_Mushroom_Idle";
        private string sheetMoveMushroom = "Enemy_Mushroom_Walk";
        private string sheetBounceMushroom = "Enemy_Mushroom_Bounce";
        private string sheetRecoverMushroom = "Enemy_Mushroom_Recover";

        //Anim Strings
        private string animIdle = "EnemyIdleAnim";
        private string animMove = "EnemyIdleAnim";
        private string animAttack = "EnemyShootAnim";
        private string animUpDown = "EnemyUpDownAnim";
        private string animDeath = "EnemyDeathAnim";

        private string animMushroomRepeat = "EnemyMushroomRepeat";
        private string animMushroomSingle = "EnemyMushroomSingle";

        //Game Object Component
        public GameObject enemyHeadObject;

        //Game Object Checking
        private GameObject mainPlayer;
        private GameObject mySpawnedObject;
        private GameObject mySpiderWeb;
        private GameObject mySpiderBase;
        private GameObject mainPlayerTongue;
        private float spiderWebStringLength;

        //Collision Checkers
        public GameObject checkerGroundObject;
        public GameObject checkerWallObject;
        private EnemyCheckers checkerGround;
        private EnemyCheckers checkerWall;

        //Health Settings
        private int healthCurrent;
        public int healthMaximum;

        //Speed Settings
        public float movementSpeed;
        public float jumpingSpeed;
        public float shootingSpeed;

        //Position Settings
        private Vector3 startingPosition;
        private Vector3 endingPosition;
        public float endingPositionOffsetMax;

        //Movement Settings
        private float horizontalMovementAxis;

        //Duration Settings
        public float behaviourTimerSet;
        private float behaviourTimerCurrent;
        public float meleeStateDurationSet;
        private float meleeStateDurationCurrent;
        public float meleeDelayDurationSet;
        private float meleeDelayDurationCurrent;
        public float meleeCooldownDurationSet;
        private float meleeCooldownDurationCurrent;
        public float shootStateDurationSet;
        private float shootStateDurationCurrent;
        public float shootDelayDurationSet;
        private float shootDelayDurationCurrent;
        public float shootCooldownDurationSet;
        private float shootCooldownDurationCurrent;
        public float invisDurationSet;
        private float invisDurationCurrent;
        public float flashDurationSet;
        private float flashDurationCurrent;

        //Ability Checks
        public bool canMove;
        public bool canJump;
        public bool canChase;
        public bool canMelee;
        public bool canShoot;
        public bool canMushroom;
        public bool canDefy;

        //Bool Checks
        public bool isOnGround;
        public bool isFacingWall;
        private bool isJumping;
        private bool isShooting;
        private bool isMeleeing;
        private bool isDefyingDown;
        private bool isDamaged;
        private bool isEaten;
        private bool isInvis;
        private bool isFlashing;
        private bool isMushroomBouncing;
        private bool isMushroomRecovering;
        private bool isNewBehaviour;
        private bool isOnMeleeCooldown;
        private bool isOnShootCooldown;
        private bool hasSavedHiddenPoint; 
        
        /// <summary>
        /// /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /// </summary>

        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        //Bool Timers
        private bool turnOnTimerMeleeState;
        private bool turnOnTimerMeleeDelay;
        private bool turnOnTimerMeleeCooldown;
        private bool turnOnTimerShootState;
        private bool turnOnTimerShootDelay;
        private bool turnOnTimerShootCooldown;
        private bool turnOnTimerInvis;
        private bool turnOnTimerFlashing;

        //Behaviour Settings
        private string[] behaviourTypeArray = new string[3];
        private string behaviourTypeCurrent;

        //Particle
        public GameObject deadParticle;
        private GameObject damagedParticle;

        void Update()
        {
            CheckComponents();
            CheckHealth();
            CheckGround();
            SelectBehaviour();
            CheckInvis();
            CheckTimer();
        }

        void FixedUpdate()
        {
            CheckDecision();
            CheckState();
            CheckAnimation();
        }

        private void CheckComponents()
        {
            if (hasInit == false)
            {
                spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
                spriteAnimator = this.gameObject.GetComponent<Animator>();
                rb2D = this.gameObject.GetComponent<RigidBody>();
                col2D = this.gameObject.GetComponent<BoxCollider>();
                tongueInteractive = this.gameObject.GetComponent<TongueInteractive>();
                if (checkerGroundObject != null)
                {
                    checkerGround = checkerGroundObject.GetComponent<EnemyCheckers>();
                }
                if (checkerWallObject != null)
                {
                    checkerWall = checkerWallObject.GetComponent<EnemyCheckers>();
                }
                if (GameObject.GetGameObjectsOfTag("Player").Length > 0)
                {
                    mainPlayer = GameObject.GetGameObjectsOfTag("Player")[0];
                }
                if (canDefy == true)
                {
                    rb2D.gravityScale = 0.0f;
                    rb2D.mass = rb2D.mass * 100.0f;
                }
                tongueInteractive.active = false;
                deadParticle.active = false;
                startingPosition = this.gameObject.transform.worldPosition;
                rb2D.velocity = new Vector2(0, 0);
                horizontalMovementAxis = FacingDirection();
                AddEnabledBehaviour();
                hasInit = true;
            }
        }

        private void AddEnabledBehaviour()
        {
            behaviourTypeArray[0] = "IDLE";
            behaviourTypeArray[1] = "MOVE";
            behaviourTypeArray[2] = "JUMP";
        }

        private void CheckHealth()
        {
            if (healthCurrent > healthMaximum)
            {
                healthCurrent = healthMaximum;
            }
            if (healthCurrent < 0)
            {
                healthCurrent = 0;
            }
        }

        private void CheckGround()
        {
            if (checkerGround != null)
            {
                isOnGround = checkerGround.GetCollisionStatus("GROUND");
            }
            if (checkerWall != null)
            {
                isFacingWall = checkerWall.GetCollisionStatus("WALL");
            }
        }

        private void SelectBehaviour()
        {
            if (behaviourTimerCurrent < behaviourTimerSet)
            {
                behaviourTimerCurrent += Time.deltaTime;
            }
            else
            {
                RandomiseBehaviour();
                behaviourTimerCurrent = 0.0f;
            }
        }

        private void RandomiseBehaviour()
        {
            Random random = new Random();
            int tempRandom = random.Next(behaviourTypeArray.Length);
            if (tempRandom == 1 && canMove == true)
            {
                behaviourTypeCurrent = "MOVE";
            }
            else if (tempRandom == 2 && canJump == true)
            {
                behaviourTypeCurrent = "JUMP";
            }
            else
            {
                behaviourTypeCurrent = "IDLE";
            }
            isNewBehaviour = true;
        }

        private void CheckDecision()
        {
            if (isEaten == true)
            {
                enemyState = EnemyState.EATEN;
            }
            else if (healthCurrent <= 0)
            {
                enemyState = EnemyState.DEAD;
            }
            else if (isDamaged == true)
            {
                enemyState = EnemyState.DAMAGED;
            }
            else if (canDefy == true)
            {
                enemyState = EnemyState.DEFY;
            }
            else if (isMushroomBouncing == true)
            {
                enemyState = EnemyState.BOUNCE;
            }
            else if (isMushroomRecovering == true)
            {
                enemyState = EnemyState.RECOVER;
            }
            else if (isOnGround == true && rb2D.velocity.y <= 0 && isJumping == true)
            {
                enemyState = EnemyState.LAND;
            }
            else if (canMelee == true && isOnGround == true && isJumping == false && PlayerIsWithinRange(1.0f, 1.0f) == true && isOnMeleeCooldown == false || (isMeleeing == true))
            {
                enemyState = EnemyState.MELEE;
            }
            else if (canShoot == true && isOnGround == true && isJumping == false && PlayerIsWithinRange(3.0f, 2.0f) == true && isOnShootCooldown == false || (isShooting == true))
            {
                enemyState = EnemyState.SHOOT;
            }
            else if (canChase == true && isOnGround == true && isJumping == false && PlayerIsWithinRange(3.0f, 3.0f) == true)
            {
                enemyState = EnemyState.CHASE;
            }
            else if (isOnGround == true && behaviourTypeCurrent == "JUMP" && isJumping == false)
            {
                enemyState = EnemyState.JUMP;
            }
            else if (isOnGround == false && rb2D.velocity.y <= 0)
            {
                enemyState = EnemyState.FALL;
            }
            else if (isJumping == true)
            {
                enemyState = EnemyState.JUMPING;
            }
            else if (behaviourTypeCurrent == "MOVE")
            {
                enemyState = EnemyState.MOVE;
            }
            else
            {
                enemyState = EnemyState.IDLE;
            }
        }

        private void CheckState()
        {
            switch (enemyState)
            {
                case EnemyState.IDLE:
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                    break;

                case EnemyState.MOVE:
                    if (isFacingWall == true && isNewBehaviour == true)
                    {
                        isFacingWall = false;
                        horizontalMovementAxis = -horizontalMovementAxis;
                        isFacingWall = false;
                        isNewBehaviour = false;
                    }
                    else if (isNewBehaviour == true)
                    {
                        Random random = new Random();
                        int tempRandom = random.Next(10);
                        if (tempRandom % 2 == 0) //Even
                        {
                            horizontalMovementAxis = 1;
                        }
                        else //Odd
                        {
                            horizontalMovementAxis = -1;
                        }
                        isNewBehaviour = false;
                    }
                    HorizontalMovement(horizontalMovementAxis);
                    if (GetDistance(this.gameObject.transform.worldPosition, mainPlayer.transform.worldPosition) < 5f)
                    {
                        if (!(Audio.IsSourcePlaying("SFX_EnemyWalk")))
                        {
                            Audio.PlaySource("SFX_EnemyWalk");
                        }
                    }
                    break;

                case EnemyState.JUMP:
                    VerticalMovement(0, 1, jumpingSpeed);
                    isJumping = true;
                    RandomiseBehaviour();
                    break;

                case EnemyState.JUMPING:
                    rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y);
                    break;

                case EnemyState.CHASE:
                    if (mainPlayer.transform.worldPosition.x > this.gameObject.transform.worldPosition.x)
                    {
                        HorizontalMovement(1);
                    }
                    else
                    {
                        HorizontalMovement(-1);
                    }
                    break;

                case EnemyState.MELEE:
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                    if (isMeleeing == false)
                    {
                        if (mainPlayer.transform.worldPosition.x > this.gameObject.transform.worldPosition.x)
                        {
                            FlipX(1);
                        }
                        else
                        {
                            FlipX(-1);
                        }
                        isMeleeing = true;
                        turnOnTimerMeleeDelay = true;
                        Audio.PlaySource("SFX_EnemyAttack");
                    }
                    break;

                case EnemyState.SHOOT:
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                    if (isShooting == false)
                    {
                        if (mainPlayer.transform.worldPosition.x > this.gameObject.transform.worldPosition.x)
                        {
                            FlipX(1);
                        }
                        else
                        {
                            FlipX(-1);
                        }
                        isShooting = true;
                        turnOnTimerShootDelay = true;
                        Audio.PlaySource("SFX_EnemyShoot");
                    }
                    break;

                case EnemyState.DEFY:
                    if (mySpiderWeb != null)
                    {
                        spiderWebStringLength = GetDistance(mySpiderWeb.transform.worldPosition, this.gameObject.transform.worldPosition);
                        mySpiderWeb.transform.localScale = new Vector3(1.0f, spiderWebStringLength, 1.0f);
                    }
                    if (isFacingWall == true)
                    {
                        if (hasSavedHiddenPoint == false)
                        {
                            startingPosition = this.gameObject.transform.worldPosition;
                            endingPosition = new Vector3(startingPosition.x, startingPosition.y - (endingPositionOffsetMax), startingPosition.z);
                            mySpiderBase = GameObject.InstantiatePrefab("SpiderWebTop");
                            mySpiderBase.transform.localPosition = new Vector3(startingPosition.x, startingPosition.y + 0.5f, startingPosition.z - 0.01f);
                            mySpiderWeb = GameObject.InstantiatePrefab("SpiderWebString");
                            mySpiderWeb.transform.localPosition = new Vector3(startingPosition.x, startingPosition.y + 0.5f, startingPosition.z - 0.02f);
                            rb2D.velocity = new Vector2(0, 0);
                            isJumping = false;
                            hasSavedHiddenPoint = true;
                        }
                        col2D.isTrigger = true;
                    }
                    else if (isOnGround == true)
                    {
                        col2D.isTrigger = true;
                    }
                    else
                    {
                        col2D.isTrigger = false;
                    }
                    if (hasSavedHiddenPoint == true)
                    {
                        if (isDefyingDown == true) //Moving Down
                        {
                            if ((isOnGround == true && isJumping == false) || this.gameObject.transform.worldPosition.y <= endingPosition.y)
                            {
                                if (PlayerIsBelowRange(0.7f, 1.0f) == true)
                                {
                                    mainPlayer.GetComponent<PlayerBehaviour>().HealthDecrease(true, this.gameObject.transform.worldPosition.x);
                                }
                                isJumping = true;
                                isDefyingDown = false;
                            }
                            else
                            {
                                if (PlayerIsBelowRange(2.0f, 3.0f) == true) //Closest Range
                                {
                                    endingPosition = new Vector3(startingPosition.x, startingPosition.y - (endingPositionOffsetMax), startingPosition.z);
                                    DefyingMovement(-1, 2.5f, false);
                                }
                                else if (PlayerIsBelowRange(3.0f, 5.0f) == true) //Mid Range
                                {
                                    endingPosition = new Vector3(startingPosition.x, startingPosition.y - (endingPositionOffsetMax / 2.0f), startingPosition.z);
                                    DefyingMovement(-1, 2.0f, false);
                                }
                                else //Far Range
                                {
                                    endingPosition = new Vector3(startingPosition.x, startingPosition.y - 0.5f, startingPosition.z);
                                    DefyingMovement(-1, 1.0f, false);
                                }
                            }
                        }
                        else //Moving Up
                        {
                            if (PlayerIsBelowRange(2.0f, 3.0f) == true)
                            {
                                if (this.gameObject.transform.worldPosition.y < startingPosition.y)
                                {
                                    DefyingMovement(1, 1.0f, true);
                                }
                                else
                                {
                                    isJumping = false;
                                    isDefyingDown = true;
                                }
                            }
                            else if (PlayerIsBelowRange(3.0f, 5.0f) == true)
                            {
                                if (this.gameObject.transform.worldPosition.y < startingPosition.y)
                                {
                                    DefyingMovement(1, 0.5f, true);
                                }
                                else
                                {
                                    isJumping = false;
                                    isDefyingDown = true;
                                }
                            }
                            else
                            {
                                if (this.gameObject.transform.worldPosition.y < startingPosition.y)
                                {
                                    DefyingMovement(1, 0.25f, true);
                                }
                                else
                                {
                                    rb2D.velocity = new Vector2(0, 0);
                                    isJumping = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        DefyingMovement(1, 2.0f, false);
                    }
                    break;

                case EnemyState.FALL:
                    rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y);
                    break;

                case EnemyState.LAND:
                    isJumping = false;
                    break;

                case EnemyState.DAMAGED:
                    isDamaged = false;
                    break;

                case EnemyState.DEAD:
                    this.gameObject.tag = "EnemyDead";
                    tongueInteractive.active = true;
                    break;

                case EnemyState.BOUNCE:
                    break;

                case EnemyState.RECOVER:
                    break;

                case EnemyState.EATEN:
                    this.gameObject.tag = "IgnoreCollision";
                    rb2D.active = false;
                    col2D.isTrigger = true;
                    this.gameObject.transform.localScale = new Vector3(0.8f * FacingDirection(), 0.8f * Math.Abs(FacingDirection()), 1.0f);
                    this.gameObject.transform.localPosition = mainPlayerTongue.transform.worldPosition;
                    if (!(Audio.IsSourcePlaying("SFX_EatEnemy")))
                    {
                        Audio.PlaySource("SFX_EatEnemy");
                    }
                    if (GetDistance(this.gameObject.transform.worldPosition, mainPlayer.GetComponent<PlayerBehaviour>().GetPlayerHeadPosition()) < 0.1f)
                    {
                        mainPlayer.GetComponent<PlayerBehaviour>().HealthIncrease();
                        Destroy(this.gameObject);
                    }
                    break;
            }
        }

        private void CheckAnimation()
        {
            switch (enemyState)
            {
                case EnemyState.IDLE:
                    if (canMushroom == true)
                    {
                        ChangeSpriteAnimation(sheetIdleMushroom, animMushroomRepeat, -1);
                    }
                    else
                    {
                        ChangeSpriteAnimation(sheetIdle, animIdle, -1);
                    }
                    break;

                case EnemyState.MOVE:
                    if (canMushroom == true)
                    {
                        ChangeSpriteAnimation(sheetMoveMushroom, animMushroomRepeat, -1);
                    }
                    else
                    {
                        ChangeSpriteAnimation(sheetMove, animMove, -1);
                    }
                    break;

                case EnemyState.JUMP:
                    ChangeSpriteAnimation(sheetIdle, animIdle, -1);
                    break;

                case EnemyState.JUMPING:
                    ChangeSpriteAnimation(sheetIdle, animIdle, -1);
                    break;

                case EnemyState.CHASE:
                    if (canMushroom == true)
                    {
                        ChangeSpriteAnimation(sheetMoveMushroom, animMushroomRepeat, -1);
                    }
                    else
                    {
                        ChangeSpriteAnimation(sheetMove, animMove, -1);
                    }
                    break;

                case EnemyState.MELEE:
                    ChangeSpriteAnimation(sheetAttack, animAttack, 1);
                    break;

                case EnemyState.SHOOT:
                    ChangeSpriteAnimation(sheetAttack, animAttack, 1);
                    break;

                case EnemyState.DEFY:
                    ChangeSpriteAnimation(sheetUpDown, animUpDown, -1);
                    break;

                case EnemyState.FALL:
                    break;

                case EnemyState.LAND:
                    break;

                case EnemyState.DAMAGED:
                    break;

                case EnemyState.DEAD:
                    ChangeSpriteAnimation(sheetDeath, animDeath, 1, "SFX_EnemyDeath");
                    spriteRenderer.r = 0.5f;
                    spriteRenderer.g = 0.4f;
                    spriteRenderer.b = 0.7f;
                    deadParticle.active = true;
                    break;

                case EnemyState.BOUNCE:
                    ChangeSpriteAnimation(sheetBounceMushroom, animMushroomRepeat, 1);
                    if (spriteRenderer.currentFrameX == spriteRenderer.maxFrameX && spriteRenderer.currentFrameY == spriteRenderer.maxFrameY)
                    {
                        isMushroomRecovering = true;
                        isMushroomBouncing = false;
                    }
                    break;

                case EnemyState.RECOVER:
                    ChangeSpriteAnimation(sheetRecoverMushroom, animMushroomSingle, 1);
                    if (spriteRenderer.currentFrameX == spriteRenderer.maxFrameX && spriteRenderer.currentFrameY == spriteRenderer.maxFrameY)
                    {
                        isMushroomRecovering = false;
                    }
                    break;

                case EnemyState.EATEN:
                    if (canMushroom == true)
                    {
                        ChangeSpriteAnimation(sheetBounceMushroom, animMushroomSingle, 1);
                    }
                    else
                    {
                        ChangeSpriteAnimation(sheetDeath, animDeath, 1);
                    }
                    deadParticle.active = false;
                    break;
            }
            if (enemyStatePrevious != enemyState)
            {
                enemyStatePrevious = enemyState;
                spriteRenderer.currentFrameX = 1;
                spriteRenderer.currentFrameY = 1;
                spriteAnimator.animateCount = spriteAnimator.animateCount;
                //Console.WriteLine(enemyState);
            }
        }

        private void ChangeSpriteAnimation(string spriteRenderName, string animationName, int loop, string audioName = null)
        {
            if (spriteRenderer.textureName != spriteRenderName)
            {
                spriteRenderer.textureName = spriteRenderName;
                spriteRenderer.currentFrameX = 1;
                spriteRenderer.currentFrameY = 1;
            }
            if (spriteAnimator.fileName != animationName)
            {
                spriteAnimator.fileName = animationName;
                spriteAnimator.animateCount = loop;
                if (audioName != null)
                {
                    if (GetDistance(this.gameObject.transform.worldPosition, mainPlayer.transform.worldPosition) < 5f)
                    {
                        Audio.PlaySource(audioName);
                    }
                }
            }
        }

        private void CheckInvis()
        {
            if (isInvis == true && isFlashing == false)
            {
                isFlashing = true;
                turnOnTimerFlashing = true;
            }
            else if (isInvis == false && isFlashing == true)
            {
                spriteRenderer.r = 1.0f;
                spriteRenderer.g = 1.0f;
                spriteRenderer.b = 1.0f;
                isFlashing = false;
            }
        }

        private void CheckTimer()
        {
            if (turnOnTimerShootDelay == true)
            {
                if (shootDelayDurationCurrent < shootDelayDurationSet)
                {
                    shootDelayDurationCurrent += Time.deltaTime;
                }
                else
                {
                    Vector3 spawnDirectionTarget = mainPlayer.transform.worldPosition - enemyHeadObject.transform.worldPosition;
                    spawnDirectionTarget = new Vector3(spawnDirectionTarget.x, spawnDirectionTarget.y, 0).normalized;
                    mySpawnedObject = GameObject.InstantiatePrefab("EnemyWeb");
                    mySpawnedObject.transform.localPosition = new Vector3(enemyHeadObject.transform.worldPosition.x, enemyHeadObject.transform.worldPosition.y, enemyHeadObject.transform.worldPosition.z + 0.1f);
                    mySpawnedObject.GetComponent<EnemyWebBehaviour>().WebSettings(this.gameObject, spawnDirectionTarget, shootingSpeed, 1.0f);
                    turnOnTimerShootDelay = false;
                    turnOnTimerShootState = true;
                    isOnShootCooldown = true;
                    turnOnTimerShootCooldown = true;
                    shootDelayDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerShootState == true)
            {
                if (shootStateDurationCurrent < shootStateDurationSet)
                {
                    shootStateDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isShooting = false;
                    turnOnTimerShootState = false;
                    shootStateDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerShootCooldown == true)
            {
                if (shootCooldownDurationCurrent < shootCooldownDurationSet)
                {
                    shootCooldownDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isOnShootCooldown = false;
                    turnOnTimerShootCooldown = false;
                    shootCooldownDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerMeleeDelay == true)
            {
                if (meleeDelayDurationCurrent < meleeDelayDurationSet)
                {
                    meleeDelayDurationCurrent += Time.deltaTime;
                }
                else
                {
                    if (PlayerIsWithinRange(1.0f, 1.0f) == true)
                    {
                        mainPlayer.GetComponent<PlayerBehaviour>().HealthDecrease(true, this.gameObject.transform.worldPosition.x);
                    }
                    turnOnTimerMeleeDelay = false;
                    turnOnTimerMeleeState = true;
                    isOnMeleeCooldown = true;
                    turnOnTimerMeleeCooldown = true;
                    meleeDelayDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerMeleeState == true)
            {
                if (meleeStateDurationCurrent < meleeStateDurationSet)
                {
                    meleeStateDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isMeleeing = false;
                    turnOnTimerMeleeState = false;
                    meleeStateDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerMeleeCooldown == true)
            {
                if (meleeCooldownDurationCurrent < meleeCooldownDurationSet)
                {
                    meleeCooldownDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isOnMeleeCooldown = false;
                    turnOnTimerMeleeCooldown = false;
                    meleeCooldownDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerInvis == true)
            {
                if (invisDurationCurrent < invisDurationSet)
                {
                    invisDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isInvis = false;
                    turnOnTimerInvis = false;
                    invisDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerFlashing == true)
            {
                if (flashDurationCurrent < flashDurationSet)
                {
                    flashDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isFlashing = false;
                    turnOnTimerFlashing = false;
                    flashDurationCurrent = 0.0f;
                }
                if (flashDurationCurrent % 2.0f == 0.0f)
                {
                    spriteRenderer.r = 1.0f;
                    spriteRenderer.g = 1.0f;
                    spriteRenderer.b = 1.0f;
                }
                else
                {
                    spriteRenderer.r = 0.5f;
                    spriteRenderer.g = 0.5f;
                    spriteRenderer.b = 0.5f;
                }
            }
        }

        private void HorizontalMovement(float directionX)
        {
            FlipX(directionX);
            rb2D.velocity = new Vector2(directionX * movementSpeed, rb2D.velocity.y);
        }

        private void VerticalMovement(float directionX, float directionY, float speed)
        {
            if (rb2D != null)
            {
                rb2D.velocity = new Vector2(0, 0);
                rb2D.AddImpulse(new Vector2(directionX * speed, directionY * speed), new Vector2(0, 0));
            }
        }

        private void DefyingMovement(float directionY, float speedMultiply, bool randomMovement)
        {
            if (randomMovement == true)
            {
                Random random = new Random();
                int tempRandom = random.Next(0, 20);
                rb2D.velocity = new Vector2(0.0f, directionY * movementSpeed * speedMultiply * (float)tempRandom / 10.0f);
            }
            else
            {
                rb2D.velocity = new Vector2(0.0f, directionY * movementSpeed * speedMultiply);
            }
        }

        private float FacingDirection()
        {
            return this.gameObject.transform.localScale.x;
        }

        private void FlipX(float directionX)
        {
            this.gameObject.transform.localScale = new Vector3(directionX, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }

        private float GetDistance(Vector3 positionSelf, Vector3 positionTarget)
        {
            float x = positionTarget.x - positionSelf.x;
            float y = positionTarget.y - positionSelf.y;
            float distance = ((float)Math.Sqrt(x * x + y * y));
            return distance;
        }

        private bool PlayerIsWithinPosition(float heightDistance)
        {
            if (mainPlayer.transform.worldPosition.x > this.gameObject.transform.worldPosition.x - 0.1f && mainPlayer.transform.worldPosition.x < this.gameObject.transform.worldPosition.x + 0.1f)
            {
                return false;
            }
            else
            {
                if (mainPlayer.transform.worldPosition.y > this.gameObject.transform.worldPosition.y - 0.5f && mainPlayer.transform.worldPosition.y < this.gameObject.transform.worldPosition.y + heightDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool PlayerIsWithinRange(float heightDistance, float rangeDistance)
        {
            if (PlayerIsWithinPosition(heightDistance) == true && GetDistance(this.gameObject.transform.worldPosition, mainPlayer.transform.worldPosition) < rangeDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool PlayerIsBelowRange(float widthDistance, float rangeDistance)
        {
            if (mainPlayer.transform.worldPosition.x < this.gameObject.transform.worldPosition.x + widthDistance && mainPlayer.transform.worldPosition.x > this.gameObject.transform.worldPosition.x - widthDistance && mainPlayer.transform.worldPosition.y < this.gameObject.transform.worldPosition.y && GetDistance(this.gameObject.transform.worldPosition, mainPlayer.transform.worldPosition) < rangeDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void HealthDecrease()
        {
            if (healthCurrent > 0)
            {
                healthCurrent -= 1;
                isDamaged = true;
                isInvis = true;
                turnOnTimerInvis = true;
                damagedParticle = GameObject.InstantiatePrefab("DamagedParticle");
                damagedParticle.transform.localPosition = new Vector3(this.gameObject.transform.worldPosition.x, this.gameObject.transform.worldPosition.y + 0.2f, this.gameObject.transform.worldPosition.z + 0.01f);
            }
        }

        public int GetHealthCurrent()
        {
            return healthCurrent;
        }

        public void TriggerMushroomBounce()
        {
            isMushroomBouncing = true;
        }

        public bool GetIsEaten()
        {
            return isEaten;
        }

        public void EatEnemy(GameObject tongueTip)
        {
            mainPlayerTongue = tongueTip;
            isEaten = true;
        }

        public void JumpStart()
        {
            behaviourTypeCurrent = "JUMP";
            enemyState = EnemyState.JUMP;
            VerticalMovement(0, 1, jumpingSpeed * 1.2f);
            isJumping = true;
            deadParticle.active = false;
        }
    }
}