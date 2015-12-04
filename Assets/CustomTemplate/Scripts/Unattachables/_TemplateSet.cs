using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace A3Utility.CustomTemplate {
    [Serializable]
    public class TemplateSet {
        public string Extension {
            get { return this.extension; }
            set { this.extension = (value.StartsWith(".") ? value : "." + value); }
        }

        public Validater validity = Validater.validate;
        public string context = "";
        public string label = "";
        public List<string> ignores = new List<string>();
        public List<string> considers = new List<string>();
        public List<ExecutorFunctionSet> functions = new List<ExecutorFunctionSet>();

        [SerializeField]
        private string extension = "";
        

        public TemplateSet() {
            this.validity = Validater.validate;

            this.Extension = ".cs";
            this.context = "using UnityEngine;\nusing System.Collections;\nusing System.Collections.Generic;\nusing System.Linq;\n\n\nnamespace %{NAMESPACE_PATH} {\n\t[AddComponentMenu(\"%{MENU_PATH}\")]\n\tpublic class %{CLASS_NAME} : MonoBehaviour {\n\t\t\n\t\t\n\t\tvoid Start () {\n\t\t\t\n\t\t}\n\t\t\n\t\tvoid Update () {\n\t\t\t\n\t\t}\n\t}\n}\n";
            this.label = "Templatable";

            this.ignores.Add("Assets/Common/");
            this.considers.Add("Scripts");

            this.functions.Add(new ExecutorFunctionSet("%{CLASS_NAME}", "GetClass"));
            this.functions.Add(new ExecutorFunctionSet("%{NAMESPACE_PATH}", "GetNamespacePath"));
            this.functions.Add(new ExecutorFunctionSet("%{MENU_PATH}", "GetMenuPath"));
        }

        public TemplateSet(TemplateSet origin) {
            this.UpdateValues(origin);
        }

        public void UpdateValues(TemplateSet origin) {
            this.validity = origin.validity;
            this.extension = origin.extension;
            this.context = origin.context;
            this.label = origin.label;

            this.ignores = new List<string>(origin.ignores);
            this.considers = new List<string>(origin.considers);
            this.functions = new List<ExecutorFunctionSet>(origin.functions);

            this.functions.ForEach(func => func.CheckMethod());
        }
    }
}
