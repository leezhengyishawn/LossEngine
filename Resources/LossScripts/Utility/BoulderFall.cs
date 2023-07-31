using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class BoulderFall : LossBehaviour
    {
        private Transform boulderTransform = null;

        public float finalPos = 0.0f;
        public float startAniPos = 0.0f;
        public bool playOnce = false;
        public bool reachPos = false;
        public GameObject spikeHole = null;

        void Start()
        {
            boulderTransform = this.gameObject.transform;
        }

        void Update()
        {
            if (boulderTransform.worldPosition.y < startAniPos && !playOnce)
            {
                if (spikeHole != null)
                    spikeHole.GetComponent<BoulderHole>().SetAnimation(true);
            }

            if (!reachPos && playOnce && boulderTransform.worldPosition.y < finalPos)
            {
                //boulderTransform.localPosition = new Vector3(boulderTransform.localPosition.x, finalPos, boulderTransform.localPosition.z);
                this.gameObject.GetComponent<RigidBody>().gravityScale = 0.0f;
                this.gameObject.GetComponent<RigidBody>().mass = 0.0f;
                this.gameObject.GetComponent<RigidBody>().velocity = new Vector2(0.0f, 0.0f);
                this.gameObject.GetComponent<RigidBody>().active = false;
                reachPos = true;
            }
        }

        void OnCollisionEnter(Collider collider)
        {
            if (!playOnce && collider.gameObject.tag == "Floor")
            {
                playOnce = true;
                Audio.PlaySource("SFX_SpikeImpact");
            }
        }
    }
}
