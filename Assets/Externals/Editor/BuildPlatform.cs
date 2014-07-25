using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BuildPlatform
{
	static string m_sDDLLocation = Application.dataPath + "/../ClassLibrary/";
	
	static string m_sBuildPath =  Application.dataPath + "/../Builds/iFashion.exe";
	static string m_sAndroidBuildPath =  Application.dataPath + "/../Builds/iFashion.apk";
	
	static string[] m_Scenes = { "Assets/Scenes/Login.unity", "Assets/Scenes/Main.unity" };
	
	[MenuItem("Tools/Build StandAlone")]
	private static void BuildStandAlone()  {
		EditorUtilities.GenerateVersion();
		
		UseBuildDLLs();
		
		//build the file 
		BuildOptions Options = BuildOptions.None;
		BuildPipeline.BuildPlayer(m_Scenes, m_sBuildPath, BuildTarget.StandaloneWindows, Options);
		
		RestoreWorkDLLs();
	}

	[MenuItem("Tools/Build Android")]
	private static void BuildAndroid()  {
		EditorUtilities.GenerateVersion();
	
		UseBuildDLLs();
		
		//build the file 
		BuildOptions Options = BuildOptions.None;		
		PlayerSettings.Android.keystoreName = Application.dataPath + "/../fairylogic.keystore";
		PlayerSettings.Android.keystorePass = "123456";
		PlayerSettings.Android.keyaliasName = "fairylogic";
		PlayerSettings.Android.keystorePass = "123456";
				
		PlayerSettings.keystorePass = "123456";
		PlayerSettings.keyaliasPass = "123456";
		
		BuildPipeline.BuildPlayer(m_Scenes, m_sAndroidBuildPath, BuildTarget.Android, Options);
		
		RestoreWorkDLLs();
	}	
	
	[MenuItem("Tools/Switch Build DLLs")]
	private static void SwitchBuildDLLs()  {
		UseBuildDLLs();		
	}	
	
	[MenuItem("Tools/Switch Editor DLLs")]
	private static void SwitchEditorDLLs()  {
		RestoreWorkDLLs();
	}	
	
	private static void UseBuildDLLs()
	{
		System.IO.Directory.CreateDirectory(Application.dataPath + "/../Builds");
	
		// Use working DLLs
		File.Copy(m_sDDLLocation + "NGUI_Build.dll", Application.dataPath + "/Externals/NGUI/NGUI.dll", true);
		
		//re-import the database
		AssetDatabase.Refresh();
	}
	
	private static void RestoreWorkDLLs()
	{
		//rename back
		File.Copy(m_sDDLLocation + "NGUI_Work.dll", Application.dataPath + "/Externals/NGUI/NGUI.dll", true);
		
		//re re-import the database
		AssetDatabase.Refresh();   
	}
}