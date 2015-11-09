﻿using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace A3Utility.CustomTemplate {
    using EditorGUIExtender.ListFields;
    using ExecutorsFunctionSet = CustomMetadatas.ExecutorsFunctionSet;

    public class CustomTemplateWindow : EditorWindow {
        private const string FILE_NAME = "CustomTemplateData.dat";
        private CustomTemplateFlash flash = null;
        private CustomMetadatas metadatas = null;
        private StringListField ignores = null;
        private StringListField considers = null;
        private ListField<ExecutorsFunctionSet> functions = null;
        private Object efobject = null;
        private string path = "";


        [MenuItem("A3Utility/Cutom Template/Open Editor Window")]
        public static CustomTemplateWindow Open() {
            var window = GetWindow<CustomTemplateWindow>();
            window.Init();

            return window;
        }

        public static CustomTemplateFlash GetFlashComponent() {
            CustomTemplateFlash flash = null;

            var obj = GameObject.Find(CustomTemplateFlash.ObjectName);
            if(obj != null) { flash = obj.GetComponent<CustomTemplateFlash>(); }

            if(flash == null) { flash = (new GameObject()).AddComponent<CustomTemplateFlash>(); }
            else { return flash; }

            var metadatas = (CustomMetadatas)CustomTemplateDataFiler.LoadFromFile(GetFullFilePath(GetFilePath(flash)));
            if(metadatas != null) {
                flash.metadatas = metadatas;
            }

            return flash;
        }

        public static string GetFilePath(CustomTemplateFlash flash) {
            var split = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(flash)).Split('/').ToList();
            split.RemoveRange(split.Count - (split.Count <= 2 ? 1 : 2), split.Count <= 2 ? 1 : 2);

            return split.Aggregate((s, c) => s + "/" + c) + "/" + FILE_NAME;
        }

        public static string GetFullFilePath(string relation) {
            return Application.dataPath.TrimEnd("Assets".ToCharArray()) + relation;
        }

        private void Init() {
            if(this.flash == null) {
                this.flash = GetFlashComponent();
            }

            this.metadatas = new CustomMetadatas(this.flash.metadatas);
            this.ignores = new StringListField("Paths that ignore", () => { this.Repaint(); }, this.metadatas.ignores);
            this.considers = new StringListField("Paths that consider", () => { this.Repaint(); }, this.metadatas.considers);
            this.functions = new ListField<ExecutorsFunctionSet>("Executors functions", (index, value) => {
                if(value == null) { return new ExecutorsFunctionSet(); }

                EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();
                        var key = EditorGUILayout.TextField("Embedded key", value.key);
                        EditorGUILayout.LabelField("");
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.BeginHorizontal();
                        var function = EditorGUILayout.TextField("Function name", value.function);
                        EditorGUILayout.LabelField(value.delegater == null ? "Not Found Function" : "");
                    EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                GUILayout.Space(7.5f);

                return (key != value.key || function != value.function ?
                    new ExecutorsFunctionSet(key, function) :
                    value
                );
            }, () => { this.Repaint(); }, this.metadatas.functions);

            this.functions.ShowHorizontalScrollBar = false;
            this.path = GetFilePath(this.flash);

            var efscript = this.path.TrimEnd(FILE_NAME.ToCharArray()) + "Scripts/_CustomTemplateExecutorsFunctions.cs";
            this.efobject = AssetDatabase.LoadAssetAtPath(efscript, typeof(TextAsset));
        }

        private void OnGUI() {
            this.titleContent.text = "CustomTemplate";
            if(this.ignores == null || this.considers == null || this.functions == null || this.flash == null) { this.Init(); }

            EditorGUILayout.LabelField("CustomTemplate Editor Window");
            GUILayout.Space(20.0f);

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Setting CustomTemplate's validation");
                this.metadatas.validity = (CustomMetadatas.Validater)EditorGUILayout.EnumPopup(this.metadatas.validity);
            EditorGUILayout.EndHorizontal();
            this.metadatas.label = EditorGUILayout.TextField("Label for customized", this.metadatas.label);

            this.ignores.Draw();
            this.considers.Draw();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Context of custom template");
            this.metadatas.context = EditorGUILayout.TextArea(this.metadatas.context);
            EditorGUILayout.Space();

            this.functions.Draw();

            if(this.functions.Folding) {
                EditorGUILayout.Space();
                EditorGUI.indentLevel = 1;

                if(this.efobject == null) { return; }
                EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Edit ExecutorsFunctions Script", GUILayout.ExpandWidth(false));
                    if(GUILayout.Button("Open Editor")) {
                        AssetDatabase.OpenAsset(this.efobject);
                    }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel = 0;
            }

            GUILayout.Space(5.0f);
            EditorGUI.BeginDisabledGroup(true);
                GUILayout.Space(20.0f);
                EditorGUILayout.TextField("File path", this.path);
            EditorGUI.EndDisabledGroup();

            if(GUILayout.Button("Save")) {
                this.Save2File();
            }
        }

        private void Save2File() {
            this.metadatas.ignores = this.ignores.GetValidityList();
            this.metadatas.considers = this.considers.GetValidityList();
            this.metadatas.functions = this.functions.GetValidityList();

            CustomTemplateDataFiler.Save2File(this.metadatas, GetFullFilePath(this.path));

            if(this.flash.metadatas.label != this.metadatas.label) {
                AssetDatabase.StartAssetEditing();

                var guids = AssetDatabase.FindAssets("t: TextAsset l: " + this.flash.metadatas.label);
                foreach(var guid in guids) {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var importer = AssetImporter.GetAtPath(path);

                    if(importer != null) {
                        var labels = AssetDatabase.GetLabels(importer).ToList();
                        labels.Remove(this.flash.metadatas.label);
                        labels.Add(this.metadatas.label);

                        AssetDatabase.SetLabels(importer, labels.ToArray());
                        AssetDatabase.ImportAsset(path);
                    }
                }

                AssetDatabase.StopAssetEditing();
            }

            this.flash.metadatas = this.metadatas;

            AssetDatabase.Refresh();

            if(EditorUtility.DisplayDialog("Success", "Custom Template Data was Saved.", "Close")) {
                this.Close();
            }
        }
    }
}
