using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Demo.LeanCloud.WindowsForms.Service;
using LeanCloud.Realtime;

namespace Demo.LeanCloud.WindowsForms
{
    public partial class Main : Form
    {
        private LeanCloudService _realTimeService;
        private readonly bool _isUsePrivateCloud;

        private delegate void OnRecivedMessage(IAVIMMessage message);

        public Main()
        {
            _isUsePrivateCloud = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsUsePrivateCloud"]);
            InitializeComponent();
            
        }

        /// <summary>
        /// Initials the real time service.
        /// </summary>
        private void InitialRealTimeService()
        {
            _realTimeService = new LeanCloudService(_isUsePrivateCloud);
            _realTimeService.InitialRealTime();
            _realTimeService.onMessageSended += DisplayMessage;
            _realTimeService.onRecivedOnLineMessageHandler += message =>
            {
                OnRecivedMessage onRecivedMessage = RecivedOnlineMessage;
                Invoke(onRecivedMessage, message);
            };

            _realTimeService.onRecivedOffLineMessageHandler += message =>
            {
                OnRecivedMessage onRecivedMessage = RecivedOfflineMessage;
                Invoke(onRecivedMessage, message);
            };
        }

        private void DecidedDisplayMessage(IAVIMMessage message)
        {

            if (_realTimeService.CurrentConversation != null && message.ConversationId == _realTimeService.CurrentConversation.ConversationId)
            {
                DisplayMessage(message);
                _realTimeService.ReadConversation();
                RefreshConversations();
            }
        }

        private void RecivedOnlineMessage(IAVIMMessage message)
        {
            AddConsoleMessage($"已接收來自到 {message.FromClientId} 的線上訊息.");
            DecidedDisplayMessage(message);
        }

        private void RecivedOfflineMessage(IAVIMMessage message)
        {
            AddConsoleMessage($"已接收來自到 {message.FromClientId} 的離線訊息.");
            DecidedDisplayMessage(message);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //減少手動測試的動作
            bt_Login_Click(null,null);
        }

        #region UI        

        /// <summary>
        ///     Adds the console message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void AddConsoleMessage(string message)
        {
            tb_Console.Text = $"{DateTime.Now.ToLongTimeString()} - {message} {Environment.NewLine}{tb_Console.Text}";
        }

        /// <summary>
        ///     Refreshes the friends.
        /// </summary>
        private void RefreshFriends()
        {
            var friends = ContactService.GetContacts(_realTimeService.Client.ClientId);
            //lb_Friends.Items.Clear();
            lb_Friends.DataSource = friends;
            lb_Friends.SelectedIndex = -1;
        }

        /// <summary>
        ///     Refreshes the conversation.
        /// </summary>
        private void RefreshConversation()
        {
            if (_realTimeService.CurrentConversation == null) return;
            lb_ConversationTitle.Text = $"對話內容:{_realTimeService.CurrentConversation.Name}";
            tb_ConversationContent.Text = string.Empty;
            foreach (var avimMessage in _realTimeService.AllMessage.Where(it => it.ConversationId == _realTimeService.CurrentConversation.ConversationId).OrderBy(it=>it.ServerTimestamp))
            {
                DisplayMessage(avimMessage);
            } 

            //tb_ConversationContent.Text = string.Empty;
        }

        /// <summary>
        ///     Creates the conversation.
        /// </summary>
        /// <param name="friendName">Name of the friend.</param>
        private async void CreateConversation(string friendName)
        {
            if (string.IsNullOrEmpty(friendName))
            {
                MessageBox.Show("friendName 不得為空");
                return;
            }

            AddConsoleMessage($"建立 {tb_UserName.Text} 與 {friendName} 對話");

            await _realTimeService.CreateConversation(friendName);

            RefreshConversations();
            RefreshConversation();
            
            AddConsoleMessage($"對話建立完成. ConversationId= { _realTimeService.CurrentConversation.ConversationId}");
        }

