// /* 
//  * Author(s): Kokowolo, Will Lacey
//  * Date Created: July 14, 2025
//  * 
//  * Additional Comments:
//  *      File Line Length: ~140
//  */

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Kokowolo.Utilities;

// namespace Kokowolo.Base.Demo.SchedulingDemo
// {
//     public class JobRunner : IDisposable
//     {
//         /*██████████████████████████████████████████████████████████*/
//         #region Events

//         public event Action OnJobAdded;

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Fields
        
//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Properties

//         

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Functions

//         bool disposed;
//         ~JobRunner() => Dispose();
//         public void Dispose()
//         {
//             if (disposed) return;
//             disposed = true;
//         }

//         public JobRunner()
//         {
//         }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//     }
// }