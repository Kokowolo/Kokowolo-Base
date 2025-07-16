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

using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

// using Kokowolo.Utilities;
using Kokowolo.Base.Demo.SchedulingDemo;

public class SchedulingDemo
{
    /*██████████████████████████████████████████████████████████*/
    #region Functions
    /*——————————————————————————————————————————————————————————*/
    #region SetUp & TearDown

    [OneTimeSetUp] 
    public virtual void OneTimeSetUp()
    {
        Utils.EnsureTestSceneIsLoaded();
    }

    #endregion
    /*——————————————————————————————————————————————————————————*/
    #region Tests

    [UnityTest]
    public IEnumerator _00()
    {
        Debug.Assert(JobManager.Instance);
        yield return null; // Wait for instance to set... for some reason this cant run in a normal Test
    }

    [UnityTest]
    public IEnumerator _01_0()
    {
        // Demo main
        float time = 0.1f;
        int value = 0;
        Job p1 = JobManager.StartJob(Function1, time);
        Job p2 = JobManager.StartJob(Function1, time);
        Job p3 = JobManager.StartJob(Function1, time);

        // Declare local function
        void Function1()
        {
            value += 1;
        }

        // Prepare GC check
        WeakReference p1Reference = new WeakReference(p1);
        WeakReference p2Reference = new WeakReference(p2);
        WeakReference p3Reference = new WeakReference(p3);
        Debug.Assert(p1Reference.IsAlive && p2Reference.IsAlive && p3Reference.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        yield return new WaitForJobManager();
        Debug.Assert(value == 3);
        Debug.Assert(p1.IsDisposed && p2.IsDisposed && p3.IsDisposed);

        // Evaluate GC
        p1 = p2 = p3 = null;
        GC.Collect();
        Debug.Assert(!p1Reference.IsAlive && !p2Reference.IsAlive && !p3Reference.IsAlive);
    }

    [UnityTest]
    public IEnumerator _01_1()
    {
        // Demo main
        float time = 0.1f;
        int value = 0;
        int increment = 1;
        Job p1 = JobManager.StartJob(Function1(increment++));
        Job p2 = JobManager.StartJob(Function1(increment++));
        Job p3 = JobManager.StartJob(Function1(increment++));

        // Declare local function
        IEnumerator Function1(int i)
        {
            yield return new WaitForSeconds(time);
            value += i;
        }

        // Prepare GC check
        WeakReference p1Reference = new WeakReference(p1);
        WeakReference p2Reference = new WeakReference(p2);
        WeakReference p3Reference = new WeakReference(p3);
        Debug.Assert(p1Reference.IsAlive && p2Reference.IsAlive && p3Reference.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        yield return new WaitForJobManager();
        Debug.Assert(value == 6);
        Debug.Assert(p1.IsDisposed && p2.IsDisposed && p3.IsDisposed);

        // Evaluate GC
        p1 = p2 = p3 = null;
        GC.Collect();
        Debug.Assert(!p1Reference.IsAlive && !p2Reference.IsAlive && !p3Reference.IsAlive);
    }

    [UnityTest]
    public IEnumerator _02_0()
    {
        // Demo main
        float time = 0.1f;
        int value = 0;
        Job p1 = JobManager.ScheduleJob(Function1, time);
        Job p2 = JobManager.ScheduleJob(Function1, time);
        Job p3 = JobManager.ScheduleJob(Function1, time);

        // Declare local function
        void Function1()
        {
            value += 1;
        }

        // Prepare GC check
        WeakReference p1Reference = new WeakReference(p1);
        WeakReference p2Reference = new WeakReference(p2);
        WeakReference p3Reference = new WeakReference(p3);
        Debug.Assert(p1Reference.IsAlive && p2Reference.IsAlive && p3Reference.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        yield return new WaitForJob(p2);
        Debug.Assert(value == 2);
        yield return new WaitForJobManager();
        Debug.Assert(value == 3);
        Debug.Assert(p1.IsDisposed && p2.IsDisposed && p3.IsDisposed);

        // Evaluate GC
        p1 = p2 = p3 = null;
        GC.Collect();
        Debug.Assert(!p1Reference.IsAlive && !p2Reference.IsAlive && !p3Reference.IsAlive);
    }

    [UnityTest]
    public IEnumerator _02_1()
    {
        // Demo main
        float time = 0.1f;
        int value = 0;
        int increment = 1;
        Job p1 = JobManager.ScheduleJob(Function1(increment++));
        Job p2 = JobManager.ScheduleJob(Function1(increment++));
        Job p3 = JobManager.ScheduleJob(Function1(increment++));

        // Declare local function
        IEnumerator Function1(int i)
        {
            yield return new WaitForSeconds(time);
            value += i;
        }

        // Prepare GC check
        WeakReference p1Reference = new WeakReference(p1);
        WeakReference p2Reference = new WeakReference(p2);
        WeakReference p3Reference = new WeakReference(p3);
        Debug.Assert(p1Reference.IsAlive && p2Reference.IsAlive && p3Reference.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        yield return new WaitForJob(p2);
        Debug.Assert(value == 3);
        yield return new WaitForJobManager();
        Debug.Assert(value == 6);
        Debug.Assert(p1.IsDisposed && p2.IsDisposed && p3.IsDisposed);

        // Evaluate GC
        p1 = p2 = p3 = null;
        GC.Collect();
        Debug.Assert(!p1Reference.IsAlive && !p2Reference.IsAlive && !p3Reference.IsAlive);
    }

    #endregion
    /*——————————————————————————————————————————————————————————*/

    #endregion
    /*██████████████████████████████████████████████████████████*/
}
