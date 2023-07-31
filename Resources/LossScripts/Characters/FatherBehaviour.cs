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
    class FatherBehaviour : LossBehaviour
    {
        //States
        public FatherState fatherState;
        public FatherState fatherStatePrevious;
        public enum FatherState
        {
            IDLE,
            MOVE,
            JUMP,
            JUMPING,
            FALL,
            LAND,
            TRAPPED_IDLE,
            TRAPPED_HIT_1,
            TRAPPED_HIT_2,
            TRAPPED_HIT_3,
            TRAPPED_RELEASED,
            TRAPPED_DROP,
            TRAPPED_WAKE
        }

        //Components
        private SpriteRenderer spriteRenderer;
        private Animator spriteAnimator;
        private RigidBody rb2D;
        private BoxCollider boxCollider;
        private TongueInteractive tongueInteractive;

        //Getting Player
        private GameObject mainPlayer;
        private Vector3 mainPlayerPosition;

        //Sprite Strings
        private string sheetIdle = "Dad_Idling";
        private string sheetMove = "Dad_Run";
        private string sheetJump = "Dad_Jump";
        private string sheetFall = "Dad_Fall";
        private string sheetLand = "Dad_Land";
        private string sheetTrappedIdle = "Dad_Trapped_00";
        private string sheetTrapped01 = "Dad_Trapped_01";
        private string sheetTrapped02 = "Dad_Trapped_02";
        private string sheetTrapped03 = "Dad_Trapped_03";
        private string sheetFree01 = "Dad_Free_01";
        private string sheetFree02 = "Dad_Free_02";

        //Anim Strings
        private string animIdle = "FatherIdleAnim";
        private string animMove = "FatherMoveAnim";
        private string animJump = "FatherJumpAnim";
        private string animFall = "FatherFallAnim";
        private string animLand = "FatherLandAnim";
        private string animTrappedIdle = "FatherTrappedIdleAnim";
        private string animTrappedHit = "FatherTrappedHitAnim";
        private string animFreeDrop = "FatherFreeDropAnim";
        private string animFreeWake = "FatherFreeWakeAnim";

        //Cacoon Sprite Strings
        private string sheetCocoonIdle = "cocoon_web_idle";
        private string sheetCocoonFall = "cocoon_web_fall";
        private string sheetFirstLayerIdle = "web_1st_layer_idle";
        private string sheetFirstLayerFall = "web_1st_layer_fall";
        private string sheetSecondLayerIdle = "web_2nd_layer_idle";
        private string sheetSecondLayerFall = "web_2nd_layer_fall";

        //Cacoon Anim String
        private string sheetWebIdle = "FatherWebIdleAnim";
        private string sheetWebFall = "FatherWebFallAnim";

        //Cacoon Game Objects
        public GameObject fatherBlockageObject;
        public GameObject webLayerCacoon;
        public GameObject webLayerFirst;
        public GameObject webLayerSecond;

        //Game Object Component
        public GameObject fatherGroundChecker;
        public GameObject fatherWallChecker;

        //Collision Size
        public Vector2 colliderSize;

        //Collision Checkers
        private FatherChecker checkerGround;
        private FatherChecker checkerWall;

        //Activate Counts
        private int activatedCount;

        //Speed Settings
        public float movementSpeed;
        public float jumpingSpeed;

        //Scale Settings
        private float fatherScaleX;

        //Duration Settings
        public float landingDurationSet;
        private float landingDurationCurrent;
        public float followingDurationSet;
        private float followingDurationCurrent;
        public float activatedDurationSet;
        private float activatedDurationCurrent;

        //Bool Checks
        private bool isOnGround;
        private bool isFacingWall;
        private bool isJumping;
        private bool isLanding;
        private bool isFollowing;
        private bool isUnstuck;
        private bool isDropping;
        private bool isWaking;

        //Ability Checks
        public bool isActivated;
        public bool canFollow;

        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        //Bool Timers
        private bool turnOnTimerLanding;
        private bool turnOnTimerFollowing;
        private bool turnOnTimerActivated;

        //Particles
        public GameObject webParticle1;
        public GameObject webParticle2;
        public GameObject webParticle3;

        void Update()
        {
            CheckComponents();
            CheckGround();
            CheckPlayerPosition();
            CheckTimer();
        }

        void FixedUpdate()
        {
            CheckPlayerPosition();
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
                boxCollider = this.gameObject.GetComponent<BoxCollider>();
                tongueInteractive = this.gameObject.GetComponent<TongueInteractive>();
                rb2D.velocity = new Vector2(0, 0);
                fatherScaleX = Math.Abs(this.gameObject.transform.localScale.x);
                mainPlayer = GameObject.GetGameObjectsOfTag("Player")[0];
                if (fatherGroundChecker != null)
                {
                    checkerGround = fatherGroundChecker.GetComponent<FatherChecker>();
                }
                if (fatherWallChecker != null)
                {
                    checkerWall = fatherWallChecker.GetComponent<FatherChecker>();
                }
                activatedCount = 0;
                hasInit = true;
            }
        }

        private void CheckPlayerPosition()
        {
            if (mainPlayer != null)
            {
                mainPlayerPosition = mainPlayer.transform.worldPosition;
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

        private void CheckDecision()
        {
            if (canFollow == false)
            {
                if (activatedCount < 4)
                {
                    if (activatedCount == 0)
                    {
                        fatherState = FatherState.TRAPPED_IDLE;
                    }
                    else if (activatedCount == 1)
                    {
                        fatherState = FatherState.TRAPPED_HIT_1;
                    }
                    else if (activatedCount == 2)
                    {
                        fatherState = FatherState.TRAPPED_HIT_2;
                    }
                    else if (activatedCount == 3)
                    {
                        fatherState = FatherState.TRAPPED_HIT_3;
                    }
                }
                else if (isUnstuck == false)
                {
                    fatherState = FatherState.TRAPPED_RELEASED;
                }
                else if (isWaking == false)
                {
                    fatherState = FatherState.TRAPPED_DROP;
                }
                else
                {
                    fatherState = FatherState.TRAPPED_WAKE;
                }
            }
            else
            {
                if ((isOnGround == true && rb2D.velocity.y <= 0.0f && isJumping == true) || (isLanding == true))
                {
                    fatherState = FatherState.LAND;
                }
                else if (isOnGround == false && rb2D.velocity.y <= -2.0f)
                {
                    fatherState = FatherState.FALL;
                }
                else if (isJumping == true)
                {
                    fatherState = FatherState.JUMPING;
                }
                else if (GetDistance(this.gameObject.transform.worldPosition, mainPlayerPosition) > 2.0f && isFollowing == true)
                {
                    if (isOnGround == true && isJumping == false && isFacingWall == true)
                    {
                        fatherState = FatherState.JUMP;
                    }
                    else
                    {
                        fatherState = FatherState.MOVE;
                    }
                }
                else
                {
                    fatherState = FatherState.IDLE;
                }
            }
        }

        private void CheckState()
        {
            switch (fatherState)
            {
                case FatherState.IDLE:
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                    if (mainPlayerPosition.x < this.gameObject.transform.worldPosition.x - 0.5f)
                    {
                        FlipX(-fatherScaleX);
                    }
                    else if (mainPlayerPosition.x > this.gameObject.transform.worldPosition.x + 0.5)
                    {
                        FlipX(fatherScaleX);
                    }
                    isJumping = false;
                    isFollowing = false;
                    turnOnTimerFollowing = true;
                    break;

                case FatherState.MOVE:
                    if (mainPlayerPosition.x < this.gameObject.transform.worldPosition.x)
                    {
                        HorizontalMovement(-fatherScaleX, movementSpeed);
                    }
                    else if (mainPlayerPosition.x > this.gameObject.transform.worldPosition.x)
                    {
                        HorizontalMovement(fatherScaleX, movementSpeed);
                    }
                    isJumping = false;
                    break;

                case FatherState.JUMP:
                    VerticalMovement(0, 1, jumpingSpeed);
                    isJumping = true;
                    break;

                case FatherState.JUMPING:
                    if (isFacingWall == false)
                    {
                        if (GetDistance(this.gameObject.transform.worldPosition, mainPlayerPosition) > 2.0f)
                        {
                            if (mainPlayerPosition.x < this.gameObject.transform.worldPosition.x)
                            {
                                HorizontalMovement(-fatherScaleX, movementSpeed);
                            }
                            else if (mainPlayerPosition.x > this.gameObject.transform.worldPosition.x)
                            {
                                HorizontalMovement(fatherScaleX, movementSpeed);
                            }
                        }
                    }
                    break;

                case FatherState.FALL:
                    if (isFacingWall == false)
                    {
                        if (GetDistance(this.gameObject.transform.worldPosition, mainPlayerPosition) > 2.0f)
                        {
                            if (mainPlayerPosition.x < this.gameObject.transform.worldPosition.x)
                            {
                                HorizontalMovement(-fatherScaleX, movementSpeed);
                            }
                            else if (mainPlayerPosition.x > this.gameObject.transform.worldPosition.x)
                            {
                                HorizontalMovement(fatherScaleX, movementSpeed);
                            }
                        }
                    }
                    isJumping = true;
                    break;

                case FatherState.LAND:
                    if (isLanding == false)
                    {
                        rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                        isJumping = false;
                        isLanding = true;
                        turnOnTimerLanding = true;
                    }
                    break;

                case FatherState.TRAPPED_IDLE:
                    rb2D.velocity = new Vector2(0, 0);
                    break;

                case FatherState.TRAPPED_HIT_1:
                    rb2D.velocity = new Vector2(0, 0);
                    if (webLayerCacoon.GetComponent<SpriteRenderer>().textureName != sheetCocoonFall)
                    {
                        webLayerCacoon.GetComponent<SpriteRenderer>().textureName = sheetCocoonFall;
                        webLayerCacoon.GetComponent<Animator>().fileName = sheetWebFall;
                        webLayerCacoon.GetComponent<SpriteRenderer>().currentFrameX = 1;
                        webLayerCacoon.GetComponent<SpriteRenderer>().currentFrameY = 1;
                        webLayerCacoon.GetComponent<Animator>().animateCount = 1;

                        //webParticle1 = GameObject.InstantiatePrefab("WebParticle1");
                        webParticle2 = GameObject.InstantiatePrefab("WebParticle2");
                        webParticle3 = GameObject.InstantiatePrefab("WebParticle3");
                    }
                    break;

                case FatherState.TRAPPED_HIT_2:
                    rb2D.velocity = new Vector2(0, 0);
                    if (webLayerFirst.GetComponent<SpriteRenderer>().textureName != sheetFirstLayerFall)
                    {
                        webLayerFirst.GetComponent<SpriteRenderer>().textureName = sheetFirstLayerFall;
                        webLayerFirst.GetComponent<Animator>().fileName = sheetWebFall;
                        webLayerFirst.GetComponent<SpriteRenderer>().currentFrameX = 1;
                        webLayerFirst.GetComponent<SpriteRenderer>().currentFrameY = 1;
                        webLayerFirst.GetComponent<Animator>().animateCount = 1;

                        //webParticle1 = GameObject.InstantiatePrefab("WebParticle1");
                        webParticle2 = GameObject.InstantiatePrefab("WebParticle2");
                        webParticle3 = GameObject.InstantiatePrefab("WebParticle3");
                    }
                    break;

                case FatherState.TRAPPED_HIT_3:
                    rb2D.velocity = new Vector2(0, 0);
                    if (webLayerSecond.GetComponent<SpriteRenderer>().textureName != sheetSecondLayerFall)
                    {
                        webLayerSecond.GetComponent<SpriteRenderer>().textureName = sheetSecondLayerFall;
                        webLayerSecond.GetComponent<Animator>().fileName = sheetWebFall;
                        webLayerSecond.GetComponent<SpriteRenderer>().currentFrameX = 1;
                        webLayerSecond.GetComponent<SpriteRenderer>().currentFrameY = 1;
                        webLayerSecond.GetComponent<Animator>().animateCount = 1;

                        //webParticle1 = GameObject.InstantiatePrefab("WebParticle1");
                        webParticle2 = GameObject.InstantiatePrefab("WebParticle2");
                        webParticle3 = GameObject.InstantiatePrefab("WebParticle3");
                    }
                    break;

                case FatherState.TRAPPED_RELEASED:
                    Destroy(fatherBlockageObject);
                    tongueInteractive.active = false;
                    boxCollider.width = colliderSize.x;
                    boxCollider.height = colliderSize.y;
                    rb2D.gravityScale = 3.0f;
                    rb2D.mass = 1.0f;
                    isUnstuck = true;
                    break;

                case FatherState.TRAPPED_DROP:
                    if (spriteRenderer.textureName == sheetFree01 && spriteRenderer.currentFrameX == spriteRenderer.maxFrameX && spriteRenderer.currentFrameY == spriteRenderer.maxFrameY)
                    {
                        isWaking = true;
                    }
                    break;

                case FatherState.TRAPPED_WAKE:
                    if (spriteRenderer.textureName == sheetFree02 && spriteRenderer.currentFrameX == spriteRenderer.maxFrameX && spriteRenderer.currentFrameY == spriteRenderer.maxFrameY)
                    {
                        canFollow = true;
                    }
                    break;
            }
        }

        private void CheckAnimation()
        {
            switch (fatherState)
            {
                case FatherState.IDLE:
                    ChangeSpriteAnimation(sheetIdle, animIdle, -1);
                    break;

                case FatherState.MOVE:
                    ChangeSpriteAnimation(sheetMove, animMove, -1);
                    break;

                case FatherState.JUMP:
                    break;

                case FatherState.JUMPING:
                    ChangeSpriteAnimation(sheetJump, animJump, 1);
                    break;

                case FatherState.FALL:
                    ChangeSpriteAnimation(sheetFall, animFall, 1);
                    break;

                case FatherState.LAND:
                    ChangeSpriteAnimation(sheetLand, animLand, 1);
                    break;

                case FatherState.TRAPPED_IDLE:
                    ChangeSpriteAnimation(sheetTrappedIdle, animTrappedIdle, -1);
                    break;

                case FatherState.TRAPPED_HIT_1:
                    ChangeSpriteAnimation(sheetTrapped01, animTrappedHit, -1);
                    break;

                case FatherState.TRAPPED_HIT_2:
                    ChangeSpriteAnimation(sheetTrapped02, animTrappedHit, -1);
                    break;

                case FatherState.TRAPPED_HIT_3:
                    ChangeSpriteAnimation(sheetTrapped03, animTrappedHit, -1);
                    break;

                case FatherState.TRAPPED_RELEASED:
                    break;

                case FatherState.TRAPPED_DROP:
                    ChangeSpriteAnimation(sheetFree01, animFreeDrop, 1);
                    break;

                case FatherState.TRAPPED_WAKE:
                    ChangeSpriteAnimation(sheetFree02, animFreeWake, 1);
                    break;
            }
            if (fatherStatePrevious != fatherState)
            {
                fatherStatePrevious = fatherState;
                spriteRenderer.currentFrameX = 1;
                spriteRenderer.currentFrameY = 1;
                spriteAnimator.animateCount = spriteAnimator.animateCount;
                //Console.WriteLine(fatherState);
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
                    Audio.PlaySource(audioName);
                }
            }
        }

        private void CheckTimer()
        {
            if (turnOnTimerLanding == true)
            {
                if (landingDurationCurrent < landingDurationSet)
                {
                    landingDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isLanding = false;
                    turnOnTimerLanding = false;
                    landingDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerFollowing == true)
            {
                if (followingDurationCurrent < followingDurationSet)
                {
                    followingDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isFollowing = true;
                    turnOnTimerFollowing = false;
                    followingDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerActivated == true)
            {
                if (activatedDurationCurrent < activatedDurationSet)
                {
                    activatedDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isActivated = false;
                    turnOnTimerActivated = false;
                    activatedDurationCurrent = 0.0f;
                }
            }
        }

        private void HorizontalMovement(float directionX, float speed)
        {
            FlipX(directionX);
            rb2D.velocity = new Vector2(directionX * speed, rb2D.velocity.y);
        }

        private void VerticalMovement(float directionX, float directionY, float speed)
        {
            rb2D.velocity = new Vector2(0, 0);
            rb2D.AddImpulse(new Vector2(directionX * speed, directionY * speed), new Vector2(0, 0));
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

        void OnCollisionStay(Collider collider)
        {
            if (isActivated == false && collider.gameObject.name == "TongueCollision")
            {
                if (activatedCount < 4)
                {
                    spriteRenderer.currentFrameX = 1;
                    spriteRenderer.currentFrameY = 1;
                    isActivated = true;
                    turnOnTimerActivated = true;
                    ++activatedCount;
                }
            }
        }
    }
}