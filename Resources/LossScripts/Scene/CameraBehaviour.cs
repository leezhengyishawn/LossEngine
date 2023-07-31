using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class CameraBehaviour : LossBehaviour
    {
        //Getting player
        private GameObject mainPlayer;
        private PlayerBehaviour mainPlayerBehaviour;

        //Offset
        public Vector3 cameraOffset = new Vector3(0.0f, 0.6f, 0.0f);
        private Vector3 cameraOffsetDefault;

        //Getting Overlay Object
        public GameObject blackVignette;
        public GameObject blackOverlay;
        public GameObject whiteOverlay;
        public GameObject blackBarTop;
        public GameObject blackBarBot;

        //Black Bar Positions
        private Vector3 blackBarTopSpawnPos;
        private Vector3 blackBarBotSpawnPos;
        private Vector3 blackBarTopHidePos;
        private Vector3 blackBarBotHidePos;
        private float blackBarLerpFragmentSpeed = 0.2f;

        //Getting Health Object
        public GameObject healthUIParent;
        public GameObject healthHeart0;
        public GameObject healthHeart1;
        public GameObject healthHeart2;
        public GameObject healthHeart3;
        public GameObject healthHeart4;
        private string sheetFullHeart = "Health_Firefly_On";
        private string sheetEmptyHeart = "Health_Firefly_Off";

        //Getting Renderer
        private SpriteRenderer blackVignetteRenderer;
        private SpriteRenderer blackOverlayRenderer;
        private SpriteRenderer whiteOverlayRenderer;

        //Mouse Positions
        private Vector3 mouseScreenPosition;
        private Vector3 mouseScreenDirection;
        private Vector3 mouseScreenMagnitude;
        private float mouseScreenDistance;

        //Position Settings
        private Vector3 transformLerp;
        private Vector3 zoomLerp;
        private float transformX;
        private float transformY;
        private float transformXYSpeed;

        //Zoom Settings
        public float transformZ;
        public float transformZTarget;
        public float transformZSpeed;
        public float transformZNormal;
        public float transformZZoomIn;
        public float transformZZoomOut;

        //Fade Settings
        private float fadeSpeedVignette;
        private float fadeValueMinVignette;
        private float fadeValueMaxVignette;
        private float fadeSpeedBlackOverlay;
        private float fadeValueMinBlackOverlay;
        private float fadeValueMaxBlackOverlay;
        private float fadeSpeedWhiteOverlay;
        private float fadeValueMinWhiteOverlay;
        private float fadeValueMaxWhiteOverlay;

        //Duration Settings
        public float loadingDurationSet;
        private float loadingDurationCurrent;

        //Bool Settings
        public bool turnOnFullBlack;
        public bool turnOnSelfBlackFading;
        public bool turnOnFullWhite;
        public bool turnOnSelfWhiteFading;
        public bool turnOnVignetteForever;
        public bool turnOnMainGameCinematics;

        //Event Trigger
        private bool isDeadLock;// = true;

        //Bool Checks
        private bool isVignetteFadingToBlack;
        private bool isOverlayFadingToBlack;
        private bool isOverlayFadingToWhite;
        private bool isCutscene;

        //Screen Shake
        private float shakeTimer = 0.0f;
        private float trauma = 2.0f;
        private float maxOffsetX = 0.25f;
        private float maxOffsetY = 0.25f;

        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        void Update()
        {
            CheckInit();
            CheckMousePosition();
            CheckVignette();
            CheckBlackOverlay();
            CheckWhiteOverlay();
            CheckBlackBar();
            CheckTimer();
        }

        void FixedUpdate()
        {
            CheckMousePosition();
            CheckMovement();
            CheckZoom();
        }

        private void CheckInit()
        {
            if (hasInit == false)
            {
                if (blackVignette != null)
                {
                    blackVignetteRenderer = blackVignette.GetComponent<SpriteRenderer>();
                }
                if (blackOverlay != null)
                {
                    blackOverlayRenderer = blackOverlay.GetComponent<SpriteRenderer>();
                }
                if (whiteOverlay != null)
                {
                    whiteOverlayRenderer = whiteOverlay.GetComponent<SpriteRenderer>();
                }
                if (turnOnFullBlack == true)
                {
                    blackVignetteRenderer.a = 1.2f;
                    blackOverlayRenderer.a = 1.2f;
                }
                if (turnOnSelfBlackFading == true)
                {
                    FadeOverlayToBlack(false, 0.0f, 1.2f, 0.3f);
                    if (turnOnVignetteForever == false)
                    {
                        FadeVignetteToBlack(false, 0.0f, 1.2f, 0.3f);
                    }
                }
                if (turnOnFullWhite == true)
                {
                    whiteOverlayRenderer.a = 1.5f;
                }
                if (turnOnSelfWhiteFading == true)
                {
                    FadeOverlayToWhite(false, 0.0f, 1.5f, 0.3f);
                }
                if (GameObject.GetGameObjectsOfTag("Player").Length > 0) //There is a player in the scene
                {
                    mainPlayer = GameObject.GetGameObjectsOfTag("Player")[0];
                    mainPlayerBehaviour = mainPlayer.GetComponent<PlayerBehaviour>();
                    DisplayHealthUI(true);
                    if (turnOnMainGameCinematics == true)
                    {
                        this.gameObject.transform.localPosition = new Vector3(mainPlayer.transform.worldPosition.x, mainPlayer.transform.worldPosition.y + 10.0f, transformZZoomIn);
                        transformXYSpeed = 0.01f;
                    }
                    else
                    {
                        transformXYSpeed = 0.1f;
                    }
                    if (mainPlayerBehaviour != null)
                    {
                        mainPlayerBehaviour.SetCanControl(false);
                    }
                }
                else
                {
                    DisplayHealthUI(false);
                }
                if (mainPlayer != null && blackBarBot != null && blackBarTop != null)
                {
                    blackBarTop.active = true;
                    blackBarBot.active = true;
                    blackBarTopSpawnPos = blackBarTop.transform.localPosition;
                    blackBarBotSpawnPos = blackBarBot.transform.localPosition;
                    blackBarTopHidePos = new Vector3(blackBarTopSpawnPos.x, blackBarTopSpawnPos.y + 0.1f, blackBarTopSpawnPos.z);
                    blackBarBotHidePos = new Vector3(blackBarBotSpawnPos.x, blackBarBotSpawnPos.y - 0.1f, blackBarBotSpawnPos.z);
                }
                cameraOffsetDefault = cameraOffset;
                hasInit = true;
            }
        }
        
        private void CheckMousePosition()
        {
            mouseScreenPosition = Camera.MouseToWorldPoint();
        }

        private void CheckMovement()
        {
            if (isDeadLock == false)
            {
                if (mainPlayer != null)
                {
                    Vector3 playerPosition = new Vector3(mainPlayer.transform.worldPosition.x + cameraOffset.x, mainPlayer.transform.worldPosition.y + cameraOffset.y, mainPlayer.transform.worldPosition.z + cameraOffset.z);
                    if (mainPlayerBehaviour.GetCanControl() == true && mouseScreenPosition.x != 0.0f && mouseScreenPosition.y != 0.0f)
                    {
                        mouseScreenDistance = GetDistance(mouseScreenPosition, playerPosition);
                        if (mouseScreenDistance > 3.0f)
                        {
                            mouseScreenDistance = 3.0f;
                        }
                        else if (mouseScreenDistance < 2.0f)
                        {
                            mouseScreenDistance = 0.0f;
                        }
                        mouseScreenDirection = mouseScreenPosition - playerPosition;
                        mouseScreenDirection = new Vector3(mouseScreenDirection.x, mouseScreenDirection.y, 0).normalized;
                        mouseScreenMagnitude =  new Vector3(mouseScreenDirection.x * (mouseScreenDistance / 30.0f), mouseScreenDirection.y * (mouseScreenDistance / 30.0f), 0.0f);
                    }
                    else
                    {
                        mouseScreenMagnitude = new Vector3(0, 0, 0);
                    }
                    transformLerp = Vector3.Lerp(this.gameObject.transform.worldPosition, playerPosition + mouseScreenMagnitude, transformXYSpeed);
                    transformX = transformLerp.x;
                    transformY = transformLerp.y;
                }
                else
                {
                    Vector3 originPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    transformLerp = Vector3.Lerp(this.gameObject.transform.worldPosition, originPosition, transformXYSpeed);
                    transformX = transformLerp.x;
                    transformY = transformLerp.y;
                }
                this.gameObject.transform.localPosition = new Vector3(transformX, transformY, this.gameObject.transform.localPosition.z);

                if (shakeTimer > 0.0f)
                {
                    Random rnd = new Random();
                    this.gameObject.transform.localPosition = new Vector3(transformX + (maxOffsetX * trauma * rnd.Next(-1, 2)), transformY + (maxOffsetX * trauma * rnd.Next(-1, 2)), this.gameObject.transform.localPosition.z);
                    shakeTimer -= Time.deltaTime;
                }
            }
            else
            {
                if (mainPlayer != null)
                {
                    Vector3 playerPosition = new Vector3(mainPlayer.transform.worldPosition.x, mainPlayer.transform.worldPosition.y + 0.3f, mainPlayer.transform.worldPosition.z + cameraOffset.z);
                    transformLerp = Vector3.Lerp(this.gameObject.transform.worldPosition, playerPosition, transformXYSpeed/5.0f);
                    transformY = transformLerp.y;
                }
                if (!isCutscene) // Used for escape
                this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, transformY, this.gameObject.transform.localPosition.z);
            }
        }

        private float GetDistance(Vector3 positionSelf, Vector3 positionTarget)
        {
            float x = positionTarget.x - positionSelf.x;
            float y = positionTarget.y - positionSelf.y;
            float distance = ((float)Math.Sqrt(x * x + y * y));
            return distance;
        }

        private void CheckZoom()
        {
            if (isDeadLock == false)
            {
                Vector3 zoomPosition = new Vector3(0.0f, 0.0f, transformZTarget);
                zoomLerp = Vector3.Lerp(this.gameObject.transform.worldPosition, zoomPosition, transformZSpeed);
                transformZ = zoomLerp.z;
                this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y, transformZ);
            }
        }

        private void CheckVignette()
        {
            if (blackVignetteRenderer != null && turnOnVignetteForever == false)
            {
                if (isVignetteFadingToBlack == true)
                {
                    if (blackVignetteRenderer.a < fadeValueMaxVignette)
                    {
                        blackVignetteRenderer.a += Time.deltaTime * fadeSpeedVignette;
                    }
                }
                else
                {
                    if (blackVignetteRenderer.a > fadeValueMinVignette)
                    {
                        blackVignetteRenderer.a -= Time.deltaTime * fadeSpeedVignette;
                    }
                }
            }
        }

        private void CheckBlackOverlay()
        {
            if (blackOverlayRenderer != null)
            {
                if (isOverlayFadingToBlack == true)
                {
                    if (blackOverlayRenderer.a < fadeValueMaxBlackOverlay)
                    {
                        blackOverlayRenderer.a += Time.deltaTime * fadeSpeedBlackOverlay;
                    }
                }
                else
                {
                    if (blackOverlayRenderer.a > fadeValueMinBlackOverlay)
                    {
                        blackOverlayRenderer.a -= Time.deltaTime * fadeSpeedBlackOverlay;
                    }
                }
            }
        }

        private void CheckWhiteOverlay()
        {
            if (whiteOverlayRenderer != null)
            {
                if (isOverlayFadingToWhite == true)
                {
                    if (whiteOverlayRenderer.a < fadeValueMaxWhiteOverlay)
                    {
                        whiteOverlayRenderer.a += Time.deltaTime * fadeSpeedWhiteOverlay;
                    }
                }
                else
                {
                    if (whiteOverlayRenderer.a > fadeValueMinWhiteOverlay)
                    {
                        whiteOverlayRenderer.a -= Time.deltaTime * fadeSpeedWhiteOverlay;
                    }
                }
            }
        }

        private void CheckBlackBar()
        {
            if (mainPlayerBehaviour != null && blackBarTop != null && blackBarBot != null)
            {
                if (mainPlayerBehaviour.GetCanControl() == true)
                {
                    blackBarTop.transform.localPosition = Vector3.Lerp(blackBarTop.transform.localPosition, blackBarTopHidePos, blackBarLerpFragmentSpeed);
                    blackBarBot.transform.localPosition = Vector3.Lerp(blackBarBot.transform.localPosition, blackBarBotHidePos, blackBarLerpFragmentSpeed);
                }
                else
                {
                    blackBarTop.transform.localPosition = Vector3.Lerp(blackBarTop.transform.localPosition, blackBarTopSpawnPos, blackBarLerpFragmentSpeed);
                    blackBarBot.transform.localPosition = Vector3.Lerp(blackBarBot.transform.localPosition, blackBarBotSpawnPos, blackBarLerpFragmentSpeed);
                }
            }
        }

        private void CheckTimer()
        {
            if (turnOnMainGameCinematics == true)
            {
                if (loadingDurationCurrent < loadingDurationSet)
                {
                    loadingDurationCurrent += Time.deltaTime;
                }
                else
                {
                    if (mainPlayerBehaviour != null)
                    {
                        mainPlayerBehaviour.SetCanControl(true);
                    }
                    transformXYSpeed = 0.1f;
                    SetFieldOfVision(0, 0.03f);
                    turnOnMainGameCinematics = false;
                    loadingDurationCurrent = 0.0f;
                }
            }
            else if (isDeadLock == false && isCutscene == false)
            {
                if (GetBlackOverlayAlpha() <= 0.8f)
                {
                    if (mainPlayerBehaviour != null )
                    {
                        mainPlayerBehaviour.SetCanControl(true);
                    }
                }
            }
        }

        public void CameraOffsetSet(Vector3 value)
        {
            cameraOffset = value;
        }

        public void CameraOffsetReset()
        {
            cameraOffset = cameraOffsetDefault;
        }

        public void FadeVignetteToBlack(bool check, float valueMin, float valueMax, float speed)
        {
            isVignetteFadingToBlack = check;
            fadeValueMinVignette = valueMin;
            fadeValueMaxVignette = valueMax;
            fadeSpeedVignette = speed;
        }

        public void FadeOverlayToBlack(bool check, float valueMin, float valueMax, float speed)
        {
            isOverlayFadingToBlack = check;
            fadeValueMinBlackOverlay = valueMin;
            fadeValueMaxBlackOverlay = valueMax;
            fadeSpeedBlackOverlay = speed;
        }

        public void FadeOverlayToWhite(bool check, float valueMin, float valueMax, float speed)
        {
            isOverlayFadingToWhite = check;
            fadeValueMinWhiteOverlay = valueMin;
            fadeValueMaxWhiteOverlay = valueMax;
            fadeSpeedWhiteOverlay = speed;
        }

        public float GetVignetteAlpha()
        {
            if (blackVignetteRenderer == null)
            {
                return 0;
            }
            else
            {
                return blackVignetteRenderer.a;
            }
        }

        public void SetVignetteAlpha()
        {
            blackVignetteRenderer.a = 1.0f;
        }

        public float GetBlackOverlayAlpha()
        {
            if (blackOverlayRenderer == null)
            {
                return 0;
            }
            else
            {
                return blackOverlayRenderer.a;
            }
        }

        public void SetBlackOverlayAlpha()
        {
            blackOverlayRenderer.a = 1.0f;
        }

        public float GetWhiteOverlayAlpha()
        {
            if (whiteOverlayRenderer == null)
            {
                return 0;
            }
            else
            {
                return whiteOverlayRenderer.a;
            }
        }

        public void SetWhiteOverlayAlpha()
        {
            whiteOverlayRenderer.a = 1.0f;
        }

        public float GetTargetFieldOfVision()
        {
            return transformZTarget;
        }

        public void SetFieldOfVision(int value, float speed)
        {
            if (value < 0)
            {
                transformZTarget = transformZZoomIn;
            }
            else if (value > 0)
            {
                transformZTarget = transformZZoomOut;
            }
            else
            {
                transformZTarget = transformZNormal;
            }
            transformZSpeed = speed;
        }

        public void ResetFieldOfVision(float value, float speed)
        {
            transformZTarget = value;
            transformZSpeed = speed;
        }

        public bool GetIsVignetteFading()
        {
            return isVignetteFadingToBlack;
        }

        public void ResetVignette(bool check, float speed)
        {
            isVignetteFadingToBlack = check;
            fadeValueMinVignette = 0.0f;
            fadeValueMaxVignette = 1.0f;
            fadeSpeedVignette = speed;
        }

        public void SetXYSpeed(float value)
        {
            transformXYSpeed = value;
        }

        public void SetDeadLock(bool check)
        {
            isDeadLock = check;
        }

        public void SetBlackBarSpeed(float speed)
        {
            blackBarLerpFragmentSpeed = speed;
        }

        public void SetShake(float time = 0.2f, float traumaSet = 1.5f, float offsetX = 0.25f, float offsetY = 0.25f)
        {
            //If already shaking we do not add a new shake
            if (time > 0 && shakeTimer > 0) return;

            shakeTimer = time;
            trauma = traumaSet;
            maxOffsetX = offsetX;
            maxOffsetY = offsetY;
        }

        public void DisplayHealthUI(bool check)
        {
            if (healthUIParent != null)
            {
                healthUIParent.active = check;
            }
        }

        public void SetCutscene(bool check)
        {
            isCutscene = check;
        }

        public void NotifyHealthHeart(int healthAmount)
        {
            if (mainPlayerBehaviour != null && healthUIParent != null && healthUIParent.active == true)
            {
                if (healthAmount >= 5)
                {
                    if (healthHeart0.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart0.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart1.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart1.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart2.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart2.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart3.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart3.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart4.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart4.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                }
                else if (healthAmount == 4)
                {
                    if (healthHeart0.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart0.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart1.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart1.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart2.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart2.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart3.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart3.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart4.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart4.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                }
                else if (healthAmount == 3)
                {
                    if (healthHeart0.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart0.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart1.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart1.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart2.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart2.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart3.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart3.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                    if (healthHeart4.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart4.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                }
                else if (healthAmount == 2)
                {
                    if (healthHeart0.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart0.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart1.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart1.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart2.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart2.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                    if (healthHeart3.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart3.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                    if (healthHeart4.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart4.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                }
                else if (healthAmount == 1)
                {
                    if (healthHeart0.GetComponent<SpriteRenderer>().textureName != sheetFullHeart)
                    {
                        healthHeart0.GetComponent<SpriteRenderer>().textureName = sheetFullHeart;
                    }
                    if (healthHeart1.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart1.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                    if (healthHeart2.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart2.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                    if (healthHeart3.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart3.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                    if (healthHeart4.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart4.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                }
                else
                {
                    if (healthHeart0.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart0.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                    if (healthHeart1.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart1.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                    if (healthHeart2.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart2.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                    if (healthHeart3.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart3.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                    if (healthHeart4.GetComponent<SpriteRenderer>().textureName != sheetEmptyHeart)
                    {
                        healthHeart4.GetComponent<SpriteRenderer>().textureName = sheetEmptyHeart;
                    }
                }
            }
        }
    }
}