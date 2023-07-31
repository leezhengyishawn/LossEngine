using System;
using LossScriptsTypes;
//-----------------------------------------------------------------------------------
//All content © 2019 DigiPen Institute of Technology Singapore. All Rights Reserved
//Authors: 
//Purpose: 
//-----------------------------------------------------------------------------------
namespace LossScripts
{
    class Shockwave : LossBehaviour
    {
        public bool waveActive = true;
        public float waveLifeTime = 0.0f;
        public float waveAmplitude = 10.0f;
        public float waveRefraction = 0.8f;
        public float waveWidth = 0.1f;
        public float waveSpeed = 1.0f;
        public float waveReduction = 80.0f;

        public GameObject objToTrigger;

        public float timer = 0.0f;

        public void Pulse()
        {
            waveLifeTime = 0.0f;
            waveActive = true;
            timer = 0.6f;
        }

        void Start()
        {
            SceneRenderer.PushBool("Shockwave", "waveActive", waveActive);
            SceneRenderer.PushFloat("Shockwave", "waveLifeTime", waveLifeTime);
            SceneRenderer.PushVec2("Shockwave", "waveCenter", new Vector2(objToTrigger.transform.worldPosition.x, objToTrigger.transform.worldPosition.y));
            SceneRenderer.PushFloat("Shockwave", "waveAmplitude", waveAmplitude);
            SceneRenderer.PushFloat("Shockwave", "waveRefraction", waveRefraction);
            SceneRenderer.PushFloat("Shockwave", "waveWidth", waveWidth);
            SceneRenderer.PushFloat("Shockwave", "waveSpeed", waveSpeed);
            SceneRenderer.PushFloat("Shockwave", "waveReduction", waveReduction);
        }

        void Update()
        {

        }

        void FixedUpdate()
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                if (timer < 0.0f)
                    waveActive = false;
            }

            if (waveActive)
            {
                // Decrease lifetime of individual crack effect
                waveLifeTime += Time.deltaTime;
            }
    
            // Continuously update render pass values
            SceneRenderer.PushBool("Shockwave", "waveActive", waveActive);
            SceneRenderer.PushFloat("Shockwave", "waveLifeTime", waveLifeTime);
            SceneRenderer.PushVec2("Shockwave", "waveCenter", new Vector2(objToTrigger.transform.worldPosition.x, objToTrigger.transform.worldPosition.y));
            SceneRenderer.PushFloat("Shockwave", "waveAmplitude", waveAmplitude);
            SceneRenderer.PushFloat("Shockwave", "waveRefraction", waveRefraction);
            SceneRenderer.PushFloat("Shockwave", "waveWidth", waveWidth);
            SceneRenderer.PushFloat("Shockwave", "waveSpeed", waveSpeed);
            SceneRenderer.PushFloat("Shockwave", "waveReduction", waveReduction);
        }
    }
}
