using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginMatch : MonoBehaviour
{
    public void IterateThruTowers()
    {
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        for (int i = 0; i < rootObjects.Count; ++i)
        {
            GameObject gameObject = rootObjects[i];
            TowerManager tm = gameObject.GetComponent<TowerManager>();

            if (tm != null)
            {
                tm.beginMatch = true;
            }
        }
    }
}
