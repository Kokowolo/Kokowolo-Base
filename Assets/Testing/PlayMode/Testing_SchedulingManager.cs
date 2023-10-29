using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

using Kokowolo.Utilities;

public class Testing_SchedulingManager
{
    /************************************************************/
    #region Fields

    public const string TestingScenePathName = "Assets/Testing/PlayMode/Testing_Scene_SchedulingManager.unity";

    #endregion
    /************************************************************/
    #region Functions

    // public static void DestroyImmediateUnityObjectsOfType<T>()/*bool destroyDontDestroyOnLoadUnityObjects = true)*/ where T : Object
    // {
    //     T[] objects = Object.FindObjectsOfType<T>();
    //     for (int i = objects.Length - 1; i >= 0; i--)
    //     {
    //         Component component = objects[i] as Component;
    //         if (component)
    //         {
    //             // if (destroyDontDestroyOnLoadUnityObjects && component.gameObject.scene.name == "DontDestroyOnLoad") continue;
    //             Object.DestroyImmediate(component.gameObject);
    //         }
    //         else
    //         {
    //             // if (destroyDontDestroyOnLoadUnityObjects && (objects[i] as GameObject).scene.name == "DontDestroyOnLoad") continue;
    //             Object.DestroyImmediate(objects[i]);
    //         }
    //     }
    // }

    #endregion
    /************************************************************/
    #region SetUp & TearDown

    [OneTimeSetUp] 
    public virtual void OneTimeSetUp()
    {
        var scene1 = UnityEngine.SceneManagement.SceneManager.GetSceneByPath(TestingScenePathName);

        // scenes might be loaded, but destroy all GameObjects might have been called
        if (scene1.isLoaded) return;

        EditorSceneManager.LoadSceneInPlayMode(TestingScenePathName, new LoadSceneParameters(LoadSceneMode.Single));
    }

    // [OneTimeTearDown]
    // public virtual void OneTimeTearDown()
    // {
    //     if (PlayModeTestsManager.Instance.destroyAllOnOneTimeTearDown)
    //     {
    //         PlayModeGlobalUtilities.DestroyImmediateUnityObjectsOfType<GameObject>();
    //     }
    // }

    // [UnitySetUp]
    // public virtual IEnumerator SetUp()
    // {
    //     yield return null; // wait for scene to load; then set up

    //     // set defaults for grid
    //     int y = PlayModeTestsManager.Instance.cellCountY;
    //     int z = PlayModeTestsManager.Instance.cellCountZ;
    //     int x = PlayModeTestsManager.Instance.cellCountX;
    //     Kokowolo.Grid.GridManager.Map.Create(y, z, x);

    //     // set defaults for tests
    //     UnitCombatSystem.RoundStepsPerRound = 4;
    //     PlayModeCombatUtilities.CanPlayerControllersCanSelectUnits(false);
    //     CombatGameManager.CanExitCombat = false;
    //     UnitCombatSystem.CanSimulate = false;
    // }

    // [TearDown] 
    // public virtual void TearDown() 
    // {
    //     PlayModeGlobalUtilities.DestroyImmediateUnityObjectsOfType<Unit>();
    //     // PlayModeGlobalUtilities.DestroyImmediateUnityObjectsOfType<UnitRagdoll>();

    //     // ensure combat is exited
    //     CombatGameManager.CanExitCombat = true;
    //     CombatGameManager.Instance.ExitCombat();
    // }

    #endregion
    /************************************************************/
    #region Tests

    [UnityTest]
    public IEnumerator Testing_0_SchedulingManager_00()
    {
        float time = 0.1f;
        int value = 0;
        ScheduledEventManager.StartEvent(Function1, time);
        ScheduledEventManager.StartEvent(Function1, time);
        ScheduledEventManager.StartEvent(Function1, time);
        yield return ScheduledEventManager.WaitWhileIsRunning();

        Debug.Assert(value == 3);

        // helper functions
        void Function1() => value += 1;
    }

    [UnityTest]
    public IEnumerator Testing_0_SchedulingManager_01()
    {
        float time = 0.1f;
        int value = 0;
        ScheduledEventManager.ScheduleEvent(Function1, time);
        ScheduledEventManager.ScheduleEvent(Function1, time);
        ScheduledEventManager.ScheduleEvent(Function1, time);
        yield return ScheduledEventManager.WaitWhileIsRunning();

        Debug.Assert(value == 3);

        // helper functions
        void Function1() => value += 1;
    }

    [UnityTest]
    public IEnumerator Testing_0_SchedulingManager_02()
    {
        float time = 0.1f;
        int value = 0;
        ScheduledEventManager.StartEvent(Function1, time);
        ScheduledEventManager.StartEvent(Function2, time * 3);
        ScheduledEventManager.StartEvent(Function3, time);
        yield return ScheduledEventManager.WaitWhileIsRunning();

        Debug.Assert(value == 16);

        // helper functions
        void Function1() => value += 4;
        void Function2() => value *= 2;
        void Function3() => value += 4;
    }

