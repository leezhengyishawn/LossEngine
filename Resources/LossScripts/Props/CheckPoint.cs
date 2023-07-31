using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class CheckPoint : LossBehaviour
    {
        public GameObject particleGlow;
        private SpriteRenderer spriteRenderer;

        private string sheetIdle = "Checkpoint_Mushroom_Idle";
        private string sheetTouched = "Checkpoint_Mushroom_Interacted";
        private string sheetInteracted = "Checkpoint_Mushroom_Interacted_Idle";

        private bool isActivated;
        private bool isTouchingPlayer;

        void Start()
        {
            spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            particleGlow.active = false;
        }

        void Update()
        {
            if (isActivated == true)
            {
                if (isTouchingPlayer == true)
                {
                    if (spriteRenderer.textureName != sheetTouched) { spriteRenderer.textureName = sheetTouched; spriteRenderer.currentFrameX = 1; spriteRenderer.currentFrameY = 1; }
                }
                else
                {
                    if (spriteRenderer.textureName != sheetInteracted) { spriteRenderer.textureName = sheetInteracted; spriteRenderer.currentFrameX = 1; spriteRenderer.currentFrameY = 1; }
                }
            }
            else
            {
                if (spriteRenderer.textureName != sheetIdle) { spriteRenderer.textureName = sheetIdle; spriteRenderer.currentFrameX = 1; spriteRenderer.currentFrameY = 1; }
            }
        }

        void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                if (isActivated == false)
                {
                    Audio.PlaySource("SFX_Checkpoint");
                    particleGlow.active = true;
                    particleGlow.GetComponent<ParticleEmitter>().Play();
                }

                isActivated = true;
                isTouchingPlayer = true;
            }

        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                isTouchingPlayer = false;
            }
        }
    }
}