#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace A3Utility.CustomTemplate {
    public class ExecutorsFunctions {
        public delegate string FunctionDelegate(ExecutorsFunctions functions, string path);


        public string GetClass(string path) {
            return Path.GetFileNameWithoutExtension(path);
        }

        public string GetNamespacePath(string path) {
            path = GetMenuPath(path);

            return path
                .Remove(path.LastIndexOf('/'))
                .Replace('/', '.')
                .Replace(" ", "");
        }

        public string GetMenuPath(string path) {
            const string SPLIT = "Scripts";

            var menu = Application.productName + path
                .Remove(path.LastIndexOf(".cs"))
                .Remove(0, path.IndexOf(SPLIT) + SPLIT.Length)
                .Replace(" ", "");

            var result = "";

            var counter = 0;
            foreach(var c in menu) {
                result += (counter > 0 && char.IsUpper(c) ? " " + c.ToString() : c.ToString());

                if(c == '/') { counter = 0; }
                else { counter++; }
            }

            return result;
        }
    }
}
#endif
