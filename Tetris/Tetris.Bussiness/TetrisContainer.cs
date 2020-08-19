using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Bussiness
{
    /// <summary>
    /// 俄罗斯方块容器
    /// </summary>
    public class TetrisContainer
    {
        private int[,] tetris = new int[10, 20];//定义二维数组，表示坐标信息，默认值为0

        public Action<Point,Point[],TetrisDirection> onPartialChanged;//局部变更事件

        public Action<int[,]> onFullChanged;//元素全变更事件，即有整行被清除事件

        public Action onCompleted; //结束事件

        public int scorce = 0;

        /// <summary>
        /// 状态发生改变
        /// </summary>
        /// <param name="element"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public TetrisElement change(TetrisElement element, TetrisDirection direction)
        {
            TetrisElement tmp=null;
            //判断不同的方向
            switch (direction) {
                case TetrisDirection.DEFAULT:
                    //如果可以向下移动
                    if (checkDefault(element))
                    {
                        //向下移动一个元素
                        element.move(element.location.X, element.location.Y + 1);
                        tmp = element;
                    }
                    else {
                        //如果不可以向下移动，则更新容器
                        updateTetris(element);
                        tmp = null;
                    }
                   
                    break;
                case TetrisDirection.DOWN:
                    break;
                case TetrisDirection.UP:
                    break;
                case TetrisDirection.LEFT:
                    if (checkLeft(element)){
                        //判断是否可以向左移动
                        //向下移动一个元素
                        element.move(element.location.X-1, element.location.Y);
                        tmp = element;
                    }
                    break;
                case TetrisDirection.RIGHT:
                    if (checkRight(element))
                    {
                        //判断是否可以右左移动
                        //向下移动一个元素
                        element.move(element.location.X+1, element.location.Y);
                        tmp = element;
                    }
                    break;
            }

            //局部变更
            if (onPartialChanged != null)
            {
                Point location = element.location;
                Point[] content = new Point[4];
                element.content.CopyTo(content, 0);

                for (int i = 0; i < content.Length; i++)
                {
                    content[i].X = location.X + content[i].X;
                    content[i].Y = location.Y + content[i].Y;
                }
                onPartialChanged(location,content,direction);
            }

            //判断游戏是否结束
            if (onCompleted != null) {
                if (checkComplete()) {
                    onCompleted();
                }
            }

            //全部变更
            if (onFullChanged != null)
            {
                //判断是是否有权为1的行，如果有则消掉
                int[] rows = checkAllTetris();
                if (rows.Length>0)
                {
                    updateAllTetris(rows);//消掉行
                    onFullChanged(tetris);
                }
            }

            return tmp;
        }

        /// <summary>
        /// 判断默认情况：下方是否有元素
        /// </summary>
        /// <returns></returns>
        private bool checkDefault(TetrisElement element) {
            Point location = element.location;
            Point[] content = element.content;
            int maxY = element.getMaxY(element.style);
            int maxX = element.getMaxX(element.style);
            int minX = element.getMinX(element.style);
            int minY = element.getMinY(element.style);
            bool flag = false;
            switch (element.style) {
                case TetrisStyle.I:
                    if (location.Y > minY)
                    {
                        //I形状，如果没有到底，且最下面一个元素为空，则返回true
                        if (location.Y + content[3].Y + 1 < 20 && this.tetris[location.X, location.Y + content[3].Y + 1] == 0)
                        {
                            flag = true;
                        }
                    }
                    else {
                        flag = true;
                    }
                  
                    break;
                case TetrisStyle.J:
                case TetrisStyle.L:
                case TetrisStyle.O:
                    if (location.Y > minY)
                    {
                        //J,L,O形状,如果没有到底，且第3，4个元素的下面都为空，则返回true
                        if (location.Y + content[3].Y + 1 < 20 && this.tetris[location.X + content[2].X, location.Y + content[2].Y + 1] == 0 && this.tetris[location.X + content[3].X, location.Y + content[3].Y + 1] == 0)
                        {
                            flag = true;
                        }
                    }
                    else {
                        flag = true;
                    }
                    break;
                case TetrisStyle.S:
                case TetrisStyle.T:
                case TetrisStyle.Z:
                    if (location.Y > minY)
                    {
                        //S,T,Z形状如果没有到底，且第1,3，4个元素的下面都为空，则返回true
                        if (location.Y + content[3].Y + 1 < 20 && this.tetris[location.X + content[0].X, location.Y + content[0].Y + 1] == 0 && this.tetris[location.X + content[2].X, location.Y + content[2].Y + 1] == 0 && this.tetris[location.X + content[3].X, location.Y + content[3].Y + 1] == 0)
                        {
                            flag = true;
                        }
                    }
                    else {
                        flag = true;
                    }
                    break;
                default:
                    if (location.Y > minY)
                    {
                        //默认I形状，如果没有到底，且最下面一个元素为空，则返回true
                        if (location.Y + content[3].Y + 1 < 20 && this.tetris[location.X, location.Y + content[3].Y + 1] == 0)
                        {
                            flag = true;
                        }
                    }
                    else {
                        flag = true;
                    }
                    break;
            }
            return flag;
        }

        /// <summary>
        /// 判断是否可以向左移动
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool checkLeft(TetrisElement element) {
            Point location = element.location;
            Point[] content = element.content;
            int maxY = element.getMaxY(element.style);
            int maxX = element.getMaxX(element.style);
            int minX = element.getMinX(element.style);
            int minY = element.getMinY(element.style);
            bool flag = false;
            //只有全部在屏幕内才可以移动
            if (location.Y >= 0)
            {
                switch (element.style)
                {
                    case TetrisStyle.I:
                        if (location.X >= minX)
                        {
                            //I形状，如果没有到最左边，且左边一排元素为空，则返回true
                            if (location.X + content[0].X - 1 >= 0 && this.tetris[location.X + content[0].X - 1, location.Y + content[0].Y] == 0 && this.tetris[location.X + content[1].X - 1, location.Y + content[1].Y] == 0 && this.tetris[location.X + content[2].X - 1, location.Y + content[2].Y] == 0 && this.tetris[location.X + content[3].X - 1, location.Y + content[3].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    case TetrisStyle.J:
                        if (location.X >= minX)
                        {
                            //J形状,如果没有到底，且第1,3，4个元素的左面都为空，则返回true
                            if (location.X + content[3].X - 1 >= 0 && this.tetris[location.X + content[0].X - 1, location.Y + content[0].Y] == 0 && this.tetris[location.X + content[1].X - 1, location.Y + content[1].Y] == 0 && this.tetris[location.X + content[3].X - 1, location.Y + content[3].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    case TetrisStyle.L:
                        //L形状,如果没有到底，且第1,2,3个元素的左面都为空，则返回true
                        if (location.X >= minX)
                        {
                            if (location.X + content[0].X - 1 >= 0 && this.tetris[location.X + content[0].X - 1, location.Y + content[0].Y] == 0 && this.tetris[location.X + content[1].X - 1, location.Y + content[1].Y] == 0 && this.tetris[location.X + content[2].X - 1, location.Y + content[2].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    case TetrisStyle.O:
                    case TetrisStyle.Z:
                        //O形状,如果没有到底，且第1,3个元素的左面都为空，则返回true
                        if (location.X >= minX)
                        {
                            if (location.X + content[0].X - 1 >= 0 && this.tetris[location.X + content[0].X - 1, location.Y + content[0].Y] == 0 && this.tetris[location.X + content[2].X - 1, location.Y + content[2].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    case TetrisStyle.S:
                        //S形状,如果没有到底，且第2,4个元素的左面都为空，则返回true
                        if (location.X >= minX)
                        {
                            if (location.X + content[3].X - 1 >= 0 && this.tetris[location.X + content[1].X - 1, location.Y + content[1].Y] == 0 && this.tetris[location.X + content[3].X - 1, location.Y + content[3].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    case TetrisStyle.T:
                        //T形状,如果没有到底，且第1,4个元素的左面都为空，则返回true
                        if (location.X >= minX)
                        {
                            if (location.X + content[0].X - 1 >= 0 && this.tetris[location.X + content[0].X - 1, location.Y + content[0].Y] == 0 && this.tetris[location.X + content[3].X - 1, location.Y + content[3].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    default:
                        if (location.X >= minX)
                        {
                            //I形状，如果没有到最左边，且左边一排元素为空，则返回true
                            if (location.X + content[0].X - 1 >= 0 && this.tetris[location.X + content[0].X - 1, location.Y + content[0].Y] == 0 && this.tetris[location.X + content[1].X - 1, location.Y + content[1].Y] == 0 && this.tetris[location.X + content[2].X - 1, location.Y + content[2].Y] == 0 && this.tetris[location.X + content[3].X - 1, location.Y + content[3].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                }
            }
            return flag;
        }

        /// <summary>
        /// 判断是否可以向右移动
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool checkRight(TetrisElement element)
        {
            Point location = element.location;
            Point[] content = element.content;
            int maxY = element.getMaxY(element.style);
            int maxX = element.getMaxX(element.style);
            int minX = element.getMinX(element.style);
            int minY = element.getMinY(element.style);
            bool flag = false;
            if (location.Y >= 0)
            {
                switch (element.style)
                {
                    case TetrisStyle.I:
                        if (location.X <= maxX)
                        {
                            //I形状，如果没有到最左边，且左边一排元素为空，则返回true
                            if (location.X + content[0].X + 1 < 10 && this.tetris[location.X + content[0].X + 1, location.Y + content[0].Y] == 0 && this.tetris[location.X + content[1].X + 1, location.Y + content[1].Y] == 0 && this.tetris[location.X + content[2].X + 1, location.Y + content[2].Y] == 0 && this.tetris[location.X + content[3].X + 1, location.Y + content[3].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    case TetrisStyle.J:
                        if (location.X <= maxX)
                        {
                            //J形状,如果没有到底，且第1,2，3个元素的左面都为空，则返回true
                            if (location.X + content[0].X + 1 < 10 && this.tetris[location.X + content[0].X + 1, location.Y + content[0].Y] == 0 && this.tetris[location.X + content[1].X + 1, location.Y + content[1].Y] == 0 && this.tetris[location.X + content[2].X + 1, location.Y + content[2].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    case TetrisStyle.L:
                        //L形状,如果没有到底，且第1,2,4个元素的左面都为空，则返回true
                        if (location.X <= maxX)
                        {
                            if (location.X + content[0].X + 1 < 10 && this.tetris[location.X + content[0].X + 1, location.Y + content[0].Y] == 0 && this.tetris[location.X + content[1].X + 1, location.Y + content[1].Y] == 0 && this.tetris[location.X + content[3].X + 1, location.Y + content[3].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    case TetrisStyle.O:
                    case TetrisStyle.Z:
                        //O形状,如果没有到底，且第2,4个元素的左面都为空，则返回true
                        if (location.X <= maxX)
                        {
                            if (location.X + content[1].X + 1 < 10 && this.tetris[location.X + content[1].X + 1, location.Y + content[1].Y] == 0 && this.tetris[location.X + content[3].X + 1, location.Y + content[3].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    case TetrisStyle.S:
                        //S形状,如果没有到底，且第1,3个元素的左面都为空，则返回true
                        if (location.X <= maxX)
                        {
                            if (location.X + content[0].X + 1 < 10 && this.tetris[location.X + content[0].X + 1, location.Y + content[0].Y] == 0 && this.tetris[location.X + content[2].X + 1, location.Y + content[2].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    case TetrisStyle.T:
                        //T形状,如果没有到底，且第3,4个元素的左面都为空，则返回true
                        if (location.X <= maxX)
                        {
                            if (location.X + content[3].X + 1 < 10 && this.tetris[location.X + content[2].X + 1, location.Y + content[2].Y] == 0 && this.tetris[location.X + content[3].X + 1, location.Y + content[3].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                    default:
                        if (location.X <= maxX)
                        {
                            //I形状，如果没有到最左边，且左边一排元素为空，则返回true
                            if (location.X + content[0].X + 1 < 10 && this.tetris[location.X + content[0].X + 1, location.Y + content[0].Y] == 0 && this.tetris[location.X + content[1].X + 1, location.Y + content[1].Y] == 0 && this.tetris[location.X + content[2].X + 1, location.Y + content[2].Y] == 0 && this.tetris[location.X + content[3].X + 1, location.Y + content[3].Y] == 0)
                            {
                                flag = true;
                            }
                        }
                        break;
                }
            }
            return flag;
        }

        /// <summary>
        /// 更新tetris
        /// </summary>
        /// <param name="element"></param>
        private void updateTetris(TetrisElement element)
        {
            Point location = element.location;
            Point[] content = element.content;
            int minX = element.getMinX(element.style);
            int maxX = element.getMaxX(element.style);
            int minY = element.getMinY(element.style);
            int maxY = element.getMaxY(element.style);
            foreach (Point p in content)
            {
                if (location.Y + p.Y < 20 && location.Y + p.Y >= 0 && location.X + p.X >= 0 && location.X + p.X < 10)
                {
                    this.tetris[location.X + p.X, location.Y + p.Y] = 1;
                }
            }
        }

        /// <summary>
        /// 检查全部列
        /// </summary>
        private int[] checkAllTetris()
        {
            List<int> lst = new List<int>();
            //20行
            for (int y = 0; y < 20; y++)
            {
                int col = 0;
                //10列
                for (int x = 0; x < 10; x++)
                {
                    if (tetris[x, y] == 0)
                    {
                        break;
                    }
                    else
                    {
                        col += 1;
                    }
                }
                if (col == 10)
                {
                    col = 0;
                    lst.Add(y);
                }
            }
            return lst.ToArray();
        }

        /// <summary>
        /// 更新
        /// </summary>
        private void updateAllTetris(int[] rows) {
            foreach (int row in rows) {
                //当前行清掉
                for (int x = 0; x < 10; x++) {
                    tetris[x, row] = 0;
                }
                //row行之上的往下移动一行
                for (int y = row-1; y >=0; y--) {
                    for (int x = 0; x < 10; x++) {
                        if (tetris[x, y] == 1) {
                            tetris[x, y + 1] = 1;
                            tetris[x, y] = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断游戏是否结束
        /// </summary>
        /// <returns></returns>
        private bool checkComplete() {
            bool isComplete = false;
            for (int i = 0; i < 10; i++) {
                if (tetris[i, 0] == 1) {
                    isComplete = true;
                    break;
                }
            }
            return isComplete;
        }

        /// <summary>
        /// 更新得分
        /// </summary>
        /// <param name="s"></param>
        public void updateScore(int s) {
            this.scorce = this.scorce + s;
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        public void Reset() {
            this.tetris = new int[10, 20];
            this.scorce = 0;
        }
    }
}
