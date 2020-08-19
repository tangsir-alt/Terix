using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Bussiness
{
    /// <summary>
    /// 俄罗斯方块移动方向
    /// </summary>
    public enum TetrisDirection
    {
        UP = 0,//上，表示顺时针旋转
        DOWN = 1,//下，表示向下移动
        LEFT = 2,//左，表示往左移动
        RIGHT = 3, //表示向右移动
        DEFAULT=4 //默认动作
    }
}
