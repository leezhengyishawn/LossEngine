using System;
using System.Collections.Generic;
using LossScriptsTypes;

namespace LossScripts
{
    class EscapeTransition : LossBehaviour
    {
        void Start()
        {
            File.WriteJsonFile("EscapeConfig");
            File.WriteDataAsBool("Cutscene", true);
        }
    }
}
