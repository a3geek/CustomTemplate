using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


namespace A3Utility.EditorGUIExtender.ListFields {
    public class IntListField : ListField<int> {
        public IntListField(string label) : this(label, null, null) {; }
        public IntListField(string label, Action repaint) : this(label, repaint, null) {; }
        public IntListField(string label, List<int> initial) : this(label, null, initial) {; }
        public IntListField(string label, Action repaint, List<int> initial)
            : this(label, (index, value) => EditorGUILayout.IntField(index.ToString(), value), repaint, initial) {; }

        public IntListField(string label, Func<int, int, int> drawer, Action repaint, List<int> initial)
            : base(label, drawer, repaint, initial) {; }
    }
}
