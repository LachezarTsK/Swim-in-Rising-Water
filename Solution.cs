
using System;
using System.Collections.Generic;

public class Solution
{
    private struct Point
    {
        public readonly int row;
        public readonly int column;
        public readonly int waterLevel;

        public Point(int row, int column, int waterLevel)
        {
            this.row = row;
            this.column = column;
            this.waterLevel = waterLevel;
        }
    };

    private readonly int[][] moves = new int[4][]
    { new int[]{-1, 0}, new int[]{1, 0}, new int[]{0, -1}, new int[]{0, 1} };

    int rows;
    int columns;

    public int SwimInWater(int[][] matrix)
    {
        rows = matrix.Length;
        columns = matrix[0].Length;

        Point start = new Point(0, 0, matrix[0][0]);
        Point goal = new Point(rows - 1, columns - 1, matrix[rows - 1][columns - 1]);

        return DijkstraSearchForMinTimePath(matrix, start, goal);
    }

    private int DijkstraSearchForMinTimePath(int[][] matrix, Point start, Point goal)
    {
        Comparer<int> CompareWaterLevel = Comparer<int>.Create((x, y) => x.CompareTo(y));
        PriorityQueue<Point, int> minHeapForWaterLevel = new PriorityQueue<Point, int>(CompareWaterLevel);
        minHeapForWaterLevel.Enqueue(start, start.waterLevel);

        int[][] minWaterLevelMatrix = CreateMinWaterLevelMatrix();
        minWaterLevelMatrix[start.row][start.column] = start.waterLevel;

        while (minHeapForWaterLevel.Count > 0)
        {
            Point current = minHeapForWaterLevel.Dequeue();
            if (current.row == goal.row && current.column == goal.column)
            {
                break;
            }

            foreach (int[] move in moves)
            {
                int nextRow = current.row + move[0];
                int nextColumn = current.column + move[1];

                if (IsInMatrix(nextRow, nextColumn))
                {
                    int newWaterLevel = Math.Max(current.waterLevel, matrix[nextRow][nextColumn]);

                    if (newWaterLevel < minWaterLevelMatrix[nextRow][nextColumn])
                    {
                        minWaterLevelMatrix[nextRow][nextColumn] = newWaterLevel;
                        minHeapForWaterLevel.Enqueue(new Point(nextRow, nextColumn, newWaterLevel), newWaterLevel);
                    }
                }
            }
        }

        return minWaterLevelMatrix[goal.row][goal.column];
    }

    private int[][] CreateMinWaterLevelMatrix()
    {
        int[][] minWaterLevelMatrix = new int[rows][];
        for (int row = 0; row < rows; ++row)
        {
            minWaterLevelMatrix[row] = new int[columns];
            Array.Fill(minWaterLevelMatrix[row], int.MaxValue);
        }
        return minWaterLevelMatrix;
    }

    private bool IsInMatrix(int row, int column)
    {
        return row < rows && row >= 0 && column < columns && column >= 0;
    }
}
