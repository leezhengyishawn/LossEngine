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
        private bool framePassed = false;
        void Start()
        { }

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
            hover = true;
        }

        void OnPointerExit()
        {
            hover = false;
        }
    }
}