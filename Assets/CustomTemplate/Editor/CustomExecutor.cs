﻿using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;


namespace A3Utility.CustomTemplate {
    public class CustomExecutor : AssetPostprocessor {
        private static ExecutorFunctions Functions = new ExecutorFunctions();
        private static MetadataHolder Holder = null;
        private static UTF8Encoding Encoding = new UTF8Encoding(true, false);


        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath) {
            if(importedAssets.Length <= 0) { return; }
            if(Holder == null) { Holder = Getters4Editor.GetHolder(); }
            
            var md = Holder.metadatas;
            if(md.validity == Validater.invalidate) { return; }

            foreach(var path in importedAssets) {
                var file = Path.GetFileName(path);
                if(file.StartsWith("_")) { return; }

                var extension = Path.GetExtension(path);
                var template = md.templates.FirstOrDefault(e => e.Extension == extension);
                if(template == null || template.validity == Validater.invalidate) { return; }

                var importer = AssetImporter.GetAtPath(path);
                if(importer == null) { continue; }

                var labels = AssetDatabase.GetLabels(importer).ToList();
                if(labels.Contains(template.label)) { continue; }

                if(template.ignores.Any(ig => path.Contains(ig)) || !template.considers.Any(co => path.Contains(co))) { continue; }

                Custom(path, template);

                labels.Add(template.label);
                AssetDatabase.SetLabels(importer, labels.ToArray());
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }

            AssetDatabase.Refresh();
        }

        private static void Custom(string path, TemplateSet template) {
            var context = template.context;
            foreach(var func in template.functions.FindAll(f => f.delegater != null)) {
                context = context.Replace(func.key, func.delegater(Functions, path));
            }
            
            using(var writer = new StreamWriter(path, false, Encoding)) {
                writer.Write(context);
            }
        }
    }
}
