// /*
//  * File Name: GridStructureDrawer.cs
//  * Description: This script is for ...
//  * 
//  * Author(s): Kokowolo, Will Lacey
//  * Date Created: December 17, 2022
//  * 
//  * Additional Comments:
//  *		File Line Length: 120
//  */

// using UnityEngine;
// using UnityEditor;

// using UnityEditor.UIElements;
// using UnityEngine.UIElements;

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;

// [CustomPropertyDrawer(typeof(GridStructure<>))]
// public class GridStructureDrawer : PropertyDrawer
// {
//     /************************************************************/
//     #region Functions

//     // Draw the property inside the given rect
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         // Using BeginProperty / EndProperty on the parent property means that
//         // prefab override logic works on the entire property.
//         EditorGUI.BeginProperty(position, label, property);

//         // Draw label
//         position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

//         // Don't make child fields be indented
//         var indent = EditorGUI.indentLevel;
//         EditorGUI.indentLevel = 0;

//         // Draw fields - pass GUIContent.none to each so they are drawn without labels
//         EditorGUI.PropertyField(position, property.FindPropertyRelative("_gridObjects2"), GUIContent.none);

//         // Set indent back to what it was
//         EditorGUI.indentLevel = indent;

//         EditorGUI.EndProperty();
//     }

//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//     {
//         SerializedProperty prop = property.FindPropertyRelative("_gridObjects2");
//         object obj = GetTargetObjectOfProperty(prop);

//         int totalLine = ((List<GridCell>) obj).Count + 2;

//         if(!prop.isExpanded)
//         {
//             totalLine = 1;
//         }

//         return EditorGUIUtility.singleLineHeight * totalLine + EditorGUIUtility.standardVerticalSpacing * (totalLine - 1);
//     }

//     public static object GetTargetObjectOfProperty(SerializedProperty prop)
//     {
//         if (prop == null) return null;

//         var path = prop.propertyPath.Replace(".Array.data[", "[");
//         object obj = prop.serializedObject.targetObject;
//         var elements = path.Split('.');
//         foreach (var element in elements)
//         {
//             if (element.Contains("["))
//             {
//                 var elementName = element.Substring(0, element.IndexOf("["));
//                 var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
//                 obj = GetValue_Imp(obj, elementName, index);
//             }
//             else
//             {
//                 obj = GetValue_Imp(obj, element);
//             }
//         }
//         return obj;
//     }

//     private static object GetValue_Imp(object source, string name)
//     {
//         if (source == null)
//             return null;
//         var type = source.GetType();

//         while (type != null)
//         {
//             var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
//             if (f != null)
//                 return f.GetValue(source);

//             var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
//             if (p != null)
//                 return p.GetValue(source, null);

//             type = type.BaseType;
//         }
//         return null;
//     }

//     private static object GetValue_Imp(object source, string name, int index)
//     {
//         var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
//         if (enumerable == null) return null;
//         var enm = enumerable.GetEnumerator();
//         //while (index-- >= 0)
//         //    enm.MoveNext();
//         //return enm.Current;

//         for (int i = 0; i <= index; i++)
//         {
//             if (!enm.MoveNext()) return null;
//         }
//         return enm.Current;
//     }
    
//     #endregion
//     /************************************************************/
// }