using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadSample
{
    public partial class Form1 : Form
    {
        private int shareresult;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnThread_Click(object sender, EventArgs e)
        {
            Thread t=new Thread(new ThreadStart(Dowork));
            t.Start();
        }

        /// <summary>
        /// ThreadPool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnThreadPool_Click(object sender, EventArgs e)
        {
            object state = new object();

            bool queReslt = ThreadPool.QueueUserWorkItem(new WaitCallback(Dowork),state);
        }

        /// <summary>
        /// Task 4.5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTask_Click(object sender, EventArgs e)
        {
            //返り値がないTask
            Action d = Dowork;
            Task task = new Task(d);
            task.Start();

            //返り値があるTask
            Func<int> dw = DoworkReturn;
            Task<int> taskint = new Task<int>(dw);//非同期で実行するメソッドの返り値をTask<T>に設定する
            taskint.Start();
            int result = taskint.Result;//返り値がResultで返却される resultはスレッドの処理が終わるまでブロックされる

            //非同期処理が完了した時点で、結果を引数として、継続して非同期処理を実行する その結果をUIに反映 UIスレッドはブロックしない
            Task<int> taskint2 = Task.Run<int>(() => DoworkReturn())
                                .ContinueWith<int>(t => DoworkReturn2(t.Result),TaskScheduler.FromCurrentSynchronizationContext());
            
        }
        
        /// <summary>
        /// lambda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLambda_Click(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        shareresult = i * j;
                    }
                }
            });
        }

       
        /// <summary>
        /// 非同期処理メソッド
        /// </summary>
        private void Dowork()
        {
            for (int i = 0; i < 100000; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    shareresult = i * j;
                }
            }
        }

        private void Dowork(object arg)
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    arg = i * j;
                }
            }
        }

        /// <summary>
        /// 非同期処理メソッド　int返却
        /// </summary>
        /// <returns></returns>
        private int DoworkReturn()
        {
            int result = 0;

            for (int i = 0; i < 1000000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    result = i * j;
                }
            }

            return result;
        }

        private int DoworkReturn2(int result)
        {
            label1.Text = result.ToString();
            return result+1;
        }

        private void SetChekresult(int result)
        {
            label1.Text = result.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
