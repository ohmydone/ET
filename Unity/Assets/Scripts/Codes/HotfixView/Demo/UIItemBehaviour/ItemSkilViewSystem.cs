
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class Scroll_ItemSkilDestroySystem : DestroySystem<Scroll_ItemSkil> 
	{
		protected override void Destroy( Scroll_ItemSkil self )
		{
			self.DestroyWidget();
		}
	}
}
