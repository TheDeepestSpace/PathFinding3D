using UnityEngine;

public class Tool {

	public static Vector3 getSceneCoordinates(Vector3 gridCoordinates) {
        return new Vector3(
            gridCoordinates.x * Grid.tileSize + Grid.tileSize / 2,
            gridCoordinates.y * Grid.tileSize + Grid.tileSize / 2,
            gridCoordinates.z * Grid.tileSize + Grid.tileSize / 2 
        );
    }
}
