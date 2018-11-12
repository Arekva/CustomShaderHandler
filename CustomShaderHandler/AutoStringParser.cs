using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomShaderHandler
{
    class AutoStringParser
    {
        private static readonly char[] numerals =
        {
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            '.',
            ',',
            'e',
            'E',
            '-',
            '+',
        };

        private static readonly string[] numeralsStrings =
        {
            "e-",
            "E-",
            "e+",
            "E+"
        };

        public static object AutoParse(string value)
        {
            int commaCount = 0;

            //String case (Texture) and comma counting for vector parsing
            foreach (char c in value)
            {
                if(!numerals.Contains(c))
                {
                    Logging.Warning("test string parse");
                    return Utils.OpenTextureFromDisk(System.IO.Path.Combine(KSPUtil.ApplicationRootPath, "GameData/" + value));
                }

                if(c == ',')
                {
                    commaCount++;
                }
            }

            #region Vector parsing
            if(commaCount == 1)
            {
                return StringIntoValue(value, typeof(Vector2));
            }

            else if (commaCount == 2)
            {
                return StringIntoValue(value, typeof(Vector3));
            }

            else if (commaCount == 3)
            {
                return StringIntoValue(value, typeof(Vector4));
            }

            else if (commaCount > 4)
            {
                Logging.Error("Error while parsing value \"" + value + "\": " + (commaCount + 1) + "D Vectors are not supported yet.");
                return null;
            }
            #endregion

            //Float case
            if (value.Contains('.'))
            {
                return StringIntoValue(value, typeof(float));
            }
            foreach (string s in numeralsStrings)
            {
                if (value.Contains(s))
                {
                    return StringIntoValue(value, typeof(float));
                }
            }

            //Int case
            return StringIntoValue(value, typeof(int));
        }

        public static object StringIntoValue(string value, Type toType)
        {
            //Float
            if(toType == typeof(float))
            {
                if(float.TryParse(value, out float n))
                {
                    return n;
                }

                else
                {
                    return UTP(value, typeof(Single));
                }
            }

            //Int
            else if (toType == typeof(int))
            {
                if (Int32.TryParse(value, out int n))
                {
                    return n;
                }

                else
                {
                    return UTP(value, typeof(Int32));
                }
            }

            //Vector2
            else if (toType == typeof(Vector2))
            {
                if (Vector2TryParse(value, out Vector2 n))
                {
                    return n;
                }

                else
                {
                    return UTP(value, typeof(Vector2));
                }
            }

            //Vector3
            else if (toType == typeof(Vector3))
            {
                if (Vector4TryParse(value, out Vector3 n))
                {
                    return n;
                }

                else
                {
                    return UTP(value, typeof(Vector3));
                }
            }

            //Vector4
            else if (toType == typeof(Vector4))
            {
                if (Vector4TryParse(value, out Vector4 n))
                {
                    return n;
                }

                else
                {
                    return UTP(value, typeof(Vector4));
                }
            }

            //Color
            else if (toType == typeof(Color))
            {
                if (Vector4TryParse(value, out Vector4 n))
                {
                    return (Color)n;
                }

                else
                {
                    return UTP(value, typeof(Vector4));
                }
            }

            else
            {
                return Logging.Error("Unable to parse \"" + value + "\": value is not compatible with the parser.");
            }
        }
        /// <summary>
        /// String need to be, for example: 0.4987, 0.6854
        /// </summary>
        /// <param name="v"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static bool Vector2TryParse(string v, out Vector2 n)
        {
            bool parsed = false;

            float x = float.NaN, y = float.NaN;

            string s = string.Empty;

            foreach (char c in v)
            {
                if (c != ',')
                {
                    s += c;
                }

                else
                {
                    x = (float)StringIntoValue(s, typeof(float));

                    s = string.Empty;
                } 
            }

            y = (float)StringIntoValue(s, typeof(float));

            if (!float.IsNaN(x) && !float.IsNaN(y))
            {
                n = new Vector2(x, y);

                parsed = true;
            }

            else
            {
                n = new Vector2(0, 0);
            }

            return parsed;
        }

        /// <summary>
        /// String need to be, for example: 0.4987, 0.6854, 0.09
        /// </summary>
        /// <param name="v"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static bool Vector4TryParse(string v, out Vector3 n)
        {
            bool parsed = false;

            float x = float.NaN, y = float.NaN, z = float.NaN;

            string s = string.Empty;

            foreach (char c in v)
            {
                if (c != ',')
                {
                    s += c;
                }

                else
                {
                    if (float.IsNaN(x))
                    {
                        x = (float)StringIntoValue(s, typeof(float));
                    }

                    else if (float.IsNaN(y))
                    {
                        y = (float)StringIntoValue(s, typeof(float));
                    }

                    s = string.Empty;
                }


            }
            if (float.IsNaN(z))
            {
                z = (float)StringIntoValue(s, typeof(float));
            }

            if (!float.IsNaN(x) && !float.IsNaN(y) && !float.IsNaN(z))
            {
                n = new Vector3(x, y, z);

                parsed = true;
            }

            else
            {
                n = new Vector4(0, 0, 0, 0);
            }

            return parsed;
        }

        /// <summary>
        /// String need to be, for example: 0.4987, 0.6854, 0.09,1
        /// </summary>
        /// <param name="v"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static bool Vector4TryParse(string v, out Vector4 n)
        {
            bool parsed = false;

            float x = float.NaN, y = float.NaN, z = float.NaN, w = float.NaN;

            string s = string.Empty;

            foreach (char c in v)
            {
                if(c != ',')
                {
                    s += c;
                }

                else
                {
                    if(float.IsNaN(x))
                    {
                        x = (float)StringIntoValue(s, typeof(float));
                    }

                    else if(float.IsNaN(y))
                    {
                        y = (float)StringIntoValue(s, typeof(float));
                    }

                    else if (float.IsNaN(z))
                    {
                        z = (float)StringIntoValue(s, typeof(float));
                    }

                    s = string.Empty;
                }


            }
            if (float.IsNaN(w))
            {
                w = (float)StringIntoValue(s, typeof(float));
            }

            if (!float.IsNaN(x) && !float.IsNaN(y) && !float.IsNaN(z) && !float.IsNaN(w))
            {
                n = new Vector4(x, y, z, w);

                parsed = true;
            }

            else
            {
                n = new Vector4(0, 0, 0, 0);
            }

            return parsed;
        }

        /// <summary>
        /// UTP : Unable To Parse; Displays an error
        /// </summary>
        private static string UTP(string v, Type t)
        {
            return Logging.Error("Unable to parse value \"" + v + "\" into " + t);
        }
    }
}
