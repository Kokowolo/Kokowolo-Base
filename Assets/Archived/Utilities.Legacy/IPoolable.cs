/*
 * Copyright (c) 2026 Kokowolo. All Rights Reserved.
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 9, 2023
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;
namespace Kokowolo.Utilities.Legacy
{
    public interface IPoolable
    {
        /*██████████████████████████████████████████████████████████*/
        #region Functions
        
        public IPoolable GenerateInstanceFrom(params object[] args);
        
        public void Release();
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
    
    public interface IPoolable<T> : IPoolable where T : IPoolable<T>
    {
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        IPoolable IPoolable.GenerateInstanceFrom(params object[] args)
        {
            return PoolSystem.Get<T>(args);
        }
        
        void IPoolable.Release()
        {
            PoolSystem.Add((T) this);
        }

        public abstract void OnAddPoolable();

        public abstract void OnGetPoolable(params object[] args);

        // NOTE: PoolSystem will call Create() if there are no IPoolable<T> in the stack
        // public abstract static T Create(params object[] args);
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}