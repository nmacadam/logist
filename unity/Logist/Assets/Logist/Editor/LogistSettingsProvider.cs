using UnityEditor;
using UnityEngine;

namespace LogistInternal
{
	/// <summary>
	/// 
	/// </summary>
	public static class LogistSettingsProvider
	{
		private static GUIContent folderIcon = EditorGUIUtility.IconContent("Folder Icon");

		[SettingsProvider]
		public static SettingsProvider CreateAssetImportPipelineSettingsProvider()
		{
			var provider = new SettingsProvider(LogistSettings.Path, SettingsScope.Project)
			{
				label = "Loggerhead Settings",
				guiHandler = (searchContext) =>
				{
					var settings = LogistSettings.GetSerializedSettings();

					var enabled = settings.FindProperty("_enabled");
					var formatOutput = settings.FindProperty("_formatOutput");
					var handleUnityLog = settings.FindProperty("_handleUnityLog");
					var savePathType = settings.FindProperty("_savePathType");
					var writePath = settings.FindProperty("_writePath");

					EditorGUI.BeginChangeCheck();

					if (string.IsNullOrEmpty(writePath.stringValue))
					{
						writePath.stringValue = Application.persistentDataPath + "/Logs/";
					}

					enabled.boolValue = EditorGUILayout.Toggle(new GUIContent("Enable", "Should logs be written?"), enabled.boolValue);
					formatOutput.boolValue = EditorGUILayout.Toggle(new GUIContent("Format output file", "Should the output file be indented or minified?"), formatOutput.boolValue);
					handleUnityLog.intValue = (int)(LogistSettings.HandleUnityLog)EditorGUILayout.EnumPopup("Handle Unity Logs By", (LogistSettings.HandleUnityLog)handleUnityLog.intValue);
					
					GUI.enabled = false;
					savePathType.intValue = (int)(LogistSettings.SavePathType)EditorGUILayout.EnumPopup("Save to", (LogistSettings.SavePathType)savePathType.intValue);
					GUI.enabled = true;

					GUILayout.BeginHorizontal();
					// if ((LoggerheadSettings.SavePathType)(savePathType.intValue) == LoggerheadSettings.SavePathType.Project)
					// {
					// 	EditorGUILayout.TextField(new GUIContent("Save folder", "The folder to write logs to"), ToLocalPath(writePath.stringValue));
					// 	if (GUILayout.Button(folderIcon, GUILayout.Width(20), GUILayout.Height(18))) 
					// 	{
					// 		writePath.stringValue = ToLocalPath(EditorUtility.OpenFolderPanel("Log Folder", "", ""));
					// 	}
					// }
					if ((LogistSettings.SavePathType)(savePathType.intValue) == LogistSettings.SavePathType.Persistent)
					{
						GUI.enabled = false;
						EditorGUILayout.TextField(new GUIContent("Save folder", "The folder to write logs to"), LogistSettings.GetPersistentSavePath());
						GUI.enabled = true;
						if (GUILayout.Button("Open", GUILayout.Width(50), GUILayout.Height(18))) 
						{
							EditorUtility.RevealInFinder(LogistSettings.GetPersistentSavePath());
						}
					}
					// if ((LoggerheadSettings.SavePathType)(savePathType.intValue) == LoggerheadSettings.SavePathType.Custom)
					// {
					// 	writePath.stringValue = EditorGUILayout.TextField(new GUIContent("Save folder", "The folder to write logs to"), writePath.stringValue);
					// 	if (GUILayout.Button(folderIcon, GUILayout.Width(20), GUILayout.Height(18)))
					// 	{
					// 		writePath.stringValue = EditorUtility.OpenFolderPanel("Log Folder", "", "");
					// 	}
					// }
					GUILayout.EndHorizontal();
					
					

					if (EditorGUI.EndChangeCheck() == true)
					{
						settings.ApplyModifiedProperties();
					}
				}
			};
			return provider;
		}

		public static string ToLocalPath(string absolutePath)
		{
			return "Assets" + absolutePath.Substring(Application.dataPath.Length);
		}
	}
}