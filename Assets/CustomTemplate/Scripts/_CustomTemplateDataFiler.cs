using System;﻿
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace A3Utility.CustomTemplate {
    public static class CustomTemplateDataFiler {
        public static object LoadFromFile(string path) {
            object obj = null;

            try {
                using(var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                    var bf = new BinaryFormatter();
                    obj = bf.Deserialize(fs);
                }
            }
            catch {
                return null;
            }

            return obj;
        }

        public static void Save2File(object obj, string path) {
            using(var fs = new FileStream(path, FileMode.Create, FileAccess.Write)) {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
            }
        }
    }
}
