using UnityEngine;
using UnityEditor;
using System;


namespace A3Utility.CustomTemplate.Window {
    using ListFields;

    [Serializable]
    public class ScrollParams {
        public Vector2 scroll = Vector2.zero;
        public bool horizontalBar = false;
        public bool verticalBar = true;
        public float minHeight = 100.0f;
    }

    public class SelectTemplate : EditorWindow {
        private MetadataHolder holder = null;
        private Metadatas metadatas = null;
        private ScrollParams sparams = new ScrollParams();


        [MenuItem("A3Utility/Custom Template/Open Edit Window")]
        public static SelectTemplate Open() {
            var window = GetWindow<SelectTemplate>();
            window.Init();

            return window;
        }

        public void Save2File() {
            var path = Getters4Editor.GetFilePath(this.holder);
            this.holder.metadatas.UpdateValues(this.metadatas);

            DataFiler.Save2File(this.metadatas, Getters4Editor.GetFullFilePath(path));
        }
        
        private void Init() {
            if(this.holder == null) { this.holder = Getters4Editor.GetHolder(); }
            this.metadatas = new Metadatas(this.holder.metadatas);
            
            this.titleContent.text = "Select Template";
            this.titleContent.tooltip = "Select Template Window";
        }

        #region "GUI"
        void OnGUI() {
            if(this.holder == null || this.metadatas == null) { this.Init(); }

            EditorGUILayout.LabelField("CustomTemplate : Select template window");
            GUILayout.Space(20.0f);

            #region "Validation"
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Setting CustomTemplate's validation");
            this.metadatas.validity = (Validater)EditorGUILayout.EnumPopup(this.metadatas.validity);
            EditorGUILayout.EndHorizontal();
            #endregion

            #region "Select template"
            if(this.metadatas.templates.Count > 0) {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(GUILayout.MinHeight(this.sparams.minHeight));
                this.sparams.scroll = EditorGUILayout.BeginScrollView(this.sparams.scroll, this.sparams.horizontalBar, this.sparams.verticalBar);

                this.metadatas.templates.ForEach(template => this.GUI4SelectTempalte(template));

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
            #endregion
            
            EditorGUILayout.Space();
            if(GUILayout.Button("Add Template")) {
                this.metadatas.templates.Add(new TemplateSet());
            }
            EditorGUILayout.Space();
            if(GUILayout.Button("Save")) {
                this.Save2File();
            }
        }
        
        private void GUI4SelectTempalte(TemplateSet template) {
            EditorGUILayout.BeginHorizontal();

            template.Extension = EditorGUILayout.TextField("Extension: ", template.Extension);
            if(GUILayout.Button("Select")) {
                EditTemplate.Open(template);
            }

            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }
}
