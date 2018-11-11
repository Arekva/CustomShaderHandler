using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kopernicus.Configuration;
using UnityEngine;
using System.IO;

namespace CustomShaderHandler
{
    class Applier
    {
        public static void Apply(ShaderManager.ConfigShader shader)
        {
            if (shader.gameObjectIsCelestialBody)
            {
                ApplyToBody(shader, shader.gameObject);
            }

            else
            {
                ApplyToAllGameObjects(shader, shader.gameObject);
            }
        }

        public static void ApplyToAllGameObjects(ShaderManager.ConfigShader shader, string gameObject)
        {
            foreach(GameObject go in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == gameObject))
            {
                MeshRenderer mr = go.GetComponent<MeshRenderer>();

                if(mr == null)
                {
                    continue;
                }

                foreach (Material m in mr.materials)
                {
                    m.shader = shader.shader;
                    ParseSettings(m, shader);
                }
            }
        }

        /// <summary>
        /// Apply a shader to a body
        /// </summary>
        /// <param name="shader"></param>
        /// <param name="body"></param>
        private static void ApplyToBody(ShaderManager.ConfigShader configShader, object body)
        {
            if (body != null)
            {
                Type t = body.GetType();
                Body b = new Body();

                if (t == typeof(Body))
                {
                    b = (Body)body;
                }

                else if (t == typeof(CelestialBody))
                {
                    b = new Body((CelestialBody)body);
                }

                else if (t == typeof(String))
                {
                    if (FlightGlobals.Bodies != null)
                    {
                        bool found = false;

                        foreach (CelestialBody cb in FlightGlobals.Bodies)
                        {
                            if (cb.name == (string)body)
                            {
                                b = new Body(cb);

                                found = true;
                            }
                        }

                        if (!found)
                        {
                            Logging.Warning(UTA(configShader) + ": Body \"" + (string)body + "\" was not found!");

                            return;
                        }
                    }

                    else
                    {
                        Logging.Warning(UTA(configShader) + ": FlightGlobals.Bodies is null!");
                    }
                }

                else
                {
                    Logging.Warning(UTA(configShader) + ": Object is not recognized as a body (its type must be CelestialBody, Kopernicus.Configuration.Body or String");

                    return;
                }

                b.scaledVersion.material.shader = configShader.shader;

                ParseSettings(b.scaledVersion.material, configShader);
            }
        }

        public static bool ParseSettings(Material m, ShaderManager.ConfigShader shader)
        {
            return ParseSettings(m, shader.settings);
        }
        public static bool ParseSettings(Material m, Dictionary<string, object> settings)
        {
            bool parsed = false;

            try
            {
                foreach(KeyValuePair<string, object> key in settings)
                {
                    //Float -
                    if(key.Value.GetType() == typeof(float))
                    {
                        m.SetFloat(key.Key, (float)key.Value);
                    }

                    //Int - 
                    else if(key.Value.GetType() == typeof(int))
                    {
                        m.SetInt(key.Key, (int)key.Value);
                    }

                    //Color -
                    else if (key.Value.GetType() == typeof(Vector4))
                    {
                        m.SetColor(key.Key, (Vector4)key.Value);
                    }

                    //ComputeBuffer
                    else if (key.Value.GetType() == typeof(ComputeBuffer))
                    {
                        m.SetBuffer(key.Key, (ComputeBuffer)key.Value);
                    }

                    //ColorArray
                    else if (key.Value.GetType() == typeof(List<Color>))
                    {
                        m.SetColorArray(key.Key, (List<Color>)key.Value);
                    }

                    //FloatArray
                    else if (key.Value.GetType() == typeof(List<float>))
                    {
                        m.SetFloatArray(key.Key, (List<float>)key.Value);
                    }

                    //Matrix (4x4)
                    else if (key.Value.GetType() == typeof(Matrix4x4))
                    {
                        m.SetMatrix(key.Key, (Matrix4x4)key.Value);
                    }

                    //MatrixArray (4x4)
                    else if (key.Value.GetType() == typeof(List<Matrix4x4>))
                    {
                        m.SetMatrixArray(key.Key, (List<Matrix4x4>)key.Value);
                    }

                    //Texture
                    else if (key.Value.GetType() == typeof(Texture))
                    {
                        m.SetTexture(key.Key, (Texture)key.Value);
                    }
                }

                parsed = true;
            }
            catch(Exception e)
            {
                Logging.Error("Unable to pass setting on material " + m + ": " + Environment.NewLine + e);
            }

            return parsed;
        }

        /// <summary>
        /// UTA: Unable To Apply - Returns the default message of apply error
        /// </summary>
        private static string UTA(ShaderManager.ConfigShader shader)
        {
            return "Unable to apply shader " + shader.name + " to body";
        }
    }
}
