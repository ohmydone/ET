
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgLobbyViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Button E_EnterMapButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_EnterMapButton == null )
     			{
		    		this.m_E_EnterMapButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Panel/E_EnterMap");
     			}
     			return this.m_E_EnterMapButton;
     		}
     	}

		public UnityEngine.UI.Image E_EnterMapImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_EnterMapImage == null )
     			{
		    		this.m_E_EnterMapImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Panel/E_EnterMap");
     			}
     			return this.m_E_EnterMapImage;
     		}
     	}

		public UnityEngine.UI.Button E_ThingsButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ThingsButton == null )
     			{
		    		this.m_E_ThingsButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Panel/E_Things");
     			}
     			return this.m_E_ThingsButton;
     		}
     	}

		public UnityEngine.UI.Image E_ThingsImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ThingsImage == null )
     			{
		    		this.m_E_ThingsImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Panel/E_Things");
     			}
     			return this.m_E_ThingsImage;
     		}
     	}

		public UnityEngine.UI.Button E_InfoButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_InfoButton == null )
     			{
		    		this.m_E_InfoButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Panel/E_Info");
     			}
     			return this.m_E_InfoButton;
     		}
     	}

		public UnityEngine.UI.Image E_InfoImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_InfoImage == null )
     			{
		    		this.m_E_InfoImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Panel/E_Info");
     			}
     			return this.m_E_InfoImage;
     		}
     	}

		public UnityEngine.UI.Button E_SettingButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_SettingButton == null )
     			{
		    		this.m_E_SettingButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Panel/E_Setting");
     			}
     			return this.m_E_SettingButton;
     		}
     	}

		public UnityEngine.UI.Image E_SettingImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_SettingImage == null )
     			{
		    		this.m_E_SettingImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Panel/E_Setting");
     			}
     			return this.m_E_SettingImage;
     		}
     	}

		public UnityEngine.UI.Button E_ExitButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ExitButton == null )
     			{
		    		this.m_E_ExitButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Panel/E_Exit");
     			}
     			return this.m_E_ExitButton;
     		}
     	}

		public UnityEngine.UI.Image E_ExitImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ExitImage == null )
     			{
		    		this.m_E_ExitImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Panel/E_Exit");
     			}
     			return this.m_E_ExitImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_EnterMapButton = null;
			this.m_E_EnterMapImage = null;
			this.m_E_ThingsButton = null;
			this.m_E_ThingsImage = null;
			this.m_E_InfoButton = null;
			this.m_E_InfoImage = null;
			this.m_E_SettingButton = null;
			this.m_E_SettingImage = null;
			this.m_E_ExitButton = null;
			this.m_E_ExitImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_E_EnterMapButton = null;
		private UnityEngine.UI.Image m_E_EnterMapImage = null;
		private UnityEngine.UI.Button m_E_ThingsButton = null;
		private UnityEngine.UI.Image m_E_ThingsImage = null;
		private UnityEngine.UI.Button m_E_InfoButton = null;
		private UnityEngine.UI.Image m_E_InfoImage = null;
		private UnityEngine.UI.Button m_E_SettingButton = null;
		private UnityEngine.UI.Image m_E_SettingImage = null;
		private UnityEngine.UI.Button m_E_ExitButton = null;
		private UnityEngine.UI.Image m_E_ExitImage = null;
		public Transform uiTransform = null;
	}
}
