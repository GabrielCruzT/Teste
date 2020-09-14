1.OrbbecTrackingManager.Init();  用于初始化标定界面
2.OrbbecTrackingManager.instance.playerMode;用于设置单双人模式
3.OrbbecTrackingManager.instance.user1;用于获取玩家1的Body骨架数据
4.OrbbecTrackingManager.instance.user2;用于获取玩家2的Body骨架数据 当playermode  == two才有效
5.OrbbecTrackingManager.instance.isActive; 当此值为true 的时候  user1、user2才有效