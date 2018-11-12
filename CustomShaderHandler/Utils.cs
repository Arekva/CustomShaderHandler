using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomShaderHandler
{
    class Utils
    {
        public static Texture2D OpenTextureFromDisk(string path)
        {
            Texture2D t = null;

            try
            {
                using (WWW www = new WWW("file://" + path))
                {
                    t = www.texture;
                }
            }
            catch (Exception e)
            {
                Logging.Error("Texture File [" + path + "] is not existing or path is incorrect." + Environment.NewLine + e);
            }

            Logging.Warning("test text opening (texture: " + t + ")");

            return t;
        }

        /// <summary>
        /// Loads Shader class from path and unity3d asset bundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="shaderName"></param>
        /// <returns></returns>
        public static Shader OpenShaderFromDisk(ShaderManager.ConfigShader shader)
        {
            Shader s = null;

            try
            {
                using (WWW www = new WWW("file://" + shader.diskPath))
                {
                    AssetBundle b = www.assetBundle;

                    s = b.LoadAsset<Shader>(shader.name);

                    if (s == null)
                    {
                        Logging.Error("Shader [" + shader.name + "] is not existing or path is incorrect. Please take in note that the unity3d file has been found.");
                    }

                    b.Unload(false);
                }
            }
            catch (Exception e)
            {
                Logging.Error("unity3d file [" + shader.path + "] is not existing or path is incorrect." + Environment.NewLine + e);
            }

            return s;
        }
    }
}
