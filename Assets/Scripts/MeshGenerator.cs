using UnityEngine;
using System.Collections;

public class MeshGenerator : MonoBehaviour {

    public SquareGrid squareGrid;

    public void GenerateMesh(int[,] map, float squareSize) {
        squareGrid = new SquareGrid(map, squareSize);
    }

    void OnDrawGizmos() {
        if (squareGrid != null) {
            for (int x = 0; x < squareGrid.squares.GetLength(0); ++x) {
                for (int y = 0; y < squareGrid.squares.GetLength(1); ++y) {

                    //drawing each node
                    Gizmos.color = (squareGrid.squares[x, y].topLeft.active)? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].topLeft.position, Vector3.one * 0.4f);

                    Gizmos.color = (squareGrid.squares[x, y].topRight.active)? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].topRight.position, Vector3.one * 0.4f);

                    Gizmos.color = (squareGrid.squares[x, y].bottomLeft.active)? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].bottomLeft.position, Vector3.one * 0.4f);

                    Gizmos.color = (squareGrid.squares[x, y].bottomRight.active)? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].bottomRight.position, Vector3.one * 0.4f);

                    //for the mid point positions
                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(squareGrid.squares[x, y].centerTop.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centerBottom.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centerLeft.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centerRight.position, Vector3.one * 0.15f);
                }
            }
        }
    }

    public class SquareGrid {

        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize) {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; ++x) {
                for (int y = 0; y < nodeCountY; ++y) {
                    Vector3 pos = new Vector3(-mapWidth/2 + x * squareSize + squareSize/2, 0, -mapHeight/2 + y * squareSize + squareSize/2);
                    controlNodes[x,y] = new ControlNode(pos, map[x,y] == 1, squareSize);
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; ++x) {
                for (int y = 0; y < nodeCountY - 1; ++y) {
                    squares[x, y] = new Square(controlNodes[x, y+1], controlNodes[x+1, y+1], controlNodes[x+1, y], controlNodes[x, y]);
                }
            }
        }
    }

    public class Square {

        //The forth controlnodes in each corner and the nodes between them
        public ControlNode topLeft, topRight, bottomLeft, bottomRight;
        public Node centerTop, centerBottom, centerRight, centerLeft;

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomLeft, ControlNode _bottomRight) {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomLeft = _bottomLeft;
            bottomRight = _bottomRight;

            centerTop = topLeft.right;
            centerBottom = bottomLeft.right;
            centerRight = bottomRight.above;
            centerLeft = bottomLeft.above;
        }
    }

    public class Node {

        public Vector3 position;
        //For the mesh
        public int vertexIndex = -1;

        public Node(Vector3 _pos) {
            position = _pos;
        }
    }

    public class ControlNode : Node {

        //If the node is or not a wall
        public bool active;
        //Nodes nearby
        public Node above, right;

        public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos) {
            active = _active;
            above = new Node(position + Vector3.forward * squareSize/2f);
            right = new Node(position + Vector3.right * squareSize/2f);
        }
    }
}
