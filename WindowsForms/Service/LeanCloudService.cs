using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeanCloud;
using LeanCloud.Realtime;
using Websockets.Net;

namespace Demo.LeanCloud.WindowsForms.Service
{
    public class LeanCloudService
    {
        /// <summary>
        ///     The messages- 對話訊息
        /// </summary>
        private readonly IList<IAVIMMessage> _messages;

        private readonly bool _usePrivateCloud;
        private IList<AVIMConversation> _conversations;

        /// <summary>
        ///     The new messages
        /// </summary>
        private IList<IAVIMMessage> _notReadMessage;

        private string _userId;

        public LeanCloudService(bool usePrivateCloud = false)
        {
            _messages = new List<IAVIMMessage>();
            _notReadMessage = new List<IAVIMMessage>();
            _conversations = new List<AVIMConversation>();
            _usePrivateCloud = usePrivateCloud;
        }


        public AVRealtime Realtime { get; set; }
        public AVIMClient Client { get; set; }
        public AVIMConversation CurrentConversation { get; private set; }

        public IEnumerable<AVIMConversation> Conversations => _conversations;

        /// <summary>
        ///     Gets all message.
        /// </summary>
        /// <value>
        ///     All message.
        /// </value>
        public IList<IAVIMMessage> AllMessage => _messages;

        /// <summary>
        ///     Initials the real time.
        /// </summary>
        public void InitialRealTime()
        {
            //地雷!! 要先執行這行, 不然後面在CreateClientAsync時會報 WebSocket 沒有初始化的錯誤
            WebsocketConnection.Link();

            if (_usePrivateCloud)
                InitialPrivateCloud();
            else
                InitialPublishCloud();

            // 建立離線訊息接收事件
            Realtime.OnOfflineMessageReceived += (o, args) =>
            {
                //將離線訊息另外儲存起來
                if (_notReadMessage.All(it => it.Id != args.Message.Id))
                    _notReadMessage.Add(args.Message);

                RecivedMessage(args.Message);
                onRecivedOffLineMessageHandler?.Invoke(args.Message);
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

            Realtime = new AVRealtime(new AVRealtime.Configuration
            {
                ApplicationId = _applicationId,
                ApplicationKey = _applicationKey
            });
            AVRealtime.WebSocketLog(s => Debug.WriteLine(s));
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

            Realtime = new AVRealtime(new AVRealtime.Configuration
            {
                ApplicationId = _applicationId,
                ApplicationKey = _applicationKey,
                RTMRouter = new Uri("http://im-router.phyuance.com")
            });
            AVRealtime.WebSocketLog(s => Debug.WriteLine(s));
        }

        public async Task CreateClient(string userId)
        {
            _userId = userId;
            Client = await Realtime.CreateClientAsync(userId);

                // 建立在線訊息接收事件
                Client.OnMessageReceived += (o, args) =>
                {
                    if (_messages.Any(it => it.Id == args.Message.Id)) return;

                    if (_notReadMessage.All(it => it.Id != args.Message.Id))
                        _notReadMessage.Add(args.Message);

                    RecivedMessage(args.Message);
                    onRecivedOnLineMessageHandler?.Invoke(args.Message);
                };

            
            await GetAllConversations();
        }

        /// <summary>
        ///     建立對話
        ///     Creates the conversation.
        /// </summary>
        /// <param name="toUserId">To user identifier.</param>
        /// <returns></returns>
        public async Task CreateConversation(string toUserId)
        {
            IDictionary<string, object> attr = new Dictionary<string, object>();
            attr.Add("type", "private");
            attr.Add("isSticky", false);

            CurrentConversation = await Client.CreateConversationAsync(toUserId, name: $"{_userId} and {toUserId} conversation", options: attr);
            
            if (_conversations.All(it => it.ConversationId != CurrentConversation.ConversationId))
                _conversations.Add(CurrentConversation);
        }

        /// <summary>
        /// Creates the group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="members">The members.</param>
        public async Task CreateGroup(string groupName, List<string> members)
        {
            var groupConversation = await Client.CreateConversationAsync(name:groupName);
            foreach (var member in members)
            {
                await groupConversation.AddMembersAsync(member);
            }
            if (_conversations.All(it => it.ConversationId != groupConversation.ConversationId))
                _conversations.Add(groupConversation);
        }

        public void ChangeConversation(string conversationId)
        {
            CurrentConversation = _conversations.FirstOrDefault(it => it.ConversationId == conversationId);
        }

        public async Task<IAVIMMessage> SendMessage(string message)
        {
            var textMessage = new AVIMTextMessage(message);

            var sendResult = await CurrentConversation.SendMessageAsync(textMessage);

            RecivedMessage(sendResult);

            onMessageSended?.Invoke(sendResult);

            return sendResult;
        }

        /// <summary>
        ///     從 LeanCloud 取得所有已存在的 Conversations
        /// </summary>
        /// <returns></returns>
        private async Task GetAllConversations()
        {
            _conversations = (await Client.GetQuery().FindAsync()).ToList();
            //_conversations = await _client.GetQuery().Limit(1000).FindAsync();
        }

        #region "Message"

        /// <summary>
        ///     接收訊息
        ///     Reciveds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void RecivedMessage(IAVIMMessage message)
        {
            if (_messages.Any(it => it.Id == message.Id)) return;

            _messages.Add(message);
        }

        #endregion

        /// <summary>
        /// 將未讀訊息清掉
        /// Reads the conversation.
        /// </summary>
        public void ReadConversation()
        {
            _notReadMessage = _notReadMessage.Where(it => it.ConversationId == CurrentConversation.ConversationId).ToList();
        }

        #region "Events"

        /// <summary>
        ///     On recived offline message
        /// </summary>
        /// <param name="message">The message.</param>
        public delegate void OnRecivedOffLineMessageHandler(IAVIMMessage message);

        public event OnRecivedOffLineMessageHandler onRecivedOffLineMessageHandler;

        /// <summary>
        ///     On recived online message
        /// </summary>
        /// <param name="message">The message.</param>
        public delegate void OnRecivedOnLineMessageHandler(IAVIMMessage message);

        public event OnRecivedOnLineMessageHandler onRecivedOnLineMessageHandler;

        /// <summary>
        ///     On message sended
        /// </summary>
        /// <param name="message">The message.</param>
        public delegate void OnMessageSended(IAVIMMessage message);

        public event OnMessageSended onMessageSended;



        #endregion

        /// <summary>
        /// 取得 Conversation 未讀數
        /// Gets the not read amount.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        /// <returns></returns>
        public int GetNotReadAmount(string conversationId)
        {
            return _notReadMessage.Count(it => it.ConversationId == conversationId);
        }

        public void LoadMessage()
        {
            var firstmessage = _messages.Where(it => it.ConversationId == CurrentConversation.ConversationId).OrderBy(it => it.ServerTimestamp).FirstOrDefault()?.ConversationId;

            var querymessage = Client.QueryMessageAsync(CurrentConversation, firstmessage, null, null, null, 20).Result;

            foreach (var message in querymessage)
            {
                RecivedMessage(message);
            }
        }


        public void DeleteConversation(string conversationId)
        {
            //SDK 未提供刪除 Conversation 功能
        }
    }
}