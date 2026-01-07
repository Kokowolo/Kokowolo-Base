// /* 
//  * Author(s): Kokowolo, Will Lacey
//  * Date Created: July 23, 2025
//  * 
//  * Additional Comments:
//  *      File Line Length: ~140
//  */

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Kokowolo.Utilities
// {
//     public class ID<T> where T : class
//     {
//         /*██████████████████████████████████████████████████████████*/
//         #region Fields

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Properties

//         public int id { get; }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Functions

//         public ID()
//         {
//             id = Reference<T>.id++;
//         }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Subclasses

//         static class Reference<T> where T : class
//         {
//             public static int id = 0;
//         }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//     }
// }