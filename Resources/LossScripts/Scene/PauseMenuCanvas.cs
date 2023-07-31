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
    class PauseMenuCanvas : LossBehaviour
    {
        private PauseMenuState pauseMenuState;
        private enum PauseMenuState
        {
            PAUSE_MENU,
            HOW_TO_PLAY_PANEL,
            OPTIONS_PANEL,
            RETURN_TO_MAIN_PANEL,
            QUIT_GAME_PANEL,
            CUTSCENE
        }

        //Background objects
        public GameObject leafLeft;
        public GameObject leafRight;
        private RigidBody rb2DleafLeft;
        private RigidBody rb2DleafRight;
        private Vector3 leafLeftStartingPos;
        private Vector3 leafRightStartingPos;
        public float leafMovementSpeed;

        //Pause menu objects
        public GameObject panelObjects;
        public GameObject blackOverlay;

        //Pause menu pause panel
        public GameObject panelPause;

        //Pause menu buttons
        public GameObject buttonResume;
        public GameObject buttonHowToPlay;
        public GameObject buttonOptions;
        public GameObject buttonReturnToMain;
        public GameObject buttonQuit;

        //Pause menu how to play panels
        public GameObject panelHowToPlayPage1;
        public GameObject panelHowToPlayPage2;
        public GameObject panelHowToPlayLeft;
        public GameObject panelHowToPlayRight;

        //Pause menu option panels
        public GameObject panelOptions;
        public GameObject panelOptionsToggleMute;
        public GameObject panelOptionsToggleMuteTick;

        //Pause menu return to main menu panels
        public GameObject panelReturnToMainPage1;
        public GameObject panelReturnToMainNo;
        public GameObject panelReturnToMainYes;

        //Pause menu quit panels
        public GameObject panelQuitGame;
        public GameObject panelQuitGameNo;
        public GameObject panelQuitGameYes;

        //Selection variables for main menu
        public int buttonPauseMenuCurrent;
        public int buttonPauseMenuMaximum;

        //Selection variables for pages
        public int pageNumberCurrent;
        public int pageNumberHowToPlayMaximum;
        public int pageNumberReturnToMainMaximum;
        public int pageNumberQuitMaximum;

        //Analog stick movement
        private bool analogMove = true;
        private short analogX = 0;
        private short analogY = 0;

        //Temp Variable (To be deleted after testing)
        private bool hasInit;

        void Update()
        {
            CheckInit();
            CheckPauseState();
            CheckAnalog();
            CheckPauseOverlay();

            if(pauseMenuState != PauseMenuState.PAUSE_MENU)
            {
                buttonHowToPlay.GetComponent<MenuButton>().active = false;
                buttonQuit.GetComponent<MenuButton>().active = false;
                buttonResume.GetComponent<MenuButton>().active = false;
                buttonReturnToMain.GetComponent<MenuButton>().active = false;
            }
            else
            {
                buttonHowToPlay.GetComponent<MenuButton>().active = true;
                buttonQuit.GetComponent<MenuButton>().active = true;
                buttonResume.GetComponent<MenuButton>().active = true;
                buttonReturnToMain.GetComponent<MenuButton>().active = true;
            }
        }

        void FixedUpdate()
        {
            LeafMovements();
        }

        private void CheckInit()
        {
            if (hasInit == false)
            {
                leafLeftStartingPos = leafLeft.transform.worldPosition;
                leafRightStartingPos = leafRight.transform.worldPosition;
                rb2DleafLeft = leafLeft.GetComponent<RigidBody>();
                rb2DleafRight = leafRight.GetComponent<RigidBody>();
                leafLeft.transform.localPosition = new Vector3(leafLeft.transform.localPosition.x - 7.0f, leafLeft.transform.localPosition.y, leafLeft.transform.localPosition.z);
                leafRight.transform.localPosition = new Vector3(leafRight.transform.localPosition.x + 7.0f, leafRight.transform.localPosition.y, leafRight.transform.localPosition.z);
                pauseMenuState = PauseMenuState.PAUSE_MENU;
                NotifyPauseMenuButton();
                hasInit = true;
            }
        }

        private void LeafMovements()
        {
            if (pauseMenuState == PauseMenuState.CUTSCENE)
            {
                HorizontalMovement(rb2DleafLeft, -2);
                HorizontalMovement(rb2DleafRight, 2);
                Unpause();
            }
            else
            {
                if (leafLeft.transform.worldPosition.x < leafLeftStartingPos.x)
                {
                    HorizontalMovement(rb2DleafLeft, 1);
                }
                else
                {
                    rb2DleafLeft.velocity = new Vector2(0, 0);
                    leafLeft.transform.localPosition = leafLeftStartingPos;
                }
                if (leafRight.transform.worldPosition.x > leafRightStartingPos.x)
                {
                    HorizontalMovement(rb2DleafRight, -1);
                }
                else
                {
                    rb2DleafRight.velocity = new Vector2(0, 0);
                    leafRight.transform.localPosition = leafRightStartingPos;
                }
            }
        }

        private void HorizontalMovement(RigidBody rb, float directionX)
        {
            rb.velocity = new Vector2(directionX * leafMovementSpeed, 0.0f);
        }

        private void CheckPauseOverlay()
        {
            if (pauseMenuState == PauseMenuState.PAUSE_MENU || pauseMenuState == PauseMenuState.CUTSCENE)
            {
                //panelPause.active = true;
                blackOverlay.active = false;
            }
            else
            {
                //panelPause.active = false;
                blackOverlay.active = true;
            }
        }

        private void CheckPauseState()
        {
            switch (pauseMenuState)
            {
                case PauseMenuState.PAUSE_MENU:
                    CheckPauseMenuInput();
                    break;

                case PauseMenuState.HOW_TO_PLAY_PANEL:
                    CheckHowToPlayInput();
                    break;

                case PauseMenuState.OPTIONS_PANEL:
                    CheckOptionsInput();
                    break;

                case PauseMenuState.RETURN_TO_MAIN_PANEL:
                    CheckReturnToMainInput();
                    break;

                case PauseMenuState.QUIT_GAME_PANEL:
                    CheckQuitGameInput();
                    break;

                case PauseMenuState.CUTSCENE:
                    break;
            }
        }

        private void CheckAnalog()
        {
            analogX = Input.GetGamepadLeftStickX(0);
            analogY = Input.GetGamepadLeftStickY(0);
            if (analogX == 0 && analogY == 0)
            {
                analogMove = true;
            }
        }

        public void Unpause()
        {
            if (panelObjects.transform.worldPosition.y > 10.0f)
            {
                Scene.PopScene();
            }
            else
            {
                panelObjects.GetComponent<RigidBody>().gravityScale = -Math.Abs(panelObjects.GetComponent<RigidBody>().gravityScale * 1.3f);
            }
        }

        private void CloseAllMenuButtons()
        {
            Audio.PlaySource("SFX_Mouse_Click");
            buttonResume.GetComponent<SpriteRenderer>().textureName = "Pause_Menu_Resume_Off";
            buttonHowToPlay.GetComponent<SpriteRenderer>().textureName = "Pause_Menu_How_To_Play_Off";
            buttonOptions.GetComponent<SpriteRenderer>().textureName = "Pause_Menu_Options_Off";
            buttonReturnToMain.GetComponent<SpriteRenderer>().textureName = "Pause_Menu_Return_Menu_Off";
            buttonQuit.GetComponent<SpriteRenderer>().textureName = "Pause_Menu_Quit_Game_Off";
        }

        private void CloseAllPanels()
        {
            Audio.PlaySource("SFX_Mouse_Click");
            panelHowToPlayPage1.active = false;
            panelHowToPlayPage2.active = false;
            panelOptions.active = false;
            panelHowToPlayLeft.active = false;
            panelHowToPlayRight.active = false;
            panelReturnToMainPage1.active = false;
            panelQuitGame.active = false;
            buttonResume.GetComponent<MenuButton>().ResetButton();
            buttonHowToPlay.GetComponent<MenuButton>().ResetButton();
            buttonOptions.GetComponent<MenuButton>().ResetButton();
            buttonReturnToMain.GetComponent<MenuButton>().ResetButton();
            buttonQuit.GetComponent<MenuButton>().ResetButton();
        }

        private void CloseQuitButtons()
        {
            Audio.PlaySource("SFX_Mouse_Click");
            panelQuitGameYes.GetComponent<SpriteRenderer>().textureName = "Menu_Text_Yes_Off";
            panelQuitGameNo.GetComponent<SpriteRenderer>().textureName = "Menu_Text_No_Off";
        }

        private void CloseReturnToMainButtons()
        {
            Audio.PlaySource("SFX_Mouse_Click");
            panelReturnToMainYes.GetComponent<SpriteRenderer>().textureName = "Menu_Text_Yes_Off";
            panelReturnToMainNo.GetComponent<SpriteRenderer>().textureName = "Menu_Text_No_Off";
        }

        private void NotifyPauseMenuButton()
        {
            CloseAllMenuButtons();
            if (buttonPauseMenuCurrent == 0) //Resume
            {
                buttonResume.GetComponent<SpriteRenderer>().textureName = "Pause_Menu_Resume_On";
            }
            else if (buttonPauseMenuCurrent == 1) //How to play
            {
                buttonHowToPlay.GetComponent<SpriteRenderer>().textureName = "Pause_Menu_How_To_Play_On";
            }
            else if (buttonPauseMenuCurrent == 2) //Options
            {
                buttonOptions.GetComponent<SpriteRenderer>().textureName = "Pause_Menu_Options_On";
            }
            else if (buttonPauseMenuCurrent == 3) //Return to Main Menu
            {
                buttonReturnToMain.GetComponent<SpriteRenderer>().textureName = "Pause_Menu_Return_Menu_On";
            }
            else if (buttonPauseMenuCurrent == 4) //Quit Game
            {
                buttonQuit.GetComponent<SpriteRenderer>().textureName = "Pause_Menu_Quit_Game_On";
            }
        }

        private void NotifyHowToPlayPages()
        {
            CloseAllPanels();
            if (pageNumberCurrent == 0)
            {
                panelHowToPlayPage1.active = true;
                panelHowToPlayLeft.active = false;
                panelHowToPlayRight.active = true;
            }
            else if (pageNumberCurrent == 1)
            {
                panelHowToPlayPage2.active = true;
                panelHowToPlayLeft.active = true;
                panelHowToPlayRight.active = false;
            }
            panelHowToPlayLeft.GetComponent<MenuButton>().ResetButton();
            panelHowToPlayRight.GetComponent<MenuButton>().ResetButton();
        }

        private void NotifyOptionsPages()
        {
            CloseAllPanels();
            panelOptions.active = true;
            if (Audio.masterMute == true)
                panelOptionsToggleMuteTick.GetComponent<SpriteRenderer>().active = true;
            else
                panelOptionsToggleMuteTick.GetComponent<SpriteRenderer>().active = false;
            panelOptionsToggleMute.GetComponent<MenuButton>().ResetButton();
        }

        private void NotifyQuitGamePages()
        {
            CloseQuitButtons();
            if (pageNumberCurrent == 0)
            {
                panelQuitGameNo.GetComponent<SpriteRenderer>().textureName = "Menu_Text_No_On";
            }
            else if (pageNumberCurrent == 1)
            {
                panelQuitGameYes.GetComponent<SpriteRenderer>().textureName = "Menu_Text_Yes_On";
            }
            panelQuitGameYes.GetComponent<MenuButton>().ResetButton();
            panelQuitGameNo.GetComponent<MenuButton>().ResetButton();
        }

        private void NotifyReturnToMainPages()
        {
            CloseReturnToMainButtons();
            if (pageNumberCurrent == 0)
            {
                panelReturnToMainNo.GetComponent<SpriteRenderer>().textureName = "Menu_Text_No_On";
            }
            else if (pageNumberCurrent == 1)
            {
                panelReturnToMainYes.GetComponent<SpriteRenderer>().textureName = "Menu_Text_Yes_On";
            }
            panelReturnToMainNo.GetComponent<MenuButton>().ResetButton();
            panelReturnToMainYes.GetComponent<MenuButton>().ResetButton();
        }

        private void CheckPauseMenuInput()
        {
            if (Input.GetKeyPress(KEYCODE.KEY_ESCAPE) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_B, 0) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_START, 0) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_BACK, 0) || Input.GetMousePress(1))
            {
                pauseMenuState = PauseMenuState.CUTSCENE;
            }

            bool buttonsClicked = buttonResume.GetComponent<MenuButton>().click || buttonHowToPlay.GetComponent<MenuButton>().click || buttonOptions.GetComponent<MenuButton>().click  || buttonReturnToMain.GetComponent<MenuButton>().click || buttonQuit.GetComponent<MenuButton>().click;

            if (Input.GetKeyPress(KEYCODE.KEY_ENTER) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_A, 0) || buttonsClicked)
            {
                if (buttonPauseMenuCurrent == 0) //Resume
                {
                    pauseMenuState = PauseMenuState.CUTSCENE;
                }
                else if (buttonPauseMenuCurrent == 1) //How to play
                {
                    pauseMenuState = PauseMenuState.HOW_TO_PLAY_PANEL;
                    pageNumberCurrent = 0;
                    NotifyHowToPlayPages();
                }
                else if (buttonPauseMenuCurrent == 2) // Options 
                {
                    pauseMenuState = PauseMenuState.OPTIONS_PANEL;
                    NotifyOptionsPages();
                }
                else if (buttonPauseMenuCurrent == 3) // Return to Main Menu 
                {
                    pauseMenuState = PauseMenuState.RETURN_TO_MAIN_PANEL;
                    pageNumberCurrent = 0;
                    panelReturnToMainPage1.active = true;
                    NotifyReturnToMainPages();
                }
                else if (buttonPauseMenuCurrent == 4) //Quit Game
                {
                    pauseMenuState = PauseMenuState.QUIT_GAME_PANEL;
                    pageNumberCurrent = 0;
                    panelQuitGame.active = true;
                    NotifyQuitGamePages();
                }
            }

            if (buttonResume.GetComponent<MenuButton>().hover && buttonPauseMenuCurrent != 0)
            {
                buttonPauseMenuCurrent = 0;
                NotifyPauseMenuButton();
            }
            else if (buttonHowToPlay.GetComponent<MenuButton>().hover && buttonPauseMenuCurrent != 1)
            {
                buttonPauseMenuCurrent = 1;
                NotifyPauseMenuButton();
            }
            else if (buttonOptions.GetComponent<MenuButton>().hover && buttonPauseMenuCurrent != 2)
            {
                buttonPauseMenuCurrent = 2;
                NotifyPauseMenuButton();
            }
            else if (buttonReturnToMain.GetComponent<MenuButton>().hover && buttonPauseMenuCurrent != 3)
            {
                buttonPauseMenuCurrent = 3;
                NotifyPauseMenuButton();
            }
            else if (buttonQuit.GetComponent<MenuButton>().hover && buttonPauseMenuCurrent != 4)
            {
                buttonPauseMenuCurrent = 4;
                NotifyPauseMenuButton();
            }

            else if (Input.GetKeyPress(KEYCODE.KEY_W) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_UP, 0) || (analogY > GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Up
            {
                if (buttonPauseMenuCurrent > 0)
                {
                    --buttonPauseMenuCurrent;
                    NotifyPauseMenuButton();
                    if (analogY > GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_S) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_DOWN, 0) || (analogY < -GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Down
            {
                if (buttonPauseMenuCurrent < buttonPauseMenuMaximum - 1)
                {
                    ++buttonPauseMenuCurrent;
                    NotifyPauseMenuButton();
                    if (analogY < -GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
        }

        private void CheckHowToPlayInput()
        {
            if (Input.GetKeyPress(KEYCODE.KEY_ESCAPE) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_B, 0) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_BACK, 0) || Input.GetMousePress(1))
            {
                pauseMenuState = PauseMenuState.PAUSE_MENU;
                CloseAllPanels();
            }

            if (Input.GetKeyPress(KEYCODE.KEY_A) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_LEFT, 0) || (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove) || panelHowToPlayLeft.GetComponent<MenuButton>().click) //Left
            {
                if (pageNumberCurrent > 0)
                {
                    --pageNumberCurrent;
                    NotifyHowToPlayPages();
                    if (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_D) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_RIGHT, 0) || (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove) || panelHowToPlayRight.GetComponent<MenuButton>().click) //Right
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
            if (Input.GetKeyPress(KEYCODE.KEY_ESCAPE) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_B, 0) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_BACK, 0) || Input.GetMousePress(1))
            {
                pauseMenuState = PauseMenuState.PAUSE_MENU;
                CloseAllPanels();
            }
            bool clicked = panelOptionsToggleMute.GetComponent<MenuButton>().click;

            if (Input.GetKeyPress(KEYCODE.KEY_ENTER) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_A, 0) || clicked)
            {
                Audio.masterMute = !Audio.masterMute;

                if (Audio.masterMute == true)
                    panelOptionsToggleMuteTick.GetComponent<SpriteRenderer>().active = true;
                else
                    panelOptionsToggleMuteTick.GetComponent<SpriteRenderer>().active = false;

                panelOptionsToggleMute.GetComponent<MenuButton>().ResetButton();
            }
        }

        private void CheckQuitGameInput()
        {
            if (Input.GetKeyPress(KEYCODE.KEY_ESCAPE) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_B, 0) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_BACK, 0) || Input.GetMousePress(1))
            {
                pauseMenuState = PauseMenuState.PAUSE_MENU;
                CloseAllPanels();
            }

            bool clicked = panelQuitGameNo.GetComponent<MenuButton>().click || panelQuitGameYes.GetComponent<MenuButton>().click;

            if (Input.GetKeyPress(KEYCODE.KEY_ENTER) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_A, 0) || clicked)
            {
                if (pageNumberCurrent == 0) //No
                {
                    pauseMenuState = PauseMenuState.PAUSE_MENU;
                    CloseAllPanels();
                }
                else if (pageNumberCurrent == 1) //Yes
                {
                    Audio.StopAllSource();
                    Scene.Quit();
                }
            }
            if (panelQuitGameNo.GetComponent<MenuButton>().hover && pageNumberCurrent != 0)
            {
                pageNumberCurrent = 0;
                NotifyQuitGamePages();
            }
            else if (panelQuitGameYes.GetComponent<MenuButton>().hover && pageNumberCurrent != 1)
            {
                pageNumberCurrent = 1;
                NotifyQuitGamePages();
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_A) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_LEFT, 0) || (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Left
            {
                if (pageNumberCurrent > 0)
                {
                    --pageNumberCurrent;
                    NotifyQuitGamePages();
                    if (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_D) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_RIGHT, 0) || (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Right
            {
                if (pageNumberCurrent < pageNumberQuitMaximum - 1)
                {
                    ++pageNumberCurrent;
                    NotifyQuitGamePages();
                    if (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
        }

        private void CheckReturnToMainInput()
        {
            if (Input.GetKeyPress(KEYCODE.KEY_ESCAPE) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_B, 0) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_BACK, 0) || Input.GetMousePress(1))
            {
                pauseMenuState = PauseMenuState.PAUSE_MENU;
                CloseAllPanels();
            }

            bool clicked = panelReturnToMainYes.GetComponent<MenuButton>().click || panelReturnToMainNo.GetComponent<MenuButton>().click;

            if (Input.GetKeyPress(KEYCODE.KEY_ENTER) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_A, 0) || clicked)
            {
                if (pageNumberCurrent == 0) //No
                {
                    pauseMenuState = PauseMenuState.PAUSE_MENU;
                    CloseAllPanels();
                }
                else if (pageNumberCurrent == 1) //Yes
                {
                    Audio.StopAllSource();
                    Scene.ClearScenes("01_MainMenu");
                }
            }

            if (panelReturnToMainNo.GetComponent<MenuButton>().hover && pageNumberCurrent != 0)
            {
                pageNumberCurrent = 0;
                NotifyReturnToMainPages();
            }
            else if (panelReturnToMainYes.GetComponent<MenuButton>().hover && pageNumberCurrent != 1)
            {
                pageNumberCurrent = 1;
                NotifyReturnToMainPages();
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_A) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_LEFT, 0) || (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Left
            {
                if (pageNumberCurrent > 0)
                {
                    --pageNumberCurrent;
                    NotifyReturnToMainPages();
                    if (analogX < -GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
            else if (Input.GetKeyPress(KEYCODE.KEY_D) || Input.GetGamepadButtonPress(GAMEPADCODE.GAMEPAD_RIGHT, 0) || (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK && analogMove)) //Right
            {
                if (pageNumberCurrent < pageNumberReturnToMainMaximum - 1)
                {
                    ++pageNumberCurrent;
                    NotifyReturnToMainPages();
                    if (analogX > GAMEPADDEADZONES.LEFT_THUMBSTICK) analogMove = false;
                }
            }
        }
    }
}