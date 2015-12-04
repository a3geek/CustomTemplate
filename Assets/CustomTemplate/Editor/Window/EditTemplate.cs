using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


namespace A3Utility.CustomTemplate.Window {
    using ListFields;
    using Object = UnityEngine.Object;

    public class EditTemplate : EditorWindow {
        private string previousLabel = "";
        private TemplateSet template = null;

        private StringListField ignores = null;
        private StringListField considers = null;
        private ListField<ExecutorFunctionSet> functions = null;
        private Object efobject = null;
        private string efpath = "";


        public static EditTemplate Open(TemplateSet template) {
            var window = GetWindow<EditTemplate>();
            window.Init(template);

            return window;
        }
        
        private void Init(TemplateSet template) {
            if(template == null) { this.Close(); }

            this.previousLabel = template.label;
            this.template = template;

            Action repaint = () => this.Repaint();
            this.ignores = new StringListField("Paths that ignore", repaint, this.template.ignores);
            this.considers = new StringListField("Paths that consider", repaint, this.template.considers);
            this.functions = new ListField<ExecutorFunctionSet>("Executor functions",
                (index, value) => this.GUI4ExecutorFunction(index, value),
                repaint, 
                this.template.functions);

            this.functions.ShowHorizontalScrollBar = false;
            this.efpath = Getters4Editor.GetExecutorFunctionsPath(MetadataHolder.Instance);
            this.efobject = Getters4Editor.GetExecutorFunctionsObject(MetadataHolder.Instance);

            this.titleContent.text = "Edit Template";
            this.titleContent.tooltip = "Edit Template Window";
        }

        private void ChangeLabel() {
            var previous = this.previousLabel;

            if(previous != this.template.label) {
                AssetDatabase.StartAssetEditing();

                var guids = AssetDatabase.FindAssets("t: TextAsset l: " + previous);
                foreach(var guid in guids) {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var importer = AssetImporter.GetAtPath(path);

                    if(importer != null) {
                        var labels = AssetDatabase.GetLabels(importer).ToList();
                        labels.Remove(previous);
                        labels.Add(this.template.label);

                        AssetDatabase.SetLabels(importer, labels.ToArray());
                        AssetDatabase.ImportAsset(path);
                    }
                }

                this.previousLabel = this.template.label;
                AssetDatabase.StopAssetEditing();
            }
        }

        #region "GUI"
        void OnGUI() {
            if(this.ignores == null || this.considers == null || this.functions == null) { this.Init(this.template); }

            EditorGUILayout.LabelField("CustomTemplate: Edit Tempalte for " + this.template.Extension);
            GUILayout.Space(20.0f);

            #region "Validation"
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Setting template's validation");
            this.template.validity = (Validater)EditorGUILayout.EnumPopup(this.template.validity);
            EditorGUILayout.EndHorizontal();
            #endregion

            #region "Label"
            EditorGUILayout.BeginHorizontal();
            this.template.label = EditorGUILayout.TextField("Label for customized", this.template.label);
            if(GUILayout.Button("Apply")) { this.ChangeLabel(); }
            EditorGUILayout.EndHorizontal();
            #endregion

            this.ignores.Draw();
            this.considers.Draw();

            #region "Context"
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Context of custom template for " + this.template.Extension);
            this.template.context = EditorGUILayout.TextArea(this.template.context);
            EditorGUILayout.Space();
            #endregion

            #region "Edit Executor Functions"
            this.functions.Draw();
            if(this.functions.Folding) {
                EditorGUILayout.Space();
                EditorGUI.indentLevel += 1;
                if(this.efobject == null) { return; }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Edit Executor Functions Script", GUILayout.ExpandWidth(false));
                if(GUILayout.Button("Open Editor")) {
                    AssetDatabase.OpenAsset(this.efobject);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel -= 1;
            }
            #endregion

            #region "Path of Executor Functions Script"
            GUILayout.Space(5.0f);
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.Space(20.0f);
            EditorGUILayout.TextField("File path", this.efpath);
            EditorGUI.EndDisabledGroup();
            #endregion

            this.template.ignores = this.ignores.GetValidityList();
            this.template.considers = this.considers.GetValidityList();
            this.template.functions = this.functions.GetValidityList();
        }

        private ExecutorFunctionSet GUI4ExecutorFunction(int index, ExecutorFunctionSet value) {
            if(value == null) { return new ExecutorFunctionSet(); }

            EditorGUILayout.BeginVertical();

            #region "Embedded Key"
            EditorGUILayout.BeginHorizontal();
            var key = EditorGUILayout.TextField("Embedded key", value.key);
            EditorGUILayout.LabelField("");
            EditorGUILayout.EndHorizontal();
            #endregion

            #region "Funciton name"
            EditorGUILayout.BeginHorizontal();
            var function = EditorGUILayout.TextField("Function name", value.function);
            EditorGUILayout.LabelField(value.delegater == null ? "Not Found Function" : "");
            EditorGUILayout.EndHorizontal();
            #endregion

            EditorGUILayout.EndVertical();
            GUILayout.Space(7.5f);

            return (key != value.key || function != value.function ? new ExecutorFunctionSet(key, function) : value);
        }
        #endregion
    }
}
