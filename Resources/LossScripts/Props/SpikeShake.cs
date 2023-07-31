using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class SpikeShake : LossBehaviour
    {
        private RigidBody spikeRB = null;
        private Transform spikeTransform = null;
        private GameObject dustGO = null;
        //private Random rand = null;
        private float timer = 0.0f;

        public float shakeVal = 0.0f;
        public float shakeInc = 0.0f;
        public float dropDelay = 0.0f;
        public bool shakeSpike = false;
        public bool dropSpike = false;
        //public string spikeSfx = "SFX_SpikeImpact";
        //public string spike2Sfx = "SFX_RockDrop2";

        void Start()
        {
            spikeTransform = this.gameObject.transform;
            spikeRB = this.gameObject.GetComponent<RigidBody>();

            if (spikeRB != null)
                spikeRB.active = false;

            //rand = new Random();
        }

        void Update()
        {
            if (shakeSpike)
            {
                timer += Time.deltaTime;
                spikeTransform.localRotation = new Vector3(spikeTransform.localRotation.x,
                                                           spikeTransform.localRotation.y,
                                                           spikeTransform.localRotation.z + shakeInc);

                if (spikeTransform.localRotation.z > shakeVal ||
                    spikeTransform.localRotation.z < -shakeVal)
                    shakeInc = -shakeInc;

                if (timer >= dropDelay)
                {
                    Audio.PlaySource("SFX_RockImpact");
                    dropSpike = true;
                    shakeSpike = false;
                }
            }

            if (dropSpike)
            {
                spikeRB.active = true;
            }
        }

        public void SetSpikeShake(bool shake)
        {
            shakeSpike = shake;
        }

        public bool GetSpikeShake()
        {
            return shakeSpike;
        }

        private bool HasIgnoreColliderTag(string tag)
        {
            if (tag != "IgnoreCollision" && tag != "HingePoint" && tag != "SlingPoint" && tag != "Checkpoint" && tag != "Thorn" && tag != "CameraOffset")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void OnTriggerStay(Collider collider)
        {
            if (HasIgnoreColliderTag(collider.gameObject.tag))
            {
                if (collider.gameObject.tag == "Enemy")
                    collider.gameObject.GetComponent<EnemyBehaviour>().HealthDecrease();
                else if (collider.gameObject.tag == "Player")
                    collider.gameObject.GetComponent<PlayerBehaviour>().HealthDecrease(true, this.gameObject.transform.worldPosition.x);

                dustGO = GameObject.InstantiatePrefab("Dust");

                if (collider.gameObject.tag != "Player" ||
                    collider.gameObject.tag != "Enemy")
                {
                        Audio.PlaySource("SFX_RockDrop2");
                }

                if (dustGO != null && this != null)
                    dustGO.transform.localPosition = this.gameObject.transform.worldPosition;

                Destroy(this.gameObject);
            }
        }
    }
}
