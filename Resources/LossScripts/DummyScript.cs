using System;
using LossScriptsTypes;

namespace LossScripts
{
    class DummyScript : LossBehaviour
    {
        GameObject obj;
        RigidBody rigid;

        void Start()
        {
            obj = new GameObject("lol");
            GameObject obj1 = obj;

            rigid = obj.AddComponent<RigidBody>();
            SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
            renderer.textureName = "Crate";
            Mouse.SetCursor("", true);
            //BoxCollider col = obj.AddComponent<BoxCollider>();
            //
            //col.position = new Vector2(4.0f, 3.0f);
            //col.width = 4.0f;
            //col.height = 6.0f;

            //CircleCollider col = obj.AddComponent<CircleCollider>();
            //col.radius = 5.5f;

            // FixedDistanceConstraint constraint = Physics.AddFixedDistanceConstraint(rigid, 2, new Vector2(0.0f, 0.0f), new Vector2(0.0f, 0.0f));
        }

        void Update()
        {
            rigid.AddForce(new Vector2(100.0f, 10.0f));
        }

        void FixedUpdate()
        {
            rigid.AddForce(new Vector2(-200.0f, 10.0f));
        }

        void LateUpdate()
        {
        }

        void OnCollisionEnter(Collider collider)
        {
        }

        void OnCollisionStay(Collider collider)
        {
        }

        void OnCollisionExit(Collider collider)
        {
        }

        void OnTriggerEnter(Collider collider)
        {
        }

        void OnTriggerStay(Collider collider)
        {
        }

        void OnTriggerExit(Collider collider)
        {
        }
    }
}
