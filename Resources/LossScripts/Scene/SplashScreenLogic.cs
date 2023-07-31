using System;
using LossScriptsTypes;

namespace LossScripts
{
    class SplashScreenLogic : LossBehaviour
    {
        private enum Screen
        {
            DIGIPEN,
            TEAM,
            SCREEN_MAX
        }

        private enum State
        {
            FADE_IN,
            FADE_OUT,
            DISPLAY,
            STATE_MAX
        }

        private Screen currScreen = Screen.DIGIPEN;

        private State currState = State.FADE_IN;

        private SpriteRenderer sprite;
        private Animator spriteAnimator;
        private Transform goTransform;

        private float displayTime = 2.0f;
        private float fadeTime = 1.0f;
        private float currTime = 0.0f;

        void Start()
        {
            sprite = this.gameObject.GetComponent<SpriteRenderer>();
            spriteAnimator = this.gameObject.GetComponent<Animator>();
            goTransform = this.gameObject.GetComponent<Transform>();
            ChangeScreen();
        }

        void Update()
        {
            currTime += Time.deltaTime;

            switch (currState)
            {
                case State.FADE_IN:
                    sprite.a = currTime / fadeTime;

                    if (currTime > fadeTime)
                    {
                        currTime = 0.0f;
                        currState = State.DISPLAY;
                    }

                    break;

                case State.FADE_OUT:
                    sprite.a = (fadeTime - currTime) / fadeTime;

                    if (currTime > fadeTime)
                    {
                        currTime = 0.0f;
                        currState = State.FADE_IN;

                        // Change the screen
                        currScreen++;
                        ChangeScreen();
                    }
                    break;

                case State.DISPLAY:
                    if (currTime > displayTime)
                    {
                        currTime = 0.0f;
                        currState = State.FADE_OUT;
                    }
                    break;
            }
        }

        void ChangeScreen()
        {
            switch (currScreen)
            {
                case Screen.DIGIPEN:
                    sprite.textureName = "DigiPen_Singapore_WEB_WHITE";
                    break;

                case Screen.TEAM:
                    // TODO: set the team's logo
                    spriteAnimator.fileName = "LogoAnim";
                    goTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    break;

                case Screen.SCREEN_MAX:
                    Scene.ChangeScene("01_MainMenu");
                    break;
            }
        }
    }
}