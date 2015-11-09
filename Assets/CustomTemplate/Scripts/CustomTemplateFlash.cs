#if UNITY_EDITOR﻿﻿
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace A3Utility.CustomTemplate {
    [ExecuteInEditMode()]
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    public class CustomTemplateFlash : MonoBehaviour {
        public static readonly string ObjectName = "_CustomTemplateFlash";
        public CustomMetadatas metadatas = new CustomMetadatas();


        void Awake() {
            gameObject.name = ObjectName;
            this.Hide();
        }

        public void Show() {
            gameObject.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
        }

        public void Hide() {
            gameObject.hideFlags = HideFlags.HideAndDontSave | HideFlags.NotEditable;
        }
    }
}
#endif
