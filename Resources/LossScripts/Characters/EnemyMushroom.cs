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
    class EnemyMushroom : LossBehaviour
    {
        public GameObject parentSpider;
        public bool isActivated;

        void Update()
        {
            CheckActivated();
            CheckParentEaten();
        }

        private void CheckActivated()
        {
            if (isActivated == true)
            {
                if (parentSpider != null)
                {
                    parentSpider.GetComponent<EnemyBehaviour>().TriggerMushroomBounce();
                }
                isActivated = false;
            }
        }

        private void CheckParentEaten()
        {
            if (parentSpider.GetComponent<EnemyBehaviour>().GetIsEaten() == true)
            {
                Destroy(this.gameObject);
            }
        }
    }
}