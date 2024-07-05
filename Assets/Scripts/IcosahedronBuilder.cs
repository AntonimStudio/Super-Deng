using UnityEngine;

public class IcosahedronBuilder : MonoBehaviour
{
    public GameObject prismPrefab; // Префаб треугольной призмы
    public float scale = 1.0f; // Масштаб икосаэдра
    public float prismScaleFactor = 0.9f; // Коэффициент масштаба для префабов

    void Start()
    {
        if (prismPrefab == null)
        {
            Debug.LogError("Prism Prefab не назначен. Пожалуйста, назначьте его в инспекторе.");
            return;
        }

        BuildIcosahedron();
    }

    void BuildIcosahedron()
    {
        // Золотое сечение
        float phi = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        // Координаты 12 вершин икосаэдра
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-1,  phi,  0),
            new Vector3( 1,  phi,  0),
            new Vector3(-1, -phi,  0),
            new Vector3( 1, -phi,  0),
            new Vector3( 0, -1,  phi),
            new Vector3( 0,  1,  phi),
            new Vector3( 0, -1, -phi),
            new Vector3( 0,  1, -phi),
            new Vector3( phi,  0, -1),
            new Vector3( phi,  0,  1),
            new Vector3(-phi,  0, -1),
            new Vector3(-phi,  0,  1)
        };

        // Нормализация вершин для получения правильных размеров
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i].normalized * scale;
        }

        // Определение треугольников (граней) икосаэдра
        int[][] triangles = new int[][]
        {
            new int[] { 0, 11, 5 },
            new int[] { 0, 5, 1 },
            new int[] { 0, 1, 7 },
            new int[] { 0, 7, 10 },
            new int[] { 0, 10, 11 },
            new int[] { 1, 5, 9 },
            new int[] { 5, 11, 4 },
            new int[] { 11, 10, 2 },
            new int[] { 10, 7, 6 },
            new int[] { 7, 1, 8 },
            new int[] { 3, 9, 4 },
            new int[] { 3, 4, 2 },
            new int[] { 3, 2, 6 },
            new int[] { 3, 6, 8 },
            new int[] { 3, 8, 9 },
            new int[] { 4, 9, 5 },
            new int[] { 2, 4, 11 },
            new int[] { 6, 2, 10 },
            new int[] { 8, 6, 7 },
            new int[] { 9, 8, 1 }
        };

        // Создание и размещение треугольных призм
        foreach (var triangle in triangles)
        {
            // Координаты вершин треугольника
            Vector3 v0 = vertices[triangle[0]];
            Vector3 v1 = vertices[triangle[1]];
            Vector3 v2 = vertices[triangle[2]];

            // Центр треугольника
            Vector3 center = (v0 + v1 + v2) / 3.0f;

            // Ориентация треугольника
            Quaternion rotation = Quaternion.LookRotation((v1 - v0).normalized, Vector3.Cross(v1 - v0, v2 - v0).normalized);

            // Создание и настройка префаба
            GameObject prism = Instantiate(prismPrefab, center, rotation);
            // Применяем уменьшенный масштаб для префаба
            prism.transform.localScale = new Vector3(prismScaleFactor, prismScaleFactor, prismScaleFactor);
        }
    }
}
