namespace ET
{
	
	[ComponentOf(typeof(Scene))]
	public class UnitComponent: Entity, IAwake, IDestroy
	{
		[StaticField]
		public static UnitComponent Instance;
		public Unit My { get; set; }

	}
}