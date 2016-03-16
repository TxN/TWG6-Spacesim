using UnityEngine;
using UnityEditor;
using System.Collections;

/**
 *	WFX_ParticleMeshBillboard Editor
 *	
 *	Same effect as WFX_ParticleMeshBillboard Script but in the Editor.
 *	
 *	(c) 2012, Jean Moreno
**/

[CustomEditor(typeof(WFX_ParticleMeshBillboard))]
public class WFX_ParticleMeshBillboardEditor : Editor
{
	public override void OnInspectorGUI()
	{
		EditorGUILayout.HelpBox("This script makes the meshes of a ParticleSystem act as billboard sprites, so that we can use other meshes than quads to render the particles. Useful to reduce overdraw on mobile platforms!", MessageType.Info);
		
		GUILayout.Space(4);
		
		EditorGUILayout.HelpBox("The effects will look right only in runtime mode!\nTo have a correct preview in the editor, please look at their Desktop version!", MessageType.Warning);
	}
	
	/*
	private Mesh editorMesh;
	private Mesh objectMesh;
	
	private GameObject _pmb = null;
	private GameObject pmb
	{
		get
		{
			if(_pmb == null && this.target != null)
				_pmb = (this.target as WFX_ParticleMeshBillboard).gameObject;
			
			if(this.target == null)
				return null;
			
			return _pmb;
		}
	}
	
	
	void OnEnable()
	{
		if(!EditorApplication.isPlaying)
		{
			if(pmb.GetComponent<ParticleSystemRenderer>().mesh != null)
			{
				InitiateMesh();
			}
		}
		
		EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
	}
	
	void OnDisable()
	{
		RestoreMesh();
	}
	
	private void RestoreMesh()
	{
		//Restore initial Mesh
		if(objectMesh != null && pmb != null)
		{
			pmb.GetComponent<ParticleSystemRenderer>().mesh = objectMesh;
		}
		
		DestroyEditorMesh();
	}
	
	private void DestroyEditorMesh()
	{
		if(editorMesh != null)
			Object.DestroyImmediate(editorMesh);
	}
	
	private void InitiateMesh()
	{
		//Get Ref to initial Mesh
		objectMesh = pmb.GetComponent<ParticleSystemRenderer>().mesh;
		
		//Clone current Mesh
		if(editorMesh == null && pmb != null)
			editorMesh = (Mesh)Instantiate(pmb.GetComponent<ParticleSystemRenderer>().mesh);
		
		pmb.GetComponent<ParticleSystemRenderer>().mesh = editorMesh;
		
		//Initiates Vertices arrays
		vertices = new Vector3[editorMesh.vertices.Length];
		for(int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = editorMesh.vertices[i];
		}
		rvertices = new Vector3[vertices.Length];
	}
	
	private Vector3[] vertices;
	private Vector3[] rvertices;
	
	void OnSceneGUI ()
	{
		if(!EditorApplication.isPlaying && editorMesh != null)
		{
			//Detect if Mesh has been changed
			if(objectMesh != null && pmb.GetComponent<ParticleSystemRenderer>().mesh != editorMesh)
			{
				DestroyEditorMesh();
				InitiateMesh();
			}
			
			//Rotate editor Mesh
			Quaternion angle = Quaternion.LookRotation(Camera.current.transform.forward, Camera.current.transform.up);
			Quaternion rotation = Quaternion.Inverse(pmb.transform.rotation);
			
			for(int i = 0; i < rvertices.Length; i++)
			{
				rvertices[i] = angle * vertices[i];
				rvertices[i] = rotation * rvertices[i];
			}
			
			editorMesh.vertices = rvertices;
		}
	}
	
	private void PlaymodeStateChanged()
	{
		if(!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
			RestoreMesh();
	}
	*/
}
