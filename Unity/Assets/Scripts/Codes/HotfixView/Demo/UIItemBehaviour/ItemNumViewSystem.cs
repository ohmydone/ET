
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class Scroll_ItemNumDestroySystem : DestroySystem<Scroll_ItemNum> 
	{
		protected override void Destroy( Scroll_ItemNum self )
		{
			self.DestroyWidget();
		}
	}
}
