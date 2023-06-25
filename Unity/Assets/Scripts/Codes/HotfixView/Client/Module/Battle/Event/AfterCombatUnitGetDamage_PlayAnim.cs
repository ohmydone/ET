using ET.Client;

namespace ET
{
    [Event(SceneType.Client)]
    public class AfterCombatUnitGetDamage_PlayAnim:AEvent<Scene,EventType.AfterCombatUnitGetDamage>
    {
        protected override async ETTask Run(Scene scene, EventType.AfterCombatUnitGetDamage a)
        {
            var anim = a.Unit.unit.GetComponent<AnimationComponent>();
            if (anim != null)
            {
                if(a.Unit.unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp)<=0)
                {
                    anim.Play(AnimClipType.Died);
                }
                else
                    anim.Play(AnimClipType.Damage);
            }
            else if(a.Unit.unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp)<=0)//直接死了
            {
                a.Unit.unit.Dispose();
            }

            await ETTask.CompletedTask;
        }
        
    }
}