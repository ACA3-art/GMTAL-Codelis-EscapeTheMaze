using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FOVTest : MonoBehaviour
{
    public Transform target;
    public float viewDistance = 25f;
    public LayerMask wallMask;
    [Range(4, 180)] public int rayCount = 64;

    private Mesh mesh;
    private MeshFilter mf;

    void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.name = "FOV_Mesh";
        mf.mesh = mesh;
    }

    void LateUpdate()
    {
        if(transform != null)
        {
            transform.position = target.position;
        }
        DrawFOV();
    }

    void DrawFOV()
    {
        Vector3 origin = transform.position;
        // Açıyı saat yönünün tersine döndürmek için 360'tan başlıyoruz veya adım yönünü değiştiriyoruz
        float angleStep = 360f / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero; // Merkez

        for (int i = 0; i <= rayCount; i++)
        {
            // Açı hesaplama: 2D düzlemde düzgün çizim için
            float angle = i * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

            RaycastHit2D hit = Physics2D.Raycast(origin, dir, viewDistance, wallMask);

            if (hit.collider != null)
            {
                // Dünyadaki çarpma noktasını Local koordinata çeviriyoruz
                vertices[i + 1] = transform.InverseTransformPoint(hit.point);
            }
            else
            {
                vertices[i + 1] = dir * viewDistance;
            }
        }

        // ÜÇGEN DİZİLİMİ: Stencil'in 'görmesi' için saat yönü tersi sıra
        for (int i = 0; i < rayCount; i++)
        {
            triangles[i * 3] = 0;             // Merkez
            triangles[i * 3 + 1] = i + 2;     // İleri nokta
            triangles[i * 3 + 2] = i + 1;     // Geri nokta (Bu sıra 'saat yönü tersi'dir)
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Önemli: Stencil görünmez olduğu için bounds'u geniş tutalım ki kamera hep görsün
        mesh.RecalculateBounds();
    }
}