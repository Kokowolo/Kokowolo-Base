// /*
//  * File Name: GridCoordinatesDrawer.cs
//  * Description: This script is for ...
//  * 
//  * Author(s): Kokowolo, Will Lacey
//  * Date Created: November 29, 2022
//  * 
//  * Additional Comments:
//  *		File Line Length: 120
//  */

// using UnityEngine;
// using UnityEditor;

// [CustomPropertyDrawer(typeof(GridCoordinates))]
// public class GridCoordinatesDrawer : PropertyDrawer
// {
//     /************************************************************/
//     #region Functions

//     /// <summary>
//     /// Unity Function; OnGUI is called for rendering and handling GUI events
//     /// </summary>
//     /// <param name="position">where to draw in the Editor</param>
//     /// <param name="property">data to draw</param>
//     /// <param name="label">label to append to data</param>
// 	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
// 	{
//         EditorGUI.BeginProperty(position, label, property);

//         // fetches the HexCoordinates' serialized properties x, y, & z
//         GridCoordinates coordinates = new GridCoordinates(
// 			property.FindPropertyRelative("y").intValue,
//             property.FindPropertyRelative("z").intValue,
//             property.FindPropertyRelative("x").intValue
//         );

//         // adjusts the position of the label, idk how though
//         position = EditorGUI.PrefixLabel(position, label);

//         // sets the label value in the editor
// 		GUI.Label(position, $"{coordinates.Index} @ {coordinates}");

//         EditorGUI.EndProperty();
// 	}

//     #endregion
//     /************************************************************/
// }