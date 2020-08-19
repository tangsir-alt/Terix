using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Bussiness
{
    /// <summary>
    /// 俄罗斯方块元素
    /// </summary>
    public class TetrisElement
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="style"></param>
        public TetrisElement(TetrisStyle style) {
            this.style = style;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="style">形状</param>
        /// <param name="content">内容</param>
        /// <param name="location">位置</param>
        public TetrisElement(TetrisStyle style, Point[] content, Point location)
        {
            this.style = style;
            this.content = content;
            this.location = location;
        }

        /// <summary>
        /// 元素字母类型
        /// </summary>
        public TetrisStyle style { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public Point[] content { get; set; }

        /// <summary>
        /// 元素位置
        /// </summary>
        public Point location { get; set; }



        /// <summary>
        /// 位置改变
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void move(int x, int y)
        {
            this.location = new Point(x, y);
        }

        /// <summary>
        /// 获取x轴最小值
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public int getMinX(TetrisStyle style)
        {
            int x = 0;
            return x;
        }

        /// <summary>
        /// 获取x轴最大值
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public int getMaxX(TetrisStyle style)
        {
            int x = 10;
            switch (style)
            {
                case TetrisStyle.I:
                    //I形状
                    x = 10;
                    break;
                case TetrisStyle.J:  //J形状
                case TetrisStyle.L:  //L形状
                case TetrisStyle.O:  //O形状
                    x = 9;
                    break;
                case TetrisStyle.S:  //S形状
                case TetrisStyle.T:  //T形状
                case TetrisStyle.Z:  //Z形状
                    x = 8;
                    break;
                default:
                    //默认I
                    x = 10;
                    break;
            }
            x = x - 1;
            return x;
        }

        /// <summary>
        /// 获取y轴最小值
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public int getMinY(TetrisStyle style)
        {
            int y = 0;
            switch (style)
            {
                case TetrisStyle.I:
                    //I形状
                    y = -3;
                    break;
                case TetrisStyle.J://J形状
                case TetrisStyle.L://L形状
                    y = -2;
                    break;
                case TetrisStyle.O: //O形状
                case TetrisStyle.S: //S形状
                case TetrisStyle.T: //T形状
                case TetrisStyle.Z://Z形状
                    y = -1;
                    break;
                default: //默认I
                    y = -3;
                    break;
            }
            return y;
        }

        /// <summary>
        /// 获取y轴最大值
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public int getMaxY(TetrisStyle style)
        {
            int y = 20;
            switch (style)
            {
                case TetrisStyle.I:
                    //I形状
                    y = 16;
                    break;
                case TetrisStyle.J://J形状
                case TetrisStyle.L://L形状
                    y = 17;
                    break;
                case TetrisStyle.O: //O形状
                case TetrisStyle.S: //S形状
                case TetrisStyle.T: //T形状
                case TetrisStyle.Z://Z形状
                    y = 18;
                    break;
                default: //默认I
                    y = 16;
                    break;
            }
            y = y - 1;
            return y;
        }

        public Point[] getContent(TetrisStyle style)
        {
            //内容由四个点组成，顺序：先上后下，先左后右
            Point[] content = new Point[4];
            switch (style)
            {
                case TetrisStyle.I:
                    //I形状
                    content[0] = new Point(0, 0);
                    content[1] = new Point(0, 1);
                    content[2] = new Point(0, 2);
                    content[3] = new Point(0, 3);
                    break;
                case TetrisStyle.J:
                    //J形状
                    content[0] = new Point(1, 0);
                    content[1] = new Point(1, 1);
                    content[2] = new Point(1, 2);
                    content[3] = new Point(0, 2);
                    break;
                case TetrisStyle.L:
                    //L形状
                    content[0] = new Point(0, 0);
                    content[1] = new Point(0, 1);
                    content[2] = new Point(0, 2);
                    content[3] = new Point(1, 2);
                    break;
                case TetrisStyle.O:
                    //O形状
                    content[0] = new Point(0, 0);
                    content[1] = new Point(1, 0);
                    content[2] = new Point(0, 1);
                    content[3] = new Point(1, 1);
                    break;
                case TetrisStyle.S:
                    //S形状
                    content[0] = new Point(2, 0);
                    content[1] = new Point(1, 0);
                    content[2] = new Point(1, 1);
                    content[3] = new Point(0, 1);
                    break;
                case TetrisStyle.T:
                    //T形状
                    content[0] = new Point(0, 0);
                    content[1] = new Point(1, 0);
                    content[2] = new Point(2, 0);
                    content[3] = new Point(1, 1);
                    break;
                case TetrisStyle.Z:
                    //Z形状
                    content[0] = new Point(0, 0);
                    content[1] = new Point(1, 0);
                    content[2] = new Point(1, 1);
                    content[3] = new Point(2, 1);
                    break;
                default:
                    //默认I
                    content[0] = new Point(0, 0);
                    content[1] = new Point(0, 1);
                    content[2] = new Point(0, 2);
                    content[3] = new Point(0, 3);
                    break;
            }
            return content;
        }
    }
}
