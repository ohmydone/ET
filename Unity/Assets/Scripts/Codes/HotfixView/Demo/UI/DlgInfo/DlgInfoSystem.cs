using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[InputSystem((int)KeyCode.Tab,InputType.KeyDown)]
	public class KeyDown_Tab : InputSystem<UIComponent>
	{
		public override void Run(UIComponent self, int key, int type, ref bool stop)
		{
			if (self.IsWindowVisible(WindowID.WindowID_Info))
			{
				self.HideWindow<DlgInfo>();
			}
			else
			{
				self.ShowWindow<DlgInfo>();
			}
		}
	}
	
	[FriendOf(typeof(DlgInfo))]
	public static  class DlgInfoSystem
	{

		public static void RegisterUIEvent(this DlgInfo self)
		{
		 
		}

		public static void ShowWindow(this DlgInfo self, Entity contextData = null)
		{
		}

		 

	}
}
