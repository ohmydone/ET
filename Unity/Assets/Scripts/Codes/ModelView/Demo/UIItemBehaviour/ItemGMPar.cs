
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[EnableMethod]
	public  class Scroll_ItemGMPar : Entity,IAwake,IDestroy,IUIScrollItem 
	{
		public long DataId {get;set;}
		private bool isCacheNode = false;
		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public Scroll_ItemGMPar BindTrans(Transform trans)
		{
			this.uiTransform = trans;
			return this;
		}

		public UnityEngine.UI.Image E_testImage
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
     				if( this.m_E_testImage == null )
     				{
		    			this.m_E_testImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_test");
     				}
     				return this.m_E_testImage;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_test");
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
		    			this.m_ELab_NameText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"ELab_Name");
     				}
     				return this.m_ELab_NameText;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"ELab_Name");
     			}
     		}
     	}

		public UnityEngine.UI.InputField EInp_ParInputField
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
     				if( this.m_EInp_ParInputField == null )
     				{
		    			this.m_EInp_ParInputField = UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"EInp_Par");
     				}
     				return this.m_EInp_ParInputField;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"EInp_Par");
     			}
     		}
     	}

		public UnityEngine.UI.Image EInp_ParImage
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
     				if( this.m_EInp_ParImage == null )
     				{
		    			this.m_EInp_ParImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EInp_Par");
     				}
     				return this.m_EInp_ParImage;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EInp_Par");
     			}
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_testImage = null;
			this.m_ELab_NameText = null;
			this.m_EInp_ParInputField = null;
			this.m_EInp_ParImage = null;
			this.uiTransform = null;
			this.DataId = 0;
		}

		private UnityEngine.UI.Image m_E_testImage = null;
		private UnityEngine.UI.Text m_ELab_NameText = null;
		private UnityEngine.UI.InputField m_EInp_ParInputField = null;
		private UnityEngine.UI.Image m_EInp_ParImage = null;
		public Transform uiTransform = null;
	}
}
