/* 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: July 17, 2025
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kokowolo.Utilities;

namespace Kokowolo.Base.Demos.SchedulingDemo
{
    [CreateAssetMenu(menuName = "Kokowolo/Base/Demos/SchedulingDemo/PlayModeConfig", fileName = "PlayModeConfig")]
    public class PlayModeConfig : ScriptableObject
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        [SerializeField, Min(-0.01f)] float time;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        static PlayModeConfig _Instance;
        public static PlayModeConfig Instance 
        {
            get
            {
                if (!_Instance)
                {
                    _Instance = Utilities.Editor.EditorOnlyUtils.FindFirstAssetByType<PlayModeConfig>();
                }
                return _Instance;
            }
        }

        public static float Time => Instance.time;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}