﻿using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;


namespace A3Utility.CustomTemplate {
    public class CustomTemplateExecutor : AssetPostprocessor {
        private static ExecutorsFunctions Functions = new ExecutorsFunctions();
        private static CustomTemplateFlash Flash = null;


        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath) {
            if(importedAssets.Length <= 0) { return; }
            if(Flash == null) { Flash = CustomTemplateWindow.GetFlashComponent(); }
            
            var md = Flash.metadatas;
            if(md.validity == CustomMetadatas.Validater.invalidate) { return; }
            

            foreach(var path in importedAssets) {
                var file = Path.GetFileName(path);
                if(!file.EndsWith(".cs") || file.StartsWith("_")) { continue; }
                
                var importer = AssetImporter.GetAtPath(path);
                if(importer == null) { continue; }

                var labels = AssetDatabase.GetLabels(importer).ToList();
                if(labels.Contains(md.label)) { continue; }

                if(md.ignores.Any(ignore => path.Contains(ignore)) || !md.considers.Any(consider => path.Contains(consider))) {
                    continue;
                }

                var context = md.context;
                foreach(var func in md.functions.FindAll(f => f.delegater != null)) { context = context.Replace(func.key, func.delegater(Functions, path)); }
                if(Application.platform == RuntimePlatform.WindowsEditor) { context = context.Replace("\n", "\r\n"); }

                using(var writer = new StreamWriter(path, false, new UTF8Encoding(true, false))) {
                    writer.Write(context);
                }

                labels.Add(md.label);
                AssetDatabase.SetLabels(importer, labels.ToArray());
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }

            AssetDatabase.Refresh();
        }
    }
}
