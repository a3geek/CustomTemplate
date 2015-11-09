#if UNITY_EDITOR﻿
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;


namespace A3Utility.CustomTemplate {
    using FunctionDelegate = ExecutorsFunctions.FunctionDelegate;

    [System.Serializable]
    public class CustomMetadatas {
        [System.Serializable]
        public class ExecutorsFunctionSet {
            public string key = "";
            public string function = "";
             [NonSerializedAttribute()]
            public FunctionDelegate delegater = null;


            public ExecutorsFunctionSet() : this("", "") {
                ;
            }

            public ExecutorsFunctionSet(string key, string function) {
                this.key = key;
                this.function = function;
                this.CheckMethod();
            }

            public void CheckMethod() {
                if(this.function == "") {
                    this.delegater = null;
                    return;
                }

                var method = typeof(ExecutorsFunctions).GetMethod(this.function);
                if(method == null) {
                    this.delegater = null;
                }
                else {
                    this.delegater = (FunctionDelegate)Delegate.CreateDelegate(typeof(FunctionDelegate), method);
                }
            }
        }

        public enum Validater { validate = 1, invalidate = 2 };

        public Validater validity = Validater.validate;
        public string context = "";
        public string label = "";
        public List<string> ignores = new List<string>();
        public List<string> considers = new List<string>();
        public List<ExecutorsFunctionSet> functions = new List<ExecutorsFunctionSet>();


        public CustomMetadatas() {
            this.validity = Validater.validate;
            this.context = "using UnityEngine;\nusing System.Collections;\nusing System.Collections.Generic;\nusing System.Linq;\n\n\nnamespace %{NAMESPACE_PATH} {\n\t[AddComponentMenu(\"%{MENU_PATH}\")]\n\tpublic class %{CLASS_NAME} : MonoBehaviour {\n\t\t\n\t\t\n\t\tvoid Start () {\n\t\t\t\n\t\t}\n\t\t\n\t\tvoid Update () {\n\t\t\t\n\t\t}\n\t}\n}\n";
            this.label = "Templatable";

            this.ignores.Add("Assets/Common/");
            this.considers.Add("Scripts");

            this.functions.Add(new ExecutorsFunctionSet("%{CLASS_NAME}", "GetClass"));
            this.functions.Add(new ExecutorsFunctionSet("%{NAMESPACE_PATH}", "GetNamespacePath"));
            this.functions.Add(new ExecutorsFunctionSet("%{MENU_PATH}", "GetMenuPath"));
        }

        public CustomMetadatas(CustomMetadatas origin) {
            this.validity = origin.validity;
            this.context = origin.context;
            this.label = origin.label;
            this.ignores = new List<string>(origin.ignores);
            this.considers = new List<string>(origin.considers);
            this.functions = new List<ExecutorsFunctionSet>(origin.functions);
            this.functions.ForEach(func => func.CheckMethod());
        }
    }
}
#endif
