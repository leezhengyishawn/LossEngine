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
    class Mushroom : LossBehaviour
    {
        //Components
        private SpriteRenderer spriteRenderer;
        private Animator spriteAnimator;

        //Settings
        public bool isActivated = false;
        public bool isCave = false;
        private float currentTimer = 0;
        private float maxTimer = 1;

        //Sprite Strings
        private string sheetIdle = "Forests_Mushroom_Idle_Default";
        private string sheetBounce = "Forests_Mushroom_Interacted_Default";

        private string caveIdle = "Cavern_Mushroom_Idle_Default";
        private string caveBounce = "Cavern_Mushroom_Interacted_Default";

        //Temp Variable (To be deleted after testing)
        private bool hasInit;


        void Start()
        {
            spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            spriteAnimator = this.gameObject.GetComponent<Animator>();

            if (spriteRenderer.textureName == caveIdle)
            {
                isCave = true;
            }

            if (isCave == true)
            {
                sheetIdle = caveIdle;
                sheetBounce = caveBounce;
            }
        }

        void Update()
        {
            if (isActivated == true)
            {
                PlayBounce();
            }
            else
            {
                PlayIdle();
            }
        }

        private void PlayIdle()
        {
            if (spriteRenderer.textureName != sheetIdle)
            {
                spriteRenderer.textureName = sheetIdle;
                spriteRenderer.currentFrameX = 1;
                spriteRenderer.currentFrameY = 1;
                spriteAnimator.animateCount = -1;
            }
        }

        private void PlayBounce()
        {
            if (spriteRenderer.textureName != sheetBounce)
            {
                spriteRenderer.textureName = sheetBounce;
                spriteRenderer.currentFrameX = 1;
                spriteRenderer.currentFrameY = 1;
                spriteAnimator.animateCount = 1;
            }
            if (currentTimer < maxTimer)
            {
                currentTimer += Time.deltaTime;
            }
            else
            {
                currentTimer = 0.0f;
                isActivated = false;
            }
        }
    }
}