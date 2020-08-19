using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Tetris.Bussiness
{
    /// <summary>
    /// 俄罗斯方块控件
    /// </summary>
    public partial class TetrisControl : UserControl
    {
        #region 构造函数和属性

        TetrisContainer tcontainer;

        private TetrisElement element; //元素

        private BackgroundWorker bgWorker;//后台进程，控制元素的生成

        private bool isRunning = false; //是否运行

        private bool isMove = false;//是否正在左右移动

        public Action onFocusChanged;

        public TetrisControl()
        {
            InitializeComponent();
            //以下采用双缓冲方式，减少闪烁
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        #endregion

        #region 事件

        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TetrisControl_Load(object sender, EventArgs e)
        {
            //后台事件
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
            //填充信息
            for (int i = 0; i < 20; i++) {
                for (int j = 0; j < 10; j++) {
                    Label lbl = new Label();
                    lbl.Name = string.Format("lblr{0}c{1}" , i.ToString() , j.ToString());
                    lbl.Margin = new Padding(1);
                    lbl.Padding = new Padding(1);
                    lbl.BackColor = SystemColors.Control;
                    //lbl.BorderStyle = BorderStyle.FixedSingle;
                    lbl.Width = 15;
                    lbl.Height = 15;
                    lbl.Dock = DockStyle.Fill;
                    tbControl.Controls.Add(lbl, j, i);
                }
            }

            //容器
            if (tcontainer == null)
            {
                tcontainer = new TetrisContainer();
                tcontainer.onPartialChanged += new Action<Point,Point[],TetrisDirection>(container_PartialChanged);
                tcontainer.onFullChanged += new Action<int[,]>(container_FullChanged);
                tcontainer.onCompleted += new Action(container_Completed);
            }
        }

        /// <summary>
        /// 后台线程执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (isRunning)
            {
                if (element == null)
                {
                    //如果元素为空，则创建元素，并启动新线程
                    element = TetrisFactory.generate();
                    Thread t = new Thread(new ParameterizedThreadStart(elementMove));
                    t.Start(element);
                }
                else {
                    //暂停1秒钟
                    Thread.Sleep(1000);
                }
            }
           
        }

        private void elementMove(object obj) {
            TetrisElement telement = obj as TetrisElement;
            while (isRunning && telement!=null) {
                if (!isMove)
                {
                    telement = TetrisFactory.move(tcontainer, telement, TetrisDirection.DEFAULT);
                }
                Thread.Sleep(500);
            }
            element = telement;
        }
        
        /// <summary>
        /// 生成状态发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// 线程结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.lblStatus.Text = "GameOver";
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            //初始化信息
            InitInfo();
            //启动
            Start();
        }

        /// <summary>
        /// 结束游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnd_Click(object sender, EventArgs e)
        {
            End();
        }


        /// <summary>
        /// 按下键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TetrisControl_KeyDown(object sender, Keys keyCode)
        {
            //只有开始时才响应事件
            if (isRunning && element != null)
            {
                switch (keyCode)
                {
                    case Keys.Left:
                        isMove = true;
                        TetrisFactory.move(tcontainer, element, TetrisDirection.LEFT);
                        break;
                    case Keys.Right:
                        isMove = true;
                        TetrisFactory.move(tcontainer, element, TetrisDirection.RIGHT);
                        break;
                }
                isMove = false;//移动完毕
            }
        }

        /// <summary>
        /// 容器部分变更
        /// </summary>
        /// <param name="cotent"></param>
        private void container_PartialChanged(Point location ,Point[] cotent,TetrisDirection direction) {
            if (location.Y == 0) {
                this.tcontainer.updateScore(10);
                this.lblScore.Invoke(new Action(() =>
                {
                    this.lblScore.Text = tcontainer.scorce.ToString();
                }));
            }
            //必须倒着来
            for(int i=3;i>=0;i--)
            {
                Point p = cotent[i];
                //如果不在范围内，则不必更新控件
                if (p.Y >= 0 && p.Y < 20 && p.X >= 0 && p.X < 10)
                {
                    this.tbControl.Controls.Find(string.Format("lblr{0}c{1}", p.Y, p.X), false)[0].BackColor = Color.Gold;
                    
                }
                switch (direction)
                {
                    case TetrisDirection.LEFT:
                        if (p.X + 1 >= 0 && p.X + 1 < 10 && p.Y >= 0 && p.Y < 20)
                        {
                            this.tbControl.Controls.Find(string.Format("lblr{0}c{1}", p.Y, p.X + 1), false)[0].BackColor = SystemColors.Control;
                        }
                        break;
                    case TetrisDirection.RIGHT:
                        if (p.X - 1 >= 0 && p.X - 1 < 10 && p.Y >= 0 && p.Y < 20)
                        {
                            this.tbControl.Controls.Find(string.Format("lblr{0}c{1}", p.Y, p.X - 1), false)[0].BackColor = SystemColors.Control;
                        }
                        break;
                    default:
                        if (p.Y - 1 >= 0 && p.Y - 1 < 20 && p.X >= 0 && p.X < 10)
                        {
                            this.tbControl.Controls.Find(string.Format("lblr{0}c{1}", p.Y - 1, p.X), false)[0].BackColor = SystemColors.Control;
                        }
                        break;
                }
                
            }
        }

        /// <summary>
        /// 容器全变更
        /// </summary>
        /// <param name="obj"></param>
        private void container_FullChanged(int[,] obj)
        {
            this.tcontainer.updateScore(100);
            this.lblScore.Invoke(new Action(() =>
            {
                this.lblScore.Text = tcontainer.scorce.ToString();
            }));
            for (int y = 0; y < 20; y++) {
                for (int x = 0; x < 10; x++) {
                    if (obj[x, y] == 1)
                    {
                        this.tbControl.Controls.Find(string.Format("lblr{0}c{1}", y, x), false)[0].BackColor = Color.Gold;
                    }
                    else {
                        this.tbControl.Controls.Find(string.Format("lblr{0}c{1}", y, x), false)[0].BackColor = SystemColors.Control;
                    }
                }
            }
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        private void container_Completed()
        {
            isRunning = false;
            this.lblStatus.Invoke(new Action(() =>
            {
                this.lblStatus.Text = "GameOver";
            }));
          
        }

        #endregion

        #region 功能函数

        /// <summary>
        /// 重置信息
        /// </summary>
        public void InitInfo() {
            this.tcontainer.Reset();
            this.lblStatus.Text = "Idle";
            foreach (Control c in this.tbControl.Controls) {
                c.BackColor = SystemColors.Control;
            }
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void Start() {
            this.isRunning = true;
            this.lblStatus.Text = "Running";
            this.bgWorker.RunWorkerAsync();
            if (onFocusChanged != null) {
                onFocusChanged();
            }
        }

        /// <summary>
        /// 结束游戏
        /// </summary>
        public void End() {
            this.isRunning = false;
        }

        #endregion

       
    }
}
