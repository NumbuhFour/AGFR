using UnityEngine;
using System.Collections;

public interface IButtonAction
{
	void OnEntityEnter(Entity e);
	void OnEntityExit(Entity e);
	void Update();
}

