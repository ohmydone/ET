using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace ET.Client
{
	[ComponentOf(typeof(Scene))]
	public class OperaComponent: Entity, IAwake, IUpdate,IDestroy
    {
        public Vector3 ClickPoint;

	    public int mapMask;

	    public InputActionAsset InputActionAsset;

	    public InputAction Slot1;
	    public InputAction Slot2;
	    public InputAction ShowInfo;

    }
}
