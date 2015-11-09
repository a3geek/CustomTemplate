using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


namespace A3Utility.EditorGUIExtender.ListFields {
    public class ListField<T> {
        public float MinHeight { get; set; }
        public bool ShowHorizontalScrollBar { get; set; }
        public bool ShowVerticalScrollBar { get; set; }
        public int Count { get; protected set; }
        public int Count4Validity { get { return this.GetValidityList().Count; } }
        public bool Folding { get; protected set; }

        protected List<T> list = new List<T>();
        protected string label = "";
        protected string controlName = "";
        protected Action repaint = () => { InternalEditorUtility.RepaintAllViews(); };
        protected Func<int, T, T> drawer = (index, value) => { return value; };

        protected int inputtingCount = 0;
        protected Vector2 scroll = Vector2.zero;


        public ListField(string label, Func<int, T, T> drawer) : this(label, drawer, null, null) {; }
        public ListField(string label, Func<int, T, T> drawer, Action repaint) : this(label, drawer, repaint, null) {; }
        public ListField(string label, Func<int, T, T> drawer, List<T> initial) {; }

        public ListField(string label, Func<int, T, T> drawer, Action repaint, List<T> initial) {
            this.label = label;
            this.drawer = drawer;
            this.repaint = (repaint != null ? repaint : this.repaint);

            this.controlName = label + Guid.NewGuid().ToString();

            if(initial != null && initial.Count > 0) {
                this.list.AddRange(initial);
                this.inputtingCount = this.Count = initial.Count;
            }

            this.MinHeight = 100.0f;
            this.ShowHorizontalScrollBar = true;
            this.ShowVerticalScrollBar = true;
        }

        public void Draw() {
            if(this.EventCheck(EventType.KeyDown, KeyCode.Return) && GUI.GetNameOfFocusedControl() == this.controlName) {
                this.Arrangement();
            }
            this.DrawGUI();
        }

        public List<T> GetList() {
            return this.list.GetRange(0, this.Count);
        }

        public List<T> GetValidityList() {
            return this.GetList().FindAll(e => e != null);
        }
        
        protected void DrawGUI() {
            this.Folding = EditorGUILayout.Foldout(this.Folding, this.label);

            if(this.Folding) {
                EditorGUI.indentLevel += 1;

                EditorGUILayout.BeginHorizontal();
                GUI.SetNextControlName(this.controlName);

                this.inputtingCount = EditorGUILayout.IntField("Count of list", this.inputtingCount);
                if(GUILayout.Button("Apply")) { this.Arrangement(); }

                EditorGUILayout.EndHorizontal();
                
                EditorGUI.indentLevel += 1;
                EditorGUILayout.BeginVertical(GUILayout.MinHeight(this.MinHeight));
                this.scroll = EditorGUILayout.BeginScrollView(this.scroll, this.ShowHorizontalScrollBar, this.ShowVerticalScrollBar);

                for(int i = 0; i < this.Count && i < this.list.Count; i++) {
                    this.list[i] = this.drawer(i, this.list[i]);
                }

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel -= 2;
            }
        }

        protected void Arrangement() {
            for(int i = this.inputtingCount - this.list.Count; i > 0; i--) {
                this.list.Add(default(T));
            }

            this.Count = this.inputtingCount;
        }

        protected bool EventCheck(EventType type, KeyCode code) {
            return (Event.current.type == type ? Event.current.keyCode == code : false);
        }
    }
}
