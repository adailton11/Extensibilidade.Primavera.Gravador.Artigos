using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artigos_ExtensibilidadePrimavera.Repositorios;
using ErpBS100;

namespace Artigos_ExtensibilidadePrimavera.Forms
{
    public partial class Artigos : CustomForm
    {
        private RepositorioArtigos RepositorioArtigos = new RepositorioArtigos();
        public Artigos()
        {
            InitializeComponent();
        }
        private void GetCodigo()
        {
            txtArtigo.Text = RepositorioArtigos.CodArtigo(BSO);
        }
        private void GetArmazen()
        {
            RepositorioArtigos.GetArmazens(BSO, cmbArmazens);
        }
        private void Artigos_Load(object sender, EventArgs e)
        {
            GetCodigo();
            GetArmazen();
        }
        private void cmbArmazens_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtLocalizacaoArm.Text = RepositorioArtigos.GetLocalizacaoArmazens(BSO, cmbArmazens.Text);
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            if (txtArtigo.Text != string.Empty || txtDescricao.Text != string.Empty || txtLocalizacaoArm.Text != string.Empty)
            {
                string SMS = "Artigo Gravado com sucesso!";
                RepositorioArtigos.SaveDados(BSO,PSO, SMS, txtArtigo.Text, txtDescricao.Text, cmbMoeda.Text, cmbArmazens.Text, txtLocalizacaoArm.Text, cmbTipo.Text, txtUnidade.Text);
                PSO.MensagensDialogos.MostraAviso(SMS);
                BeforeSave();
            }
            else
            {
                PSO.MensagensDialogos.MostraAviso("Verifica os Campos vazios!");
            }
        }
        void BeforeSave()
        {
            txtArtigo.Text = txtDescricao.Text = txtLocalizacaoArm.Text = cmbArmazens.Text = cmbTipo.Text = "";
            GetCodigo();
            GetArmazen();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            BeforeSave();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }
    }
}
