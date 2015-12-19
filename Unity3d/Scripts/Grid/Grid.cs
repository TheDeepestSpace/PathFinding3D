using UnityEngine;
using System.Collections.Generic;

public class Grid {
    public static List<Grid> list;

    public static int tileSize = 1;

    public static int pathFound = 1;
    public static int pathNotFound = 2;

    public List<List<List<GridNode>>> nodes;
    public List<GridNode> openedList;
    public List<GridNode> closedList;
    public List<GridNode> finalPath;

    private Vector3 start, end;

    string name;
    public Vector3 size;

    /** INITIALISING */

    public static void initialiseInstances() {
        list = new List<Grid>();
    }

    private void initialise() {
        this.nodes = new List<List<List<GridNode>>>();
        this.openedList = new List<GridNode>();
        this.closedList = new List<GridNode>();
        this.finalPath = new List<GridNode>();

        this.start = new Vector3();
        this.end = new Vector3();

        this.name = "n/a";
        this.size = new Vector3(0, 0, 0);
    }

    /** CREATING AND SETTING */

    public static Grid addInstance(string name, Vector3 size) {
        Grid g = new Grid();
        g.setUp(name, size);

        list.Add(g);

        return g;
    }

    private Grid() {
        this.initialise();
    }

    private void setUp(string name, Vector3 size) {
        this.name = name;
        this.size.x = size.x;
        this.size.y = size.y;
        this.size.z = size.z;

        this.createNodes();
    }

    void createNodes() {
        Vector3 nodePos = new Vector3();

        for (int x = 0; x < this.size.x; x++) {
            this.nodes.Add(new List<List<GridNode>>());
            nodePos.x = x;
            for (int y = 0; y < this.size.y; y++) {
                this.nodes[x].Add(new List<GridNode>());
                nodePos.y = y;
                for (int z = 0; z < this.size.z; z++) {
                    nodePos.z = z;
                    this.nodes[x][y].Add(new GridNode(this.name, nodePos));
                }
            }
        }
    }

    /** PATHFINDING */

    public static Vector3 openListPosition = new Vector3();

    public int findPath() {
        this.openedList.Clear();
        this.closedList.Clear();
        this.finalPath.Clear();

        this.resetNodes();

        if (this.start.x == -1 || this.start.y == -1 || this.start.z == -1
          || this.end.x == -1 || this.end.y == -1 || this.end.z == -1) {
            return pathNotFound;
        } else if (this.start.x == this.end.x
                && this.start.y == this.end.y
                && this.start.z == this.end.z) {
            return pathFound;
        } else {
            this.openedList.Add(
              this.nodes[(int)start.x][(int)start.y][(int)start.z]
            );

            this.setOpenList(this.start);

            this.closedList.Add(this.openedList[0]);
            this.openedList.Remove(this.openedList[0]);

            while (this.closedList[this.closedList.Count - 1] != this.getEndNode()) {
                if (this.openedList.Count != 0) {
                    int bestFIndex = this.getBestFIndex();
                    if (bestFIndex != -1) {
                        this.closedList.Add(this.openedList[bestFIndex]);
                        this.openedList.Remove(this.openedList[bestFIndex]);

                        openListPosition.x = this.closedList[this.closedList.Count - 1].gridCoordinates.x;
                        openListPosition.y = this.closedList[this.closedList.Count - 1].gridCoordinates.y;
                        openListPosition.z = this.closedList[this.closedList.Count - 1].gridCoordinates.z;

                        this.setOpenList(openListPosition);
                    } else {
                        return pathNotFound;
                    }
                } else {
                    return pathNotFound;
                }
            }
        }

        GridNode g = this.closedList[this.closedList.Count - 1];
        this.finalPath.Add(g);


        while (g != this.getStartNode()) {
            g = g.parent;
            this.finalPath.Add(g);
        }

        this.finalPath.Reverse();

        return pathFound;
    }

