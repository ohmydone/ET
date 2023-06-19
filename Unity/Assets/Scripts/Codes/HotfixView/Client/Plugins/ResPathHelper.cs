using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    public static class ResPathHelper 
    {
        public static string GetUIDlgPath(string ui)
        {
            return "Assets/Bundles/UI/Dlg/"+ui;
        }
        public static string GetUIItemPath(string ui)
        {
            return "Assets/Bundles/UI/Item/"+ui;
        }
        public static string GetUIRedDotPath(string ui)
        {
            return "Assets/Bundles/UI/RedDot/"+ui;
        }
        public static string GetUnitPath(string unit)
        {
            return "Assets/Bundles/Unit/"+unit;
        }
        public static string GetScenePath(string scene)
        {
            return "Assets/Scenes/"+scene;
        }

        public static string GetInputPath(string asset)
        {
            return "Assets/Bundles/InputAction/" + asset;
        }
        
        public static string GetConfigPath(string config)
        {
            return "Assets/Bundles/Config/"+config;
        }
        
        public static string GetSpriteAltasPath(string sa)
        {
            return "Assets/SpriteAltas/"+sa;
        }
        
        public static string GetSpellPreviewPath(string sa)
        {
            return "Assets/Bundles/SkillPreview/Prefabs/"+sa;
        }
    }
}
