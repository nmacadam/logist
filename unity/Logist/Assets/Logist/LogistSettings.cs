using UnityEditor;
using UnityEngine;

namespace LogistInternal
{
	/// <summary>
	/// 
	/// </summary>
	public class LogistSettings : ScriptableObject
	{
		public enum HandleUnityLog {
			IgnoreAll,
			IgnoreLogs,
			UseAll
		}

		public enum SavePathType {
			Project,
			Persistent,
			Custom
		}

		private static string settingsPath = "Assets/Logist/logistSettings.asset";
		public static string Path => settingsPath;

		[SerializeField] private bool _enabled = true;
		[SerializeField] private bool _formatOutput = false;
		[SerializeField] private HandleUnityLog _handleUnityLog = HandleUnityLog.UseAll;
		[SerializeField] private SavePathType _savePathType = SavePathType.Persistent;
		[SerializeField] private string _writePath = "";

		public bool Enabled => _enabled;
		public bool FormatOutput => _formatOutput;
		public HandleUnityLog HandleUnityLogBy => _handleUnityLog;
		//internal string WritePath => _writePath;

		public static string GetPersistentSavePath() 
		{
			return Application.persistentDataPath + "/Logs/";
		}

		// internal static string GetProjectSavePath() 
		// {
		// 	return Application.dataPath + "/Logs/";
		// }

		// internal string GetCustomSavePath() 
		// {
		// 	if (_savePathType != SavePathType.Custom) 
		// 	{
		// 		Debug.LogError("Custom save path not enabled");
		// 		return default;
		// 	}
		// 	return _writePath;
		// }

		internal static LogistSettings GetOrCreateSettings()
		{
			var settings = AssetDatabase.LoadAssetAtPath<LogistSettings>(settingsPath);
			if (settings == null)
			{
				settings = ScriptableObject.CreateInstance<LogistSettings>();
				AssetDatabase.CreateAsset(settings, settingsPath);
				AssetDatabase.SaveAssets();
			}
			return settings;
		}

		public static SerializedObject GetSerializedSettings()
		{
			return new SerializedObject(GetOrCreateSettings());
		}
	}
}