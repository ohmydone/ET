
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[EnableMethod]
	public  class Scroll_ItemSkill : Entity,IAwake,IDestroy,IUIScrollItem 
	{
		public long DataId {get;set;}
		private bool isCacheNode = false;
		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public Scroll_ItemSkill BindTrans(Transform trans)
		{
			this.uiTransform = trans;
			return this;
		}

	

		public UnityEngine.UI.Text ELabel_IndexText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_ELabel_IndexText == null )
     				{
		    			this.m_ELabel_IndexText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"ELabel_Index");
     				}
     				return this.m_ELabel_IndexText;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"ELabel_Index");
     			}
     		}
     	}

		public void DestroyWidget()
		{

			this.m_ELabel_IndexText = null;
			this.uiTransform = null;
			this.DataId = 0;
		}
		
		private UnityEngine.UI.Text m_ELabel_IndexText = null;
		public Transform uiTransform = null;
	}
}