        private void GetConversation(string conversationId)
        {
            _realTimeService.ChangeConversation(conversationId);

            RefreshConversation();
            AddConsoleMessage($"對話刷新完成. ConversationId= {_realTimeService.CurrentConversation.ConversationId}");
        }

        /// <summary>
        ///     更新 Conversations 至 UI
        /// </summary>
        private void RefreshConversations()
        {
            lb_ConversationList.DataSource = _realTimeService.Conversations.Select(it => $"[{_realTimeService.GetNotReadAmount(it.ConversationId)}]-{it.ConversationId}-{it.Name}").ToList();
        }

        /// <summary>
        ///     清掉 UI 上的 Conversations Content
        /// </summary>
        private void ClearConversationContent()
        {
            tb_ConversationContent.Text = string.Empty;
            lb_ConversationTitle.Text = "對話內容";
        }



        #endregion


        #region "Events"

        /// <summary>
        ///     Handles the Click event of the bt_Login control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private async void bt_Login_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_UserName.Text))
            {
                MessageBox.Show("請輸入使用者名稱");
                return;
            }

            AddConsoleMessage("初始化 RealTime Service.");
            InitialRealTimeService();

            AddConsoleMessage($"開始登入 {tb_UserName.Text}");

            await _realTimeService.CreateClient(tb_UserName.Text);

            RefreshFriends();
            RefreshConversations();
            ClearConversationContent();

            AddConsoleMessage($"{tb_UserName.Text} 登入成功");
        }


        /// <summary>
        ///     Handles the SelectedIndexChanged event of the lb_Friends control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void lb_Friends_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lb_Friends.SelectedIndex < 0) return;
            CreateConversation(lb_Friends.SelectedItem.ToString());
        }

        private async void bt_SendMessage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_Message.Text))
                return;

            AddConsoleMessage($"傳送訊息-開始, 訊息內容:{tb_Message.Text}");

            var sendResult = await _realTimeService.SendMessage(tb_Message.Text);
            
            AddConsoleMessage($"傳送訊息-結束, Message.Id:{sendResult.Id}");
            tb_Message.Text = string.Empty;
            tb_Message.Focus();
        }

        private void tb_Message_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int) Keys.Enter)
                bt_SendMessage_Click(bt_SendMessage, null);
        }

        private void tb_UserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int) Keys.Enter)
                bt_Login_Click(bt_Login, null);
        }

        private void lb_ConversationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lb_ConversationList.SelectedIndex < 0) return;
            AddConsoleMessage($"切換對話訊息{lb_ConversationList.SelectedItem.ToString()}");
            CreateConversationUseId(lb_ConversationList.SelectedItem.ToString().Split('-')[1]);
        }

        private void CreateConversationUseId(string conversationId)
        {
            GetConversation(conversationId);
        }

        #endregion

        /// <summary>
        ///     Displays the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void DisplayMessage(IAVIMMessage message)
        {
            switch (message.GetType().Name)
            {
                case "AVIMTextMessage":
                    if (!(message is AVIMTextMessage m)) return;
                    tb_ConversationContent.Text += $"{m.ServerTimestamp} - {m.FromClientId} 說 {m.TextContent} {Environment.NewLine}";
                    
                    break;
                case "AVIMBinaryMessage":
                    if (!(message is AVIMBinaryMessage bm)) return;
                    tb_ConversationContent.Text += $"{bm.ServerTimestamp} - {bm.FromClientId} 說 {bm.Content} {Environment.NewLine}";
                    break;
                default:
                    break;
            }
        }

        
        /// <summary>
        ///     載入歷史訊息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_LoadMessage_Click(object sender, EventArgs e)
        {
            _realTimeService.LoadMessage();
            RefreshConversation();
        }

        private void bt_AddGroup_Click(object sender, EventArgs e)
        {
            var frm = new frm_AddGroup(_realTimeService);
            frm.FormClosing += (o, args) =>
            {
                this.Show();
            };

            this.Hide();
            frm.Show();
        }

    }
}