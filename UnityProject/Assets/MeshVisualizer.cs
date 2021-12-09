using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Linq;
using SP;

[ExecuteInEditMode]
public class MeshVisualizer : MonoBehaviour
{
    public bool importMesh = false;
    public GameObject markerPrefab;
    public Material mat;
    XDocument root;
    // Update is called once per frame
    void Update()
    {
        if(importMesh) {
            importMesh = false;
            visualizeMesh();
        }
    }

    public void visualizeMesh() {
        root = getXMLDocument();
        var pos = getPos(root);
        var rot = getRot(root);
        var vertices = getVertices(root).ToArray();
        var faces = getFaces(root).ToArray();
        var uvs = GetBasicUvs(vertices);
        BuildMesh("meshVis", pos, rot.eulerAngles, vertices, faces, uvs);

        spawnMarkers(root);
    }

    private XDocument getXMLDocument() {
        string filepath = "../savedFile_224024.xml";
        return XDocument.Load(filepath);;
    }

    private void spawnMarkers(XDocument xmlToRead) {
        var markers = xmlToRead.Descendants("Marker");
        foreach(var marker in markers) {
            var posAttr = marker.Attribute("pos");
            var markerPos = StringToVector3(posAttr.Value);
            markerPos = new Vector3(markerPos.x, markerPos.y, -markerPos.z);
            var id = marker.Attribute("id");

            var go = Instantiate(markerPrefab);
            go.transform.parent = GlobalOrigin.getTransform();
            go.transform.localPosition = markerPos;
            go.name = id.Value;

            //var endPos = TransformConversions.posRelativeTo(GlobalOrigin.getTransform(), go.transform);
            //print(endPos);
            //go.transform.position = new Vector3(endPos.x, endPos.y, endPos.z) *-1;
        }
    }

    private Vector3 getPos(XDocument xmlToRead) {
        var pos = xmlToRead.Descendants("Mesh").Attributes("pos");
        foreach(var e in pos) {
            return StringToVector3(e.Value);
        }
        return Vector3.zero;
    }

    private Quaternion getRot(XDocument xmlToRead) {
        var rot = xmlToRead.Descendants("Mesh").Attributes("rot");
        foreach(var e in rot) {
            return Quaternion.Euler(StringToVector3(e.Value));
        }
        return new Quaternion();
    }

    private List<Vector3> getVertices(XDocument xmlToRead) {
        var vertices = xmlToRead.Descendants("Vertex").Attributes("position");
        var output = new List<Vector3>();
        foreach(var v in vertices) { 
            var entry = StringToVector3(v.Value);
            output.Add(StringToVector3(v.Value));
        }

        return output;
    }

    private List<int> getFaces(XDocument xmlToRead) {
        var vertices = xmlToRead.Descendants("Face").Attributes("vertices");
        var output = new List<int>();
        foreach(var v in vertices) { 
            var face = v.Value.Split();
            var entry1 = Int32.Parse(face[0]);
            var entry2 = Int32.Parse(face[1]);
            var entry3 = Int32.Parse(face[2]);
            output.Add(entry3);
            output.Add(entry2);
            output.Add(entry1);
        }
        return output;
    }

    public static Vector3 StringToVector3(string sVector)
     {
         // Remove the parentheses
         if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
             sVector = sVector.Substring(1, sVector.Length-2);
         }
 
         // split the items
         string[] sArray = null;
         // Split by comma
         if(sVector.Contains(",")){
            sArray = sVector.Split(',');
         }
         //SPlit by whitespace
         else {
            sArray = sVector.Split();
         }

         // store as a Vector3
         Vector3 result = new Vector3(
             float.Parse(sArray[0]),
             float.Parse(sArray[1]),
             float.Parse(sArray[2]));
 
         return result;
     }

    /// <summary>
    /// Build a mesh during runtime. Uses LEFT-hand coordinate system
    /// </summary>
    /// <param name="name">Name of the mesh</param>
    /// <param name="pivotPoint">Centre point of the mesh</param>
    /// <param name="orientation"></param>
    /// <param name="vertices"></param>
    /// <param name="faces"></param>
    /// <returns></returns>
    public GameObject BuildMesh(string name, Vector3 pivotPoint, Vector3 orientation, Vector3[] vertices, int[] faces, Vector2[] uvs){
        var output = new GameObject(name);
        var mesh = new Mesh();
        var outputMesh = output.AddComponent<MeshFilter>();
        var outputMeshRend = output.AddComponent<MeshRenderer>();

        output.transform.position = pivotPoint;
        output.transform.eulerAngles = orientation;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = faces;
        outputMesh.mesh = mesh;
        outputMeshRend.material = mat;

        mesh.uv = uvs;

        mesh.Optimize();
        mesh.RecalculateNormals();

        // Offset to globalOrigin
        print(GlobalOrigin.getTransform());
        var endPos = TransformConversions.posRelativeTo(GlobalOrigin.getTransform(), output.transform);
        output.transform.position = endPos;


        return output;
    }

    /// <summary>
    /// Returns an array of very basic automatically generated UVs from the given vertices.
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns></returns>
    public static Vector2[] GetBasicUvs(Vector3[] vertices) {
        var uvs = new Vector2[vertices.Length];
        // Generate UVs from the X and Y vertex values. Intended purely for visual debugging of edges
        for (int i = 1; i < uvs.Length; i++) {
            uvs[i - 1] = new Vector2(vertices[i - 1].x, vertices[i - 1].z);
        }
        return uvs;
    }
}
