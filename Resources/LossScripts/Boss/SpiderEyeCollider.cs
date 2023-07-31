using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using LossScriptsTypes;

namespace LossScripts
{
    class SpiderEyeCollider : LossBehaviour
    {
        public GameObject spider = null;
        public GameObject tongueCollider = null;
        public GameObject player = null;
        public GameObject pupil = null;
        public GameObject damagedEye = null;
        public GameObject blink = null;
        public bool damaged = false;

        private float eyeFrontPos;
        private float eyeBackPos;
        private float shadowColor = 0.5f;//, 0.15f, 0.15f);
        void Start()
        {
            eyeFrontPos = spider.GetComponent<SpiderBoss>().frontPos;
            eyeBackPos = spider.GetComponent<SpiderBoss>().backPos;
        }

        void Update()
        {
            if (!damaged)
            {
                Vector3 diff = this.gameObject.transform.worldPosition - tongueCollider.transform.worldPosition;
                float mag = (float)Math.Sqrt(diff.x*diff.x + diff.y*diff.y);
                if (mag <= 0.3f && tongueCollider.active && spider.GetComponent<SpiderBoss>().canDamage && (spider.GetComponent<SpiderBoss>().damageLimit > 0))
                {
                    tongueCollider.GetComponent<PlayerTongueCollision>().isCollidingSomething = true;
                    GameObject tempParticle;
                    tempParticle = GameObject.InstantiatePrefab("DamagedParticle");
                    tempParticle.transform.localPosition = new Vector3(this.gameObject.transform.worldPosition.x, 
                                                                    this.gameObject.transform.worldPosition.y, 
                                                                    this.gameObject.transform.worldPosition.z + 0.01f);

                    Vector3 dir = tongueCollider.transform.worldPosition - player.transform.worldPosition;
                    dir.Normalize();
                    spider.GetComponent<SpiderBoss>().damageImpulse = new Vector3(dir.x * 100.0f, dir.y * 100.0f, 0.0f);

                    spider.GetComponent<SpiderBoss>().ToggleFrontLegColliders(false);
                    damaged = true;

                    //Turn off eye animations
                    this.gameObject.GetComponent<SpriteRenderer>().r = 0;
                    this.gameObject.GetComponent<SpriteRenderer>().g = 0;
                    this.gameObject.GetComponent<SpriteRenderer>().b = 0;
                    pupil.active = false;
                    damagedEye.active = true;
                    blink.GetComponent<SpiderBlink>().canBlink = false;

                    spider.GetComponent<SpiderBoss>().damageLimit--;
                    spider.GetComponent<SpiderBoss>().spiderHealth--;


                    if (spider.GetComponent<SpiderBoss>().isSliding == false || spider.GetComponent<SpiderBoss>().spiderHealth <= 0)
                        spider.GetComponent<SpiderBoss>().spiderState = LossScripts.SpiderBoss.SpiderState.DAMAGED;
                    else if (spider.GetComponent<SpiderBoss>().isSliding == true && spider.GetComponent<SpiderBoss>().damageLimit <= 0)
                        spider.GetComponent<SpiderBoss>().spiderState = LossScripts.SpiderBoss.SpiderState.DAMAGED;


                    this.gameObject.GetComponent<TongueInteractive>().active = false;

                    Audio.PlaySource("SFX_Boss_Spider_Eye_Pop");

                    //Create blood particles
                    GameObject blood = GameObject.InstantiatePrefab("SpiderBossBlood");
                    blood.transform.localPosition = this.gameObject.transform.worldPosition;

                    //Shockwave
                    spider.GetComponent<Shockwave>().objToTrigger = this.gameObject;
                    spider.GetComponent<Shockwave>().Pulse();
                }
            }
            else
            {
                float diff = 1.0f - shadowColor;
                float diffShadow = ((spider.transform.localPosition.z - eyeBackPos) / (eyeFrontPos - eyeBackPos)) * diff;

                if (diffShadow + shadowColor > 1.0f) //Clamp to 1
                    diffShadow = 1.0f - shadowColor;
                
                damagedEye.GetComponent<SpriteRenderer>().r = shadowColor + diffShadow;
                damagedEye.GetComponent<SpriteRenderer>().g = shadowColor + diffShadow;
                damagedEye.GetComponent<SpriteRenderer>().b = shadowColor + diffShadow;
            }
        }
    }
}