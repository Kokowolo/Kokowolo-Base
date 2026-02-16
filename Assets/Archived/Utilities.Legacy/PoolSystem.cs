/*
 * Copyright (c) 2026 Kokowolo. All Rights Reserved.
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 11, 2023
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;
namespace Kokowolo.Utilities.Legacy
{
    public class PoolSystem : MonoBehaviourSingleton<PoolSystem>
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        static object[] parameters = new object[1];

        #endregion 
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public static bool HasIPoolable<T>() where T : IPoolable<T>
        {
            return GetCount<T>() != 0;
        }
        
        public static int GetCount<T>() where T : IPoolable<T>
        {
            return PoolSystemStack<T>.stack.Count;
        }

        public static void Add<T>(T poolable) where T : IPoolable<T>
        {
            poolable.OnAddPoolable();
            PoolSystemStack<T>.stack.Push(poolable);
        }

        public static T Get<T>(params object[] args) where T : IPoolable<T>
        {
            T poolable;
            if (GetCount<T>() == 0) 
            {
                poolable = CreateIPoolable<T>(args);
            }
            else 
            {
                poolable = PoolSystemStack<T>.stack.Pop();
                poolable.OnGetPoolable(args);
                
            }
            return poolable;
        }

        static T CreateIPoolable<T>(params object[] args) where T : IPoolable<T>
        {
            // NOTE: This is a hacky way to get around the fact that you can't call a static method on a generic type
            MethodInfo createMethod = typeof(T).GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
            if (createMethod == null || createMethod.ReturnType != typeof(T))
            {
                throw new System.Exception($"no static Create method found within type {nameof(T)}");
            }

            // NOTE: args requires a object array wrapper
            parameters[0] = args;
            return (T) createMethod.Invoke(null, parameters);
        }

        static void Clear()
        {
            // TODO: implement this function
        }
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Subclasses

        static class PoolSystemStack<T>
        {
            public static Stack<T> stack = new Stack<T>();
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}