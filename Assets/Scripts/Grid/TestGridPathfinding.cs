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

    List<Node> IPathfinding.GetNeighborsFromNode(Node node)
    {
        return node.GetNeighbors();
    }

    bool IPathfinding.IsValidMoveBetweenNodes(Node start, Node end)
    {
        return IsValidMoveBetweenNodes((start as Node<GridCell>).Owner, (end as Node<GridCell>).Owner);
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

    int IPathfinding.GetHeuristicCostBetweenNodes(Node start, Node end)
    {
        return start.GetCell().Coordinates.GetDistanceTo(end.GetCell().Coordinates, ignoreLevelDistance: true, ignoreFallDistance: false);
    }

    int IPathfinding.GetMoveCostBetweenNodes(Node start, Node end)
    {
        return 1;
    }

    bool IPathfinding.IsPathOutsideMovementRange(NodePath path)
    {
        return false;
    }

    #endregion
    /************************************************************/
}