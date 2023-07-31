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
    class HitSpawner : LossBehaviour
    {
        private GameObject mySpawnedObject;
        private float activatedDurationCurrent;
        public float activatedDurationMaximum;
        public int activatedLimit;
        public int activatedCount;
        private bool isActivated;
        private bool turnOnTimerActivated;
        public bool forestRock = false;

        void Update()
        {
            if (turnOnTimerActivated == true)
            {
                if (activatedDurationCurrent < activatedDurationMaximum)
                {
                    activatedDurationCurrent += Time.deltaTime;
                }
                else
                {
                    isActivated = false;
                    turnOnTimerActivated = false;
                    activatedDurationCurrent = 0.0f;
                }
            }
        }

        void OnCollisionStay(Collider collider)
        {
            if (isActivated == false && collider.gameObject.name == "TongueCollision")
            {
                if (activatedCount < activatedLimit)
                {
                    ++activatedCount;
                    isActivated = true;
                    turnOnTimerActivated = true;
                    Audio.PlaySource("SFX_RockFall");
                    SpawnObject();
                }
            }
        }

        private void SpawnObject()
        {
            mySpawnedObject = GameObject.InstantiatePrefab("Dropping_Spike_1");

            mySpawnedObject.transform.localPosition = new Vector3(this.gameObject.transform.worldPosition.x, this.gameObject.transform.worldPosition.y, this.gameObject.transform.worldPosition.z + 0.1f);

            if (this.gameObject.GetComponent<SpriteRenderer>().textureName == "UndergroundForest_Breakable_Spike_1")
            {
                forestRock = true;
            }

            if (forestRock == true)
            {
                mySpawnedObject.GetComponent<Breakable>().sfx = "SFX_RockDropForest";

                if (activatedCount == 1)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().textureName = "UndergroundForest_Breakable_Spike_2";
                    mySpawnedObject.transform.localScale = this.gameObject.transform.worldScale * 0.6f;
                    mySpawnedObject.GetComponent<Breakable>().BreakableSettings(this.gameObject, "UndergroundForest_Dropping_Spike_1");
                }

                else if (activatedCount == 2)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().textureName = "UndergroundForest_Breakable_Spike_3";
                    mySpawnedObject.transform.localScale = this.gameObject.transform.worldScale * 0.6f;
                    mySpawnedObject.GetComponent<Breakable>().BreakableSettings(this.gameObject, "UndergroundForest_Dropping_Spike_2");
                }

                else
                {
                    mySpawnedObject.GetComponent<Breakable>().BreakableSettings(this.gameObject, "UndergroundForest_Breakable_Spike_3");
                    Destroy(this.gameObject);
                }
            }

            else
            {
                mySpawnedObject.GetComponent<Breakable>().sfx = "SFX_RockDropCave";

                if (activatedCount == 1)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().textureName = "Breakable_Spike_2";
                    mySpawnedObject.GetComponent<Breakable>().BreakableSettings(this.gameObject, "Dropping_Spike_1");
                }

                else if (activatedCount == 2)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().textureName = "Breakable_Spike_3";
                    mySpawnedObject.GetComponent<Breakable>().BreakableSettings(this.gameObject, "Dropping_Spike_2");
                }

                else
                {
                    mySpawnedObject.GetComponent<Breakable>().BreakableSettings(this.gameObject, "Breakable_Spike_3");
                    Destroy(this.gameObject);
                }
            }
        }
    }
}