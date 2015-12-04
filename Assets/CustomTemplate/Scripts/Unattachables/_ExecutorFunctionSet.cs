using System;


namespace A3Utility.CustomTemplate {
    using FunctionDelegate = ExecutorFunctions.FunctionDelegate;

    [Serializable]
    public class ExecutorFunctionSet {
        public string key = "";
        public string function = "";
        [NonSerialized]
        public FunctionDelegate delegater = null;


        public ExecutorFunctionSet() : this("", "") {; }
        public ExecutorFunctionSet(string key, string function) {
            this.key = key;
            this.function = function;

            this.CheckMethod();
        }

        public void CheckMethod() {
            if(this.function == "") {
                this.delegater = null;
                return;
            }

            var method = typeof(ExecutorFunctions).GetMethod(this.function);
            if(method == null) {
                this.delegater = null;
            }
            else {
                this.delegater = (FunctionDelegate)Delegate.CreateDelegate(typeof(FunctionDelegate), method);
            }
        }
    }
}
