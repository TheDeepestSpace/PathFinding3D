using UnityEngine;

public class Game : MonoBehaviour {

    void initialise() {
        Grid.initialiseInstances();
    }
 
	void Start () {
        initialise();

        Grid.addInstance("grid", new Vector3(10, 10, 10));

        Grid.getGrid("grid").setGridNode(new Vector3(0, 0, 0), GridNode.Type.START);
        Grid.getGrid("grid").setGridNode(new Vector3(9, 9, 9), GridNode.Type.END);

        Grid.getGrid("grid").setGridNodeBlock(new Vector3(0, 0, 7), new Vector3(10, 9, 1), GridNode.Type.BLOCK);
        Grid.getGrid("grid").setGridNodeBlock(new Vector3(0, 4, 0), new Vector3(10, 1, 7), GridNode.Type.BLOCK);
        Grid.getGrid("grid").setGridNodeBlock(new Vector3(2, 4, 3), new Vector3(2, 1, 2), GridNode.Type.BLANK);

        Debug.Log(Grid.getGrid("grid").findPath());

        drawPath();
	}

    void drawPath() {
        for (int i = 1; i < Grid.getGrid("grid").finalPath.Count - 1; i++) {
            GameObject pathNode = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pathNode.GetComponent<Renderer>().material.color = Color.yellow;
            pathNode.transform.position = Tool.getSceneCoordinates(Grid.getGrid("grid").finalPath[i].gridCoordinates);
        }
    }
}
