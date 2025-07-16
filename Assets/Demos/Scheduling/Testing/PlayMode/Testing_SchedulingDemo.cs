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

public class Testing_SchedulingDemo
{
    /*██████████████████████████████████████████████████████████*/
    #region Fields

    public const string TestingScenePathName = "Assets/Demos/Scheduling/Testing/PlayMode/Testing_SchedulingManager.unity";

    #endregion
    /*██████████████████████████████████████████████████████████*/
    #region Functions
    /*——————————————————————————————————————————————————————————*/
    #region SetUp & TearDown

    [OneTimeSetUp] 
    public virtual void OneTimeSetUp()
    {
        var scene1 = UnityEngine.SceneManagement.SceneManager.GetSceneByPath(TestingScenePathName);

        // scenes might be loaded, but destroy all GameObjects might have been called
        if (scene1.isLoaded) return;

        EditorSceneManager.LoadSceneInPlayMode(TestingScenePathName, new LoadSceneParameters(LoadSceneMode.Single));
    }

    #endregion
    /*——————————————————————————————————————————————————————————*/
    #region Tests

    [UnityTest]
    public IEnumerator Testing_00()
    {
        Debug.Assert(JobManager.Instance);
        yield return null; // Wait for instance to set... for some reason this cant run in a normal Test
    }

    [UnityTest]
    public IEnumerator Testing_01_0()
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
    public IEnumerator Testing_01_1()
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
    public IEnumerator Testing_02_0()
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
    public IEnumerator Testing_02_1()
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

    // [UnityTest]
    // public IEnumerator Testing_0_SchedulingManager_02()
    // {
    //     float time = 0.1f;
    //     int value = 0;
    //     ScheduledEventManager.StartEvent(Function1, time);
    //     ScheduledEventManager.StartEvent(Function2, time * 3);
    //     ScheduledEventManager.StartEvent(Function3, time);
    //     yield return ScheduledEventManager.WaitWhileIsRunning();

    //     Debug.Assert(value == 16);

    //     // helper functions
    //     void Function1() => value += 4;
    //     void Function2() => value *= 2;
    //     void Function3() => value += 4;
    // }

    // [UnityTest]
    // public IEnumerator Testing_0_SchedulingManager_03()
    // {
    //     float time = 0.1f;
    //     int value = 0;
    //     ScheduledEventManager.ScheduleEvent(Function1, time);
    //     ScheduledEventManager.ScheduleEvent(Function2, time * 3);
    //     ScheduledEventManager.ScheduleEvent(Function1, time);
    //     yield return ScheduledEventManager.WaitWhileIsRunning();

    //     Debug.Assert(value == 12);

    //     // helper functions
    //     void Function1() => value += 4;
    //     void Function2() => value *= 2;
    // }

    // [UnityTest]
    // public IEnumerator Testing_0_SchedulingManager_04()
    // {
    //     float time = 0.2f;
    //     int value = 0;
    //     ScheduledEventManager.ScheduleEvent(Function2, time);
    //     ScheduledEventManager.ScheduleEvent(Function2, time);
    //     ScheduledEventManager.ScheduleEvent(Function2, time);
    //     ScheduledEventManager.StartEvent(Function1, time * 1.5f);
    //     ScheduledEventManager.StartEvent(Function1, time * 1.5f);
    //     ScheduledEventManager.StartEvent(Function1, time * 1.5f);
    //     yield return ScheduledEventManager.WaitWhileIsRunning();

    //     Debug.Assert(value == 48);

    //     // helper functions
    //     void Function1() => value += 4;
    //     void Function2() => value *= 2;
    // }

    // [UnityTest]
    // public IEnumerator Testing_0_SchedulingManager_05()
    // {
    //     float time = 0.1f;
    //     int value = 0;
    //     ScheduledEvent scheduledEvent = ScheduledEventManager.ScheduleEvent(Function1, time * 100);
    //     ScheduledEventManager.ScheduleEvent(Function1, time);
    //     ScheduledEventManager.StopEvent(scheduledEvent);
    //     yield return ScheduledEventManager.WaitWhileIsRunning();

