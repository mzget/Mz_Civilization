using UnityEngine;
using System.Collections;

public struct Main {
	
	public const float GAMEWIDTH = 1024;
    public const float GAMEHEIGHT = 768;
	
	public const float FixedGameWidth = 1024f;
	public const float FixedGameHeight = 768f;

    public enum AppLanguage { defualt_En = 0, Thai, };
    public static AppLanguage CurrentAppLanguage = AppLanguage.defualt_En;
	
	public enum AppVersion { Free = 0, Pro = 1};
	public static AppVersion appVersion;
}
