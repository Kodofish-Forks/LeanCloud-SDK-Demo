using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LeanCloud;
using LeanCloud.Realtime;
using Websockets.Net;

namespace Demo.LeanCloud.WindowsForms
{
    public partial class Main : Form
    {
        private AVIMClient _client;
        private AVIMConversation _conversation;
        private IEnumerable<AVIMConversation> _conversations;
        private IList<IAVIMMessage> _messages;
        private AVRealtime _realtime;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //地雷!! 要先執行這行, 不然後面在CreateClientAsync時會報 WebSocket 沒有初始化的錯誤
            WebsocketConnection.Link();

            InitialPublishCloud();
            
            _realtime.OnOfflineMessageReceived += (o, args) =>
            {
                OnRecivedIMOffLineMessage a = RecivedOfflineMessage;
                Invoke(a, args.Message);
            };
        }

        /// <summary>
        ///     載入 LeanCloud 公有雲設定
        /// </summary>
        private void InitialPublishCloud()
        {
            var _applicationId = "bBPbOUuF6mEmL0knL3wnW00b-gzGzoHsz";
            var _applicationKey = "tCx1qE0VNNghnch5n8PjpgGX";

            var config = new AVClient.Configuration
            {
                ApplicationId = _applicationId,
                ApplicationKey = _applicationKey
            };
            AVClient.Initialize(config);

            _realtime = new AVRealtime(new AVRealtime.Configuration
            {
                ApplicationId = _applicationId,
                ApplicationKey = _applicationKey
            });
        }

        /// <summary>
        ///     載入 LeanCloud 私有雲設定
        /// </summary>
        private void InitialPrivateCloud()
        {
            var _applicationId = "XtesJ6luUX17WTbKYpNtcEzf-JDEV1";
            var _applicationKey = "o11xvV4AlqYWmpRlPNwsdLyp";
            var config = new AVClient.Configuration
            {
                ApplicationId = _applicationId,
                ApplicationKey = _applicationKey,
                ApiServer = new Uri("http://im-api.phyuance.com")
                //經詢問 LeanCloud, 不需要設置以下設定
                //EngineServer = new Uri(""),
                //PushServer = new Uri(""),
                //StatsServer =  new Uri("")
            };
            AVClient.Initialize(config);

            _realtime = new AVRealtime(new AVRealtime.Configuration
            {
                ApplicationId = _applicationId,
                ApplicationKey = _applicationKey,
                RTMRouter = new Uri("http://im-router.phyuance.com")
            });
        }

        private void RecivedMessage(IAVIMMessage message)
        {
            if (_messages.Any(it => it.Id == message.Id)) return;

            _messages.Add(message);
            AddConsoleMessage($"已接收來自到 {message.FromClientId} 訊息.");
            DisplayMessage(message);
        }

        private void RecivedOfflineMessage(IAVIMMessage message)
        {
            if (_messages.Any(it => it.Id == message.Id)) return;

            _messages.Add(message);
            AddConsoleMessage($"已接收來自到 {message.FromClientId} 的離線訊息.");
            DisplayMessage(message);
        }

        private void lb_ConversationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddConsoleMessage($"切換對話訊息{lb_ConversationList.SelectedItem.ToString()}");
            CreateConversationUseId(lb_ConversationList.SelectedItem.ToString().Split('-')[0]);
        }

        private void CreateConversationUseId(string conversationId)
        {
            GetConversation(conversationId);
        }

        private delegate void OnRecivedIMMessage(IAVIMMessage message);

        private delegate void OnRecivedIMOffLineMessage(IAVIMMessage message);

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
            var friends = GetAllFriends(_client.ClientId);
            lb_Friends.Items.Clear();
            lb_Friends.DataSource = friends;
        }

        private List<string> GetAllFriends(string clientClientId)
        {
            var defaultUser = new List<string> {"Kodofish", "Andy", "Neo", "Jerry"};
            return defaultUser.Where(it => it.ToLower() != clientClientId.ToLower()).ToList();
        }

        /// <summary>
        ///     Refreshes the conversation.
        /// </summary>
        private void RefreshConversation()
        {
            lb_ConversationTitle.Text = $"對話內容:{_conversation.Name}";
            tb_ConversationContent.Text = string.Empty;
            _messages = new List<IAVIMMessage>();
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

            IDictionary<string, object> attr = new Dictionary<string, object>();
            attr.Add("type", "private");
            attr.Add("isSticky", false);

            AddConsoleMessage($"建立 {tb_UserName.Text} 與 {friendName} 對話");
            _conversation = await _client.CreateConversationAsync(friendName, name: $"{tb_UserName.Text} and {friendName} conversation", options: attr);

            RefreshConversation();
            AddConsoleMessage($"對話建立完成. ConversationId= {_conversation.ConversationId}");
        }

        private async void GetConversation(string conversationId)
        {
            _conversation = _conversations.FirstOrDefault(it => it.ConversationId == conversationId);
            RefreshConversation();
            AddConsoleMessage($"對話刷新完成. ConversationId= {_conversation.ConversationId}");
        }

        private void RefreshConversationMessage()
        {
            foreach (var message in _messages)
                DisplayMessage(message);
        }


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
            tb_ConversationContent.Text = string.Empty;
            AddConsoleMessage($"開始登入 {tb_UserName.Text}");

            _client = await _realtime.CreateClientAsync(tb_UserName.Text);
            _client.OnMessageReceived += ClientOnOnMessageReceived;

            RefreshFriends();
            GetExistConversations();
            ClearConversationContent();


            AddConsoleMessage($"{tb_UserName.Text} 登入成功");
        }

        /// <summary>
        ///     查詢 Conversation
        ///     Gets the exist conversations.
        /// </summary>
        private async void GetExistConversations()
        {
            lb_ConversationList.Items.Clear();
            _conversations = await _client.GetQuery().FindAsync();
            //_conversations = await _client.GetQuery().Limit(1000).FindAsync();
            lb_ConversationList.DataSource = _conversations.Select(it => $"{it.ConversationId}-{it.Name}").ToList();
        }

        private void ClearConversationContent()
        {
            _conversation = null;
            tb_ConversationContent.Text = string.Empty;
            lb_ConversationTitle.Text = "對話內容";
        }

        /// <summary>
        ///     在線接收訊息
        ///     Clients the on on message received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="avimMessageEventArgs">The <see cref="AVIMMessageEventArgs" /> instance containing the event data.</param>
        private void ClientOnOnMessageReceived(object sender, AVIMMessageEventArgs avimMessageEventArgs)
        {
            OnRecivedIMMessage a = RecivedMessage;
            Invoke(a, avimMessageEventArgs.Message);
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the lb_Friends control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void lb_Friends_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateConversation(lb_Friends.SelectedItem.ToString());
        }

        private async void bt_SendMessage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_Message.Text))
                return;

            AddConsoleMessage($"傳送訊息-開始, 訊息內容:{tb_Message.Text}");
            var message = new AVIMTextMessage(tb_Message.Text);

            var sendResult = await _conversation.SendMessageAsync(message);
            //_messages.Add(sendResult);
            //DisplayMessage(sendResult);
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

        #endregion
    }
}