    [UnityTest]
    public IEnumerator Testing_0_SchedulingManager_03()
    {
        float time = 0.1f;
        int value = 0;
        ScheduledEventManager.ScheduleEvent(Function1, time);
        ScheduledEventManager.ScheduleEvent(Function2, time * 3);
        ScheduledEventManager.ScheduleEvent(Function1, time);
        yield return ScheduledEventManager.WaitWhileIsRunning();

        Debug.Assert(value == 12);

        // helper functions
        void Function1() => value += 4;
        void Function2() => value *= 2;
    }

    [UnityTest]
    public IEnumerator Testing_0_SchedulingManager_04()
    {
        float time = 0.2f;
        int value = 0;
        ScheduledEventManager.ScheduleEvent(Function2, time);
        ScheduledEventManager.ScheduleEvent(Function2, time);
        ScheduledEventManager.ScheduleEvent(Function2, time);
        ScheduledEventManager.StartEvent(Function1, time * 1.5f);
        ScheduledEventManager.StartEvent(Function1, time * 1.5f);
        ScheduledEventManager.StartEvent(Function1, time * 1.5f);
        yield return ScheduledEventManager.WaitWhileIsRunning();

        Debug.Assert(value == 48);

        // helper functions
        void Function1() => value += 4;
        void Function2() => value *= 2;
    }

    [UnityTest]
    public IEnumerator Testing_0_SchedulingManager_05()
    {
        float time = 0.1f;
        int value = 0;
        ScheduledEvent scheduledEvent = ScheduledEventManager.ScheduleEvent(Function1, time * 100);
        ScheduledEventManager.ScheduleEvent(Function1, time);
        ScheduledEventManager.StopEvent(scheduledEvent);
        yield return ScheduledEventManager.WaitWhileIsRunning();

        Debug.Assert(value == 4);

        // helper functions
        void Function1() => value += 4;
    }

    [UnityTest]
    public IEnumerator Testing_0_SchedulingManager_06()
    {
        float time = 0.1f;
        int value = 0;
        ScheduledEvent scheduledEvent = ScheduledEventManager.ScheduleEvent(Function1, time * 100);
        ScheduledEventManager.ScheduleEvent(Function1, time);
        ScheduledEventManager.ScheduleEvent(Function1, time);
        ScheduledEventManager.StopEvent(scheduledEvent);
        yield return null;
        yield return ScheduledEventManager.WaitWhileIsRunning();

        Debug.Assert(value == 8);

        // helper functions
        void Function1() => value += 4;
    }

    [UnityTest]
    public IEnumerator Testing_0_SchedulingManager_07()
    {
        float time = 0.1f;
        int value = 0;
        ScheduledEvent scheduledEvent = ScheduledEventManager.StartEvent(Function1, time * 100);
        ScheduledEventManager.StartEvent(Function1, time);
        ScheduledEventManager.StartEvent(Function1, time);
        ScheduledEventManager.StopEvent(scheduledEvent);
        yield return ScheduledEventManager.WaitWhileIsRunning();

        Debug.Assert(value == 8);

        // helper functions
        void Function1() => value += 4;
    }

    [UnityTest]
    public IEnumerator Testing_0_SchedulingManager_08()
    {
        float time = 0.1f;
        int value = 0;
        ScheduledEventManager.ScheduleEvent(Function1, time);
        ScheduledEventManager.ScheduleEvent(Function1, time);
        ScheduledEvent scheduledEvent = ScheduledEventManager.ScheduleEvent(Function2, time);
        ScheduledEventManager.StopEvent(scheduledEvent);
        yield return ScheduledEventManager.WaitWhileIsRunning();

        Debug.Assert(value == 8);

        // helper functions
        void Function1() => value += 4;
        void Function2() => value += 100;
    }

    [UnityTest]
    public IEnumerator Testing_0_SchedulingManager_09()
    {
        float time =0.1f;
        int value = 0;
        ScheduledEventManager.StartEvent(Function1, time);
        ScheduledEventManager.StartEvent(Function1, time);
        ScheduledEventManager.StartEvent(Function1, time);
        ScheduledEventManager.StartEvent(Function1, time);
        ScheduledEventManager.StartEvent(Function1, time);
        ScheduledEvent scheduledEvent = ScheduledEventManager.StartEvent(Function2, time);
        ScheduledEventManager.StopEvent(scheduledEvent);
        yield return ScheduledEventManager.WaitWhileIsRunning();

        Debug.Assert(value == 20);
        LogManager.Log($"{value}");

        // helper functions
        void Function1() => value += 4;
        void Function2() => value += 100;
    }

    [UnityTest]
    public IEnumerator Testing_0_SchedulingManager_10()
    {
        float time = 0.1f;
        int value = 0;
        ScheduledEventManager.ScheduleEvent(Function1, time);
        ScheduledEventManager.ScheduleEvent(Function2, time);
        ScheduledEventManager.ScheduleEvent(Function1, time);
        ScheduledEvent scheduledEvent = ScheduledEventManager.ScheduleEvent(Function2, time);
        ScheduledEventManager.StopEvent(scheduledEvent);
        yield return ScheduledEventManager.WaitWhileIsRunning();

        Debug.Assert(value == 12);

        // helper functions
        void Function1() => value += 4;
        void Function2() => value *= 2;
    }

    #endregion
    /************************************************************/
}