    //     Debug.Assert(value == 4);

    //     // helper functions
    //     void Function1() => value += 4;
    // }

    // [UnityTest]
    // public IEnumerator Testing_0_SchedulingManager_06()
    // {
    //     float time = 0.1f;
    //     int value = 0;
    //     ScheduledEvent scheduledEvent = ScheduledEventManager.ScheduleEvent(Function1, time * 100);
    //     ScheduledEventManager.ScheduleEvent(Function1, time);
    //     ScheduledEventManager.ScheduleEvent(Function1, time);
    //     ScheduledEventManager.StopEvent(scheduledEvent);
    //     yield return null;
    //     yield return ScheduledEventManager.WaitWhileIsRunning();

    //     Debug.Assert(value == 8);

    //     // helper functions
    //     void Function1() => value += 4;
    // }

    // [UnityTest]
    // public IEnumerator Testing_0_SchedulingManager_07()
    // {
    //     float time = 0.1f;
    //     int value = 0;
    //     ScheduledEvent scheduledEvent = ScheduledEventManager.StartEvent(Function1, time * 100);
    //     ScheduledEventManager.StartEvent(Function1, time);
    //     ScheduledEventManager.StartEvent(Function1, time);
    //     ScheduledEventManager.StopEvent(scheduledEvent);
    //     yield return ScheduledEventManager.WaitWhileIsRunning();

    //     Debug.Assert(value == 8);

    //     // helper functions
    //     void Function1() => value += 4;
    // }

    // [UnityTest]
    // public IEnumerator Testing_0_SchedulingManager_08()
    // {
    //     float time = 0.1f;
    //     int value = 0;
    //     ScheduledEventManager.ScheduleEvent(Function1, time);
    //     ScheduledEventManager.ScheduleEvent(Function1, time);
    //     ScheduledEvent scheduledEvent = ScheduledEventManager.ScheduleEvent(Function2, time);
    //     ScheduledEventManager.StopEvent(scheduledEvent);
    //     yield return ScheduledEventManager.WaitWhileIsRunning();

    //     Debug.Assert(value == 8);

    //     // helper functions
    //     void Function1() => value += 4;
    //     void Function2() => value += 100;
    // }

    // [UnityTest]
    // public IEnumerator Testing_0_SchedulingManager_09()
    // {
    //     float time =0.1f;
    //     int value = 0;
    //     ScheduledEventManager.StartEvent(Function1, time);
    //     ScheduledEventManager.StartEvent(Function1, time);
    //     ScheduledEventManager.StartEvent(Function1, time);
    //     ScheduledEventManager.StartEvent(Function1, time);
    //     ScheduledEventManager.StartEvent(Function1, time);
    //     ScheduledEvent scheduledEvent = ScheduledEventManager.StartEvent(Function2, time);
    //     ScheduledEventManager.StopEvent(scheduledEvent);
    //     yield return ScheduledEventManager.WaitWhileIsRunning();

    //     Debug.Assert(value == 20);
    //     LogManager.Log($"{value}");

    //     // helper functions
    //     void Function1() => value += 4;
    //     void Function2() => value += 100;
    // }

    // [UnityTest]
    // public IEnumerator Testing_0_SchedulingManager_10()
    // {
    //     float time = 0.1f;
    //     int value = 0;
    //     ScheduledEventManager.ScheduleEvent(Function1, time);
    //     ScheduledEventManager.ScheduleEvent(Function2, time);
    //     ScheduledEventManager.ScheduleEvent(Function1, time);
    //     ScheduledEvent scheduledEvent = ScheduledEventManager.ScheduleEvent(Function2, time);
    //     ScheduledEventManager.StopEvent(scheduledEvent);
    //     yield return ScheduledEventManager.WaitWhileIsRunning();

    //     Debug.Assert(value == 12);

    //     // helper functions
    //     void Function1() => value += 4;
    //     void Function2() => value *= 2;
    // }

    #endregion
    /*——————————————————————————————————————————————————————————*/

    #endregion
    /*██████████████████████████████████████████████████████████*/
}