    private void setOpenList(Vector3 gridCoordinates) {
        bool ignoreLeft = (gridCoordinates.x - 1) < 0;
        bool ignoreRight = (gridCoordinates.x + 1) >= this.size.x;
        bool ignoreDown = (gridCoordinates.y - 1) < 0;
        bool ignoreUp = (gridCoordinates.y + 1) >= this.size.y;
        bool ignoreBack = (gridCoordinates.z - 1) < 0;
        bool ignoreFront = (gridCoordinates.z + 1) >= this.size.z;

        if (!ignoreLeft && !ignoreDown && !ignoreBack) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x - 1][(int)gridCoordinates.y - 1][(int)gridCoordinates.z - 1]);
        }

        if (!ignoreLeft && !ignoreDown) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x - 1][(int)gridCoordinates.y - 1][(int)gridCoordinates.z]);
        }

        if (!ignoreLeft && !ignoreDown && !ignoreFront) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x - 1][(int)gridCoordinates.y - 1][(int)gridCoordinates.z + 1]);
        }

        if (!ignoreDown && !ignoreBack) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y - 1][(int)gridCoordinates.z - 1]);
        }

        if (!ignoreDown) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y - 1][(int)gridCoordinates.z]);
        }

        if (!ignoreDown && !ignoreFront) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y - 1][(int)gridCoordinates.z + 1]);
        }

        if (!ignoreRight && !ignoreDown && !ignoreBack) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x + 1][(int)gridCoordinates.y - 1][(int)gridCoordinates.z - 1]);
        }

        if (!ignoreRight && !ignoreDown) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x + 1][(int)gridCoordinates.y - 1][(int)gridCoordinates.z]);
        }

        if (!ignoreRight && !ignoreDown && !ignoreFront) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x + 1][(int)gridCoordinates.y - 1][(int)gridCoordinates.z + 1]);
        }

        if (!ignoreLeft && !ignoreBack) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x - 1][(int)gridCoordinates.y][(int)gridCoordinates.z - 1]);
        }

        if (!ignoreLeft) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x - 1][(int)gridCoordinates.y][(int)gridCoordinates.z]);
        }

        if (!ignoreLeft && !ignoreFront) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x - 1][(int)gridCoordinates.y][(int)gridCoordinates.z + 1]);
        }

        if (!ignoreBack) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z - 1]);
        }

        if (!ignoreFront) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z + 1]);
        }

        if (!ignoreRight && !ignoreBack) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x + 1][(int)gridCoordinates.y][(int)gridCoordinates.z - 1]);
        }

        if (!ignoreRight) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x + 1][(int)gridCoordinates.y][(int)gridCoordinates.z]);
        }

        if (!ignoreRight && !ignoreFront) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x + 1][(int)gridCoordinates.y][(int)gridCoordinates.z + 1]);
        }

        if (!ignoreLeft && !ignoreUp && !ignoreBack) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x - 1][(int)gridCoordinates.y + 1][(int)gridCoordinates.z - 1]);
        }

        if (!ignoreLeft && !ignoreUp) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x - 1][(int)gridCoordinates.y + 1][(int)gridCoordinates.z]);
        }

        if (!ignoreLeft && !ignoreUp && !ignoreFront) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x - 1][(int)gridCoordinates.y + 1][(int)gridCoordinates.z + 1]);
        }

        if (!ignoreUp && !ignoreBack) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y + 1][(int)gridCoordinates.z - 1]);
        }

        if (!ignoreUp) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y + 1][(int)gridCoordinates.z]);
        }

        if (!ignoreUp && !ignoreFront) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y + 1][(int)gridCoordinates.z + 1]);
        }

        if (!ignoreRight && !ignoreUp && !ignoreBack) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x + 1][(int)gridCoordinates.y + 1][(int)gridCoordinates.z - 1]);
        }

        if (!ignoreRight && !ignoreUp) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x + 1][(int)gridCoordinates.y + 1][(int)gridCoordinates.z]);
        }

        if (!ignoreRight && !ignoreUp && !ignoreFront) {
            lookNode(this.nodes[(int)gridCoordinates.x][(int)gridCoordinates.y][(int)gridCoordinates.z],
              this.nodes[(int)gridCoordinates.x + 1][(int)gridCoordinates.y + 1][(int)gridCoordinates.z + 1]);
        }
    }

    private void lookNode(GridNode parentNode, GridNode currentNode) {
        if (currentNode.type != GridNode.Type.BLOCK &&
            !(this.closedList.Contains(currentNode))) {
            if (!(this.openedList.Contains(currentNode))) {
                currentNode.calculateValues(parentNode, this.getEndNode());
                this.openedList.Add(currentNode);
            } else {
                compateParentWithOpen(parentNode, currentNode);
            }
        }
    }

    private void compateParentWithOpen(GridNode parentNode, GridNode openNode) {
        double tempG = openNode.G;
        double xDistance = Mathf.Abs(openNode.gridCoordinates.x - parentNode.gridCoordinates.x) / tileSize;
        double yDistance = Mathf.Abs(openNode.gridCoordinates.y - parentNode.gridCoordinates.y) / tileSize;
        double zDistance = Mathf.Abs(openNode.gridCoordinates.z - parentNode.gridCoordinates.z) / tileSize;

        if (xDistance == 1 && yDistance == 1 && zDistance == 1) {
            tempG += 17;
        } else if ((xDistance == 1 && yDistance == 1)
                 || (xDistance == 1 && zDistance == 1)
                 || (yDistance == 1 && zDistance == 1)) {
            tempG += 14;
        } else {
            tempG += 10;
        }

        if (tempG < parentNode.G) {
            openNode.calculateValues(parentNode, this.getEndNode());
            this.openedList[this.openedList.IndexOf(openNode)] = openNode;
        }

    }

    public void setGridNodeBlock(Vector3 position, Vector3 size, GridNode.Type type) {
        for (int x = (int) position.x; x < (int) position.x + (int) size.x; x++) {
            for (int y = (int)position.y; y < (int)position.y + (int)size.y; y++) {
                for (int z = (int)position.z; z < (int)position.z + (int)size.z; z++) {
                    this.setGridNode(new Vector3(x, y, z), type);
                }
            }
        }
    }

    public void setGridNode(Vector3 position, GridNode.Type type) {
        if (position.x >= 0 && position.x < this.size.x) {
            if (position.y >= 0 && position.y < this.size.y) {
                if (position.z >= 0 && position.z < this.size.z) {
                    if (type == GridNode.Type.START || type == GridNode.Type.END) {
                        for (int x = 0; x < this.size.x; x++) {
                            for (int y = 0; y < this.size.y; y++) {
                                for (int z = 0; z < this.size.z; z++) {
                                    if (this.nodes[x][y][z].type == type) {
                                        if (type == GridNode.Type.START) {
                                            this.start.x = -1;
                                            this.start.y = -1;
                                            this.start.z = -1;
                                        } else {
                                            this.end.x = -1;
                                            this.end.y = -1;
                                            this.end.z = -1;
                                        }

                                        this.nodes[x][y][z].type = GridNode.Type.BLANK;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (this.nodes[(int)position.x][(int)position.y][(int)position.z].type == type) {
            this.nodes[(int)position.x][(int)position.y][(int)position.z].type = GridNode.Type.BLANK;
        } else {
            if (type == GridNode.Type.START) {
                this.start.x = position.x;
                this.start.y = position.y;
                this.start.z = position.z;
            } else if (type == GridNode.Type.END) {
                this.end.x = position.x;
                this.end.y = position.y;
                this.end.z = position.z;
            }
            this.nodes[(int)position.x][(int)position.y][(int)position.z].type = type;
        }

        this.nodes[(int)position.x][(int)position.y][(int)position.z].updateModel();
    }

    /** GETTERS / SETTERS */

    public GridNode getStartNode() {
        return this.nodes[(int)start.x][(int)start.y][(int)start.z];
    }

    public GridNode getEndNode() {
        return this.nodes[(int)end.x][(int)end.y][(int)end.z];
    }

    private int getBestFIndex() {
        double bestF = float.MaxValue;
        int index = -1;

        for (int i = 0; i < this.openedList.Count; i++) {
            if (bestF > this.openedList[i].F) {
                bestF = this.openedList[i].F;
                index = i;
            }
        }

        return index;
    }

    public static Grid getGrid(string gridName) {
        for (int i = 0; i < list.Count; i++) {
            if (list[i].name == gridName) {
                return list[i];
            }
        }

        return null;
    }

    /** DISPOSING / RESETTING */

    private void resetNodes() {
        for (int x = 0; x < this.size.x; x++) {
            for (int y = 0; y < this.size.y; y++) {
                for (int z = 0; z < this.size.z; z++) {
                    this.nodes[x][y][z].reset();
                }
            }
        }
    }
}
