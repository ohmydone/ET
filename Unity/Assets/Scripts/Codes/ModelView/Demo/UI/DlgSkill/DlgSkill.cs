namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgSkill :Entity,IAwake,IUILogic
	{

		public DlgSkillViewComponent View { get => this.Parent.GetComponent<DlgSkillViewComponent>();} 

		 

	}
}
