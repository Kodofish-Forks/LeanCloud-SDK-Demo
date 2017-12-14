using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Demo.LeanCloud.WindowsForms.Service;

namespace Demo.LeanCloud.WindowsForms
{
    public partial class frm_AddGroup : Form
    {
        private readonly LeanCloudService _realTimeService;

        public frm_AddGroup(LeanCloudService realTimeService)
        {
            InitializeComponent();
            _realTimeService = realTimeService;
            var list = ContactService.GetContacts(_realTimeService.Client.ClientId);
            lb_Members.DataSource = list;
            
        }


        private void bt_CloseForm_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_AddGroup_Click(object sender, EventArgs e)
        {
            //var members = clb_Members.SelectedItems;
            var members = (from object selectedItem in lb_Members.SelectedItems select selectedItem.ToString()).ToList();

            Task.Run(async () => { await _realTimeService.CreateGroup(tb_GroupName.Text, members); });

            MessageBox.Show("群組新增成功");
            Close();
        }
    }
}