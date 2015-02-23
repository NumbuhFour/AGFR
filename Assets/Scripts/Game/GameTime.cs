using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour {
	
	private static float timeOffset = 0;
	private static float chatTimeOffset = 0;
	
	public static float deltaTime{ get { return (Game.Paused ? 0:Time.deltaTime); } }
	public static float time{ get { return Time.time - timeOffset; } }
	public static float chatTime{ get { return Time.time - chatTimeOffset; } }
	
	public static void AddPauseOffset(float offset){ timeOffset += offset; }
	public static void AddChatPauseOffset(float offset){ chatTimeOffset += offset; }
}
