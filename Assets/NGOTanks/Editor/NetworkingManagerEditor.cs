using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Editor;
using UnityEditor;
using UnityEditor.TerrainTools;

namespace NGOTanks
{
    [CustomEditor(typeof(NetworkingManager))]
    public class NetworkingManagerEditor : NetworkManagerEditor
    {

        SerializedProperty Prop_GO_tankPrefab;

        private void OnEnable()
        {
            Prop_GO_tankPrefab = serializedObject.FindProperty("tankPrefab");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Custom Properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(Prop_GO_tankPrefab);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
