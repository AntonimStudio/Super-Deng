using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCounterScript : MonoBehaviour
{
    private FaceScript[] faces;
    [SerializeField] private FaceArrayScript FAS;
    Queue<FaceScript> queue = new Queue<FaceScript>();

    private void Start()
    {
        faces = FAS.GetAllFaceScripts();
        SetPathCount();

    }

    public void SetPathCount()
    {
        FaceScript startface = null;
        foreach (var face in faces)
        {
            if (face.havePlayer)
            {
                startface = face;
                //Debug.Log(face.name);
            }
            face.pathObjectCount = -1;
        }
        queue = new Queue<FaceScript>();
        queue.Enqueue(startface);
        startface.pathObjectCount = 0;
        while (queue.Count > 0)
        {
            FaceScript current = queue.Dequeue();

            FaceScript[] neighbors = { current.FS1, current.FS2, current.FS3 };
            foreach (var neighbor in neighbors)
            {
                if (neighbor != null && neighbor.pathObjectCount == -1)
                {
                    neighbor.pathObjectCount = current.pathObjectCount + 1;
                    queue.Enqueue(neighbor);
                }
            }
        }
    }
}
