using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rusk
{
    //这是一个Cinemachine的学习记录类
    //https://zhuanlan.zhihu.com/p/516625841
    /// <summary>
    /// 学习cinemachine
    /// <summary>
    public class cinemaStudy
    {
        //1从gameobject面板中新建 Virtual Camera
        //Cinemachine Brain是UnityCamera上的一个组件，相当于"大脑"，可以在每一帧来监控并计算场景中所有活动的Virtual Cameras的状态（位置，方向等），同步选择其他具有相同优先级的Virtual Cameras中或切换具有更高优先级的Virtual Camera甚至进行Virtual Cameras之间的混合（切换）。Virtual Cameras的优先级可以通过手动提前配置或者编写游戏逻辑来进行操控。


        //在了解完Cinemachine Brain之后打开场景中的CM vcam1后你会看到虚拟相机下挂载的CinemachineVirtualCamera脚本，该Object其实是一个empty Transform GameObject




    }
}