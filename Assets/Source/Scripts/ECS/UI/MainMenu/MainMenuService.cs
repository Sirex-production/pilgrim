using System;
using Ingame.UI.MainMenu;
using UnityEngine;

public sealed class MainMenuService : MonoBehaviour
{
	public event Action<UiWindowType> OnWindowChangeRequested;


	public void RequestWindowChange(UiWindowType uiWindowType)
	{
		OnWindowChangeRequested?.Invoke(uiWindowType);
	}
}
