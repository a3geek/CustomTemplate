using UnityEngine;
using UnityEditor;


namespace A3Utility.CustomTemplate {
    public static class Getters4Editor {
        private static Object ExecutorFunctionsObject = null;

        private const string EXECUTOR_FUNCTIONS_SCRIPT_PATH = "Scripts/Unattachables/_ExecutorFunctions.cs";
        private const string PROJECT_NAME = "CustomTemplate";
        private const string FILE_NAME = "CustomTemplateData.dat";
        

        public static Object GetExecutorFunctionsObject(MetadataHolder holder = null) {
            if(ExecutorFunctionsObject == null) {
                var path = GetExecutorFunctionsPath(holder);
                ExecutorFunctionsObject = AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
            }

            return ExecutorFunctionsObject;
        }
        
        public static string GetExecutorFunctionsPath(MetadataHolder holder = null) {
            var path = GetFilePath(holder == null ? GetHolder() : holder);
            return path.TrimEnd(FILE_NAME.ToCharArray()) + EXECUTOR_FUNCTIONS_SCRIPT_PATH;
        }

        public static MetadataHolder GetHolder() {
            var holder = MetadataHolder.Instance;
            if(holder != null) { return holder; }

            holder = (new GameObject()).AddComponent<MetadataHolder>();
            var metadatas = (Metadatas)DataFiler.LoadFromFile(GetFullFilePath(GetFilePath(holder)));
            if(metadatas != null) { holder.metadatas = metadatas; }

            return holder;
        }

        public static string GetFilePath(MetadataHolder holder) {
            var root = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(holder));
            return root.Substring(0, root.IndexOf(PROJECT_NAME) + PROJECT_NAME.Length) + "/" + FILE_NAME;
        }

        public static string GetFullFilePath(string relation) {
            return Application.dataPath.TrimEnd("Assets".ToCharArray()) + relation;
        }
    }
}
