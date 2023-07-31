using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class Fadeout : LossBehaviour
    {
        private SpriteRenderer blackOverlayRenderer;

        public float fadeSpeedBlackOverlay;
        public float fadeValueMinBlackOverlay;
        public float fadeValueMaxBlackOverlay;
        public bool isOverlayFadingToBlack;
        
        void Start()
        {
            blackOverlayRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            blackOverlayRenderer.a = 1f;
            FadeOverlayToBlack(true);
        }

        void Update()
        {
            CheckBlackOverlay();
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
                    else
                    {
                        blackOverlayRenderer.a = fadeValueMaxBlackOverlay;
                    }
                }
                else
                {
                    if (blackOverlayRenderer.a > fadeValueMinBlackOverlay)
                    {
                        blackOverlayRenderer.a -= Time.deltaTime * fadeSpeedBlackOverlay;
                    }
                    else
                    {
                        blackOverlayRenderer.a = fadeValueMinBlackOverlay;
                    }
                }
            }
        }

        public void FadeOverlayToBlack(bool check)
        {
            isOverlayFadingToBlack = check;
        }
    }
}