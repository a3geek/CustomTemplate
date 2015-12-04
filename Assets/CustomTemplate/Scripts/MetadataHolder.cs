using UnityEngine;


namespace A3Utility.CustomTemplate {
    [ExecuteInEditMode()]
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    public class MetadataHolder : MonoBehaviour {
        public static MetadataHolder Instance {
            get {
                if(instance == null) {
                    var obj = GameObject.Find(ObjectName);
                    if(obj != null) { instance = obj.GetComponent<MetadataHolder>(); }
                }
                return instance;
            }
        }
        private static MetadataHolder instance = null;

        public static readonly string ObjectName = "_CustomTemplateFlash";
        public Metadatas metadatas = new Metadatas();

        
        void OnEnable() {
            gameObject.name = ObjectName;
            this.Show();
        }
        
        public void Show() {
            gameObject.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
        }

        public void Hide() {
            gameObject.hideFlags = HideFlags.HideAndDontSave | HideFlags.NotEditable;
        }
    }
}
