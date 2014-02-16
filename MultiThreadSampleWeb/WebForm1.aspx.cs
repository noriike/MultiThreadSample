using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadSampleWeb
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        /// <summary>
        /// RegisterAsyncTaskで非同期操作をページに登録
        /// .aspxにAsync="true"属性追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(LoadSomeData));
        }

        /// <summary>
        /// async awaitでPageLoad時に非同期処理
        /// </summary>
        /// <returns></returns>
        public async Task LoadSomeData()
        {
            var result2 = Task.Run<int>(() => DoworkReturn());
            var result = Task.Run<int>(() => DoworkReturnTyp2());

            //すべての処理が完了するまで待機
            await Task.WhenAll(result2, result);

            DoworkReturn2(result2.Result);
            DoworkReturn3(result.Result);
        }

        protected async void Button1_Click(object sender, EventArgs e)
        {
            int result2 = await Task.Run<int>(() => DoworkReturn());
            DoworkReturn2(result2);
        }

        protected async void Button2_Click(object sender, EventArgs e)
        {
            var result2 = Task.Run<int>(() => DoworkReturn());
            var result = Task.Run<int>(() => DoworkReturnTyp2());

            //すべての処理が完了するまで待機
            await Task.WhenAll(result2, result);

            DoworkReturn2(result2.Result);
            DoworkReturn3(result.Result);
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

        private int DoworkReturnTyp2()
        {
            int result = 0;

            for (int i = 0; i < 2000000; i++)
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
            Label1.Text = result.ToString();
            return result + 1;
        }
        private int DoworkReturn3(int result)
        {
            Label2.Text = result.ToString();
            return result + 1;
        }

        private void SetChekresult(int result)
        {
            Label1.Text = result.ToString();
        }
        private void SetChekresult2(int result)
        {
            Label2.Text = result.ToString();
        }
        
    }
}