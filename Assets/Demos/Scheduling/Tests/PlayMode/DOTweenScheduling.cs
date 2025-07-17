/* 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: July 15, 2025
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using DG.Tweening;

using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

// using Kokowolo.Utilities;
using Kokowolo.Base.Demos.SchedulingDemo;

public class DOTweenScheduling
{
    /*██████████████████████████████████████████████████████████*/
    #region Fields
    
    const float time = 0.1f;
    
    #endregion
    /*██████████████████████████████████████████████████████████*/
    #region Functions
    /*——————————————————————————————————————————————————————————*/
    #region SetUp & TearDown

    [OneTimeSetUp] 
    public virtual void OneTimeSetUp()
    {
        TestsUtils.EnsureTestSceneIsLoaded();
    }

    #endregion
    /*——————————————————————————————————————————————————————————*/
    #region Tests

    [UnityTest]
    public IEnumerator _00()
    {
        GameObject gameObject = new GameObject();
        Tween t1 = gameObject.transform.DOMoveX(2, 1);
        yield return t1.WaitForCompletion();
        t1 = null;

        Tween t2 = gameObject.transform.DOMoveX(-2, 1);
        yield return t2.WaitForCompletion();
        t2 = null;

        Debug.Assert(gameObject.transform.position.x == -2);
        GameObject.DestroyImmediate(gameObject);
    }

    // [UnityTest]
    // public IEnumerator _00()
    // {
    //     GameObject gameObject = new GameObject();
    //     var s1 = JobSequence.Get();

    //     gameObject.transform.DOMoveX(2, 1).OnPause();

    //     // void _F1()
    //     // {
            
    //     // }
    //     // s1.Append(_F1, 0);

        
    //     Tween t1 = 
    //     yield return t1.WaitForCompletion();
    //     t1 = null;

    //     Tween t2 = gameObject.transform.DOMoveX(-2, 1);
    //     yield return t2.WaitForCompletion();
    //     t2 = null;

    //     Debug.Assert(gameObject.transform.position.x == -2);
    // }

    #endregion
    /*——————————————————————————————————————————————————————————*/

    #endregion
    /*██████████████████████████████████████████████████████████*/
}
