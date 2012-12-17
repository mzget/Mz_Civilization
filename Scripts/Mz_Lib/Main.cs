using UnityEngine;
using System.Collections;

public struct Main {
		
	public const float FixedGameWidth = 1024f;
    public const float FixedGameHeight = 768f;
    public const float HD_HEIGHT = 720f;

    public enum AppLanguage { defualt_En = 0, Thai, };
    public static AppLanguage CurrentAppLanguage = AppLanguage.defualt_En;
	
	public enum AppVersion { Free = 0, Pro = 1};
	public static AppVersion appVersion;
}
