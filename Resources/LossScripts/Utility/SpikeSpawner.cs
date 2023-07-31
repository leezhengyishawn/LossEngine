using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class SpikeSpawner : LossBehaviour
    {
        private const uint MAX_SPIKES = 20;
        private uint spikeCount = 0;
        private float currTime = 0.0f;
        private float delayTime = 0.0f;
        private float lastSpawn = 0.0f;
        private float speed = 5.0f;
        private Random m_randomGen;
        private bool m_stopSpawn = false;
        private int RAND_MAX = 0x7fff;

        public int minSpawnTime = 1;
        public int maxSpawnTime = 3;
        public float spawnFactor = 1.0f;
        public float minDistance = 5.0f;
        public float maxDistance = 30.0f;
        public float delay = 1.0f;
        public GameObject minSpawnPt = null;
        public GameObject player = null;
        public GameObject cutsceneController = null;

        void Start()
        {
            m_randomGen = new Random();
            lastSpawn = (float)m_randomGen.Next(minSpawnTime, maxSpawnTime);
        }

        void Update()
        {
            if (!m_stopSpawn)
            {
                currTime += Time.deltaTime;

                // Spike count reach the max, start delay counter
                if (spikeCount >= MAX_SPIKES)
                    delayTime += Time.deltaTime;

                // Reset spike count once delay reach over min delay
                if (delayTime > delay)
                {
                    spikeCount = 0;
                    delayTime = 0.0f;
                }

                // Optimize this
                if (currTime >= lastSpawn && spikeCount < MAX_SPIKES)
                {
                    int min = (int)(minSpawnPt.transform.worldPosition.x);
                    int max = (int)(this.gameObject.transform.worldPosition.x);
                    int randomX = m_randomGen.Next(min, max);

                    GameObject spike = GameObject.InstantiatePrefab("Dropping_Spike_Escape");
                    spike.transform.localPosition = new Vector3(randomX, this.gameObject.transform.worldPosition.y, 0.02f);
                    spike.GetComponent<Breakable>().sfx = "SFX_RockDrop";
                    ++spikeCount;

                    //lastSpawn = (float)(currTime + m_randomGen.Next((int)(minSpawnTime), (int)(maxSpawnTime * spawnFactor)));
                    lastSpawn += minSpawnTime + (float)(m_randomGen.Next(0, RAND_MAX) / (float)(RAND_MAX / (maxSpawnTime - minSpawnTime)));
                }

                if (cutsceneController != null && cutsceneController.GetComponent<EscapeCutscene>().GetTargetReached())
                {
                    if ((this.gameObject.transform.localPosition.x - player.transform.localPosition.x) < 15.0f)
                        this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x + speed * Time.deltaTime, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);

                    if ((this.gameObject.transform.localPosition.x - minSpawnPt.transform.localPosition.x) > maxDistance)
                    {
                        minSpawnPt.transform.localPosition = new Vector3(minSpawnPt.transform.localPosition.x + speed * Time.deltaTime, minSpawnPt.transform.localPosition.y, minSpawnPt.transform.localPosition.z);
                    }
                    else if ((this.gameObject.transform.localPosition.x - minSpawnPt.transform.localPosition.x) < minDistance)
                    {
                        minSpawnPt.transform.localPosition = new Vector3(minSpawnPt.transform.localPosition.x - speed * Time.deltaTime, minSpawnPt.transform.localPosition.y, minSpawnPt.transform.localPosition.z);
                    }
                }
            }
        }

        void OnCollisionStay(Collider collider)
        {
            if (collider.gameObject.tag == "SafetyCheckpoint")
            {
                m_stopSpawn = true;
            }
            if (collider.gameObject.tag == "CamShake")
            {
                m_stopSpawn = false;
            }
        }

        public void DecreaseSpikeCount()
        {
            --spikeCount;
        }
    }
}
