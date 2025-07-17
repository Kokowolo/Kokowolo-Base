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
using Kokowolo.Base.Demos.SchedulingDemo;

public class SchedulingDemo
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
        Debug.Assert(JobManager.Instance);
        yield return null; // Wait for instance to set... for some reason this cant run in a normal Test
    }

    [UnityTest]
    public IEnumerator _01_0()
    {
        // Demo main
        int value = 0;
        Job p1 = Job.Get(Function1, time);
        Job p2 = Job.Get(Function1, time);
        Job p3 = Job.Get(Function1, time);

        // Declare local function
        void Function1()
        {
            value += 1;
        }

        // Prepare GC check
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        WeakReference r3 = new WeakReference(p3);
        Debug.Assert(r1.IsAlive && r2.IsAlive && r3.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        yield return new WaitForJobManager();
        Debug.Assert(value == 3);
        Debug.Assert(p1.IsDisposed && p2.IsDisposed && p3.IsDisposed);

        // Evaluate GC
        p1 = p2 = p3 = null;
        GC.Collect();
        Debug.Assert(!r1.IsAlive && !r2.IsAlive && !r3.IsAlive);
    }

    [UnityTest]
    public IEnumerator _01_1()
    {
        // Demo main
        int value = 0;
        int increment = 1;
        Job p1 = Job.Get(Function1(increment++));
        Job p2 = Job.Get(Function1(increment++));
        Job p3 = Job.Get(Function1(increment++));

        // Declare local function
        IEnumerator Function1(int i)
        {
            yield return new WaitForSeconds(time);
            value += i;
        }

        // Prepare GC check
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        WeakReference r3 = new WeakReference(p3);
        Debug.Assert(r1.IsAlive && r2.IsAlive && r3.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        yield return new WaitForJobManager();
        Debug.Assert(value == 6);
        Debug.Assert(p1.IsDisposed && p2.IsDisposed && p3.IsDisposed);

        // Evaluate GC
        p1 = p2 = p3 = null;
        GC.Collect();
        Debug.Assert(!r1.IsAlive && !r2.IsAlive && !r3.IsAlive);
    }

    [UnityTest]
    public IEnumerator _02_0()
    {
        // Demo main
        int value = 0;
        Job p1 = Job.Schedule(Function1, time);
        Job p2 = Job.Schedule(Function1, time);
        Job p3 = Job.Schedule(Function1, time);

        // Declare local function
        void Function1()
        {
            value += 1;
        }

        // Prepare GC check
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        WeakReference r3 = new WeakReference(p3);
        Debug.Assert(r1.IsAlive && r2.IsAlive && r3.IsAlive);

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
        Debug.Assert(!r1.IsAlive && !r2.IsAlive && !r3.IsAlive);
    }

    [UnityTest]
    public IEnumerator _02_1()
    {
        // Demo main
        int value = 0;
        int increment = 1;
        Job p1 = Job.Schedule(Function1(increment++));
        Job p2 = Job.Schedule(Function1(increment++));
        Job p3 = Job.Schedule(Function1(increment++));

        // Declare local function
        IEnumerator Function1(int i)
        {
            yield return new WaitForSeconds(time);
            value += i;
        }

        // Prepare GC check
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        WeakReference r3 = new WeakReference(p3);
        Debug.Assert(r1.IsAlive && r2.IsAlive && r3.IsAlive);

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
        Debug.Assert(!r1.IsAlive && !r2.IsAlive && !r3.IsAlive);
    }

    [UnityTest]
    public IEnumerator _03_0()
    {
        // Demo main
        int value = 0;
        JobSequence s1 = JobSequence.Get();
        Job p2 = Job.Get(Function1, time); s1.Append(p2);
        s1.Append(Function1, time);
        Job p3 = Job.Get(Function1, time); s1.Append(p3);

        // Declare local function
        void Function1()
        {
            value += 1;
        }

        // Prepare GC check
        WeakReference r1 = new WeakReference(s1);
        WeakReference r2 = new WeakReference(p2);
        WeakReference r3 = new WeakReference(p3);
        Debug.Assert(r1.IsAlive && r2.IsAlive && r3.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        yield return new WaitForJob(p2);
        Debug.Assert(value == 1);
        yield return new WaitForJobManager();
        Debug.Assert(value == 3);
        yield return new WaitForJob(p3);
        Debug.Assert(value == 3);
        yield return new WaitForJob(s1);
        Debug.Assert(value == 3);
        Debug.Assert(s1.IsDisposed && p2.IsDisposed && p3.IsDisposed);

        // Evaluate GC
        s1 = null;
        p2 = null;
        p3 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r1.IsAlive && !r2.IsAlive && !r3.IsAlive);
    }

    [UnityTest]
    public IEnumerator _03_1()
    {
        // Demo main
        int value = 0;
        JobSequence s1 = JobSequence.Get();
        Job p2 = Job.Get(Function1(1, time * 3)); s1.Append(p2);
        Job p3 = Job.Get(Function1(3, time)); s1.Append(p3);
        s1.Append(Function1(2, time));
        
        // Declare local function
        IEnumerator Function1(int i, float time)
        {
            yield return new WaitForSeconds(time);
            value += i;
        }

        // Prepare GC check
        WeakReference r1 = new WeakReference(s1);
        WeakReference r2 = new WeakReference(p2);
        WeakReference r3 = new WeakReference(p3);
        Debug.Assert(r1.IsAlive && r2.IsAlive && r3.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        yield return new WaitForJob(p2);
        Debug.Assert(value == 1);
        yield return new WaitForJob(p3);
        Debug.Assert(value == 4);
        yield return new WaitForJobManager();
        Debug.Assert(value == 6);
        yield return new WaitForJob(s1);
        Debug.Assert(value == 6);
        Debug.Assert(s1.IsDisposed && p2.IsDisposed && p3.IsDisposed);

        // Evaluate GC
        s1 = null;
        p2 = null;
        p3 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r1.IsAlive && !r2.IsAlive && !r3.IsAlive);
    }

    [UnityTest]
    public IEnumerator _03_2()
    {
        // Demo main
        int value = 0;
        JobSequence s1 = JobSequence.Get();
        s1.Append(Job.Get(Function1, time * 3));
        Job p2 = Job.Get(Function1, -1); s1.Append(p2);
        s1.Append(Job.Get(Function1, -1));
        Job p3 = Job.Get(Function1, time);

        // Declare local function
        void Function1()
        {
            value += 1;
        }

        // Prepare GC check
        WeakReference r1 = new WeakReference(s1);
        WeakReference r2 = new WeakReference(p2);
        WeakReference r3 = new WeakReference(p3);
        Debug.Assert(r1.IsAlive && r2.IsAlive && r3.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        yield return new WaitForJob(p3);
        Debug.Assert(value == 1);
        yield return new WaitForJob(s1);
        Debug.Assert(value == 4);
        Debug.Assert(JobManager.Instance.IsFree);
        Debug.Assert(s1.IsDisposed && p2.IsDisposed && p3.IsDisposed);

        // Evaluate GC
        s1 = null;
        p2 = null;
        p3 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r1.IsAlive && !r2.IsAlive && !r3.IsAlive);
    }

    [UnityTest]
    public IEnumerator _04()
    {
        // Demo main
        float time = -1;
        int value = 0; // v_f = 2 + 5 * 2
        Job p1 = Job.Get(Add(5))
            .OnComplete(
                () => Job.Get(Mult(2))
                    .OnComplete(
                        () => Job.Get(Add(2))
                    )
            );

        // Declare local function
        IEnumerator Add(int number)
        {
            if (time >= 0) yield return new WaitForSeconds(time);
            value += number;
        }
        IEnumerator Mult(int number) 
        {
            if (time >= 0) yield return new WaitForSeconds(time);
            value *= number;
        }

        // Prepare GC check
        WeakReference r1 = new WeakReference(p1);
        Debug.Assert(r1.IsAlive);

        // Demo check
        yield return new WaitForJobManager();
        Debug.Assert(value == 12);
        Debug.Assert(JobManager.Instance.IsFree);
        Debug.Assert(p1.IsDisposed);

        // Evaluate GC
        p1 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r1.IsAlive);
    }

    [UnityTest]
    public IEnumerator _05()
    {
        // Demo main
        int value = 0;
        Job p2 = Job.Schedule(Add(1, .1f));
        Job p3 = Job.Schedule(Add(1, .1f));
        JobSequence s1 = JobSequence.Schedule();
        s1.Append(Add(3, -1));
        s1.Append(Mult(2, -1));
        s1.Append(Add(2, -1));
        Job p4 = Job.Schedule(Add(1000, .1f));

        // Declare local function
        IEnumerator Add(int number, float time)
        {
            if (time >= 0) yield return new WaitForSeconds(time);
            value += number;
        }
        IEnumerator Mult(int number, float time) 
        {
            if (time >= 0) yield return new WaitForSeconds(time);
            value *= number;
        }

        // Prepare GC check
        WeakReference r1 = new WeakReference(s1);
        WeakReference r2 = new WeakReference(p2);
        WeakReference r3 = new WeakReference(p3);
        WeakReference r4 = new WeakReference(p4);
        Debug.Assert(r1.IsAlive && r2.IsAlive && r3.IsAlive && r4.IsAlive);

        // Demo check
        yield return new WaitForJob(s1);
        Debug.Assert(value == 12);
        yield return new WaitForJobManager();
        Debug.Assert(value == 1012);
        Debug.Assert(JobManager.Instance.IsFree);
        Debug.Assert(s1.IsDisposed && p2.IsDisposed && p3.IsDisposed && p4.IsDisposed);

        // Evaluate GC
        s1 = null;
        p2 = null;
        p3 = null;
        p4 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r1.IsAlive && !r2.IsAlive && !r3.IsAlive && !r4.IsAlive);
    }

    [UnityTest]
    public IEnumerator _06()
    {
        yield return null;

        var p1 = Job.Get(null);
        var p2 = Job.Schedule(null);
        var s1 = JobSequence.Get();
        var s2 = JobSequence.Schedule();
    }

    #endregion
    /*——————————————————————————————————————————————————————————*/

    #endregion
    /*██████████████████████████████████████████████████████████*/
}
