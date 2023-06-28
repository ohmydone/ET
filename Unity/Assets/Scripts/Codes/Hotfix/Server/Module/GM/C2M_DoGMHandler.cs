using ET.EventType;
using ET.Server;

namespace ET
{
    [MessageHandler(SceneType.Gate)]
    public class C2M_DoGMHandler: AMRpcHandler<C2M_GM, M2C_GM>
    {
        protected override async ETTask Run(Session session, C2M_GM request, M2C_GM response)
        {
            await Root.Instance.Scene.GetComponent<ConsoleComponent>().RunGM(request.GM);
            response.Error = ErrorCode.ERR_Success;
        }
    }
}