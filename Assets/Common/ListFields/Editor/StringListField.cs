using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


namespace A3Utility.ListFields {
    public class StringListField : ListField<string> {
        public StringListField(string label) : this(label, null, null) {; }
        public StringListField(string label, Action repaint) : this(label, repaint, null) {; }
        public StringListField(string label, List<string> initial) : this(label, null, initial) {; }
        public StringListField(string label, Action repaint, List<string> initial)
            : this(label, (index, value) => EditorGUILayout.TextField(index.ToString(), value), repaint, initial) {; }

        public StringListField(string label, Func<int, string, string> drawer, Action repaint, List<string> initial)
            : base(label, drawer, repaint, initial) {; }
    }
}
