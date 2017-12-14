using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            clb_Members.DataSource = list;
            
        }



        private void bt_CloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_AddGroup_Click(object sender, EventArgs e)
        {
            var members = clb_Members.SelectedItems;
            
            //_realTimeService.CreateGroup(tb_GroupName.Text,members);
        }
    }
}
