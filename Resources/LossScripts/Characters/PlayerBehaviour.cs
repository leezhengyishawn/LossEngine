using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class PlayerBehaviour : LossBehaviour
    {
        //States
        public PlayerState playerState;
        public PlayerState playerStatePrevious;
        public enum PlayerState
        {
            IDLE,
            TURN,
            MOVE,
            STATIC_OPEN,
            JUMP_OPEN,
            JUMP,
            BOUNCE,
            WALL_SLIDE,
            WALL_JUMPING,
            WALL_JUMPED,
            WALL_STOP,
            JUMPING,
            SLIDE,
            SWING,
            SLING,
            FALL,
            LAND,
            DAMAGED,
            STUN,
            DEAD
        }

        //Components
        private SpriteRenderer spriteRenderer;
        private Animator spriteAnimator;
        private RigidBody rb2D;
        private Constraint hingedObject;

        //Getting Camera
        private GameObject camera;
        private CameraBehaviour cameraBehaviour;

        //Getting BGM Controller
        private GameObject bgmControllerObject;
        private BGMController bgmController;

        //Sprite Strings
        private string sheetIdle = "Character_Idle";
        private string sheetMove = "Character_Move";
        private string sheetJump = "Character_Jump";
        private string sheetSlide = "Character_Slide";
        private string sheetSwing = "Character_Swing";
        private string sheetWallJump = "Character_Wall_Jump";
        private string sheetFall = "Character_Fall";
        private string sheetLand = "Character_Land";
        private string sheetStun = "Character_stunned";
        private string sheetDeath = "Character_Death";
        private string sheetStaticOpen = "Character_Idle_Out";
        private string sheetJumpOpen = "Character_Jump_Out";

        //Anim Strings
        private string animIdle = "PlayerIdleAnim";
        private string animMove = "PlayerMoveAnim";
        private string animJump = "PlayerJumpAnim";
        private string animSlide = "PlayerSlideAnim";
        private string animSwing = "PlayerSwingAnim";
        private string animWallJump = "PlayerWallJumpAnim";
        private string animFall = "PlayerFallAnim";
        private string animLand = "PlayerLandAnim";
        private string animStun = "PlayerStunAnim";
        private string animDeath = "PlayerDeathAnim";
        private string animStaticOpen = "PlayerStaticOpen";
        private string animJumpOpen = "PlayerJumpOpen";

        //Player Tongue Positions
        public Vector3 tonguePositionNormal;
        public Vector3 tonguePositionStatic;
        public Vector3 tonguePositionJump;
        public Vector3 tonguePositionSwing;

        //Game Object Component
        public GameObject playerGroundChecker;
        public GameObject playerWallChecker;
        public GameObject playerHeadObject;
        public GameObject playerTongueObject;
        public GameObject playerTongueSprite;
        public GameObject playerTongueChecker;
        public GameObject mouseInteractiveChecker;
        public SpriteRenderer playerTongueSpriteRenderer;

        //Collision Checkers
        private PlayerCheckers checkerGround;
        private PlayerCheckers checkerWall;
        private PlayerTongueCollision checkerTongue;
        private MouseController checkerMouse;

        //Position Checkings
        private Vector3 mouseScreenPosition;
        private Vector3 checkpointPosition;
        private Vector3 tonguePositionTarget;
        private Vector3 tongueDirectionTarget;

        //Saved Values
        private int savedBGMValue;
        private float savedVisionValue;
        private bool savedVignetteOn;

        //Tongue Settings
        private GameObject tongueCurrentObject;
        private Vector3 tongueObjectPosition;
        private Vector3 tongueCurrentScale;
        private float tongueLengthTarget;
        public float tongueLengthMaximum;
        public float swingLengthMax;

        //Health Settings
        private int healthCurrent;
        public int healthMaximum;

        //Speed Settings
        public float movementSpeed;
        public float jumpingSpeed;
        public float wallJumpingSpeed;
        public float bouncingSpeed;
        public float slidingSpeed;
        public float swingingSpeed;
        public float slingingSpeed;
        public float tongueSpeed;
        private float wallSlidingSpeed;

        //Movement Settings
        private float horizontalMovementAxis;
        private float verticalMovementAxis;

        //Duration Settings
        public float tongueDurationSet;
        private float tongueDurationCurrent;
        public float landingDurationSet;
        private float landingDurationCurrent;
        public float launchingDurationSet;
        private float launchingDurationCurrent;
        public float slidingDurationSet;
        private float slidingDurationCurrent;
        public float slideCooldownDurationSet;
        private float slideCooldownDurationCurrent;
        public float stunDurationSet;
        private float stunDurationCurrent;
        public float invisDurationSet;
        private float invisDurationCurrent;
        public float flashDurationSet;
        private float flashDurationCurrent;
        public float respawnDurationSet;
        private float respawnDurationCurrent;

        private float turningDurationSet;
        private float turningDurationCurrent;

        //Ability Checks
        public bool canDie;
        public bool canControl;
        public bool canUseTongue;

        //Bool Checks
        private bool isTongueOut;
        private bool isTongueExtending;
        private bool isOnGround;
        private bool isOnBounce;
        private bool isFacingWall;
        private bool isFacingJumpWall;
        private bool isJumping;
        private bool isWallJumping;
        private bool isLaunching;
        private bool isLaunched;
        private bool isLanding;
        private bool isSliding;
        private bool isSwinging;
        private bool isSlinging;
        private bool isStunned;
        private bool isDamaged;
        private bool isInvis;
        private bool isFlashing;
        private bool isRespawning;
        private bool isTurning;
        private bool isFalling;

        public bool isGamepadControlled = false;

        private bool hasPressedJump;
        private bool hasReleasedJump;
        private bool canWallJump;

        private bool m_startShake = false;

        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        //Bool Timers
        private bool turnOnTimerTongue;
        private bool turnOnTimerLanding;
        private bool turnOnTimerLaunching;
        private bool turnOnTimerSliding;
        private bool turnOnTimerSlideCooldown;
        private bool turnOnTimerStun;
        private bool turnOnTimerInvis;
        private bool turnOnTimerFlashing;
        private bool turnOnTimerRespawn;
        private bool turnOnTimerTurning;

        //Particle
        public GameObject dustParticle;
        public GameObject grassParticle;
        public GameObject caveParticle;
        private GameObject tempParticle;

        public bool isGrass;

        //Gamepad - Written by Shawn
        private Vector3 lastTongueDir;

        void Update()
        {
            CheckComponents();
            CheckHealth();
            CheckGround();
            CheckPositions();
            CheckTonguePosition();
            CheckWallSlidingSpeed();
            if (canControl == true)
            {
                CheckInput();
            }
            CheckInvis();
            CheckTimer();
            CheckCamShake();
            //if (isGamepadControlled)
            //    Mouse.SetCursor("", true);
            //else
            //    Mouse.SetCursor("Normal_Cursor", true);
        }

        void FixedUpdate()
        {
            CheckDecision();
            CheckState();
            CheckPositions();
            CheckTongue();
            CheckAnimation();
        }

        private void CheckComponents()
        {
            if (hasInit == false)
            {
                spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
                spriteAnimator = this.gameObject.GetComponent<Animator>();
                rb2D = this.gameObject.GetComponent<RigidBody>();
                camera = GameObject.GetGameObjectsOfTag("MainCamera")[0];
                cameraBehaviour = camera.GetComponent<CameraBehaviour>();
                bgmControllerObject = GameObject.GetGameObjectsOfTag("BGMController")[0];
                bgmController = bgmControllerObject.GetComponent<BGMController>();
                if (playerGroundChecker != null)
                {
                    checkerGround = playerGroundChecker.GetComponent<PlayerCheckers>();
                }
                if (playerWallChecker != null)
                {
                    checkerWall = playerWallChecker.GetComponent<PlayerCheckers>();
                }
                if (playerTongueChecker != null)
                {
                    checkerTongue = playerTongueChecker.GetComponent<PlayerTongueCollision>();
                }
                if (mouseInteractiveChecker != null)
                {
                    checkerMouse = mouseInteractiveChecker.GetComponent<MouseController>();
                }
                if (playerTongueSprite != null)
                {
                    playerTongueSpriteRenderer = playerTongueSprite.GetComponent<SpriteRenderer>();
                }
                checkpointPosition = this.gameObject.transform.worldPosition;
                savedBGMValue = bgmController.GetBGM();
                savedVisionValue = cameraBehaviour.GetTargetFieldOfVision();
                savedVignetteOn = cameraBehaviour.GetIsVignetteFading();
                tongueCurrentScale = new Vector3(0, 1, 1);
                playerTongueObject.transform.localScale = tongueCurrentScale;
                rb2D.velocity = new Vector2(0, 0);
                playerTongueObject.active = false;
                turningDurationSet = 0.05f;
                lastTongueDir = new Vector3(0,0,0);
                hasInit = true;
            }
        }

        private void CheckPositions()
        {
            mouseScreenPosition = Camera.MouseToWorldPoint();
            playerTongueObject.transform.localPosition = GetPlayerHeadPosition();
        }

        private void CheckTonguePosition()
        {
            switch (playerState)
            {
                case PlayerState.STATIC_OPEN:
                    SetTonguePosition(tonguePositionStatic);
                    break;

                case PlayerState.JUMP_OPEN:
                    SetTonguePosition(tonguePositionJump);
                    break;

                case PlayerState.SWING:
                    SetTonguePosition(tonguePositionSwing);
                    break;

                default:
                    SetTonguePosition(tonguePositionNormal);
                    break;
            }
        }

        private void CheckWallSlidingSpeed()
        {
            switch (playerState)
            {
                case PlayerState.WALL_SLIDE:
                    if (wallSlidingSpeed < 2.0f)
                    {
                        wallSlidingSpeed += 0.5f * Time.deltaTime;
                    }
                    else
                    {
                        wallSlidingSpeed = 2.0f;
                    }
                    break;

                default:
                    wallSlidingSpeed = 0.0f;
                    break;
            }
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
                isOnBounce = checkerGround.GetCollisionStatus("BOUNCE");
            }
            if (checkerWall != null)
            {
                isFacingWall = checkerWall.GetCollisionStatus("WALL");
                isFacingJumpWall = checkerWall.GetCollisionStatus("JUMPWALL");
            }
        }

        private void CheckInput()
        {
            //Horizontal Movement
            if ((Input.GetKey(KEYCODE.KEY_A) && Input.GetKey(KEYCODE.KEY_D)) || (Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_LEFT, 0) && Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_RIGHT, 0))) //Pressing left and right together
            {
                horizontalMovementAxis = 0;
            }
            else if (Input.GetKey(KEYCODE.KEY_A) || Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_LEFT, 0) || Input.GetGamepadLeftStickX(0) < -GAMEPADDEADZONES.LEFT_THUMBSTICK) //Pressing left
            {
                horizontalMovementAxis = -1;
                isGamepadControlled = (Input.GetGamepadLeftStickX(0) < -GAMEPADDEADZONES.LEFT_THUMBSTICK) ? true : false;
            }
            else if (Input.GetKey(KEYCODE.KEY_D) || Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_RIGHT, 0) || Input.GetGamepadLeftStickX(0) > GAMEPADDEADZONES.LEFT_THUMBSTICK) //Pressing right
            {
                horizontalMovementAxis = 1;
                isGamepadControlled = (Input.GetGamepadLeftStickX(0) > GAMEPADDEADZONES.LEFT_THUMBSTICK) ? true : false;
            }
            else //No left or right input
            {
                horizontalMovementAxis = 0;
            }
            //Vertical Movement
            if (((Input.GetKey(KEYCODE.KEY_W) || Input.GetKey(KEYCODE.KEY_SPACE) || Input.GetMouse(1)) && Input.GetKey(KEYCODE.KEY_S)) || ((Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_UP, 0) || Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_A, 0)) && Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_DOWN, 0))) //Pressing up or space, and down together
            {
                verticalMovementAxis = 0;
            }
            else if ((Input.GetKey(KEYCODE.KEY_W) || Input.GetKey(KEYCODE.KEY_SPACE)) || (Input.GetMouse(1)) || (Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_UP, 0) || Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_A, 0)) || (Input.GetLeftTrigger(0) > 0)/*|| Input.GetGamepadLeftStickY(0) > GAMEPADDEADZONES.LEFT_THUMBSTICK*/) //Pressing up or space
            {
                verticalMovementAxis = 1;
                hasPressedJump = true;
                canWallJump = hasReleasedJump;
                isGamepadControlled = ((Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_UP, 0) || Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_A, 0)) || (Input.GetLeftTrigger(0) > 0)) ? true : false;
            }
            else if (Input.GetKey(KEYCODE.KEY_S) || Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_DOWN, 0) || Input.GetGamepadLeftStickY(0) < -(GAMEPADDEADZONES.LEFT_THUMBSTICK * 2.0f)) //Pressing down
            {
                verticalMovementAxis = -1;
                isGamepadControlled = (Input.GetGamepadButton(GAMEPADCODE.GAMEPAD_DOWN, 0) || Input.GetGamepadLeftStickY(0) < -(GAMEPADDEADZONES.LEFT_THUMBSTICK * 2.0f)) ? true : false;
            }
            else //No up or down input
            {
                verticalMovementAxis = 0;
                hasReleasedJump = hasPressedJump;
            }
            //Tongue
            if (Input.GetGamepadRightStickX(0) >= GAMEPADDEADZONES.RIGHT_THUMBSTICK || Input.GetGamepadRightStickY(0) >= GAMEPADDEADZONES.RIGHT_THUMBSTICK)
            {
                lastTongueDir = new Vector3(Input.GetGamepadRightStickX(0), Input.GetGamepadRightStickY(0), 0);
            }

            if (Input.GetMouse(0) && isTongueOut == false && canUseTongue == true && (playerState == PlayerState.IDLE || playerState == PlayerState.MOVE || playerState == PlayerState.JUMPING || playerState == PlayerState.FALL))
            {
                if (checkerMouse != null && checkerMouse.GetHover() == true)
                {
                    tonguePositionTarget = new Vector3(checkerMouse.GetHoverPosition().x, checkerMouse.GetHoverPosition().y, GetPlayerHeadPosition().z);
                }
                else
                {
                    tonguePositionTarget = new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, GetPlayerHeadPosition().z);
                }
                isTongueExtending = true;
                isTongueOut = true;
                canUseTongue = false;
                turnOnTimerTongue = true;
                Audio.PlaySource("SFX_Tongue_Out");
                isGamepadControlled = false;
            }
            else if (Input.GetRightTrigger(0) > 0 && isTongueOut == false && canUseTongue == true && (playerState == PlayerState.IDLE || playerState == PlayerState.MOVE || playerState == PlayerState.JUMPING || playerState == PlayerState.FALL))
            {
                // Get right analog stick for targeting tongue
                short rightAnalogDirX = Input.GetGamepadRightStickX(0);
                short rightAnalogDirY = Input.GetGamepadRightStickY(0);
                Vector3 tongueDirection;

                if ((Math.Abs(rightAnalogDirX+1) <= GAMEPADDEADZONES.RIGHT_THUMBSTICK && Math.Abs(rightAnalogDirY+1) <= GAMEPADDEADZONES.RIGHT_THUMBSTICK)) //If the right joystick hasn't moved, use the last dir
                {
                    tongueDirection = lastTongueDir;
                }
                else
                {
                    tongueDirection = new Vector3(rightAnalogDirX, rightAnalogDirY, 0.0f);
                    tongueDirection.Normalize();
                }
                //if ((Math.Abs(rightAnalogDirX) <= GAMEPADDEADZONES.RIGHT_THUMBSTICK && Math.Abs(rightAnalogDirY) <= GAMEPADDEADZONES.RIGHT_THUMBSTICK) || Math.Abs(rightAnalogDirX) == 0 && Math.Abs(rightAnalogDirY) == 0) tongueDirection.x = this.gameObject.transform.localScale.x;
                //else tongueDirection.Normalize();
                
                double angle = Math.Atan2((double)tongueDirection.y, (double)tongueDirection.x); //radians
                float xDir = (float)(Math.Cos(angle) / Math.PI) * 180.0f;
                float yDir = (float)(Math.Sin(angle) / Math.PI) * 180.0f;

                Vector3 headPosition = this.gameObject.transform.worldPosition;
                tonguePositionTarget = new Vector3(headPosition.x + (xDir * tongueLengthMaximum), headPosition.y + (yDir * tongueLengthMaximum), 0.0f);

                isTongueExtending = true;
                isTongueOut = true;
                canUseTongue = false;
                turnOnTimerTongue = true;
                Audio.PlaySource("SFX_Tongue_Out");
                isGamepadControlled = true;
            }
            else if (((!Input.GetMouse(0)) && Input.GetRightTrigger(0) == 0) && isTongueOut == true && isSwinging == true)
            {
                Physics.RemoveConstraint(hingedObject);
                hingedObject = null;
                isTongueExtending = false;
                isSwinging = false;
                turnOnTimerTongue = true;
            }
            //Cheat Codes
            if (Input.GetKey(KEYCODE.KEY_CONTROL) && Input.GetKey(KEYCODE.KEY_Q))
            {
                HealthFullRestore();
                canDie = false;
            }
        }

        private void CheckTongue()
        {
            if (isSwinging == true)
            {
                tongueDirectionTarget = tonguePositionTarget - GetPlayerHeadPosition();
                tongueDirectionTarget = new Vector3(tongueDirectionTarget.x, tongueDirectionTarget.y, 0).normalized;
                tongueCurrentScale = new Vector3(GetDistance(GetPlayerHeadPosition(), tonguePositionTarget), 1, 1);
                playerTongueObject.transform.localRotation = new Vector3(0, 0, (float)Math.Atan2(tongueDirectionTarget.y, tongueDirectionTarget.x) * (float)(180.0f / Math.PI));
            }
            else if (isTongueOut == true && isTongueExtending == true)
            {
                tongueDirectionTarget = tonguePositionTarget - GetPlayerHeadPosition();
                tongueDirectionTarget = new Vector3(tongueDirectionTarget.x, tongueDirectionTarget.y, 0).normalized;
                tongueLengthTarget = GetDistance(GetPlayerHeadPosition(), tonguePositionTarget);
                playerTongueObject.transform.localRotation = new Vector3(0, 0, (float)Math.Atan2(tongueDirectionTarget.y, tongueDirectionTarget.x) * (float)(180.0f / Math.PI));
                ExtendTongue();
            }
            else
            {
                RetractTongue();
            }
            playerTongueObject.transform.localScale = tongueCurrentScale;
        }

        private float GetDistance(Vector3 positionSelf, Vector3 positionTarget)
        {
            float x = positionTarget.x - positionSelf.x;
            float y = positionTarget.y - positionSelf.y;
            float distance = ((float)Math.Sqrt(x * x + y * y));
            return distance;
        }

        private void ExtendTongue()
        {
            playerTongueObject.active = true;
            if (checkerTongue != null && checkerTongue.GetCollisionSomething() == true && tongueCurrentObject == null)
            {
                tongueObjectPosition = checkerTongue.GetTouchedPosition();
                tongueCurrentObject = checkerTongue.GetTouchedObject();
                Audio.PlaySource("SFX_Tongue_Hit");
                if (checkerTongue.GetCollisionSling() == true)
                {
                    isSlinging = true;
                }
                if (checkerTongue.GetCollisionHinge() == true)
                {
                    tonguePositionTarget = checkerTongue.GetTouchedPosition();
                    if (tongueCurrentScale.x < swingLengthMax)
                    {
                        hingedObject = Physics.AddFixedDistanceConstraint(rb2D, tongueCurrentScale.x, new Vector2(tonguePositionTarget.x, tonguePositionTarget.y), new Vector2(tonguePositionSwing.x, tonguePositionSwing.y));
                    }
                    else
                    {
                        hingedObject = Physics.AddFixedDistanceConstraint(rb2D, swingLengthMax, new Vector2(tonguePositionTarget.x, tonguePositionTarget.y), new Vector2(tonguePositionSwing.x, tonguePositionSwing.y));
                    }
                    isSwinging = true;
                    Audio.PlaySource("SFX_State_Hinge");
                }
                else
                {
                    isTongueExtending = false;
                }
            }
            else
            {
                if (tongueCurrentScale.x < tongueLengthTarget && tongueCurrentScale.x < tongueLengthMaximum)
                {
                    tongueCurrentScale = Vector3.Lerp(tongueCurrentScale, new Vector3(tongueLengthMaximum + 0.5f, 1, 1), 0.2f);
                }
                else
                {
                    isTongueExtending = false;
                }
            }
        }

        private void RetractTongue()
        {
            if (tongueCurrentScale.x > 0)
            {
                tongueCurrentScale = Vector3.Lerp(tongueCurrentScale, new Vector3(-0.5f, 1, 1), 0.1f);
            }
            else
            {
                checkerTongue.ResetCollision();
                tongueCurrentScale = new Vector3(0, 1, 1);
                playerTongueObject.transform.localScale = tongueCurrentScale;
                playerTongueObject.active = false;
                tongueCurrentObject = null;
                isTongueOut = false;
            }
        }

        private void CheckDecision()
        {
            if (healthCurrent <= 0 && canDie == true)
            {
                playerState = PlayerState.DEAD;
            }
            else if (isDamaged == true)
            {
                playerState = PlayerState.DAMAGED;
            }
            else if (isStunned == true)
            {
                playerState = PlayerState.STUN;
            }
            else if ((isOnGround == true && rb2D.velocity.y <= 0 && isJumping == true && isSwinging == false && isSlinging == false) || (isLanding == true))
            {
                playerState = PlayerState.LAND;
            }
            else if (isSwinging == true)
            {
                playerState = PlayerState.SWING;
            }
            else if (isSlinging == true)
            {
                playerState = PlayerState.SLING;
            }
            else if (isOnBounce == true && rb2D.velocity.y <= 0)
            {
                playerState = PlayerState.BOUNCE;
            }





            else if (isLaunched == true)
            {
                playerState = PlayerState.WALL_JUMPED; //Launch jump
            }
            else if (isOnGround == false && isJumping == true && isFacingJumpWall == true && isWallJumping == false && isTongueOut == false && ((isLaunching == true) || (verticalMovementAxis > 0 && canWallJump == true)))
            {
                playerState = PlayerState.WALL_JUMPING; //Delay before launching jump
            }
            else if (isOnGround == false && isJumping == true && isFacingJumpWall == true && isWallJumping == false && isTongueOut == false && horizontalMovementAxis != 0 && isFalling == true && horizontalMovementAxis == FacingDirection())
            {
                playerState = PlayerState.WALL_SLIDE; //Wall sliding
            }
            else if (isOnGround == false && isJumping == true && isFacingJumpWall == true && isWallJumping == false && isTongueOut == false && horizontalMovementAxis != 0 && isFalling == true && horizontalMovementAxis != FacingDirection())
            {
                playerState = PlayerState.WALL_STOP; //Move away from wall
            }





            else if (isOnGround == false && rb2D.velocity.y <= -2.0f)
            {
                if (isTongueOut == true)
                {
                    playerState = PlayerState.JUMP_OPEN;
                }
                else
                {
                    playerState = PlayerState.FALL;
                }
            }
            else if ((isOnGround == true && verticalMovementAxis < 0 && isJumping == false && isTongueOut == false && isOnBounce == false && turnOnTimerSlideCooldown == false) || (isSliding == true))
            {
                playerState = PlayerState.SLIDE;
            }
            else if (isOnGround == true && verticalMovementAxis > 0 && isJumping == false)
            {
                playerState = PlayerState.JUMP;
            }
            else if (isJumping == true)
            {
                if (isTongueOut == true)
                {
                    playerState = PlayerState.JUMP_OPEN;
                }
                else
                {
                    playerState = PlayerState.JUMPING;
                }
            }
            else if (horizontalMovementAxis != 0 && horizontalMovementAxis != FacingDirection() && isTurning == false)
            {
                playerState = PlayerState.TURN;
            }
            else if (horizontalMovementAxis != 0 && isTurning == false)
            {
                if (isTongueOut == true)
                {
                    playerState = PlayerState.STATIC_OPEN;
                }
                else
                {
                    playerState = PlayerState.MOVE;
                }
            }
            else
            {
                if (isTongueOut == true)
                {
                    playerState = PlayerState.STATIC_OPEN;
                }
                else
                {
                    playerState = PlayerState.IDLE;
                }
            }
        }

        private void CheckState()
        {
            switch (playerState)
            {
                case PlayerState.IDLE:
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                    JumpState(false, false);
                    SetCanUseTongue();
                    break;

                case PlayerState.TURN:
                    FlipX(horizontalMovementAxis);
                    JumpState(false, false);
                    SetCanUseTongue();
                    isTurning = true;
                    turnOnTimerTurning = true;
                    break;

                case PlayerState.MOVE:
                    HorizontalMovement(horizontalMovementAxis, movementSpeed);
                    JumpState(false, false);
                    SetCanUseTongue();
                    if (!isGrass)
                    {
                        PlayParticle(caveParticle);                    
                        if (!(Audio.IsSourcePlaying("SFX_State_Running")))
                        {
                        Audio.PlaySource("SFX_State_Running");
                        }
                    }
                    else
                    {
                        PlayParticle(grassParticle);
                        if (!(Audio.IsSourcePlaying("SFX_Leaves")))
                        {
                            Audio.PlaySource("SFX_Leaves");
                        }
                    }

                    break;

                case PlayerState.STATIC_OPEN:
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                    JumpState(false, false);
                    break;

                case PlayerState.JUMP_OPEN:
                    if (horizontalMovementAxis != 0 && isWallJumping == false)
                    {
                        if (isFacingWall == false || (isFacingWall == true && horizontalMovementAxis != FacingDirection()))
                        {
                            HorizontalMovement(horizontalMovementAxis, movementSpeed);
                        }
                    }
                    break;

                case PlayerState.JUMP:
                    VerticalMovement(0, 1, jumpingSpeed);
                    JumpState(true, false);

                    break;

                case PlayerState.BOUNCE:
                    VerticalMovement(0, 1, bouncingSpeed);
                    JumpState(true, false);
                    PlayParticle(dustParticle);
                    Audio.PlaySource("SFX_State_Bounce");
                    break;

                case PlayerState.WALL_SLIDE:
                    WallMovement(FacingDirection());
                    JumpState(true, false);
                    isLaunching = false;
                    PlayParticle(dustParticle);
                    break;

                case PlayerState.WALL_JUMPING:
                    if (isLaunching == false)
                    {
                        turnOnTimerLaunching = true;
                        isLaunching = true;
                    }
                    WallMovement(FacingDirection());
                    JumpState(true, false);
                    if (!isGrass)
                    {
                        PlayParticle(caveParticle);
                    }
                    else
                    {
                        PlayParticle(grassParticle);
                    }
                    break;

                case PlayerState.WALL_JUMPED:
                    FlipX(-FacingDirection());
                    VerticalMovement(FacingDirection(), 1.5f, wallJumpingSpeed);
                    JumpState(true, true);
                    isLaunched = false;
                    if (!isGrass)
                    {
                        PlayParticle(caveParticle);
                        Audio.PlaySource("SFX_State_Running");
                    }
                    else
                    {
                        PlayParticle(grassParticle);
                        Audio.PlaySource("SFX_Leaves");
                    }
                    break;

                case PlayerState.WALL_STOP:
                    FlipX(-FacingDirection());
                    VerticalMovement(FacingDirection(), 1.0f, wallJumpingSpeed * 0.5f);
                    JumpState(true, false);
                    isLaunched = false;
                    if (!isGrass)
                    {
                        PlayParticle(caveParticle);
                        Audio.PlaySource("SFX_State_Running");
                    }
                    else
                    {
                        PlayParticle(grassParticle);
                        Audio.PlaySource("SFX_Leaves");
                    }
                    break;

                case PlayerState.JUMPING:
                    if (horizontalMovementAxis != 0 && isWallJumping == false)
                    {
                        if (isFacingWall == false || (isFacingWall == true && horizontalMovementAxis != FacingDirection()))
                        {
                            HorizontalMovement(horizontalMovementAxis, movementSpeed);
                        }
                    }
                    break;

                case PlayerState.SLIDE:
                    HorizontalMovement(FacingDirection(), slidingSpeed);
                    if (isSliding == false)
                    {
                        JumpState(false, false);
                        isSliding = true;
                        turnOnTimerSliding = true;
                        turnOnTimerSlideCooldown = true;
                        Audio.PlaySource("SFX_State_Sliding");
                    }
                    if (!isGrass)
                    {
                        PlayParticle(caveParticle);
                    }
                    else
                    {
                        PlayParticle(grassParticle);
                    }
                    PlayParticle(dustParticle);
                    break;

                case PlayerState.SWING:
                    if (horizontalMovementAxis != 0)
                    {
                        if (GetPlayerHeadPosition().y < tongueObjectPosition.y - (tongueCurrentScale.x * 0.5f))
                        {
                            SwingingMovement(horizontalMovementAxis, 0, swingingSpeed);
                        }
                    }
                    if (GetPlayerHeadPosition().x < tongueObjectPosition.x - 0.3f)
                    {
                        FlipX(1);
                    }
                    else if (GetPlayerHeadPosition().x > tongueObjectPosition.x + 0.3f)
                    {
                        FlipX(-1);
                    }
                    JumpState(true, false);
                    break;

                case PlayerState.SLING:
                    VerticalMovement(tongueDirectionTarget.x, tongueDirectionTarget.y, slingingSpeed);
                    if (GetPlayerHeadPosition().x < tongueObjectPosition.x)
                    {
                        FlipX(1);
                    }
                    else if (GetPlayerHeadPosition().x > tongueObjectPosition.x)
                    {
                        FlipX(-1);
                    }
                    if (tongueCurrentObject.GetComponent<Dandelion>() != null)
                    {
                        tongueCurrentObject.GetComponent<Dandelion>().SpawnParticle();
                    }
                    JumpState(true, false);
                    turnOnTimerTongue = true;
                    isSlinging = false;
                    Audio.PlaySource("SFX_State_Sling");
                    break;

                case PlayerState.FALL:
                    if (horizontalMovementAxis != 0 && isWallJumping == false)
                    {
                        if (isFacingWall == false || (isFacingWall == true && horizontalMovementAxis != FacingDirection()))
                        {
                            HorizontalMovement(horizontalMovementAxis, movementSpeed);
                        }
                    }
                    if (rb2D.velocity.y < -10.0f)
                    {
                        rb2D.velocity = new Vector2(rb2D.velocity.x / 1.5f, -10.0f);
                    }
                    JumpState(true, false);
                    isFalling = true;
                    break;

                case PlayerState.LAND:
                    if (isLanding == false)
                    {
                        rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                        JumpState(false, false);
                        ResetWallJumpInput();
                        isFalling = false;
                        SetCanUseTongue();
                        isLaunching = false;
                        isLaunched = false;
                        turnOnTimerLaunching = false;
                        launchingDurationCurrent = 0.0f;
                        if (horizontalMovementAxis != 0)
                        {
                            landingDurationCurrent = landingDurationSet;
                        }
                        turnOnTimerLanding = true;
                        isLanding = true;
                    }
                    if (!isGrass)
                    {
                        PlayParticle(caveParticle);
                        if (!(Audio.IsSourcePlaying("SFX_State_Running")))
                        {
                            Audio.PlaySource("SFX_State_Running");
                        }
                    }
                    else
                    {
                        PlayParticle(grassParticle);
                        if (!(Audio.IsSourcePlaying("SFX_Leaves")))
                        {
                            Audio.PlaySource("SFX_Leaves");
                        }
                    }
                    PlayParticle(dustParticle);
                    break;

                case PlayerState.DAMAGED:
                    isDamaged = false;
                    Audio.PlaySource("SFX_State_Damaged");
                    Audio.PlaySource("SFX_Talk_Damaged");
                    break;

                case PlayerState.STUN:
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                    break;

                case PlayerState.DEAD:
                    if (isRespawning == false)
                    {
                        Physics.RemoveConstraint(hingedObject);
                        hingedObject = null;
                        isTongueExtending = false;
                        JumpState(false, false);
                        isSwinging = false;
                        isRespawning = true;
                        turnOnTimerRespawn = true;
                        Audio.PlaySource("SFX_State_Damaged");
                        Audio.PlaySource("SFX_Talk_Damaged");
                    }
                    if (turnOnTimerRespawn == true && respawnDurationCurrent < 1.2f && 0.2f < respawnDurationCurrent)
                    {
                        PlayParticle(dustParticle);

                        if (!isGrass)
                        {
                            PlayParticle(caveParticle);
                        }
                        else
                        {
                            PlayParticle(grassParticle);
                        }
                    }
                    break;
            }
        }

        private void CheckAnimation()
        {
            switch (playerState)
            {
                case PlayerState.IDLE:
                    ChangeSpriteAnimation(sheetIdle, animIdle, -1);
                    break;

                case PlayerState.TURN:
                    ChangeSpriteAnimation(sheetIdle, animIdle, -1);
                    break;

                case PlayerState.MOVE:
                    ChangeSpriteAnimation(sheetMove, animMove, -1);
                    break;

                case PlayerState.STATIC_OPEN:
                    ChangeSpriteAnimation(sheetStaticOpen, animStaticOpen, 1);
                    break;

                case PlayerState.JUMP_OPEN:
                    ChangeSpriteAnimation(sheetJumpOpen, animJumpOpen, 1);
                    break;

                case PlayerState.JUMP:
                    Audio.PlaySource("SFX_State_Jumping");
                    break;

                case PlayerState.BOUNCE:
                    break;

                case PlayerState.WALL_SLIDE:
                case PlayerState.WALL_STOP:
                case PlayerState.WALL_JUMPING:
                case PlayerState.WALL_JUMPED:
                    ChangeSpriteAnimation(sheetWallJump, animWallJump, 1, "SFX_State_Running");
                    break;

                case PlayerState.JUMPING:
                    ChangeSpriteAnimation(sheetJump, animJump, 1);
                    break;

                case PlayerState.SLIDE:
                    ChangeSpriteAnimation(sheetSlide, animSlide, 1);
                    break;

                case PlayerState.SWING:
                case PlayerState.SLING:
                    ChangeSpriteAnimation(sheetSwing, animSwing, 1);
                    break;

                case PlayerState.FALL:
                    ChangeSpriteAnimation(sheetFall, animFall, 1);
                    break;

                case PlayerState.LAND:
                    ChangeSpriteAnimation(sheetLand, animLand, 1);
                    break;

                case PlayerState.DAMAGED:
                    ChangeSpriteAnimation(sheetJumpOpen, animJumpOpen, 1);
                    break;

                case PlayerState.STUN:
                    ChangeSpriteAnimation(sheetStun, animStun, 1);
                    break;

                case PlayerState.DEAD:
                    ChangeSpriteAnimation(sheetDeath, animDeath, 1, "SFX_RockDropForest");
                    break;
            }
            if (playerStatePrevious != playerState)
            {
                playerStatePrevious = playerState;
                if (playerState != PlayerState.WALL_JUMPING && playerState != PlayerState.WALL_SLIDE && playerState != PlayerState.WALL_STOP && playerState != PlayerState.WALL_JUMPED)
                {
                    spriteRenderer.currentFrameX = 1;
                    spriteRenderer.currentFrameY = 1;
                    spriteAnimator.animateCount = spriteAnimator.animateCount;
                }
                //Console.WriteLine(playerState);
            }
        }

        private void PlayParticle(GameObject particle)
        {
            particle.GetComponent<ParticleEmitter>().Play();
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
            playerTongueSpriteRenderer.r = spriteRenderer.r;
            playerTongueSpriteRenderer.g = spriteRenderer.g;
            playerTongueSpriteRenderer.b = spriteRenderer.b;
        }

        private void SetCanUseTongue()
        {
            canUseTongue = true;
            turnOnTimerTongue = false;
            tongueDurationCurrent = 0.0f;
        }

        private void CheckTimer()
        {
            if (turnOnTimerTongue == true)
            {
                if (tongueDurationCurrent < tongueDurationSet)
                {
                    tongueDurationCurrent += Time.deltaTime;
                }
                else
                {
                    SetCanUseTongue();
                }
            }
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
            if (turnOnTimerLaunching == true)
            {
                if (launchingDurationCurrent < launchingDurationSet)
                {
                    launchingDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isLaunching = false;
                    isLaunched = true;
                    turnOnTimerLaunching = false;
                    launchingDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerSliding == true)
            {
                if (slidingDurationCurrent < slidingDurationSet)
                {
                    slidingDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isSliding = false;
                    turnOnTimerSliding = false;
                    slidingDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerSlideCooldown == true)
            {
                if (slideCooldownDurationCurrent < slideCooldownDurationSet)
                {
                    slideCooldownDurationCurrent += Time.deltaTime;
                }
                else
                {
                    turnOnTimerSlideCooldown = false;
                    slideCooldownDurationCurrent = 0.0f;
                }
            }
            if (turnOnTimerStun == true)
            {
                if (stunDurationCurrent < stunDurationSet)
                {
                    stunDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isStunned = false;
                    turnOnTimerStun = false;
                    stunDurationCurrent = 0.0f;
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
            if (turnOnTimerRespawn == true)
            {
                if (respawnDurationCurrent < respawnDurationSet)
                {
                    respawnDurationCurrent += Time.deltaTime;
                }
                else
                {
                    SendToCheckPoint(checkpointPosition);
                    HealthFullRestore();
                    bgmController.SetBGM(savedBGMValue);
                    cameraBehaviour.ResetFieldOfVision(savedVisionValue, 1.0f);
                    cameraBehaviour.ResetVignette(savedVignetteOn, 1.0f);
                    isRespawning = false;
                    turnOnTimerRespawn = false;
                    respawnDurationCurrent = 0.0f;
                    SpawnParticle("RespawnParticle", this.gameObject, -0.8f);
                    SpawnParticle("RespawnParticle2", this.gameObject, 0f);
                    SpawnParticle("RespawnParticle3", this.gameObject, 0f);
                    Audio.PlaySource("SFX_Poof");
                }
            }
            if (turnOnTimerTurning == true)
            {
                if (turningDurationCurrent < turningDurationSet)
                {
                    turningDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isTurning = false;
                    turnOnTimerTurning = false;
                    turningDurationCurrent = 0.0f;
                }
            }
        }

        private void CheckCamShake()
        {
            if (m_startShake)
                camera.GetComponent<CameraBehaviour>().SetShake(0.5f, 0.5f, 0.05f, 0.05f);
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

        private void SwingingMovement(float directionX, float directionY, float speed)
        {
            rb2D.AddImpulse(new Vector2(directionX * speed, directionY * speed), new Vector2(0, 0));
        }

        private void WallMovement(float directionX)
        {
            FlipX(directionX);
            rb2D.velocity = new Vector2(directionX, -wallSlidingSpeed);
        }

        private float FacingDirection()
        {
            return this.gameObject.transform.localScale.x;
        }

        private void FlipX(float directionX)
        {
            this.gameObject.transform.localScale = new Vector3(directionX, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }

        private void JumpState(bool jump, bool wallJump)
        {
            isJumping = jump;
            isWallJumping = wallJump;
        }

        private void SetTonguePosition(Vector3 tonguePosition)
        {
            playerHeadObject.transform.localPosition = tonguePosition;
        }

        public Vector3 GetPlayerHeadPosition()
        {
            return playerHeadObject.transform.worldPosition;
        }

        public void HealthDecrease(bool canKnockback, float impactX)
        {
            if (healthCurrent > 0 && isInvis == false)
            {
                healthCurrent -= 1;

                if (canKnockback == true)
                {
                    if (this.gameObject.transform.worldPosition.x < impactX)
                    {
                        VerticalMovement(-0.5f, 0.5f, jumpingSpeed);
                    }
                    else if (this.gameObject.transform.worldPosition.x > impactX)
                    {
                        VerticalMovement(0.5f, 0.5f, jumpingSpeed);
                    }
                    else
                    {
                        VerticalMovement(FacingDirection() * -0.5f, 0.5f, jumpingSpeed);
                    }
                    JumpState(true, false);
                }
                isDamaged = true;
                isInvis = true;
                turnOnTimerInvis = true;
                cameraBehaviour.NotifyHealthHeart(healthCurrent);
                SpawnParticle("DamagedParticle", this.gameObject, 0f);
            }
        }

        public void StunPlayer(bool canKnockback, float impactX)
        {
            if (healthCurrent > 0 && isInvis == false)
            {
                if (canKnockback == true && playerState != PlayerState.WALL_SLIDE)
                {
                    if (this.gameObject.transform.worldPosition.x < impactX)
                    {
                        VerticalMovement(-0.5f, 0.5f, jumpingSpeed);
                    }
                    else if (this.gameObject.transform.worldPosition.x > impactX)
                    {
                        VerticalMovement(0.5f, 0.5f, jumpingSpeed);
                    }
                    else
                    {
                        VerticalMovement(FacingDirection() * -0.5f, 0.5f, jumpingSpeed);
                    }
                    JumpState(true, false);
                }
                isStunned = true;
                turnOnTimerStun = true;
            }
        }

        public void HealthIncrease()
        {
            healthCurrent += 1;
            cameraBehaviour.NotifyHealthHeart(healthCurrent);
        }

        private void SpawnParticle(string particleName, GameObject spawnPoint, float y)
        {
            tempParticle = GameObject.InstantiatePrefab(particleName);
            tempParticle.transform.localPosition = new Vector3(spawnPoint.transform.worldPosition.x, spawnPoint.transform.worldPosition.y + y, spawnPoint.transform.worldPosition.z + 0.01f);
        }

        public void HealthFullRestore()
        {
            healthCurrent = healthMaximum;
            cameraBehaviour.NotifyHealthHeart(healthCurrent);
        }

        public void SendToCheckPoint(Vector3 position)
        {
            cameraBehaviour.CameraOffsetReset();
            Physics.RemoveConstraint(hingedObject);
            hingedObject = null;
            isTongueExtending = false;
            JumpState(false, false);
            isSwinging = false;
            rb2D.velocity = new Vector2(0, 0);
            this.gameObject.transform.localPosition = new Vector3(position.x, position.y, this.gameObject.transform.worldPosition.z);
            rb2D.velocity = new Vector2(0, 0);
        }



        public void SetCanControl(bool control)
        {
            canControl = control;
            horizontalMovementAxis = 0;
            verticalMovementAxis = 0;
        }

        public bool GetCanControl()
        {
            return canControl;
        }

        public void ResetSlideCooldown()
        {
            if (slidingDurationCurrent > slidingDurationSet/1.1f)
            {
                JumpState(false, false);
                isSliding = true;
                turnOnTimerSliding = true;
                turnOnTimerSlideCooldown = true;
                slidingDurationCurrent = slidingDurationSet / 1.1f;
                slideCooldownDurationCurrent = 0.0f;
            }
        }

        private void ResetWallJumpInput()
        {
            hasPressedJump = false;
            hasReleasedJump = false;
            canWallJump = false;
        }

        void OnCollisionStay(Collider collider)
        {
            if (collider.gameObject.tag != "IgnoreCollision")
            {
                if (healthCurrent > 0)
                {
                    if (collider.gameObject.tag == "Checkpoint")
                    {
                        checkpointPosition = collider.gameObject.transform.worldPosition;
                        savedBGMValue = bgmController.GetBGM();
                        savedVisionValue = cameraBehaviour.GetTargetFieldOfVision();
                        savedVignetteOn = cameraBehaviour.GetIsVignetteFading();
                    }
                    if (isInvis == false)
                    {
                        if (collider.gameObject.tag == "Enemy" || (collider.gameObject.tag == "Thorn" && isSliding == false))
                        {
                            HealthDecrease(true, collider.gameObject.transform.worldPosition.x);
                        }
                        if (collider.gameObject.tag == "Spike")
                        {
                            if (collider.gameObject.GetComponent<RespawnPoint>() != null && collider.gameObject.GetComponent<RespawnPoint>().SpawnPointObject() != null && collider.gameObject.GetComponent<RespawnPoint>().active == true)
                            {
                                HealthDecrease(false, collider.gameObject.transform.worldPosition.x);
                                SendToCheckPoint(collider.gameObject.GetComponent<RespawnPoint>().SpawnPointObject().transform.worldPosition);
                            }
                            else
                            {
                                HealthDecrease(true, collider.gameObject.transform.worldPosition.x);
                            }
                        }
                    }
                    if (collider.gameObject.tag == "EventTrigger")
                    {
                        if (collider.gameObject.GetComponent<EventTrigger>().GetIsActivated() == false)
                        {
                            SetCanControl(false);
                            collider.gameObject.GetComponent<EventTrigger>().ActivateEvent();
                        }
                    }
                    if (collider.gameObject.tag == "RoomTransition")
                    {
                        if (collider.gameObject.GetComponent<RoomTransition>().GetIsActivated() == false)
                        {
                            SetCanControl(false);
                            if (this.gameObject.transform.worldPosition.x < collider.gameObject.transform.worldPosition.x) //If player is on the left of the trigger
                            {
                                horizontalMovementAxis = 1;
                            }
                            else
                            {
                                horizontalMovementAxis = -1;
                            }
                            playerState = PlayerState.MOVE;
                            collider.gameObject.GetComponent<RoomTransition>().ActivateTransition();
                        }
                    }
                    if (collider.gameObject.tag == "HiddenRoom")
                    {
                        collider.gameObject.GetComponent<Fadeout>().FadeOverlayToBlack(false);
                    }
                    if (collider.gameObject.tag == "CameraOffset")
                    {
                        cameraBehaviour.CameraOffsetSet(collider.gameObject.GetComponent<CameraOffset>().cameraOffsetSetting);
                    }
                }
            }
        }

        void OnCollisionExit(Collider collider)
        {
            if (collider.gameObject.tag != "IgnoreCollision")
            {
                if (healthCurrent > 0)
                {
                    if (collider.gameObject.tag == "CameraZone")
                    {
                        CameraZone cameraZone = collider.gameObject.GetComponent<CameraZone>();
                        cameraBehaviour.SetFieldOfVision(cameraZone.GetPlayerFacingVision(FacingDirection()), cameraZone.GetPlayerFacingSpeed(FacingDirection()));
                        if (cameraZone.GetAllowVignetteChange() == true)
                        {
                            cameraBehaviour.FadeVignetteToBlack(cameraZone.GetPlayerFacingVignette(FacingDirection()), 0.0f, 1.0f, 0.4f);
                        }
                        if (cameraZone.GetAllowBGMChange() == true)
                        {
                            bgmController.SetBGM(cameraZone.GetPlayerFacingBGM(FacingDirection()));
                        }
                    }
                    if (collider.gameObject.tag == "HiddenRoom")
                    {
                        collider.gameObject.GetComponent<Fadeout>().FadeOverlayToBlack(true);
                    }
                }
                if (collider.gameObject.tag == "CameraOffset")
                {
                    cameraBehaviour.CameraOffsetReset();
                }
            }
        }
    }
}