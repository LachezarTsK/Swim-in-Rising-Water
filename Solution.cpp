
#include <span>
#include <array>
#include <queue>
#include <limits>
#include <vector>
#include <algorithm>
using namespace std;

class Solution {

    struct Point {
        int row;
        int column;
        int waterLevel;

        Point(int row, int column, int waterLevel) :
        row{row}, column{column}, waterLevel{waterLevel}{}
    };

    struct CompareWaterLevel {
        auto operator()(const Point& first, const Point& second) const {
            return first.waterLevel > second.waterLevel;
        }
    };

    inline static const array<array<int, 2>, 4> moves { {{-1, 0}, {1, 0}, {0, -1}, {0, 1}} };

    int rows = 0;
    int columns = 0;

public:
    int swimInWater(const vector<vector<int>>& matrix) {
        rows = matrix.size();
        columns = matrix[0].size();

        Point start(0, 0, matrix[0][0]);
        Point goal(rows - 1, columns - 1, matrix[rows - 1][columns - 1]);

        return dijkstraSearchForMinTimePath(matrix, start, goal);
    }

private:
    int dijkstraSearchForMinTimePath(span<const vector<int>> matrix, const Point& start, const Point& goal) const {
        priority_queue<Point, vector<Point>, CompareWaterLevel> minHeapForWaterLevel;
        minHeapForWaterLevel.push(start);

        vector<vector<int>> minWaterLevelMatrix = createMinWaterLevelMatrix();
        minWaterLevelMatrix[start.row][start.column] = start.waterLevel;

        while (!minHeapForWaterLevel.empty()) {
            Point current = minHeapForWaterLevel.top();
            minHeapForWaterLevel.pop();

            if (current.row == goal.row && current.column == goal.column) {
                break;
            }

            for (const auto& move : moves) {
                int nextRow = current.row + move[0];
                int nextColumn = current.column + move[1];

                if (isInMatrix(nextRow, nextColumn)) {
                    int newWaterLevel = max(current.waterLevel, matrix[nextRow][nextColumn]);

                    if (newWaterLevel < minWaterLevelMatrix[nextRow][nextColumn]) {
                        minWaterLevelMatrix[nextRow][nextColumn] = newWaterLevel;
                        minHeapForWaterLevel.emplace(nextRow, nextColumn, newWaterLevel);
                    }
                }
            }
        }

        return minWaterLevelMatrix[goal.row][goal.column];
    }

    vector<vector<int>> createMinWaterLevelMatrix() const {
        // alternative for this case: INT_MAX from <climts>
        return vector<vector<int>>(rows, vector<int>(columns, numeric_limits<int>::max()));
    }

    bool isInMatrix(int row, int column) const {
        return row < rows && row >= 0 && column < columns && column >= 0;
    }
};
