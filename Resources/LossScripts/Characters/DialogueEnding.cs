using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class DialogueEnding : LossBehaviour
    {
        //Getting Objects
        public GameObject fatherObject;
        public GameObject fatherDialogue;
        public Vector3 fatherDialogueOffset;

        //Different voice
        public bool changeVoice;

        //Dialogue Components
        private SpriteRenderer dialogueSprite;
        private Animator dialogueAnimator;

        //Anim Strings
        private string animEnter = "DialogueAnim";
        private string animExit = "DialogueOutAnim";
        private bool hasPlayedSound = false;

        void Start()
        {
            dialogueSprite = fatherDialogue.GetComponent<SpriteRenderer>();
            dialogueAnimator = fatherDialogue.GetComponent<Animator>();
            DialogueOff();
        }

        void Update()
        {
            CheckPosition();
        }

        void FixedUpdate()
        {
            CheckPosition();
        }

        private void CheckPosition()
        {
            fatherDialogue.transform.localPosition = fatherObject.transform.worldPosition + fatherDialogueOffset;
        }

        void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                DialogueOn();
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                DialogueOff();
            }
        }

        private void DialogueOn()
        {
            if (fatherObject != null && fatherObject.GetComponent<FatherBehaviour>().canFollow == true && dialogueAnimator.fileName != animEnter)
            {
                dialogueAnimator.fileName = animEnter;
                dialogueAnimator.animateCount = 1;
                if (hasPlayedSound == false)
                {
                    if (!changeVoice)
                    {
                        Audio.PlaySource("SFX_Talk_Player2");
                    }
                    else
                    {
                        Audio.PlaySource("SFX_Talk_Player1");
                    }
                    hasPlayedSound = true;
                }
            }
        }

        private void DialogueOff()
        {
            if (dialogueAnimator.fileName != animExit)
            {
                dialogueAnimator.fileName = animExit;
                dialogueAnimator.animateCount = 1;
                hasPlayedSound = false;
            }
        }
    }
}