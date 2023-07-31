using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using LossScriptsTypes;

namespace LossScripts
{
    class SpiderEye : LossBehaviour
    {
        public GameObject frog; //follow the frog
        public float      maxDistance = 0.25f; //maxium displacement of pupil from eyeball
        
        public bool      followFrog = true;
        
        private Vector3   startingPos = new Vector3(0,0,0);
        public Vector3    targetPos = new Vector3(0,0,0);  //For when the eyes points to somewhere other than frog
        
        void Start()
        {
            startingPos = gameObject.transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            
            //Direction of frog from eyes
            Vector3 direction;

            if (followFrog)
                direction = frog.transform.worldPosition - this.gameObject.transform.worldPosition;
            else
                direction = targetPos - this.gameObject.transform.worldPosition;

            //Move pupil towards frog
            this.gameObject.transform.localPosition = new Vector3(
                this.gameObject.transform.localPosition.x + (direction.normalized.x * maxDistance),
                this.gameObject.transform.localPosition.y + (direction.normalized.y * maxDistance),
                this.gameObject.transform.localPosition.z);

            //Check current distance of pupil from frog
            float currentDist;
            if (followFrog)
                currentDist = (frog.transform.worldPosition - this.gameObject.transform.worldPosition).magnitude;
            else
                currentDist = (targetPos - this.gameObject.transform.worldPosition).magnitude;

            //Bind the pupil to edge of eye
            if (currentDist > maxDistance)
            {
                this.gameObject.transform.localPosition = new Vector3(startingPos.x + (direction.normalized.x * maxDistance),
                                                                      startingPos.y + (direction.normalized.y * maxDistance),
                                                                      this.gameObject.transform.localPosition.z);
            }

        }

        void OnCollisionEnter(Collider collider)
        {
            if (collider.gameObject.tag == "IgnoreCollision")
            {
                Destroy(gameObject);
            }
        }
    }
}