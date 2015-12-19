public class Grid3d {
  public static List<Grid3d> list; // or ArrayList
  
  public static const int tileSize = 1;
  
  public static const int pathFound = 1;
  public static const int pathNotFound = 2;
  
  public List<List<List<GridNode3d>>> nodes;
  public List<GridNode3d> openedList;
  public List<GridNode3d> closedList;
  public List<GridNode3d> finalPath;
  
  private Vector3 start, end;
  
  string name;
  public Vector3 size;
  
  /** INITIALISING */
  
  public static void initialiseInstances() {
    list = new List<Grid>();
  }
  
  private void initialise() {
    this.nodes = new List<List<List<GridNode3d>>>();
    this.openedList = new List<GridNode3d>();
    this.closedList = new List<GridNode3d>();
    this.finalPath = new List<GridNode3d>();
    
    this.start = new Vector3();
    this.end = new Vector3();
    
    this.name = "n/a";
    this.size = new Vecor3(0, 0, 0);
  }
  
  /** CREATING AND SETTING */
  
  private Grid() {
    this.initialise();
  }
  
  private void setUp(string name, Vector3 size) {
    this.name = name;
    this.size.x = size.x;
    this.size.y = size.y;
    this.size.z = size.z;
  }
  
  void createNodes() {
    Vector3 nodePos = new Vector3();
    
    for (int x = 0; x < this.size.x; x++) {
      this.nodes.Add(new List<List<GridNode3d>>());
      nodePos.x = x;
      for (int y = 0; y < this.size.y; y++) {
        this.nodes[x].Add(new List<GridNode3d>());
        nodePos.y = y;
        for (int z = 0; z < this.size.z; z++) {
          this.nodes[x][y].Add(new GridNode3d(this.name, nodePos));
        }
      }
    }
  }
  
  /** PATHFINDING */
  
  static Vector3 openListPosition = new Vector3();
  
  public int findPath() {
    this.openedList.Clear();
    this.closedList.Clear();
    this.finalPath.Clear();
    
    this.resetNodes();
    
    if (this.start.x == -1 || this.start.y == -1 || this.start.z == -1
      ||this.end.x == -1 || this.end.y == -1 || this.end.z == -1) {
      return pathNotFound;
    } else if (this.start.x == this.end.x
            &&this.start.y == this.end.y
            &&this.start.z == this.end.z) {
      return pathFound;        
    } else {
      this.openedList.Add(
        this.nodes[(int) start.x][(int) start.y][(int) start.z]  
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
          }else {
            return pathNotFound;
          }
        }else {
          return pathNotFound;
        }
      }
    }
    
    GridNode3d g = this.closedList[this.closedList.Count - 1];
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
    bool ignoreRight = (gridCoordinates.x + 1) >= this.nodes.Count;
    bool ignoreDown = (gridCoordinates.y - 1) < 0;
    bool ignoreUp = (gridCoordinates.y + 1) >= this.nodes[0].Count;
    bool ignoreBack = (gridCoordinates.z - 1) < 0;
    bool ignoreFront = (gridCoordinates.z + 1) >= this.nodes[0][0].Count;
    
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
  
  private void lookNode(GridNode3d parentNode, GridNode3d currentNode) {
    if (currentNode.type != GridNode3d.Type.BLOCK &&
        !(this.closedList.Contains(currentNode))) {
      if (!(this.openedList.Contains(currentNode))) {
        currentNode.calculateValues(parentNode, this.getEndNode());
        this.openedList.Add(currentNode);
      } else {
        compateParentWithOpen(parentNode, currentNode); // !
      }
    }
  }
  
  private void compateParentWithOpen(GridNode3d parentNode, GridNode3d openNode) {
    double tempG = openNode.G;
    double xDistance = Mathf.Abs(openNode.gridCoordinates.x - parentNode.gridCoordinates.x) / tileSize;
    double yDistance = Mathf.Abs(openNode.gridCoordinates.y - parentNode.gridCoordinates.y) / tileSize;
    double zDistance = Mathf.Abs(openNode.gridCoordinates.z - parentNode.gridCoordinates.z) / tileSize;
    
    if (xDistance == 1 && yDistance == 1 && zDistance == 1) {
      tempG += 17;
    }else if ((xDistance == 1 && yDistance == 1)
            ||(xDistance == 1 && zDistance == 1)
            ||(yDistance == 1 && zDistance == 1)) {
      tempG += 14;        
    }else {
      tempG += 10;
    }
    
    if (tempG < parentNode.G) {
      openNode.calculateValues(parentNode, this.getEndNode());
      this.openedList[this.openedList.IndexOf(openNode)] = openNode;
    }
    
  }
  
  public void setGridNode(Vector3 position, GridNode.Type type) {
    if (position.x >= 0 && position.x < this.nodes.Count) {
      if (position.y >= 0 && position.y < this.nodes[(int)position.x].Count) {
        if (position.z >= 0 && position.z < this.nodes[(int)position.x][(int)position.y].Count) {
          if (type == GridNode.Type.START || type == GridNode.Type.END) {
            for (int x = 0; x < this.nodes.Count; x++) {
              for (int y = 0; y < this.nodes[x].Count; y++) {
                for (int z = 0; z < this.nodes[x][y].Count; z++) {
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
                    
                    this.nodes[x][y][z].type = GridNode.Type.NONE;
                  }
                }
              }
            }
          }
        }
      }
    }
    
    if (this.nodes[(int)position.x][(int)position.y][(int)position.z].type == type) {
      this.nodes[(int)position.x][(int)position.y][(int)position.z].type = GridNode.Type.NONE;
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
  }
  
  /** GETTERS / SETTERS */
  
  public GridNode3d getStartNode() {
    return this.nodes[(int) start.x][(int) start.y][(int) start.z];
  }
  
  public GridNode3d getEndNode() {
    return this.nodes[(int) end.x][(int) end.y][(int) end.z];
  }
  
  private int getBestFIndex () {
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
