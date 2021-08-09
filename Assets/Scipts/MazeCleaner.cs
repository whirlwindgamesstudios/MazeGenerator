using UnityEngine;

public class MazeCleaner : MonoBehaviour
{
    public MazeSpawner MazeCreationObject;
    
    public void StartClear()
    {
        for (var x = 0; x <  MazeCreationObject.sizeMaze; ++x)
        for (var y = 0; y <  MazeCreationObject.sizeMaze; ++y)
        {
            if(MazeCreationObject.cellScripts[x, y] != null)
            if (MazeCreationObject.cellScripts[x, y].gameObject != null)
            {
                Destroy(MazeCreationObject.cellScripts[x, y].gameObject);
            }
            else
            {
                x = MazeCreationObject.sizeMaze;
                y = MazeCreationObject.sizeMaze;
            }
        }
    }
}
