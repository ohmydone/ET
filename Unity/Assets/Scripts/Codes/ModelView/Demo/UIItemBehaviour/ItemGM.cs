
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[EnableMethod]
	public  class Scroll_ItemGM : Entity,IAwake,IDestroy,IUIScrollItem 
	{
		public long DataId {get;set;}
		private bool isCacheNode = false;
		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public Scroll_ItemGM BindTrans(Transform trans)
		{
			this.uiTransform = trans;
			return this;
		}

		public UnityEngine.UI.Button EBtn_ChooseButton
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
     				if( this.m_EBtn_ChooseButton == null )
     				{
		    			this.m_EBtn_ChooseButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EBtn_Choose");
     				}
     				return this.m_EBtn_ChooseButton;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EBtn_Choose");
     			}
     		}
     	}

		public UnityEngine.UI.Image EBtn_ChooseImage
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
     				if( this.m_EBtn_ChooseImage == null )
     				{
		    			this.m_EBtn_ChooseImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EBtn_Choose");
     				}
     				return this.m_EBtn_ChooseImage;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EBtn_Choose");
     			}
     		}
     	}

		public UnityEngine.UI.Text ELab_NameText
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
     				if( this.m_ELab_NameText == null )
     				{
		    			this.m_ELab_NameText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"EBtn_Choose/ELab_Name");
     				}
     				return this.m_ELab_NameText;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"EBtn_Choose/ELab_Name");
     			}
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EBtn_ChooseButton = null;
			this.m_EBtn_ChooseImage = null;
			this.m_ELab_NameText = null;
			this.uiTransform = null;
			this.DataId = 0;
		}

		private UnityEngine.UI.Button m_EBtn_ChooseButton = null;
		private UnityEngine.UI.Image m_EBtn_ChooseImage = null;
		private UnityEngine.UI.Text m_ELab_NameText = null;
		public Transform uiTransform = null;
	}
}
