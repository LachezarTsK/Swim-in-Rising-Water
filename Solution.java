
import java.util.Arrays;
import java.util.PriorityQueue;

public class Solution {

    private record Point(int row, int column, int waterLevel) {}

    private static final int[][] moves = {{-1, 0}, {1, 0}, {0, -1}, {0, 1}};

    private int rows;
    private int columns;

    public int swimInWater(int[][] matrix) {
        rows = matrix.length;
        columns = matrix[0].length;

        Point start = new Point(0, 0, matrix[0][0]);
        Point goal = new Point(rows - 1, columns - 1, matrix[rows - 1][columns - 1]);

        return dijkstraSearchForMinTimePath(matrix, start, goal);
    }

    private int dijkstraSearchForMinTimePath(int[][] matrix, Point start, Point goal) {
        PriorityQueue<Point> minHeapForWaterLevel = new PriorityQueue<>((x, y) -> x.waterLevel - y.waterLevel);
        minHeapForWaterLevel.add(start);

        int[][] minWaterLevelMatrix = createMinWaterLevelMatrix();
        minWaterLevelMatrix[start.row][start.column] = start.waterLevel;

        while (!minHeapForWaterLevel.isEmpty()) {
            Point current = minHeapForWaterLevel.poll();
            if (current.row == goal.row && current.column == goal.column) {
                break;
            }

            for (int[] move : moves) {
                int nextRow = current.row + move[0];
                int nextColumn = current.column + move[1];

                if (isInMatrix(nextRow, nextColumn)) {
                    int newWaterLevel = Math.max(current.waterLevel, matrix[nextRow][nextColumn]);

                    if (newWaterLevel < minWaterLevelMatrix[nextRow][nextColumn]) {
                        minWaterLevelMatrix[nextRow][nextColumn] = newWaterLevel;
                        minHeapForWaterLevel.add(new Point(nextRow, nextColumn, newWaterLevel));
                    }
                }
            }
        }

        return minWaterLevelMatrix[goal.row][goal.column];
    }

    private int[][] createMinWaterLevelMatrix() {
        int[][] minWaterLevelMatrix = new int[rows][columns];
        for (int row = 0; row < rows; ++row) {
            Arrays.fill(minWaterLevelMatrix[row], Integer.MAX_VALUE);
        }
        return minWaterLevelMatrix;
    }

    private boolean isInMatrix(int row, int column) {
        return row < rows && row >= 0 && column < columns && column >= 0;
    }
}
