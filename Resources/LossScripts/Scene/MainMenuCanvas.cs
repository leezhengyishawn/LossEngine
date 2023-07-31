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
    class MainMenuCanvas : LossBehaviour
    {
        //States
        private MainMenuState mainMenuState;
        private enum MainMenuState
        {
            MAIN_PAGE,
            HOW_TO_PLAY_PANEL,
            OPTIONS_PANEL,
            CREDITS_PANEL,
            QUIT_PANEL,
            CUTSCENE
        }

        //Getting Camera
        private GameObject camera;
        private CameraBehaviour cameraBehaviour;

        //Main menu buttons
        public GameObject buttonHowToPlay;
        public GameObject buttonPressStart;
        public GameObject buttonOptions;
        public GameObject buttonCredits;
        public GameObject buttonQuit;

        //Main menu how to play panels
        public GameObject panelHowToPlayPage1;
        public GameObject panelHowToPlayPage2;
        public GameObject panelHowToPlayLeftArrow;
        public GameObject panelHowToPlayRightArrow;
        public GameObject panelHowToPlayClose;

        //Main menu option panels
        public GameObject panelOptionsPage1;
        public GameObject panelOptionsYes;
        public GameObject panelOptionsNo;
        public GameObject panelOptions;
        public GameObject panelOptionsMuteCheckbox;
        public GameObject panelOptionsMuteTick;
        public GameObject panelOptionsClose;

        //Main menu credits panels
        public GameObject panelCreditsPage1;
        public GameObject panelCreditsPage2;
        public GameObject panelCreditsPage3;
        public GameObject panelCreditsLeftArrow;
        public GameObject panelCreditsRightArrow;
        public GameObject panelCreditsClose;

        //Main menu quit panels
        public GameObject panelQuitPage1;
        public GameObject panelQuitYes;
        public GameObject panelQuitNo;

        //Selection variables for main menu
        public int buttonMainMenuCurrent;
        public int buttonMainMenuMaximum;

        //Selection variables for pages
        public int pageNumberCurrent;
        public int pageNumberHowToPlayMaximum;
        public int pageNumberOptionsMaximum;
        public int pageNumberCreditsMaximum;
        public int pageNumberQuitMaximum;

        //Overlay Object
        public GameObject blackOverlay;

        //Sound Settings
        private AudioSource myAudioSourceSFX;

        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        //Cutscene Lerping Objects
        public GameObject cloud;
        public GameObject lerpL;
        public GameObject lerpR;
        public GameObject lerpLMG;
        public GameObject lerpRMG;
        private Lerp lerpScript1;
        private Lerp lerpScript2;
        private Lerp lerpScript3;
        private Lerp lerpScript4;
        private Lerp titleLerp;
        private Lerp cloudLerp;
        private bool startCutscene = false;

        //Fade
        public GameObject title;
        public GameObject bg;
        public GameObject buttons;
        public GameObject particle;
        private SpriteRenderer bgSprite;
        private SpriteRenderer lmgSprite;
        private SpriteRenderer rmgSprite;
        private bool startFade = false;

        //Scene Change
        private float currentTimer;
        public float maxTimer;
        private bool changeScene = false;

        // Analog stick movement
        private bool analogMove = true;
        private short analogX = 0;
        private short analogY = 0;

        void Start()
        {
            bgSprite = bg.GetComponent<SpriteRenderer>();
            lmgSprite = lerpLMG.GetComponent<SpriteRenderer>();
            rmgSprite = lerpRMG.GetComponent<SpriteRenderer>();
            titleLerp = title.GetComponent<Lerp>();
            lerpScript1 = lerpL.GetComponent<Lerp>();
            lerpScript2 = lerpR.GetComponent<Lerp>();
            lerpScript3 = lerpLMG.GetComponent<Lerp>();
            lerpScript4 = lerpRMG.GetComponent<Lerp>();
            cloudLerp = cloud.GetComponent<Lerp>();
        }

        void Update()
        {
            CheckInit();
            CheckMenuState();
            CheckCutscene();
            CheckAnalog();
            CheckMenuOverlay();

            if( mainMenuState != MainMenuState.MAIN_PAGE)
            {
                buttonCredits.GetComponent<MenuButton>().active = false;
                buttonHowToPlay.GetComponent<MenuButton>().active = false;
                buttonOptions.GetComponent<MenuButton>().active = false;
                buttonPressStart.GetComponent<MenuButton>().active = false;
                buttonQuit.GetComponent<MenuButton>().active = false;
            }
            else {
                buttonCredits.GetComponent<MenuButton>().active = true;
                buttonHowToPlay.GetComponent<MenuButton>().active = true;
                buttonOptions.GetComponent<MenuButton>().active = true;
                buttonPressStart.GetComponent<MenuButton>().active = true;
                buttonQuit.GetComponent<MenuButton>().active = true;
            }
        }

        private void CheckInit()
        {
            if (hasInit == false)
            {
                camera = GameObject.GetGameObjectsOfTag("MainCamera")[0];
                cameraBehaviour = camera.GetComponent<CameraBehaviour>();
                myAudioSourceSFX = this.gameObject.AddComponent<AudioSource>();
                myAudioSourceSFX.playOnStart = false;
                myAudioSourceSFX.volume = 0.3f;
                myAudioSourceSFX.sourceName = "SFX_Mouse_Click";
                mainMenuState = MainMenuState.MAIN_PAGE;
                NotifyMainMenuButton();
                hasInit = true;
            }
        }

        private void CheckMenuState()
        {
            switch (mainMenuState)
            {
                case MainMenuState.MAIN_PAGE:
                    CheckMainMenuInput();
                    break;

                case MainMenuState.HOW_TO_PLAY_PANEL:
                    CheckHowToPlayInput();
                    break;

                case MainMenuState.OPTIONS_PANEL:
                    CheckOptionsInput();
                    break;

                case MainMenuState.CREDITS_PANEL:
                    CheckCreditsInput();
                    break;

                case MainMenuState.QUIT_PANEL:
                    CheckQuitInput();
                    break;

                case MainMenuState.CUTSCENE:
                    break;
            }
        }

        private void CheckCutscene()
        {
            if (startFade == true)
            {
                bgSprite.a -= Time.deltaTime * 1f;
                lmgSprite.a -= Time.deltaTime * 1f;
                rmgSprite.a -= Time.deltaTime * 1f;
                if (Audio.masterVolume > 0)
                {
                    Audio.masterVolume -= Time.deltaTime * 1f;
                }
            }
            if (startCutscene == true)
            {
                title.GetComponent<RigidBody>().active = false;
                lerpScript1.Translate();
                lerpScript2.Translate();
                lerpScript3.Translate();
                lerpScript4.Translate();
                titleLerp.Translate();
                cloudLerp.Translate();
            }
            if (changeScene == true && currentTimer < maxTimer)
            {
                currentTimer += Time.deltaTime * 1f;
            }
            else if (changeScene == true && currentTimer >= maxTimer)
            {
                Audio.StopAllSource();
                Audio.masterVolume = 1f;
                Scene.ChangeScene("02_Tutorial");
            }
        }

        private void CheckAnalog()
        {
            analogX = Input.GetGamepadLeftStickX(0);
            analogY = Input.GetGamepadLeftStickY(0);
            if (analogX == 0 && analogY == 0) analogMove = true;
        }

        private void CheckMenuOverlay()
        {
            if (mainMenuState == MainMenuState.MAIN_PAGE)
            {
                blackOverlay.active = false;
            }
            else
            {
                blackOverlay.active = true;
            }
        }

        private void CloseAllMenuButtons()
        {
            myAudioSourceSFX.pitch = 1.0f;
            Audio.PlaySource("SFX_Mouse_Click");
            buttonHowToPlay.GetComponent<SpriteRenderer>().textureName = "Menu_Button_Off";
            buttonPressStart.GetComponent<SpriteRenderer>().textureName = "Menu_Button_Off";
            buttonOptions.GetComponent<SpriteRenderer>().textureName = "Menu_Button_Off";
            buttonCredits.GetComponent<SpriteRenderer>().textureName = "Menu_Button_Off";
            buttonQuit.GetComponent<SpriteRenderer>().textureName = "Menu_Button_Off";
        }

        private void CloseAllPanels()
        {
            myAudioSourceSFX.pitch = 0.5f;
            Audio.PlaySource("SFX_Mouse_Click");
            panelHowToPlayPage1.active = false;
            panelHowToPlayPage2.active = false;
            panelHowToPlayLeftArrow.active = false;
            panelHowToPlayRightArrow.active = false;

            panelOptionsPage1.active = false;

            panelOptions.active = false;

            panelCreditsPage1.active = false;
            panelCreditsPage2.active = false;
            panelCreditsPage3.active = false;
            panelCreditsLeftArrow.active = false;
            panelCreditsRightArrow.active = false;

            panelQuitPage1.active = false;
        }

        private void CloseOptionsButtons()
        {
            myAudioSourceSFX.pitch = 1.0f;
            Audio.PlaySource("SFX_Mouse_Click");
            panelOptionsYes.GetComponent<SpriteRenderer>().textureName = "Menu_Text_Yes_Off";
            panelOptionsNo.GetComponent<SpriteRenderer>().textureName = "Menu_Text_No_Off";
            panelOptionsYes.GetComponent<MenuButton>().ResetButton();
            panelOptionsNo.GetComponent<MenuButton>().ResetButton();

            panelOptionsMuteCheckbox.GetComponent<MenuButton>().ResetButton();
        }

        private void CloseQuitButtons()
        {
            myAudioSourceSFX.pitch = 1.0f;
            Audio.PlaySource("SFX_Mouse_Click");
            panelQuitYes.GetComponent<SpriteRenderer>().textureName = "Menu_Text_Yes_Off";
            panelQuitNo.GetComponent<SpriteRenderer>().textureName = "Menu_Text_No_Off";
            panelQuitYes.GetComponent<MenuButton>().ResetButton();
            panelQuitNo.GetComponent<MenuButton>().ResetButton();
        }

        private void NotifyMainMenuButton()
        {
            CloseAllMenuButtons();
            if (buttonMainMenuCurrent == 0) //How to play
            {
                buttonHowToPlay.GetComponent<SpriteRenderer>().textureName = "Menu_Button_On";
            }
            else if (buttonMainMenuCurrent == 1) //Start
            {
                buttonPressStart.GetComponent<SpriteRenderer>().textureName = "Menu_Button_On";
            }
            else if (buttonMainMenuCurrent == 2) //Options
            {
                buttonOptions.GetComponent<SpriteRenderer>().textureName = "Menu_Button_On";
            }
            else if (buttonMainMenuCurrent == 3) //Credits
            {
                buttonCredits.GetComponent<SpriteRenderer>().textureName = "Menu_Button_On";
            }
            else if (buttonMainMenuCurrent == 4) //Quit Game
            {
                buttonQuit.GetComponent<SpriteRenderer>().textureName = "Menu_Button_On";
            }
        }

        private void NotifyHowToPlayPages()
        {
            CloseAllPanels();
            if (pageNumberCurrent == 0)
            {
                panelHowToPlayPage1.active = true;
            }
            else if (pageNumberCurrent == 1)
            {
                panelHowToPlayPage2.active = true;
            }

            if (pageNumberCurrent == 0)
            {
                panelHowToPlayLeftArrow.active = false;
                panelHowToPlayRightArrow.active = true;
            }
            else if (pageNumberCurrent == 1)
            {
                panelHowToPlayLeftArrow.active = true;
                panelHowToPlayRightArrow.active = false;
            }

            panelHowToPlayLeftArrow.GetComponent<MenuButton>().ResetButton();
            panelHowToPlayRightArrow.GetComponent<MenuButton>().ResetButton();
        }

        private void NotifyOptionsPages()
        {
            CloseOptionsButtons();
            if (pageNumberCurrent == 0)
            {
                panelOptionsNo.GetComponent<SpriteRenderer>().textureName = "Menu_Text_No_On";
            }
            else if (pageNumberCurrent == 1)
            {
                panelOptionsYes.GetComponent<SpriteRenderer>().textureName = "Menu_Text_Yes_On";
            }
        }

        private void NotifyCreditsPages()
        {
            CloseAllPanels();
            if (pageNumberCurrent == 0)
            {
                panelCreditsPage1.active = true;
            }
            else if (pageNumberCurrent == 1)
            {
                panelCreditsPage2.active = true;
            }
            else if (pageNumberCurrent == 2)
            {
                panelCreditsPage3.active = true;
            }

            if (pageNumberCurrent == 0)
            {
                panelCreditsLeftArrow.active = false;
                panelCreditsRightArrow.active = true;
            }
            else if (pageNumberCurrent == pageNumberHowToPlayMaximum)
            {
                panelCreditsLeftArrow.active = true;
                panelCreditsRightArrow.active = false;
            }
            else
            {
                panelCreditsLeftArrow.active = true;
                panelCreditsRightArrow.active = true;
            }
            panelCreditsLeftArrow.GetComponent<MenuButton>().ResetButton();
            panelCreditsRightArrow.GetComponent<MenuButton>().ResetButton();
        }

        private void NotifyQuitPages()
        {
            CloseQuitButtons();
            if (pageNumberCurrent == 0)
            {
                panelQuitNo.GetComponent<SpriteRenderer>().textureName = "Menu_Text_No_On";
            }
            else if (pageNumberCurrent == 1)
            {
                panelQuitYes.GetComponent<SpriteRenderer>().textureName = "Menu_Text_Yes_On";
            }
        }

        private void CheckMainMenuInput()
        {
            bool buttonsClicked = buttonHowToPlay.GetComponent<MenuButton>().click || buttonPressStart.GetComponent<MenuButton>().click || buttonOptions.GetComponent<MenuButton>().click || buttonCredits.GetComponent<MenuButton>().click || buttonQuit.GetComponent<MenuButton>().click;

            if (Input.GetKeyPress(KEYCODE.KEY_ENTER) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_A, 0) || buttonsClicked)
            {
                if (buttonMainMenuCurrent == 0) //How to play
                {
                    mainMenuState = MainMenuState.HOW_TO_PLAY_PANEL;
                    pageNumberCurrent = 0;
                    panelHowToPlayClose.active = true;
                    NotifyHowToPlayPages();
                }
                else if (buttonMainMenuCurrent == 1) //Start
                {
                    mainMenuState = MainMenuState.CUTSCENE;
                    if (!startCutscene) Audio.PlaySource("SFX_Mouse_Click");
                    startCutscene = true;
                    startFade = true;
                    buttons.active = false;
                    particle.active = false;
                    cameraBehaviour.FadeOverlayToWhite(true, 0.0f, 1.5f, 0.5f);
                    changeScene = true;
                    return;
                }
                else if (buttonMainMenuCurrent == 2) //Options
                {
                    mainMenuState = MainMenuState.OPTIONS_PANEL;
                    pageNumberCurrent = 0;
                    //panelOptionsPage1.active = true;
                    panelOptions.active = true;
                    NotifyOptionsPages();
                }
                else if (buttonMainMenuCurrent == 3) //Credits
                {
                    mainMenuState = MainMenuState.CREDITS_PANEL;
                    pageNumberCurrent = 0;
                    panelCreditsRightArrow.active = true;
                    panelCreditsClose.active = true;
                    NotifyCreditsPages();
                }
                else if (buttonMainMenuCurrent == 4) //Quit Game
                {
                    mainMenuState = MainMenuState.QUIT_PANEL;
                    pageNumberCurrent = 0;
                    panelQuitPage1.active = true;
                    NotifyQuitPages();
                }
            }

            if (buttonHowToPlay.GetComponent<MenuButton>().hover && buttonMainMenuCurrent != 0)
            {
                buttonMainMenuCurrent = 0;
                NotifyMainMenuButton();
            }
            else if (buttonPressStart.GetComponent<MenuButton>().hover && buttonMainMenuCurrent != 1)
            {
                buttonMainMenuCurrent = 1;
                NotifyMainMenuButton();
            }
            else if (buttonOptions.GetComponent<MenuButton>().hover && buttonMainMenuCurrent != 2)
            {
                buttonMainMenuCurrent = 2;
                NotifyMainMenuButton();
            }
            else if (buttonCredits.GetComponent<MenuButton>().hover && buttonMainMenuCurrent != 3)
            {
                buttonMainMenuCurrent = 3;
                NotifyMainMenuButton();
            }
            else if (buttonQuit.GetComponent<MenuButton>().hover && buttonMainMenuCurrent != 4)
            {
                buttonMainMenuCurrent = 4;
                NotifyMainMenuButton();
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_W) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_UP, 0) || (analogY > GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Up
            {
                if (buttonMainMenuCurrent == 3 || buttonMainMenuCurrent == 4)
                {
                    buttonMainMenuCurrent = 1;
                    NotifyMainMenuButton();

                    if (analogY > GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_S) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_DOWN, 0) || (analogY < -GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Down
            {
                if (buttonMainMenuCurrent == 0 || buttonMainMenuCurrent == 1 || buttonMainMenuCurrent == 2)
                {
                    buttonMainMenuCurrent = 3;
                    NotifyMainMenuButton();

                    if (analogY < -GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_A) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_LEFT, 0) || (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Left
            {
                if (buttonMainMenuCurrent > 0)
                {
                    --buttonMainMenuCurrent;
                    NotifyMainMenuButton();

                    if (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_D) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_RIGHT, 0) || (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Right
            {
                if (buttonMainMenuCurrent < buttonMainMenuMaximum - 1)
                {
                    ++buttonMainMenuCurrent;
                    NotifyMainMenuButton();

                    if (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
        }

        private void CheckHowToPlayInput()
        {
            if (!panelHowToPlayClose.GetComponent<MenuButton>().hover)
            {
                panelHowToPlayClose.GetComponent<SpriteRenderer>().textureName = "Menu_X_Off";
            }
            else
            {
                panelHowToPlayClose.GetComponent<SpriteRenderer>().textureName = "Menu_X_On";
            }

            if (Input.GetKeyPress(KEYCODE.KEY_ESCAPE) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_B, 0) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_BACK, 0) || Input.GetMousePress(1) || panelHowToPlayClose.GetComponent<MenuButton>().click)
            {
                mainMenuState = MainMenuState.MAIN_PAGE;
                panelHowToPlayClose.active = false;
                CloseAllPanels();
            }
            if (Input.GetKeyPress(KEYCODE.KEY_A) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_LEFT, 0) || (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove) || panelHowToPlayLeftArrow.GetComponent<MenuButton>().click) //Left
            {
                if (pageNumberCurrent > 0)
                {
                    --pageNumberCurrent;
                    NotifyHowToPlayPages();

                    if (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_D) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_RIGHT, 0) || (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove) || panelHowToPlayRightArrow.GetComponent<MenuButton>().click) //Right
            {
                if (pageNumberCurrent < pageNumberHowToPlayMaximum - 1)
                {
                    ++pageNumberCurrent;
                    NotifyHowToPlayPages();

                    if (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
        }

        private void CheckOptionsInput()
        {
            if (!panelOptionsClose.GetComponent<MenuButton>().hover)
            {
                panelOptionsClose.GetComponent<SpriteRenderer>().textureName = "Menu_X_Off";
            }
            else
            {
                panelOptionsClose.GetComponent<SpriteRenderer>().textureName = "Menu_X_On";
            }

            if (Input.GetKeyPress(KEYCODE.KEY_ESCAPE) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_B, 0) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_BACK, 0) || Input.GetMousePress(1) || panelOptionsClose.GetComponent<MenuButton>().click)
            {
                mainMenuState = MainMenuState.MAIN_PAGE;
                CloseAllPanels();
            }

            bool clicked = panelOptionsYes.GetComponent<MenuButton>().click || panelOptionsNo.GetComponent<MenuButton>().click;

            bool newClicked = panelOptionsMuteCheckbox.GetComponent<MenuButton>().click;
            if (newClicked)
            {
                if (pageNumberCurrent == 0)
                {
                    panelOptionsMuteTick.GetComponent<SpriteRenderer>().active = true;
                    pageNumberCurrent = 1;
                }
                else
                {
                    panelOptionsMuteTick.GetComponent<SpriteRenderer>().active = false;
                    pageNumberCurrent = 0;
                }
                panelOptionsMuteCheckbox.GetComponent<MenuButton>().ResetButton();
            }

            if (Input.GetKeyPress(KEYCODE.KEY_ENTER) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_A, 0) || clicked || newClicked)
            {
                Audio.masterMute = !Audio.masterMute;
                if (Audio.masterMute == true)
                    panelOptionsMuteTick.GetComponent<SpriteRenderer>().active = true;
                else
                    panelOptionsMuteTick.GetComponent<SpriteRenderer>().active = false;
                /*
                if (pageNumberCurrent == 0) //No
                {
                    Audio.masterMute = false;
                    //mainMenuState = MainMenuState.MAIN_PAGE;
                    //CloseAllPanels();
                }
                else if (pageNumberCurrent == 1) //Yes
                {
                    Audio.masterMute = true;
                    //mainMenuState = MainMenuState.MAIN_PAGE;
                    //CloseAllPanels();
                }
                */
            }
            /*
            if (panelOptionsNo.GetComponent<MenuButton>().hover && pageNumberCurrent != 0)
            {
                pageNumberCurrent = 0;
                NotifyOptionsPages();
            }
            else if (panelOptionsYes.GetComponent<MenuButton>().hover && pageNumberCurrent != 1)
            {
                pageNumberCurrent = 1;
                NotifyOptionsPages();
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_A) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_LEFT, 0) || (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Left
            {
                if (pageNumberCurrent > 0)
                {
                    --pageNumberCurrent;
                    NotifyOptionsPages();

                    if (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_D) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_RIGHT, 0) || (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Right
            {
                if (pageNumberCurrent < pageNumberOptionsMaximum - 1)
                {
                    ++pageNumberCurrent;
                    NotifyOptionsPages();

                    if (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            */
        }

        private void CheckCreditsInput()
        {
            if (!panelCreditsClose.GetComponent<MenuButton>().hover)
            {
                panelCreditsClose.GetComponent<SpriteRenderer>().textureName = "Menu_X_Off";
            }
            else
            {
                panelCreditsClose.GetComponent<SpriteRenderer>().textureName = "Menu_X_On";
            }

            if (Input.GetKeyPress(KEYCODE.KEY_ESCAPE) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_B, 0) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_BACK, 0) || Input.GetMousePress(1) || panelCreditsClose.GetComponent<MenuButton>().click)
            {
                mainMenuState = MainMenuState.MAIN_PAGE;
                panelCreditsClose.active = false;
                CloseAllPanels();
            }
            if (Input.GetKeyPress(KEYCODE.KEY_A) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_LEFT, 0) || (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove) || panelCreditsLeftArrow.GetComponent<MenuButton>().click) //Left
            {
                if (pageNumberCurrent > 0)
                {
                    --pageNumberCurrent;
                    NotifyCreditsPages();

                    if (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_D) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_RIGHT, 0) || (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove) || panelCreditsRightArrow.GetComponent<MenuButton>().click) //Right
            {
                if (pageNumberCurrent < pageNumberCreditsMaximum - 1)
                {
                    ++pageNumberCurrent;
                    NotifyCreditsPages();

                    if (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
        }

        private void CheckQuitInput()
        {
            if (Input.GetKeyPress(KEYCODE.KEY_ESCAPE) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_B, 0) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_BACK, 0) || Input.GetMousePress(1))
            {
                mainMenuState = MainMenuState.MAIN_PAGE;
                CloseAllPanels();
            }

            bool clicked = panelQuitYes.GetComponent<MenuButton>().click || panelQuitNo.GetComponent<MenuButton>().click;

            if (Input.GetKeyPress(KEYCODE.KEY_ENTER) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_A, 0) || clicked)
            {
                if (pageNumberCurrent == 0) //No
                {
                    mainMenuState = MainMenuState.MAIN_PAGE;
                    CloseAllPanels();
                }
                else if (pageNumberCurrent == 1) //Yes
                {
                    Audio.StopAllSource();
                    Scene.Quit();
                }
            }

            if (panelQuitNo.GetComponent<MenuButton>().hover && pageNumberCurrent != 0)
            {
                pageNumberCurrent = 0;
                NotifyQuitPages();
            }
            else if (panelQuitYes.GetComponent<MenuButton>().hover && pageNumberCurrent != 1)
            {
                pageNumberCurrent = 1;
                NotifyQuitPages();
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_A) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_LEFT, 0) || (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Left
            {
                if (pageNumberCurrent > 0)
                {
                    --pageNumberCurrent;
                    NotifyQuitPages();

                    if (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_D) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_RIGHT, 0) || (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Right
            {
                if (pageNumberCurrent < pageNumberQuitMaximum - 1)
                {
                    ++pageNumberCurrent;
                    NotifyQuitPages();

                    if (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
        }
    }
}