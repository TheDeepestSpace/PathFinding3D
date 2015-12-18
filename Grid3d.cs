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
  
  
  
  /** PATHFINDING */
  
  /** GETTERS / SETTERS */
  
  /** DISPOSING / RESETTING */
}
