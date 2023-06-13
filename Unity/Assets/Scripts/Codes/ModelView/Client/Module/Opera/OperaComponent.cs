using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ET.Client
{
	[ComponentOf(typeof(Scene))]
	public class OperaComponent: Entity, IAwake, IUpdate,IDestroy,IOpera
	{
		[StaticField]
		public static OperaComponent Instance;
	    
        public Vector3 ClickPoint;

	    public int mapMask;

	    public List<string> ActionForListen;

	    public InputActionAsset InputActionAsset;
	    public Dictionary<string,InputAction> Actions;
	}
}
