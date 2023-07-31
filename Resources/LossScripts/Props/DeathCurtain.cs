using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class DeathCurtain : LossBehaviour
    {
        private GameObject player = null;
        private PlayerBehaviour pBehavior = null;
        private RigidBody curtainRB = null;
        private ParticleEmitter pEmitter = null;
        
        public float curtainSpeed = 0.01f;
        public float speedIncrement = 0.5f;
        public float timer = 0.0f;
        public float incrementDelay = 300.0f;
        public GameObject checkpoint_1 = null;
        public GameObject checkpoint_2 = null;
        public GameObject checkpoint_3 = null;

        void Start()
        {
            player = GameObject.GetGameObjectsOfTag("Player")[0];
            pBehavior = player.GetComponent<PlayerBehaviour>();
            pEmitter = this.gameObject.GetComponent<ParticleEmitter>();
            curtainRB = this.gameObject.GetComponent<RigidBody>();
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (curtainRB != null)
            {
                curtainRB.velocity = new Vector2(curtainSpeed, curtainRB.velocity.y);
            }

            if (timer >= incrementDelay)
            {
                timer = 0.0f;
                curtainSpeed += speedIncrement;
                float vel = pEmitter.GetStartingVelMin() + curtainSpeed;
                pEmitter.SetStartingVelMin(vel);
                pEmitter.SetStartingVelMax(vel);
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                pBehavior.HealthDecrease(true, collider.gameObject.transform.localPosition.x);
            }
            else if (collider.gameObject.tag == "Checkpoint")
            {
                speedIncrement *= 2.0f;
                curtainSpeed += speedIncrement;
                float vel = pEmitter.GetStartingVelMin() + curtainSpeed;
                pEmitter.SetStartingVelMin(vel);
                pEmitter.SetStartingVelMax(vel);
            }
        }

        void OnTriggerStay(Collider collider)
        {
            // Scene if player stay in the curtain for too long
            if (collider.gameObject.tag == "Player")
            {
                Audio.StopAllSource();
                Audio.masterVolume = 1.0f;
                Scene.ChangeScene("08_Escape");
            }
        }
    }
}
