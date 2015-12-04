using System.Collections.Generic;
using System;


namespace A3Utility.CustomTemplate {
    [Serializable]
    public enum Validater {
        validate = 0, invalidate
    };

    [Serializable]
    public class Metadatas {
        public Validater validity = Validater.validate;
        public List<TemplateSet> templates = new List<TemplateSet>();


        public Metadatas() {; }

        public Metadatas(Metadatas origin) {
            this.validity = origin.validity;
            this.templates = new List<TemplateSet>(origin.templates);
        }

        public void UpdateValues(Metadatas origin) {
            this.validity = origin.validity;
            this.templates = new List<TemplateSet>(origin.templates);
        }
    }
}
