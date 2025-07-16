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

public class ScheduledEventManagerFunctionality
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
    public IEnumerator SchedulingManager_00()
    {
        float time = 0.1f;
        int value = 0;
        JobManager.StartJob(Function1, time);
        JobManager.StartJob(Function1, time);
        JobManager.StartJob(Function1, time);
        yield return new WaitForJobManager();

        Debug.Assert(value == 3);

        // helper functions
        void Function1() => value += 1;
    }

    [UnityTest]
    public IEnumerator SchedulingManager_01()
    {
        float time = 0.1f;
        int value = 0;
        JobManager.ScheduleJob(Function1, time);
        JobManager.ScheduleJob(Function1, time);
        JobManager.ScheduleJob(Function1, time);
        yield return new WaitForJobManager();

        Debug.Assert(value == 3);

        // helper functions
        void Function1() => value += 1;
    }

    [UnityTest]
    public IEnumerator SchedulingManager_02()
    {
        float time = 0.1f;
        int value = 0;
        JobManager.StartJob(Function1, time);
        JobManager.StartJob(Function2, time * 3);
        JobManager.StartJob(Function3, time);
        yield return new WaitForJobManager();

        Debug.Assert(value == 16);

        // helper functions
        void Function1() => value += 4;
        void Function2() => value *= 2;
        void Function3() => value += 4;
    }

    [UnityTest]
    public IEnumerator SchedulingManager_03()
    {
        float time = 0.1f;
        int value = 0;
        JobManager.ScheduleJob(Function1, time);
        JobManager.ScheduleJob(Function2, time * 3);
        JobManager.ScheduleJob(Function1, time);
        yield return new WaitForJobManager();

        Debug.Assert(value == 12);

        // helper functions
        void Function1() => value += 4;
        void Function2() => value *= 2;
    }

    [UnityTest]
    public IEnumerator SchedulingManager_04()
    {
        float time = 0.2f;
        int value = 0;
        JobManager.ScheduleJob(Function2, time);
        JobManager.ScheduleJob(Function2, time);
        JobManager.ScheduleJob(Function2, time);
        JobManager.StartJob(Function1, time * 1.5f);
        JobManager.StartJob(Function1, time * 1.5f);
        JobManager.StartJob(Function1, time * 1.5f);
        yield return new WaitForJobManager();

        Debug.Assert(value == 48);

        // helper functions
        void Function1() => value += 4;
        void Function2() => value *= 2;
    }

    [UnityTest]
    public IEnumerator SchedulingManager_05()
    {
        float time = 0.1f;
        int value = 0;
        Job p = JobManager.ScheduleJob(Function1, time * 100);
        JobManager.ScheduleJob(Function1, time);
        p.Dispose();
        yield return new WaitForJobManager();

        Debug.Assert(value == 4);

        // helper functions
        void Function1() => value += 4;
    }

    [UnityTest]
    public IEnumerator SchedulingManager_06()
    {
        float time = 0.1f;
        int value = 0;
        Job p = JobManager.ScheduleJob(Function1, time * 100);
        JobManager.ScheduleJob(Function1, time);
        JobManager.ScheduleJob(Function1, time);
        p.Dispose();
        yield return null;
        yield return new WaitForJobManager();

        Debug.Assert(value == 8);

        // helper functions
        void Function1() => value += 4;
    }

    [UnityTest]
    public IEnumerator SchedulingManager_07()
    {
        float time = 0.1f;
        int value = 0;
        Job p = JobManager.StartJob(Function1, time * 100);
        JobManager.StartJob(Function1, time);
        JobManager.StartJob(Function1, time);
        p.Dispose();
        yield return new WaitForJobManager();

        Debug.Assert(value == 8);

        // helper functions
        void Function1() => value += 4;
    }

    [UnityTest]
    public IEnumerator SchedulingManager_08()
    {
        float time = 0.1f;
        int value = 0;
        JobManager.ScheduleJob(Function1, time);
        JobManager.ScheduleJob(Function1, time);
        Job p = JobManager.ScheduleJob(Function2, time);
        p.Dispose();
        yield return new WaitForJobManager();

        Debug.Assert(value == 8);

        // helper functions
        void Function1() => value += 4;
        void Function2() => value += 100;
    }

    [UnityTest]
    public IEnumerator SchedulingManager_09()
    {
        float time =0.1f;
        int value = 0;
        JobManager.StartJob(Function1, time);
        JobManager.StartJob(Function1, time);
        JobManager.StartJob(Function1, time);
        JobManager.StartJob(Function1, time);
        JobManager.StartJob(Function1, time);
        Job p = JobManager.StartJob(Function2, time);
        p.Dispose();
        yield return new WaitForJobManager();

        Debug.Assert(value == 20);

        // helper functions
        void Function1() => value += 4;
        void Function2() => value += 100;
    }

    [UnityTest]
    public IEnumerator SchedulingManager_10()
    {
        float time = 0.1f;
        int value = 0;
        JobManager.ScheduleJob(Function1, time);
        JobManager.ScheduleJob(Function2, time);
        JobManager.ScheduleJob(Function1, time);
        Job job = JobManager.ScheduleJob(Function2, time);
        job.Dispose();
        yield return new WaitForJobManager();

        Debug.Assert(value == 12);

        // helper functions
        void Function1() => value += 4;
        void Function2() => value *= 2;
    }

    #endregion
    /*——————————————————————————————————————————————————————————*/

    #endregion
    /*██████████████████████████████████████████████████████████*/
}
