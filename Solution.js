
/**
 * @param {number[][]} matrix
 * @return {number}
 */
var swimInWater = function (matrix) {
    this.moves = [[-1, 0], [1, 0], [0, -1], [0, 1]];
    this.rows = matrix.length;
    this.columns = matrix[0].length;

    const start = new Point(0, 0, matrix[0][0]);
    const goal = new Point(rows - 1, columns - 1, matrix[rows - 1][columns - 1]);

    return dijkstraSearchForMinTimePath(matrix, start, goal);
};

/**
 * @param {number} row
 * @param {number} column
 * @param {number} waterLevel
 */
function  Point(row, column, waterLevel) {
    this.row = row;
    this.column = column;
    this.waterLevel = waterLevel;
}

/**
 * @param {number[][]} matrix
 * @param {Point} start
 * @param {Point} goal
 * @return {number}
 */
function dijkstraSearchForMinTimePath(matrix, start, goal) {
    // const {MinPriorityQueue} = require('@datastructures-js/priority-queue');
    // PriorityQueue<Point>
    const minHeapForWaterLevel = new MinPriorityQueue({compare: (x, y) => x.waterLevel - y.waterLevel});
    minHeapForWaterLevel.enqueue(start);

    const minWaterLevelMatrix = createMinWaterLevelMatrix();
    minWaterLevelMatrix[start.row][start.column] = start.waterLevel;

    while (!minHeapForWaterLevel.isEmpty()) {
        const current = minHeapForWaterLevel.dequeue();
        if (current.row === goal.row && current.column === goal.column) {
            break;
        }

        for (let move of this.moves) {
            let nextRow = current.row + move[0];
            let nextColumn = current.column + move[1];

            if (isInMatrix(nextRow, nextColumn)) {
                let newWaterLevel = Math.max(current.waterLevel, matrix[nextRow][nextColumn]);

                if (newWaterLevel < minWaterLevelMatrix[nextRow][nextColumn]) {
                    minWaterLevelMatrix[nextRow][nextColumn] = newWaterLevel;
                    minHeapForWaterLevel.enqueue(new Point(nextRow, nextColumn, newWaterLevel));
                }
            }
        }
    }

    return minWaterLevelMatrix[goal.row][goal.column];
}

/**
 * @return {number[][]}
 */
function createMinWaterLevelMatrix() {
    return Array.from(new Array(this.rows), () => new Array(this.columns).fill(Number.MAX_SAFE_INTEGER));
}

/**
 * @param {number} row
 * @param {number} column
 * @return {boolean}
 */
function isInMatrix(row, column) {
    return row < this.rows && row >= 0 && column < this.columns && column >= 0;
}
