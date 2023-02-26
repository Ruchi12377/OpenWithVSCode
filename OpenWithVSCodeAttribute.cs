#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Ruchi.Editor
{
	public class OpenWithVSCodeAttribute : PropertyAttribute
	{
	}

	[CustomPropertyDrawer(typeof(OpenWithVSCodeAttribute))]
	public class OpenWithVSCodeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(rect, property, label);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			var height = EditorGUI.GetPropertyHeight(property, label);
			var buttonPosition = rect;
			buttonPosition.position += new Vector2(0, height);
			if (GUI.Button(buttonPosition, "Open with VSCode ↑"))
			{
				var textAsset = property.objectReferenceValue as TextAsset;

				if (textAsset == null) return;

				var path = AssetDatabase.GetAssetPath(textAsset);
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
				var absolutePath = path.Replace("Assets", Application.dataPath).Replace("/", "\\");
#else
				var absolutePath = path.Replace("Assets", Application.dataPath);
#endif

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
				var app = new ProcessStartInfo
				{
					FileName = "code",
					Arguments = path
				};
#else
				var app = new ProcessStartInfo
				{
					FileName = "open",
					Arguments = $"-a \"Visual Studio Code\" --args \"{absolutePath}\""
				};
#endif

				Process.Start(app);
			}
		}
	}
}
#endif
