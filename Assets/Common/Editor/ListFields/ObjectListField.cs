using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


namespace A3Utility.EditorGUIExtender.ListFields {
    public class ObjectListField<T> : ListField<T> where T : UnityEngine.Object {
        public ObjectListField(string label) : this(label, null, null) {; }
        public ObjectListField(string label, Action repaint) : this(label, repaint, null) {; }
        public ObjectListField(string label, List<T> initial) : this(label, null, initial) {; }
        public ObjectListField(string label, Action repaint, List<T> initial)
            : this(label, (index, value) => (T)EditorGUILayout.ObjectField(index.ToString(), value, typeof(T), true), repaint, initial) {; }
        
        public ObjectListField(string label, Func<int, T, T> drawer, Action repaint, List<T> initial)
            : base(label, drawer, repaint, initial) {; }
    }
}
