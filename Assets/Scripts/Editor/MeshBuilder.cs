using UnityEngine;
using UnityEditor;

public static class MeshBuilder
{
	[MenuItem("CONTEXT/MeshHandler/Generate Mesh...")]
	public static void SaveMeshInPlace(MenuCommand menuCommand)
	{
		MeshHandler meshHandler = menuCommand.context as MeshHandler;
	}

}
