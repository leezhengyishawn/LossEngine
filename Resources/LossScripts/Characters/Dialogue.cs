using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class Dialogue : LossBehaviour
    {
        //Getting Objects
        public GameObject daddy;

        //Different voice
        public bool changeVoice;

        //Self Components
        private SpriteRenderer mySprite;
        private Animator myAnimator;

        //Dad Components
        private SpriteRenderer daddySprite;
        private Animator daddyAnimator;

        //Hide and Appear Position
        private Vector3 spawnPosition;
        private Vector3 hiddenPosition;
        private Vector3 spawnScale;
        private Vector3 hiddenScale;
        private bool isVisible;
        private bool isSinking;


        //Anim Strings
        private string animEnter = "DialogueAnim";
        private string animExit = "DialogueOutAnim";


        void Start()
        {
            mySprite = this.gameObject.GetComponent<SpriteRenderer>();
            myAnimator = this.gameObject.GetComponent<Animator>();
            daddySprite = daddy.GetComponent<SpriteRenderer>();
            daddyAnimator = daddy.GetComponent<Animator>();
            spawnPosition = daddy.transform.localPosition;
            hiddenPosition = new Vector3(spawnPosition.x, spawnPosition.y - 2.0f, spawnPosition.z);
            spawnScale = daddy.transform.localScale;
            hiddenScale = new Vector3(0.0f, spawnScale.y, spawnScale.z);
            isVisible = false;
            isSinking = false;
            DialogueOff();
            DadFlex();
        }

        void Update()
        {
            CheckVisible();
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                isVisible = true;
                isSinking = true;
                DadIdle();
                DadFlex();
                if (!changeVoice)
                {
                    Audio.PlaySource("SFX_Talk_Player2");
                }
                else
                {
                    Audio.PlaySource("SFX_Talk_Player1");
                }
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                isVisible = false;
                isSinking = false;
                DialogueOff();
                DadFlex();
            }
        }

        private void CheckVisible()
        {
            if (isVisible == true)
            {
                daddy.transform.localPosition = Vector3.Lerp(daddy.transform.localPosition, spawnPosition, 0.3f);
                daddy.transform.localScale = Vector3.Lerp(daddy.transform.localScale, spawnScale, 0.2f);
                if (isSinking == true)
                {
                    if (Math.Abs(daddy.transform.localPosition.y - spawnPosition.y) < 0.05f)
                    {
                        daddy.transform.localPosition = spawnPosition;
                        isSinking = false;
                    }
                }
                else
                {
                    if (daddySprite.textureName == "Dad_Talk_Flexing")
                    {
                        if (Vector3.MagnitudeSq(daddy.transform.localPosition - spawnPosition) <= float.Epsilon) // Dialogue pops up when dad is in correct position
                            DialogueOn();
                        if (daddySprite.currentFrameX == daddySprite.maxFrameX && daddySprite.currentFrameY == daddySprite.maxFrameY) // Last Frame
                            DadTalk();
                    }
                }
            }
            else
            {
                if (isSinking == false)
                {
                    if (myAnimator.fileName == animExit && mySprite.currentFrameX == 1 && mySprite.currentFrameY == 1)
                    {
                        isSinking = true;
                    }
                }
                else
                {
                    daddy.transform.localPosition = Vector3.Lerp(daddy.transform.localPosition, hiddenPosition, 0.1f);
                    daddy.transform.localScale = Vector3.Lerp(daddy.transform.localScale, hiddenScale, 0.2f);
                    if (Math.Abs(daddy.transform.localPosition.y - hiddenPosition.y) < 0.05f)
                    {
                        daddy.transform.localPosition = hiddenPosition;
                        if (daddySprite.textureName == "Dad_Talk_Flexing" && daddySprite.currentFrameX == daddySprite.maxFrameX && daddySprite.currentFrameY == daddySprite.maxFrameY)
                        {
                            DadIdle();
                        }
                    }
                }
            }
        }

        private void DialogueOn()
        {
            if (myAnimator.fileName != animEnter)
            {
                myAnimator.fileName = animEnter;
                myAnimator.animateCount = 1;
            }
        }

        private void DialogueOff()
        {
            if (myAnimator.fileName != animExit)
            {
                myAnimator.fileName = animExit;
                myAnimator.animateCount = 1;
            }
        }

        private void DadTalk()
        {
            if (daddySprite.textureName != "Dad_Talk_Normal")
            {
                daddySprite.textureName = "Dad_Talk_Normal";
                daddyAnimator.animateCount = -1;
            }
        }

        private void DadFlex()
        {
            if (daddySprite.textureName != "Dad_Talk_Flexing")
            {
                daddySprite.textureName = "Dad_Talk_Flexing";
                daddySprite.currentFrameX = 1;
                daddySprite.currentFrameY = 1;
                daddyAnimator.animateCount = 1;
            }
        }

        private void DadIdle()
        {
            if (daddySprite.textureName != "Dad_Idling")
            {
                daddySprite.textureName = "Dad_Idling";
                daddyAnimator.animateCount = -1;
            }
        }
    }
}