
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgInfoViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.LoopVerticalScrollRect ELSV_NumsLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELSV_NumsLoopVerticalScrollRect == null )
     			{
		    		this.m_ELSV_NumsLoopVerticalScrollRect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopVerticalScrollRect>(this.uiTransform.gameObject,"Panel/Image/ELSV_Nums");
     			}
     			return this.m_ELSV_NumsLoopVerticalScrollRect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_ELSV_NumsLoopVerticalScrollRect = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.LoopVerticalScrollRect m_ELSV_NumsLoopVerticalScrollRect = null;
		public Transform uiTransform = null;
	}
}
