using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace PreviewLabs
{
    public static class PlayerPrefs
    {
        private static readonly Hashtable playerPrefsHashtable = new Hashtable();
        private static bool hashTableChanged = false;
        private static string serializedOutput = "";
        private static string serializedInput = "";
        private const string PARAMETERS_SEPERATOR = ";";
        private const string KEY_VALUE_SEPERATOR = ":";
        private static string[] seperators = new string[] { PARAMETERS_SEPERATOR, KEY_VALUE_SEPERATOR };
        private static readonly string fileName = Application.persistentDataPath + "/beesmarty_data.txt";
        private static readonly string secureFileName = Application.persistentDataPath + "/sec_beesmarty_data.txt";
        private readonly static byte[] bytes;
        private static bool wasEncrypted = false;
        private static bool securityModeEnabled = false;

        static PlayerPrefs()
        {
#if !UNITY_WEBPLAYER && !UNITY_WEBGL
            bytes = ASCIIEncoding.ASCII.GetBytes("bs12" + SystemInfo.deviceUniqueIdentifier.Substring(0, 4));
            //load previous settings
            StreamReader fileReader = null;


            if (File.Exists(secureFileName))
            {
                fileReader = new StreamReader(secureFileName);
                wasEncrypted = true;
                serializedInput = Decrypt(fileReader.ReadToEnd());
            }
            else if (File.Exists(fileName))
            {
                fileReader = new StreamReader(fileName);
                serializedInput = fileReader.ReadToEnd();
            }
#else
            bytes = ASCIIEncoding.ASCII.GetBytes("ap12sqqw");

            if(UnityEngine.PlayerPrefs.HasKey("encryptedData"))
                securityModeEnabled = bool.Parse(UnityEngine.PlayerPrefs.GetString("encryptedData"));

            serializedInput = securityModeEnabled ? Decrypt(UnityEngine.PlayerPrefs.GetString("data")) : UnityEngine.PlayerPrefs.GetString("data");
#endif

            if (!string.IsNullOrEmpty(serializedInput))
            {
                //In the old PlayerPrefs, a WriteLine was used to write to the file.
                if (serializedInput.Length > 0 && serializedInput[serializedInput.Length - 1] == '\n')
                {
                    serializedInput = serializedInput.Substring(0, serializedInput.Length - 1);

                    if (serializedInput.Length > 0 && serializedInput[serializedInput.Length - 1] == '\r')
                    {
                        serializedInput = serializedInput.Substring(0, serializedInput.Length - 1);
                    }
                }

                Deserialize();
            }

#if !UNITY_WEBPLAYER && !UNITY_WEBGL
            if (fileReader != null)
            {
                fileReader.Close();
            }
#endif
        }

        public static bool HasKey(string key)
        {
            return playerPrefsHashtable.ContainsKey(key);
        }

        public static void SetString(string key, string value)
        {
            if (!playerPrefsHashtable.ContainsKey(key))
            {
                playerPrefsHashtable.Add(key, value);
            }
            else
            {
                playerPrefsHashtable[key] = value;
            }

            hashTableChanged = true;
        }

        public static void SetInt(string key, int value)
        {
            if (!playerPrefsHashtable.ContainsKey(key))
            {
                playerPrefsHashtable.Add(key, value);
            }
            else
            {
                playerPrefsHashtable[key] = value;
            }

            hashTableChanged = true;
        }

        public static void SetFloat(string key, float value)
        {
            if (!playerPrefsHashtable.ContainsKey(key))
            {
                playerPrefsHashtable.Add(key, value);
            }
            else
            {
                playerPrefsHashtable[key] = value;
            }

            hashTableChanged = true;
        }

        public static void SetBool(string key, bool value)
        {
            if (!playerPrefsHashtable.ContainsKey(key))
            {
                playerPrefsHashtable.Add(key, value);
            }
            else
            {
                playerPrefsHashtable[key] = value;
            }

            hashTableChanged = true;
        }

        public static void SetLong(string key, long value)
        {
            if (!playerPrefsHashtable.ContainsKey(key))
            {
                playerPrefsHashtable.Add(key, value);
            }
            else
            {
                playerPrefsHashtable[key] = value;
            }

            hashTableChanged = true;
        }

        public static string GetString(string key)
        {
            if (playerPrefsHashtable.ContainsKey(key))
            {
                return playerPrefsHashtable[key].ToString();
            }

            return null;
        }

        public static string GetString(string key, string defaultValue)
        {
            if (playerPrefsHashtable.ContainsKey(key))
            {
                return playerPrefsHashtable[key].ToString();
            }
            else
            {
                playerPrefsHashtable.Add(key, defaultValue);
                hashTableChanged = true;
                return defaultValue;
            }
        }

        public static int GetInt(string key)
        {
            if (playerPrefsHashtable.ContainsKey(key))
            {
                return (int)playerPrefsHashtable[key];
            }

            return 0;
        }

        public static int GetInt(string key, int defaultValue)
        {
            if (playerPrefsHashtable.ContainsKey(key))
            {
                return (int)playerPrefsHashtable[key];
            }
            else
            {
                playerPrefsHashtable.Add(key, defaultValue);
                hashTableChanged = true;
                return defaultValue;
            }
        }

        public static long GetLong(string key)
        {
            if (playerPrefsHashtable.ContainsKey(key))
            {
                return (long)playerPrefsHashtable[key];
            }

            return 0;
        }

        public static long GetLong(string key, long defaultValue)
        {
            if (playerPrefsHashtable.ContainsKey(key))
            {
                return (long)playerPrefsHashtable[key];
            }
            else
            {
                playerPrefsHashtable.Add(key, defaultValue);
                hashTableChanged = true;
                return defaultValue;
            }
        }

        public static float GetFloat(string key)
        {
            if (playerPrefsHashtable.ContainsKey(key))
            {
                return (float)playerPrefsHashtable[key];
            }

            return 0.0f;
        }

        public static float GetFloat(string key, float defaultValue)
        {
            if (playerPrefsHashtable.ContainsKey(key))
            {
                return (float)playerPrefsHashtable[key];
            }
            else
            {
                playerPrefsHashtable.Add(key, defaultValue);
                hashTableChanged = true;
                return defaultValue;
            }
        }

        public static bool GetBool(string key)
        {
            if (playerPrefsHashtable.ContainsKey(key))
            {
                return (bool)playerPrefsHashtable[key];
            }

            return false;
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            if (playerPrefsHashtable.ContainsKey(key))
            {
                return (bool)playerPrefsHashtable[key];
            }
            else
            {
                playerPrefsHashtable.Add(key, defaultValue);
                hashTableChanged = true;
                return defaultValue;
            }
        }

        public static void DeleteKey(string key)
        {
            playerPrefsHashtable.Remove(key);
            hashTableChanged = true;
        }

        public static void DeleteAll()
        {
            playerPrefsHashtable.Clear();
            hashTableChanged = true;
        }

        //This is important to check to avoid a weakness in your security when you are using encryption to avoid the users from editing your playerprefs.
        public static bool WasReadPlayerPrefsFileEncrypted()
        {
            return wasEncrypted;
        }

        public static void EnableEncryption(bool enabled)
        {
            securityModeEnabled = enabled;
        }

        public static void Flush()
        {
            if (hashTableChanged)
            {
                Serialize();

                string output = (securityModeEnabled ? Encrypt(serializedOutput) : serializedOutput);
#if !UNITY_WEBPLAYER && !UNITY_WEBGL
                StreamWriter fileWriter = null;

                fileWriter = File.CreateText((securityModeEnabled ? secureFileName : fileName));

                File.Delete((securityModeEnabled ? fileName : secureFileName));

                if (fileWriter == null)
                {
                    Debug.LogWarning("PlayerPrefs::Flush() opening file for writing failed: " + fileName);
                    return;
                }

                fileWriter.Write(output);

                fileWriter.Close();

#else
                UnityEngine.PlayerPrefs.SetString("data", output);
                UnityEngine.PlayerPrefs.SetString("encryptedData", securityModeEnabled.ToString());
                
                UnityEngine.PlayerPrefs.Save();
#endif

                serializedOutput = "";

                hashTableChanged = false;
            }
        }

        private static void Serialize()
        {
            IDictionaryEnumerator myEnumerator = playerPrefsHashtable.GetEnumerator();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            bool firstString = true;
            while (myEnumerator.MoveNext())
            {
                //if(serializedOutput != "")
                if (!firstString)
                {
                    sb.Append(" ");
                    sb.Append(PARAMETERS_SEPERATOR);
                    sb.Append(" ");
                }
                sb.Append(EscapeNonSeperators(myEnumerator.Key.ToString(), seperators));
                sb.Append(" ");
                sb.Append(KEY_VALUE_SEPERATOR);
                sb.Append(" ");
                sb.Append(EscapeNonSeperators(myEnumerator.Value.ToString(), seperators));
                sb.Append(" ");
                sb.Append(KEY_VALUE_SEPERATOR);
                sb.Append(" ");
                sb.Append(myEnumerator.Value.GetType());
                firstString = false;
            }
            serializedOutput = sb.ToString();
        }

        private static void Deserialize()
        {
            string[] parameters = serializedInput.Split(new string[] { " " + PARAMETERS_SEPERATOR + " " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string parameter in parameters)
            {
                string[] parameterContent = parameter.Split(new string[] { " " + KEY_VALUE_SEPERATOR + " " }, StringSplitOptions.None);

                playerPrefsHashtable.Add(DeEscapeNonSeperators(parameterContent[0], seperators), GetTypeValue(parameterContent[2], DeEscapeNonSeperators(parameterContent[1], seperators)));

                if (parameterContent.Length > 3)
                {
                    Debug.LogWarning("PlayerPrefs::Deserialize() parameterContent has " + parameterContent.Length + " elements");
                }
            }
        }

        public static string EscapeNonSeperators(string inputToEscape, string[] seperators)
        {
            inputToEscape = inputToEscape.Replace("\\", "\\\\");

            for (int i = 0; i < seperators.Length; ++i)
            {
                inputToEscape = inputToEscape.Replace(seperators[i], "\\" + seperators[i]);
            }

            return inputToEscape;
        }

        public static string DeEscapeNonSeperators(string inputToDeEscape, string[] seperators)
        {

            for (int i = 0; i < seperators.Length; ++i)
            {
                inputToDeEscape = inputToDeEscape.Replace("\\" + seperators[i], seperators[i]);
            }

            inputToDeEscape = inputToDeEscape.Replace("\\\\", "\\");

            return inputToDeEscape;
        }

        private static string Encrypt(string originalString)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                return "";
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        private static string Decrypt(string cryptedString)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                return "";
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }

        private static object GetTypeValue(string typeName, string value)
        {
            if (typeName == "System.String")
            {
                return (object)value.ToString();
            }
            if (typeName == "System.Int32")
            {
                return Convert.ToInt32(value);
            }
            if (typeName == "System.Boolean")
            {
                return Convert.ToBoolean(value);
            }
            if (typeName == "System.Single")
            { //float
                return Convert.ToSingle(value);
            }
            if (typeName == "System.Int64")
            { //long
                return Convert.ToInt64(value);
            }
            else
            {
                Debug.LogError("Unsupported type: " + typeName);
            }

            return null;
        }
    }
}