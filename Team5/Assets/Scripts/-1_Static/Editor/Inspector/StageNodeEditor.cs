// using UnityEditor;
// using UnityEngine;

// [CustomEditor(typeof(StageNode))]
// public class StageNodeEditor : Editor
// {
//     private bool showPrevNodes = true;
//     private bool showNextNodes = true;

//     public override void OnInspectorGUI()
//     {
//         StageNode node = (StageNode)target;

//         DrawStageNode(node, 0);
//     }

//     private void DrawStageNode(StageNode node, int indentLevel)
//     {
//         if (node == null) return;

//         EditorGUI.indentLevel = indentLevel;

//         // StageNode 기본 정보 표시
//         node.type = (StageNodeType)EditorGUILayout.EnumPopup("Type", node.type);
//         node.level = EditorGUILayout.IntField("Level", node.level);
//         node.number = EditorGUILayout.IntField("Number", node.number);

//         // prevNodes 표시 제어
//         showPrevNodes = EditorGUILayout.Foldout(showPrevNodes, "Previous Nodes");
//         if (showPrevNodes)
//         {
//             EditorGUI.indentLevel++;
//             if (node.prevNodes != null)
//             {
//                 for (int i = 0; i < node.prevNodes.Count; i++)
//                 {
//                     EditorGUILayout.LabelField($"Prev Node {i + 1}");
//                 }
//             }
//             else
//             {
//                 EditorGUILayout.LabelField("No Previous Nodes");
//             }
//             EditorGUI.indentLevel--;
//         }

//         // nextNodes 표시 제어
//         showNextNodes = EditorGUILayout.Foldout(showNextNodes, "Next Nodes");
//         if (showNextNodes)
//         {
//             EditorGUI.indentLevel++;
//             if (node.nextNodes != null)
//             {
//                 for (int i = 0; i < node.nextNodes.Count; i++)
//                 {
//                     EditorGUILayout.LabelField($"Next Node {i + 1}");
//                 }
//             }
//             else
//             {
//                 EditorGUILayout.LabelField("No Next Nodes");
//             }
//             EditorGUI.indentLevel--;
//         }
//     }
// }