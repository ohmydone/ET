using System.Collections.Generic;

namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgSkill :Entity,IAwake,IUILogic
	{
		public DlgSkillViewComponent View { get => this.Parent.GetComponent<DlgSkillViewComponent>();}

		public Dictionary<int, Scroll_ItemSkil> ScrollItemSkils;
	}
}
