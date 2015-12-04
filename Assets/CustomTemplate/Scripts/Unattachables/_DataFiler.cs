using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace A3Utility.CustomTemplate {
    public static class DataFiler {
        public static object LoadFromFile(string path) {
            object obj = null;

            try {
                using(var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                    var bf = new BinaryFormatter();
                    obj = bf.Deserialize(fs);
                }
            }
            catch {
            //catch(Exception e) {
                ///Debug.LogError(e);
                return null;
            }

            return obj;
        }

        public static bool Save2File(object obj, string path) {
            try {
                using(var fs = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                    var bf = new BinaryFormatter();
                    bf.Serialize(fs, obj);
                }
            }
            catch(Exception e) {
                Debug.LogError(e);
                return false;
            }

            return true;
        }
    }
}
