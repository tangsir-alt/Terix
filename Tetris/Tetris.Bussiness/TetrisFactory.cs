using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Bussiness
{
    /// <summary>
    /// 俄罗斯方块工厂
    /// </summary>
     public class TetrisFactory
    {
        /// <summary>
        /// 静态函数，生成Tetris元素对象
        /// </summary>
        /// <returns></returns>
        public static TetrisElement generate()
        {
            Random r = new Random(0);
            //随机生成形状
            int tstyle = getRandom();
            tstyle = tstyle % 7;
            TetrisStyle style = TetrisStyle.I;
            style = (TetrisStyle)Enum.Parse(typeof(TetrisStyle), tstyle.ToString());
            //随机生成起始坐标
            int x = getRandom();
            x = x % 10;
            int y = 0;
            //根据形状生成位置信息
            TetrisElement element = new TetrisElement(style);
            //内容由四个点组成，顺序：先上后下，先左后右
            Point[] content = element.getContent(style);
            //获取最小坐标和最大坐标，防止越界
            int minX = element.getMinX(style);
            int minY = element.getMinY(style);
            int maxX = element.getMaxX(style);
            int maxY = element.getMaxY(style);
            //修正起始坐标
            x = (x <= minX) ? minX : x;
            x = (x >= maxX) ? maxX : x;
            y = minY;
            Point location = new Point(x, y);
            element.location = location;
            element.content = content;
            return element;
        }

        /// <summary>
        /// 移动元素
        /// </summary>
        /// <param name="container"></param>
        /// <param name="element"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static TetrisElement move(TetrisContainer container,TetrisElement element,TetrisDirection direction) {

            return container.change(element,direction);
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <returns></returns>
        public static int getRandom() {
            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return r.Next(0,100);
        }
    }
}
