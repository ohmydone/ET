using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ObjectSystem]
    public class SpellComponentAwakeSystem : AwakeSystem<SpellComponent>
    {
        protected override void Awake(SpellComponent self)
        {
            self.Init();
        }
    }
	
    [ObjectSystem]
    public class SpellComponentDestroySystem : DestroySystem<SpellComponent>
    {
        protected override void Destroy(SpellComponent self)
        {
        }
    }
	
    public static class SpellComponentSystem
    {
        public static void Init(this SpellComponent self)
        {
            foreach (var spellConfig in SpellConfigCategory.Instance.GetAll())
            {
                self.AddChild<Spell, int>(spellConfig.Key);
            }
        }

        public static Spell Get(this SpellComponent self, long id)
        {
            Spell spell = self.GetChild<Spell>(id);
            return spell;
        }

        public static void Remove(this SpellComponent self, long id)
        {
            Spell spell = self.GetChild<Spell>(id);
            spell?.Dispose();
        }
    }
    
}
