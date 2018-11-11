using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using Kopernicus.Configuration;

namespace CustomShaderHandler
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class CustomShaderHandler : MonoBehaviour
    {
        public static GameScenes lastLoadedScene = GameScenes.LOADING;

        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void Update()
        {
            if (HighLogic.LoadedScene != lastLoadedScene)
            {
                Logging.Normal("Applying shaders to new loaded scene..");
                lastLoadedScene = HighLogic.LoadedScene;
                ApplyShaders();
            }

            if(Input.GetKeyDown(KeyCode.End))
            {
                Logging.Normal("Applying shaders by force..");
                ApplyShaders();
            }
        }

        void ApplyShaders()
        {
            foreach(ShaderManager.ConfigShader c in ShaderManager.GetShadersConfigs)
            {
                Applier.Apply(c);
            }
        }
    }
}
