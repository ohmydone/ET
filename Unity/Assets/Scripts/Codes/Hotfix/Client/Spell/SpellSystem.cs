using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    public class SpellSystem :  AwakeSystem<Spell, int>
    {
        protected override void Awake(Spell self, int configId)
        {
            self.ConfigId = configId;
        }
    }
}
