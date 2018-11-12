using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace CustomShaderHandler
{
    class ShaderManager
    {  
        /// <summary>
        /// Returns all the configs for the mod
        /// </summary>
        protected static ConfigNode[] GetConfigs
        {
            get
            {
                UrlDir.UrlConfig[] urls = GameDatabase.Instance.GetConfigs("CustomShaderHandler");

                ConfigNode[] cns = new ConfigNode[urls.Length];

                for (int i = 0; i < urls.Length; i++)
                {
                    cns[i] = urls[i].config;
                }

                return cns;
            }
        }

        /// <summary>
        /// Returns all config relative shaders
        /// </summary>
        public static List<ConfigShader> GetShadersConfigs
        {
            get
            {
                ConfigNode[] cn = GetConfigs;

                List<ConfigShader> confshaders = new List<ConfigShader>();

                //For each CustomShaderHandler node
                for (int i = 0; i < cn.Length; i++)
                {
                    ConfigNode[] cns = cn[i].GetNodes("Shader");

                    //For each Shader node
                    for (int j = 0; j < cns.Length; j++)
                    {

                        //Gets shader's settings node
                        ConfigNode csettings = cns[j].GetNode("Settings");

                        Dictionary<string, object> settings = new Dictionary<string, object>();
                        //Gets each setting in config
                        if (csettings != null)
                        {
                            foreach (ConfigNode.Value value in csettings.values)
                            {
                                settings.Add(value.name, AutoStringParser.AutoParse(value.value));
                            }
                            
                        }

                        //Creates a ConfigShader struct and adds it to the list
                        bool.TryParse(cns[j].GetValue("gameObjectIsCelestialBody"), out bool iscb);
                        confshaders.Add(new ConfigShader(cns[j].GetValue("path"), cns[j].GetValue("name"), cns[j].GetValue("gameObject"), settings, iscb));
                    }
                }

                return confshaders;
            }
        }

        /// <summary>
        /// Contains config relative shader's datas
        /// </summary>
        public struct ConfigShader
        {
            public string path, name, gameObject;
            public bool gameObjectIsCelestialBody;

            public string diskPath
            {
                get
                {
                    return Path.Combine(KSPUtil.ApplicationRootPath, "GameData/" + path);
                }
            }


            public Shader shader
            {
                get
                {
                    return Utils.OpenShaderFromDisk(this);
                }
            }

            public Dictionary<string, object> settings;

            public ConfigShader(string filePath, string shaderName, string gameObject, Dictionary<string, object> shaderSettings, bool isCB)
            {
                path = filePath;
                name = shaderName;
                this.gameObject = gameObject;
                settings = shaderSettings;
                gameObjectIsCelestialBody = isCB;
            }

            public override string ToString()
            {
                return "Path: " + path + " | Shader Name: " + name;
            }
        }
    }
}
