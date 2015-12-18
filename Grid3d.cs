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
    this.width = this.height = this.depth = 0;
  }
  
  /** CREATING AND SETTING */
  
  private Grid() {
    this.initialise();
  }
  
  private void setUp(string name, Vector3 size) {
    this.name = name;
    this.size.x = size.x;
    this.size.y = size.y;
    this.size.z = zise.z;
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
  
  /** GETTERS / SETTERS */
  
  /** DISPOSING / RESETTING */
}
