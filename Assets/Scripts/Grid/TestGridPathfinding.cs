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

    public List<Node> GetNeighborsFromNode(Node node)
    {
        return node.GetNeighbors();
    }

    public bool IsValidMoveBetweenNodes(Node start, Node end)
    {  
        int height = 2;
        GridCell startCell = start.Instance as GridCell;
        GridCell endCell = end.Instance as GridCell;
        GridDirection direction = GridCoordinates.GetDirectionToCoordinates(startCell.Coordinates, endCell.Coordinates);

        // HACK: [LUTRO-238] Dynamically Sized Units - clean this up
        bool lowerOption = !startCell.HasBlockingObstacle(direction, fromRelativeHeight: 0, height);
        bool upperOption = !startCell.HasBlockingObstacle(direction, fromRelativeHeight: 1, height + 1);
        return lowerOption || upperOption; 
        
        // TODO: Check for Enough Vertical Space ?
        // GridManager.Map.GetCellsBelowCoordinates
    }

    public int GetDistanceBetweenNodes(Node start, Node end)
    {
        return start.GetCell().Coordinates.GetDistanceTo(end.GetCell().Coordinates, ignoreFallDistance: false);
    }

    public int GetMoveCostBetweenNodes(Node start, Node end)
    {
        return 1;
    }

    public bool IsPathOutsideMovementRange(NodePath path)
    {
        return false;
    }

    #endregion
    /************************************************************/
}