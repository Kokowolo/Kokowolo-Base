/*
 * File Name: IPoolable.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 9, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable<T> 
{
    /************************************************************/
    #region Functions

    // public abstract static T Create();

    public abstract void OnAddPoolable();

    public abstract void OnGetPoolable();
    
    #endregion
    /************************************************************/
}