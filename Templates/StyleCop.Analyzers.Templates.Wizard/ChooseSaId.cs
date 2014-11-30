namespace StyleCop.Analyzers.Templates.Wizard
{
    using System;
    using System.Windows.Forms;

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
            decimal selectedSa = nudSaId.Value;

            if (!await descriptionService.RuleExists(selectedSa))
            {
                MessageBox.Show(
                    "Rule not found or status code different than 200 received!", 
                    "Rule not found", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                nudSaId.Focus();

                return;
            }

            PageInfo = await descriptionService.GetPageInfo(selectedSa);

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
