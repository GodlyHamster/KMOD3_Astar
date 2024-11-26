using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path from the startPos to the endPos
    /// Note that you will probably need to add some helper functions
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        Cell currentCell = grid[startPos.x, startPos.y];
        Node currentNode = new Node(currentCell.gridPosition, null, 0, CalcDistance(startPos, endPos));

        Dictionary<Vector2Int, Node> dictOpen = new Dictionary<Vector2Int, Node>() { { currentNode.position, currentNode} };
        Dictionary<Vector2Int, Node> dictClosed = new Dictionary<Vector2Int, Node>();

        while (dictOpen.Count > 0)
        {
            dictOpen.Remove(currentNode.position);
            dictClosed.Add(currentNode.position, currentNode);

            if (currentNode.position == endPos)
            {
                Node reversePath = currentNode;
                while (reversePath.position != startPos)
                {
                    path.Add(reversePath.position);
                    reversePath = reversePath.parent;
                }
                path.Reverse();
                return path;
            }

            foreach (Cell cell in grid[currentNode.position.x, currentNode.position.y].GetAvailableNeighbours(grid))
            {
                if (dictOpen.ContainsKey(cell.gridPosition) || dictClosed.ContainsKey(cell.gridPosition)) continue;

                Node newNode = new Node(cell.gridPosition, currentNode, currentNode.GScore + CalcDistance(cell.gridPosition, currentNode.position), CalcDistance(cell.gridPosition, endPos));
                dictOpen.Add(cell.gridPosition, newNode);
            }

            Vector2Int lowestScoreTile = dictOpen.Keys.FirstOrDefault();
            int currentValue = int.MaxValue;
            foreach (KeyValuePair<Vector2Int, Node> node in dictOpen)
            {
                if (node.Value.FScore < currentValue)
                {
                    currentValue = node.Value.FScore;
                    lowestScoreTile = node.Key;
                }
            }
            currentNode = dictOpen[lowestScoreTile];
        }

        return path;
    }

    public static int CalcDistance(Vector2Int start, Vector2Int end)
    {
        return Mathf.Abs(start.x - end.x + start.y - end.y);
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public int FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public int GScore; //Current Travelled Distance
        public int HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
