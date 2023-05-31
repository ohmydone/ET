namespace ET
{
	
	[ComponentOf(typeof(Scene))]
	public class UnitComponent: Entity, IAwake, IDestroy
	{
		public Unit My { get; set; }

	}
}