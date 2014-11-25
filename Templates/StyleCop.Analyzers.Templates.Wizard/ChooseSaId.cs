using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StyleCop.Analyzers.Templates.Wizard
{
    public partial class ChooseSaId : Form
    {
        IDescriptionService descriptionService;

        public PageInfo PageInfo { get; private set; }

        public ChooseSaId(IDescriptionService descriptionService)
        {
            this.descriptionService = descriptionService;

            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            PageInfo = await descriptionService.GetPageInfo(nudSaId.Value);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void ChooseSaId_Activated(object sender, EventArgs e)
        {
            nudSaId.Focus();
        }

        private void nudSaId_Enter(object sender, EventArgs e)
        {
            nudSaId.Select(0, nudSaId.Text.Length);
        }
    }
}
