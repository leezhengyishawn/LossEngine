
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class Blockage : LossBehaviour
    {
        //Getting Object
        public GameObject blockageObject;
        private BoxCollider blockageCollider;

        void Start()
        {
            if (blockageObject != null)
            {
                blockageCollider = blockageObject.GetComponent<BoxCollider>();
            }
        }

        void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                if (collider.gameObject.GetComponent<PlayerBehaviour>().playerState == PlayerBehaviour.PlayerState.SLIDE)
                {
                    collider.gameObject.GetComponent<PlayerBehaviour>().ResetSlideCooldown();
                    blockageCollider.isTrigger = true;
                }
                else
                {
                    blockageCollider.isTrigger = false;
                }
            }
        }
        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                blockageCollider.isTrigger = false;
            }
        }
    }
}