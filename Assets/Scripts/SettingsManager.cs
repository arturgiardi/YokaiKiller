using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class SettingsJson
{
	public string shadowQaulity;
	public int[][] resolution;
	public bool dof;
	public string antiAliasing;

	public SettingsJson ()
	{
		int cpuTier = 0; // 0->10
		int gpuTier = 0; // 0->10
	}

}

public class SettingsManager : MonoBehaviour 
{
	public static SettingsJson currentSettings;
	string iniPath;


	void Start()
	{
		/*print(SystemInfo.processorFrequency);
		print(SystemInfo.processorCount);
		print(SystemInfo.processorType);

		print(SystemInfo.graphicsDeviceName);
		print(SystemInfo.graphicsDeviceType);
		print(SystemInfo.graphicsDeviceVendor);
		print(SystemInfo.graphicsDeviceVendorID);

		print(SystemInfo.graphicsShaderLevel);
		print(SystemInfo.supportsRenderTextures);
		print(SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32));
		print(SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB64));
		Debug.LogError("STOP");*/
	}

	void GetIni()
	{
		iniPath = Application.dataPath;
		System.IO.Directory.GetParent(iniPath);
		iniPath += "/config.ini";
		if(!File.Exists(iniPath))
		{
			Debug.LogWarning("ini file does not exist, creatign one!");
		}
	}

	void LoadSettings()
	{
		if(!File.Exists(iniPath))
		{

		}
	}
}
