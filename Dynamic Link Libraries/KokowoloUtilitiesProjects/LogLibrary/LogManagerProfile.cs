/*
 * File Name: LogManagerProfile.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: September 5, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Utilities//.Analytics
{
    [CreateAssetMenu(menuName = "Kokowolo/Utilities/LogManager Profile", fileName = LogManager.LogManagerProfileString)]
    public class LogManagerProfile : ScriptableObject
    {
        /************************************************************/
        #region Fields

        [SerializeField] private bool _LogMessageWithClassTag = true;
        [SerializeField] private bool _ThrowWhenLoggingException = true;

        #endregion
        /************************************************************/
        #region Properties

        public bool LogMessageWithClassTag => _LogMessageWithClassTag;
        public bool ThrowWhenLoggingException => _ThrowWhenLoggingException;

        #endregion
        /************************************************************/
        #region Functions
        
        #endregion
        /************************************************************/
    }
}