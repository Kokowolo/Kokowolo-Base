/*
 * File Name: TestGridPathfinding.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: February 26, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Grid;
using Kokowolo.Pathfinding;

public class TestGridPathfinding : IPathfinding
{
    /************************************************************/
    #region Fields

    #endregion
	/************************************************************/
    #region Properties

    public static TestGridPathfinding _instance;
    public static TestGridPathfinding Instance     
    {
        get 
        {
            if (_instance == null) 
            {
                _instance = new TestGridPathfinding();
            }
            return _instance;
        }
    }

    public bool CanCreatePathsWithRepeatNodes { get; private set; } = false;

    #endregion
    /************************************************************/
    #region Functions

    List<INode> IPathfinding.GetNeighborsFromNode(INode iNode)
    {
        return iNode.Node.GetNeighbors();
    }

    bool IPathfinding.IsValidMoveBetweenNodes(INode start, INode end)
    {
        return IsValidMoveBetweenNodes(start.ToCell(), end.ToCell());
    }

    private bool IsValidMoveBetweenNodes(GridCell start, GridCell end)
    {
        const int height = 2;
        GridDirection direction = GridCoordinates.GetDirectionToCoordinates(start.Coordinates, end.Coordinates);

        // HACK: [LUTRO-238] Dynamically Sized Units - clean this up
        bool lowerOption = !start.HasBlockingObstacle(direction, fromRelativeHeight: 0, height);
        bool upperOption = !start.HasBlockingObstacle(direction, fromRelativeHeight: 1, height + 1);
        return lowerOption || upperOption; 
        
        // TODO: Check for Enough Vertical Space ?
        // GridManager.Map.GetCellsBelowCoordinates
    }

    int IPathfinding.GetHeuristicCostBetweenNodes(INode start, INode end)
    {
        return start.ToCell().Coordinates.GetDistanceTo(end.ToCell().Coordinates, ignoreLevelDistance: true, ignoreFallDistance: false);
    }

    int IPathfinding.GetMoveCostBetweenNodes(INode start, INode end)
    {
        return 1;
    }

    bool IPathfinding.IsPathOutsideMovementRange(INodePath path)
    {
        return false;
    }

    #endregion
    /************************************************************/
}