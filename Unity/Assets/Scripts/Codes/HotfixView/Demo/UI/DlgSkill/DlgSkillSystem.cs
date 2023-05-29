using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[FriendOf(typeof(DlgSkill))]
	public static  class DlgSkillSystem
	{

		public static void RegisterUIEvent(this DlgSkill self)
		{
		 
		}

		public static void ShowWindow(this DlgSkill self, Entity contextData = null)
		{
			self.View.ELoopScrollList_SkillLoopHorizontalScrollRect.totalCount = 10;
			self.View.ELoopScrollList_SkillLoopHorizontalScrollRect.RefillCells();
		}

		 

	}
}
