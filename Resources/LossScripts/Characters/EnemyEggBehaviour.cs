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
    class EnemyEggBehaviour : LossBehaviour
    {
        public GameObject enemyEggWeb;
        public GameObject enemyEggSplatter;
        public GameObject spawnedSpider;
        public float detectionRadius;

        private SpriteRenderer spriteRendererSelf;
        private Animator animatorSelf;
        private string sheetEggIdle = "Enemy_Egg_Idle";
        private string sheetEggBlow = "Enemy_Egg_Blow";

        private SpriteRenderer spriteRendererWeb;
        private Animator animatorWeb;
        private string sheetWebIdle = "Enemy_Web_Idle";
        private string sheetWebBlow = "Enemy_Web_Blow";

        private SpriteRenderer spriteRendererSplatter;
        private Animator animatorSplatter;

        private string animIdle = "EnemyEggIdle";
        private string animBlow = "EnemyEggBlow";

        private GameObject mainPlayer;
        private bool isActivated;
        private bool turnOnTimerHatch;
        private bool hasInit;

        void Update()
        {
            CheckComponents();
            CheckPlayerDistance();
            CheckTimer();
            CheckSplatterLastFrame();
        }

        private void CheckComponents()
        {
            if (hasInit == false)
            {
                if (enemyEggSplatter != null) { enemyEggSplatter.active = false; }
                if (spawnedSpider != null) { spawnedSpider.active = false; }
                spriteRendererSelf = this.gameObject.GetComponent<SpriteRenderer>();
                animatorSelf = this.gameObject.GetComponent<Animator>();
                spriteRendererWeb = enemyEggWeb.GetComponent<SpriteRenderer>();
                animatorWeb = enemyEggWeb.GetComponent<Animator>();
                spriteRendererSplatter = enemyEggSplatter.GetComponent<SpriteRenderer>();
                animatorSplatter = enemyEggSplatter.GetComponent<Animator>();
                mainPlayer = GameObject.GetGameObjectsOfTag("Player")[0];
                spriteRendererSelf.textureName = sheetEggIdle;
                spriteRendererWeb.textureName = sheetWebIdle;
                animatorSelf.fileName = animIdle;
                animatorWeb.fileName = animIdle;
                animatorSelf.animateCount = -1;
                animatorWeb.animateCount = -1;
                hasInit = true;
            }
        }

        private void CheckPlayerDistance()
        {
            if (isActivated == false && (GetDistance(this.gameObject.transform.worldPosition, mainPlayer.transform.worldPosition) < detectionRadius))
            {
                spriteRendererSelf.currentFrameX = 1;
                spriteRendererSelf.currentFrameY = 1;
                spriteRendererWeb.currentFrameX = 1;
                spriteRendererWeb.currentFrameY = 1;
                spriteRendererSplatter.currentFrameX = 1;
                spriteRendererSplatter.currentFrameY = 1;
                spriteRendererSelf.textureName = sheetEggBlow;
                spriteRendererWeb.textureName = sheetWebBlow;
                animatorSelf.fileName = animBlow;
                animatorWeb.fileName = animBlow;
                animatorSelf.animateCount = 1;
                animatorWeb.animateCount = 1;
                animatorSplatter.animateCount = 1;
                isActivated = true;
                turnOnTimerHatch = true;
            }
        }

        private void CheckTimer()
        {
            if (turnOnTimerHatch == true)
            {
                if (spriteRendererSelf.currentFrameX == spriteRendererSelf.maxFrameX && spriteRendererSelf.currentFrameY == spriteRendererSelf.maxFrameY)
                {
                    HatchEnemy();
                    turnOnTimerHatch = false;
                }
            }
        }

        private void HatchEnemy()
        {
            if (spawnedSpider != null)
            {
                enemyEggSplatter.active = true;
                spawnedSpider.active = true;
                spawnedSpider.transform.localPosition = new Vector3(this.gameObject.transform.worldPosition.x, this.gameObject.transform.worldPosition.y, spawnedSpider.transform.worldPosition.z);
                spawnedSpider.GetComponent<EnemyBehaviour>().JumpStart();
            }
        }

        private void CheckSplatterLastFrame()
        {
            if (isActivated == true && spriteRendererSplatter.currentFrameX == spriteRendererSplatter.maxFrameX && spriteRendererSplatter.currentFrameY == spriteRendererSplatter.maxFrameY)
            {
                enemyEggSplatter.active = false;
            }
        }

        private float GetDistance(Vector3 positionSelf, Vector3 positionTarget)
        {
            float x = positionTarget.x - positionSelf.x;
            float y = positionTarget.y - positionSelf.y;
            float distance = ((float)Math.Sqrt(x * x + y * y));
            return distance;
        }
    }
}