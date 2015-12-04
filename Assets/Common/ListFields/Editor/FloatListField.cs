using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


namespace A3Utility.ListFields {
    public class FloatListField : ListField<float> {
        public FloatListField(string label) : this(label, null, null) {; }
        public FloatListField(string label, Action repaint) : this(label, repaint, null) {; }
        public FloatListField(string label, List<float> initial) : this(label, null, initial) {; }
        public FloatListField(string label, Action repaint, List<float> initial)
            : this(label, (index, value) => EditorGUILayout.FloatField(index.ToString(), value), repaint, initial) {; }

        public FloatListField(string label, Func<int, float, float> drawer, Action repaint, List<float> initial)
            : base(label, drawer, repaint, initial) {; }
    }
}
