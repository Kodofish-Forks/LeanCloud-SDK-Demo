using System;
using System.Windows.Forms;
using LeanCloud;
using LeanCloud.Realtime;

namespace Demo.LeanCloud.WindowsForms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            var config = new AVClient.Configuration()
            {
                ApplicationId = "Id",
                ApplicationKey = "Key",
                ApiServer = new Uri("http://im-api.phyuance.com"),
                //經詢問 LeanCloud, 不需要設置以下設定
                //EngineServer = new Uri(""),
                //PushServer = new Uri(""),
                //StatsServer =  new Uri("")
            };
            AVClient.Initialize(config);


            var realtime = new AVRealtime(new AVRealtime.Configuration
            {
                ApplicationId = "XtesJ6luUX17WTbKYpNtcEzf-JDEV1",
                ApplicationKey = "o11xvV4AlqYWmpRlPNwsdLyp",
                RTMRouter = new Uri("http://im-router.phyuance.com")
            });


        }

        private void btn_Login_Click(object sender, EventArgs e)
        {

        }
    }
}
