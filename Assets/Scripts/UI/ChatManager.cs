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
	private int typingCountOffset = 0; //Tags dont count to max length
	private string typingEndTag = ""; //For when <color> is started but hasnt finished yet
	
	private PauseMode mode = PauseMode.NO_PAUSE;
	private bool isWaiting = false;
	private MonoBehaviour finishCall = null;
	
	private bool wasUsePressed = false;
	
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
		
		bool usePressed = Input.GetAxis("Use") != 0;
		
		if (!wasUsePressed && usePressed){
			if(typingMsg != ""){
				while(typingMsg != "") //Finish typing
					AddNextLetter(); //Doing it this way so that no overflow is added by accident
					
			}else if(isWaiting){
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
	}
	
	public void PushText(string msg){
		this.PushText("", msg);
	}
	
	public void PushText(string head, string msg, PauseMode mode=PauseMode.NO_PAUSE, MonoBehaviour finishCall=null){
		wasUsePressed = Input.GetAxis("Use") > 0;
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
		
		if(add == "<"){
			string build = add;
			typingCountOffset++; //First <
			while(typingMsg[typingIndex] != '>'){
				build += "" + typingMsg[typingIndex];
				typingIndex++;
				typingCountOffset++;
			}
			build += '>'; //Account for last > missed in the whileloop
			typingIndex ++;
			typingCountOffset ++;
			
			/*if(typingIndex < typingMsg.Length -1){ // Add one more for next visible letter
				build += "" + typingMsg[typingIndex];
				typingIndex++;
				typingCountOffset++;
			}*/ //I don't remember why this is here anymore. The next letter should always be >
			
			if(build.Substring(0,2) == "</"){ //End tag
				typingEndTag = "";
				add = build;
			}else{
				string endBuild = build.Split('=')[0];
				typingEndTag = "</" + endBuild.Substring(1) + (endBuild.IndexOf('>') == -1 ? ">":"");// Building endtag
				add = build;
			}
		}
		messages[0] += add;
		if((typingIndex - typingCountOffset) >= maxLength && typingIndex != typingMsg.Length-1){
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
			if(i == 0) boxes[i].text += typingEndTag;
		}
	}
}
