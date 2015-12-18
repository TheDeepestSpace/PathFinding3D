public class Grid3d {
  public static List<Grid3d> list; // or ArrayList
  
  public static const int tileSize = 1;
  
  public static const int pathFound = 1;
  public static const int pathNotFound = 2;
  
  public List<List<List<GridNode3d>>> nodes;
  public List<GridNode> openedList;
  public List<GridNode> closedList;
  public List<GridNode> finalPath;
  
  private Vector3 start, end;
  
  string name;
  public Vector3 size;
  
  /** INITIALISING */
  
  public static void initialiseInstances() {
    
  }
  
  /** CREATING AND SETTING */
  
  /** PATHFINDING */
  
  /** GETTERS / SETTERS */
  
  /** DISPOSING / RESETTING */
}
