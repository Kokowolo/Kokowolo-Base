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
        Debug.Assert(PlayModeConfig.Instance);
        Debug.Assert(JobManager.Instance);
        yield return null; // Wait for instance to set... for some reason this cant run in a normal Test
    }

    [UnityTest]
    public IEnumerator _01_0()
    {
        // Demo main
        int value = 0;
        Job p0 = Job.Get(Function1, PlayModeConfig.Time);
        Job p1 = Job.Get(Function1, PlayModeConfig.Time);
        Job p2 = Job.Get(Function1, PlayModeConfig.Time);

        // Declare local function
        void Function1()
        {
            value += 1;
        }

        // Prepare GC check
        WeakReference r0 = new WeakReference(p0);
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        Debug.Assert(r0.IsAlive && r1.IsAlive && r2.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        yield return new WaitForJobManager();
        Debug.Assert(value == 3);
        Debug.Assert(p0.IsDisposed && p1.IsDisposed && p2.IsDisposed);

        // Evaluate GC
        p0 = p1 = p2 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r0.IsAlive && !r1.IsAlive && !r2.IsAlive);
    }

    [UnityTest]
    public IEnumerator _01_1()
    {
        // Demo main
        int value = 0;
        int increment = 1;
        Job p0 = Job.Get(Function1(increment++));
        Job p1 = Job.Get(Function1(increment++));
        Job p2 = Job.Get(Function1(increment++));

        // Declare local function
        IEnumerator Function1(int i)
        {
            yield return new WaitForSeconds(PlayModeConfig.Time);
            value += i;
        }

        // Prepare GC check
        WeakReference r0 = new WeakReference(p0);
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        Debug.Assert(r0.IsAlive && r1.IsAlive && r2.IsAlive);

        // Demo check
        Debug.Assert(value == 0, $"v1:{value}");
        yield return new WaitForJobManager();
        Debug.Assert(value == 6, $"v1:{value}");
        Debug.Assert(p0.IsDisposed && p1.IsDisposed && p2.IsDisposed);

        // Evaluate GC
        p0 = p1 = p2 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r0.IsAlive && !r1.IsAlive && !r2.IsAlive);
    }

    [UnityTest]
    public IEnumerator _02_0()
    {
        // Demo main
        int value = 0;
        Job p0 = Job.Schedule(Function1, PlayModeConfig.Time);
        Job p1 = Job.Schedule(Function1, PlayModeConfig.Time);
        Job p2 = Job.Schedule(Function1, PlayModeConfig.Time);

        // Declare local function
        void Function1()
        {
            value += 1;
        }

        // Prepare GC check
        WeakReference r0 = new WeakReference(p0);
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        Debug.Assert(r0.IsAlive && r1.IsAlive && r2.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        p1.OnComplete(() => Debug.Assert(value == 2));
        yield return new WaitForJobManager();
        Debug.Assert(value == 3);
        Debug.Assert(p0.IsDisposed && p1.IsDisposed && p2.IsDisposed);

        // Evaluate GC
        p0 = p1 = p2 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r0.IsAlive && !r1.IsAlive && !r2.IsAlive);
    }

    [UnityTest]
    public IEnumerator _02_1()
    {
        // Demo main
        int value = 0;
        int increment = 1;
        Job p0 = Job.Schedule(Function1(increment++));
        Job p1 = Job.Schedule(Function1(increment++));
        Job p2 = Job.Schedule(Function1(increment++));

        // Declare local function
        IEnumerator Function1(int i)
        {
            yield return new WaitForSeconds(PlayModeConfig.Time);
            value += i;
        }

        // Prepare GC check
        WeakReference r0 = new WeakReference(p0);
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        Debug.Assert(r0.IsAlive && r1.IsAlive && r2.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        yield return new WaitForJob(p1);
        Debug.Assert(value == 3, $"value:{value}");
        yield return new WaitForJobManager();
        Debug.Assert(value == 6, $"value:{value}");
        Debug.Assert(p0.IsDisposed && p1.IsDisposed && p2.IsDisposed);

        // Evaluate GC
        p0 = p1 = p2 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r0.IsAlive && !r1.IsAlive && !r2.IsAlive);
    }

    [UnityTest]
    public IEnumerator _03_0()
    {
        // Demo main
        int value = 0;
        JobSequence s0 = JobSequence.Get();
        Job p1 = Job.Get(Function1, PlayModeConfig.Time); s0.Append(p1);
        s0.Append(Function1, PlayModeConfig.Time);
        Job p2 = Job.Get(Function1, PlayModeConfig.Time); s0.Append(p2);

        // Declare local function
        void Function1()
        {
            value += 1;
        }

        // Prepare GC check
        WeakReference r0 = new WeakReference(s0);
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        Debug.Assert(r0.IsAlive && r1.IsAlive && r2.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        p1.OnComplete(() => Debug.Assert(value == 1));
        yield return new WaitForJobManager();
        Debug.Assert(value == 3);
        yield return new WaitForJob(p2);
        Debug.Assert(value == 3);
        yield return new WaitForJob(s0);
        Debug.Assert(value == 3);
        Debug.Assert(s0.IsDisposed && p1.IsDisposed && p2.IsDisposed);

        // Evaluate GC
        s0 = null;
        p1 = null;
        p2 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r0.IsAlive && !r1.IsAlive && !r2.IsAlive);
    }

    [UnityTest]
    public IEnumerator _03_1()
    {
        // Demo main
        int value = 0;
        JobSequence s0 = JobSequence.Get();
        Job p1 = Job.Get(Function1(1, PlayModeConfig.Time * 3)); s0.Append(p1);
        Job p2 = Job.Get(Function1(3, PlayModeConfig.Time)); s0.Append(p2);
        s0.Append(Function1(2, PlayModeConfig.Time));
        
        // Declare local function
        IEnumerator Function1(int i, float time)
        {
            yield return new WaitForSeconds(time);
            value += i;
        }

        // Prepare GC check
        WeakReference r0 = new WeakReference(s0);
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        Debug.Assert(r0.IsAlive && r1.IsAlive && r2.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        p1.OnComplete(() => Debug.Assert(value == 1));
        p2.OnComplete(() => Debug.Assert(value == 4));
        yield return new WaitForJobManager();
        Debug.Assert(value == 6);
        yield return new WaitForJob(s0);
        Debug.Assert(value == 6);
        Debug.Assert(s0.IsDisposed && p1.IsDisposed && p2.IsDisposed);

        // Evaluate GC
        s0 = null;
        p1 = null;
        p2 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r0.IsAlive && !r1.IsAlive && !r2.IsAlive);
    }

    [UnityTest]
    public IEnumerator _03_2()
    {
        // Demo main
        int value = 0;
        JobSequence s0 = JobSequence.Get();
        s0.Append(Job.Get(Function1, .3f));
        Job p1 = Job.Get(Function1, -1); s0.Append(p1);
        s0.Append(Job.Get(Function1, -1));
        Job p2 = Job.Get(Function1, -1f);

        // Declare local function
        void Function1()
        {
            value += 1;
        }

        // Prepare GC check
        WeakReference r0 = new WeakReference(s0);
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        Debug.Assert(r0.IsAlive && r1.IsAlive && r2.IsAlive);

        // Demo check
        Debug.Assert(value == 0);
        p2.OnComplete(() => Debug.Assert(value == 1));
        yield return new WaitForJob(s0);
        Debug.Assert(value == 4);
        Debug.Assert(JobManager.Instance.IsFree);
        Debug.Assert(s0.IsDisposed && p1.IsDisposed && p2.IsDisposed);

        // Evaluate GC
        s0 = null;
        p1 = null;
        p2 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r0.IsAlive && !r1.IsAlive && !r2.IsAlive);
    }

    [UnityTest]
    public IEnumerator _04()
    {
        // Demo main
        float time = -1;
        int value = 0; // v_f = 2 + 5 * 2
        Job p0 = Job.Get(Add(5))
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
        WeakReference r0 = new WeakReference(p0);
        Debug.Assert(r0.IsAlive);

        // Demo check
        yield return new WaitForJobManager();
        Debug.Assert(value == 12);
        Debug.Assert(JobManager.Instance.IsFree);
        Debug.Assert(p0.IsDisposed);

        // Evaluate GC
        p0 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r0.IsAlive);
    }

    [UnityTest]
    public IEnumerator _05()
    {
        // Demo main
        int value = 0;
        Job p1 = Job.Schedule(Add(1, .1f));
        Job p2 = Job.Schedule(Add(1, .1f));
        JobSequence s0 = JobSequence.Schedule();
        s0.Append(Add(3, -1));
        s0.Append(Mult(2, -1));
        s0.Append(Add(2, -1));
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
        WeakReference r0 = new WeakReference(s0);
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        WeakReference r4 = new WeakReference(p4);
        Debug.Assert(r0.IsAlive && r1.IsAlive && r2.IsAlive && r4.IsAlive);

        // Demo check
        yield return new WaitForJob(s0);
        Debug.Assert(value == 12);
        yield return new WaitForJobManager();
        Debug.Assert(value == 1012);
        Debug.Assert(JobManager.Instance.IsFree);
        Debug.Assert(s0.IsDisposed && p1.IsDisposed && p2.IsDisposed && p4.IsDisposed);

        // Evaluate GC
        s0 = null;
        p1 = null;
        p2 = null;
        p4 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r0.IsAlive && !r1.IsAlive && !r2.IsAlive && !r4.IsAlive);
    }

    [UnityTest]
    public IEnumerator _06()
    {
        // Demo main
        int value = 0;
        JobSequence s0 = JobSequence.Schedule();
        s0.Append(() => Add(1));
        var p1 = Job.Get(() => Add(2)); s0.Append(p1);
        var p2 = Job.Get(() => Add(3)); s0.Append(p2);
        s0.Append(() => Add(9));

        p1.OnComplete(() => p2.Dispose(false));

        // Declare local function
        void Add(int number) => value += number;

        // Prepare GC check
        WeakReference r0 = new WeakReference(s0);
        WeakReference r1 = new WeakReference(p1);
        WeakReference r2 = new WeakReference(p2);
        Debug.Assert(r0.IsAlive && r1.IsAlive && r2.IsAlive);

        // Demo check
        yield return new WaitForJobManager();
        Debug.Assert(value == 12, $"value:{value}");
        Debug.Assert(s0.IsDisposed && p1.IsDisposed && p2.IsDisposed);

        // Evaluate GC
        s0 = null;
        p1 = null;
        p2 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r0.IsAlive && !r1.IsAlive && !r2.IsAlive);
    }

    [UnityTest]
    public IEnumerator _07()
    {
        // Demo main
        int value = 0;
        JobSequence s0 = JobSequence.Schedule();
        var p1 = Job.Get(() => Add(1)); s0.Append(p1);
        s0.Append(() => Add(2));

        Job p3 = null;
        WeakReference r3 = null;
        p1.OnStart(() =>
        {
            Debug.Assert(value == 0);
            p3 = Job.Get(() => Add(100)); 
            r3 = new WeakReference(p3);
            Debug.Assert(r3.IsAlive);
            s0.Append(p3);
            p3.Dispose();

            p3.OnStart(() => Debug.Assert(false));
        });

        // Declare local function
        void Add(int number) => value += number;

        // Prepare GC check
        WeakReference r0 = new WeakReference(s0);
        WeakReference r1 = new WeakReference(p1);
        Debug.Assert(r0.IsAlive && r1.IsAlive);

        // Demo check
        yield return new WaitForJobManager();
        Debug.Assert(value == 3, $"value:{value}");
        Debug.Assert(s0.IsDisposed && p1.IsDisposed && p3.IsDisposed);

        // Evaluate GC
        s0 = null;
        p1 = null;
        p3 = null;
        yield return null;
        GC.Collect();
        Debug.Assert(!r0.IsAlive && !r1.IsAlive && !r3.IsAlive);
    }

    #endregion
    /*——————————————————————————————————————————————————————————*/

    #endregion
    /*██████████████████████████████████████████████████████████*/
}
