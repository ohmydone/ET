namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgInfo :Entity,IAwake,IUILogic
	{

		public DlgInfoViewComponent View { get => this.Parent.GetComponent<DlgInfoViewComponent>();} 

		 

	}
}
