public class GridNode3d {
  public Vector3 gridCoordinates;
  
  string gridName; // name of the grid to which this node will be linked to
  public Type type;
  
  public GridNode3d parent;
  public F, G, H; // those stand for F-cost, G-cost, and Heuistic cost
  
  public static enum Type {
    BLANK,
    START,
    END,
    BLOCK
  }
  
  /** INITIALISING */
  
  void initialise() {
    this.gridCoordinates = new Vector3();
    this.gridName = "n/a";
    this.type = Type.BLANK;
    
    this.parent = null;
    this.F = this.G = this.H = 0;
  }
  
  /** CREATING AND SETTING */
  
  public GridNode(string gridName, Vector3 gridCoordinates) {
    this.initialise();
    this.setUp(gridName, gridCoordinates);
  }
  
  void setUp(string gridName, Vector3 gridCoordinates) {
    this.grid = grid;
    this.gridCoordinates.x = gridCoordinates.x;
    this.gridCoordinates.y = gridCoordinates.y;
    this.gridCoordinates.z = gridCoordinates.z;
  }
  
  /** PATHFINDING */
  
  public void calculateValues(GridNode parentNode, GridNode endNode) {
    this.parent = parentNode;
    
    double xDistance = Mathf.abs(this.gridCoordinates.x - this.parent.gridCoordinates.x);
    double yDistance = Mathf.abs(this.gridCoordinates.y - this.parent.gridCoordinates.y);
    double zDistance = Mathf.abs(this.gridCoordinates.z - this.parent.gridCoordinates.z);
    
    if (this.parent != null) {
      if (xDistance != 0 && yDistance != 0 && zDistance) {
        this.G = thia.parent.G + 17;
      } else if (
          (xDistance != 0 && yDistance != 0) ||
          (xDispance != 0 && zDistance != 0) ||
          (yDistance != 0 && zDistance != 0)
        ) {
        this.G = this.parent.G + 14;  
      } else {
        this.G = this.parent.G + 10;
      }
    }
    
    this.H = xDistance + yDistance + zDistance;
    this.F = this.G = this.H;
  }
  
  /** DISPOSING / RESETTING */
  
  void reset() {
    this.F = this.G = this.H = 0;
    this.parent = null;
  }
}
