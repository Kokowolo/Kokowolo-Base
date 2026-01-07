/*
 * Copyright (c) 2025 Kokowolo. All Rights Reserved. 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 14, 2024
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Kokowolo.Utilities
{
    [Serializable]
    public struct Bitmask
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        [SerializeField] int mask;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public int Value
        {
            get => mask;
            set => mask = value;
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions
        
        public Bitmask(int mask)
        {
            this.mask = mask;
        }

        public static implicit operator int(Bitmask mask)
        {
            return mask;
        }

        public static implicit operator Bitmask(int value)
        {
            return new Bitmask(value);
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}