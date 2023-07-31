using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class MenuButton : LossBehaviour
    {
        public bool hover = false;
        public bool click = false;
        public bool isMenuButton = false;
        public bool isPauseButton = false;
        private bool framePassed = false;
        private Vector3 scale;

        private GameObject grassParticle;
        void Start()
        {
            scale = this.gameObject.transform.localScale;
        }

        void Update()
        {
            if (framePassed)
                click = false;
            framePassed = click;
        }

        public void ResetButton()
        {
            hover = false;
            click = false;
            framePassed = false;
        }

        void OnPointerClick()
        {
            click = true;
        }

        void OnPointerDeselect()
        {
            click = false;
        }

        void OnPointerEnter()
        {
            if (this.active)
            {
                hover = true;
            }
            if (hover == true && isMenuButton)
            {
                grassParticle = GameObject.InstantiatePrefab("GrassButtonParticle");            
                grassParticle.transform.localPosition = new Vector3 (
                    this.gameObject.transform.worldPosition.x, 
                    this.gameObject.transform.worldPosition.y + 0.2f, 
                    this.gameObject.transform.worldPosition.z);

                this.gameObject.transform.localScale = scale * 1.1f;
                Audio.PlaySource("SFX_Leaves");
                Mouse.SetCursor("Normal_Cursor_Interacted", true);
            }
            if (hover == true && isPauseButton)
            {
                grassParticle = GameObject.InstantiatePrefab("LeafButtonParticle");
                grassParticle.transform.localPosition = new Vector3(
                    this.gameObject.transform.worldPosition.x-(this.gameObject.transform.localScale.x / 2) * 7.5f,
                    this.gameObject.transform.worldPosition.y,
                    this.gameObject.transform.worldPosition.z);

                this.gameObject.transform.localScale = scale * 1.1f;
                Audio.PlaySource("SFX_Leaves");
                Mouse.SetCursor("Normal_Cursor_Interacted", true);
            }
        }

        void OnPointerExit()
        {
            hover = false;
            if ( isMenuButton || isPauseButton)
            {
                this.gameObject.transform.localScale = scale;
            }
            Mouse.SetCursor("Normal_Cursor", true);
        }
    }
}