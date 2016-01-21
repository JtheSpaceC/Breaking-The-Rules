using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CursorLockModeWeb : Button{

	public override void OnPointerDown (UnityEngine.EventSystems.PointerEventData eventData)
	{
		base.OnPointerDown (eventData);
		Cursor.lockState = CursorLockMode.Confined;
	}
}
