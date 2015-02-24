using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[AddComponentMenu("Scripts/UI/Chat Manager")]
public class ChatManager : MonoBehaviour {
	public enum PauseMode { NO_PAUSE, PAUSE_GAME, STOP_AFTER_FINISH }; //Stop_after requires pause_game

	public Text[] boxes;
	public int typeDelay = 10;
	public int maxLength = 35;
	
	private string lastMsg = "";
	private string[] messages = {"",""};
	private string typingMsg = "";
	private int typingIndex = 0;
	
	private PauseMode mode = PauseMode.NO_PAUSE;
	private bool isWaiting = false;
	private MonoBehaviour finishCall = null;
	
	private long lastTime;
	public bool pauseReleased = true;
	// Use this for initialization
	void Start () {
		lastTime = (long)(GameTime.chatTime*1000);
		UpdateTextBoxes();
		foreach(Text t in boxes){
			t.gameObject.GetComponent<FlashText>().enabled = false;
		}
	}
	
	private void CheckKeys(){
		float pause = Input.GetAxisRaw("Pause");
		if(pause > 0 && pauseReleased){
			Game.Paused = !Game.Paused;
			Game.ChatPause = Game.Paused;
			pauseReleased = false;
		}
		
		if(pause <= 0) pauseReleased = true;
	}
	
	// Update is called once per frame
	void Update () {
		long time = (long)(GameTime.chatTime*1000);
		if(time - lastTime >= typeDelay){
			lastTime = time;
			if(typingMsg != "")
				AddNextLetter();
		}
		
		if(isWaiting && Input.GetAxis("Use") > 0){
			isWaiting = false;
			this.mode = PauseMode.NO_PAUSE;
			Game.Paused = false;
			foreach(Text t in boxes){
				t.gameObject.GetComponent<FlashText>().Stop ();
			}
			if(finishCall) 
				finishCall.SendMessage("OnFinishText");
		}
	}
	
	public void PushText(string head, string msg, PauseMode mode=PauseMode.NO_PAUSE, MonoBehaviour finishCall=null){
		this.finishCall = finishCall;
		this.mode = mode;
		if(this.mode != PauseMode.NO_PAUSE) Game.NoMenuPauseGame();
		foreach(Text t in boxes){
			t.gameObject.GetComponent<FlashText>().Stop ();
		}
		
		while (typingIndex < typingMsg.Length-1){ //Finish typing
			AddNextLetter();
		}
		messages[1] = messages[0];
		messages[0] = head;
		
		lastMsg = head + msg;
		typingMsg = msg;
		typingIndex = 0;
		lastTime = (long)(GameTime.time*1000);
	}
	
	private void AddNextLetter(){
		string add = "" + typingMsg[typingIndex];
		typingIndex ++;
		
		/*if(add == "<"){
			while(typingMsg[typingIndex] != '>'){
				add += "" + typingMsg[typingIndex];
				typingIndex++;
			}
			if(typingIndex < typingMsg.Length -1){ // Add one more for next visible letter
				add += "" + typingMsg[typingIndex];
				typingIndex++;
			}
		}*/
		messages[0] += add;
		if(typingIndex >= maxLength && typingIndex != typingMsg.Length-1){
			messages[1] = messages[0];
			messages[0] = "";
			typingMsg = typingMsg.Substring(typingIndex);
			typingIndex = 0;
		}
		if(typingIndex > typingMsg.Length -1){
			typingMsg = "";
			OnFinishText();
		}
			
		UpdateTextBoxes();
	}
	
	private void OnFinishText(){
		if(mode == PauseMode.STOP_AFTER_FINISH) {
			isWaiting = true;
			boxes[0].gameObject.GetComponent<FlashText>().enabled = true;
		}else if(finishCall) 
			finishCall.SendMessage("OnFinishText");
	}
	
	private void UpdateTextBoxes(){
		for(int i = 0; i < boxes.Length && i < messages.Length; i++){
			boxes[i].text = messages[i];
		}
	}
}
