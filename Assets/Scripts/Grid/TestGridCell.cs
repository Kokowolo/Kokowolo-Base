/*
 * File Name: TestGridCell.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: March 1, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kokowolo.Grid;

public class TestGridCell : GridCell
{
    /************************************************************/
    #region Fields

    // public static readonly GridCell Invalid = new GridCell(GridCoordinates.Invalid);

    // private List<Unit> unitQueue = new List<Unit>();

    #endregion
    /************************************************************/
    #region Properties

    // public List<GridCell> Neighbors { get; private set; } = new List<GridCell>();
    // public Unit Unit => unitQueue.Count > 0 ? unitQueue[0] : null;
    // public List<Unit> UnitQueue => unitQueue;

    #endregion
    /************************************************************/
    #region Functions

    public static implicit operator GridCoordinates(TestGridCell instance)
    {
        if (instance == null) return GridCoordinates.Invalid;
        
        return instance.Coordinates;
    }

    public TestGridCell(GridCoordinates coordinates) : base(coordinates)
    {
    }

    // public void AddUnitToQueue(Unit unit)
    // {
    //     foreach (GridCoordinates coordinates in unit.GridTransform.GetRelativeOverlappingCoordinatesList(Coordinates))
    //     {
    //         GridCell cell = GridMap.Instance.GetCell(coordinates);
    //         if (!cell.unitQueue.Contains(unit)) cell.UnitQueue.Add(unit);
    //     }
    // }

    // public void RemoveUnitFromQueue(Unit unit)
    // {
    //     foreach (GridCoordinates coordinates in unit.GridTransform.GetRelativeOverlappingCoordinatesList(Coordinates))
    //     {
    //         GridCell cell = GridMap.Instance.GetCell(coordinates);
    //         cell.UnitQueue.Remove(unit);
    //         // if (!cell.UnitQueue.Remove(unit)) Debug.LogError("Could not find Unit on GridCell");
    //     }
    // }

    #endregion
    /************************************************************/
    
}