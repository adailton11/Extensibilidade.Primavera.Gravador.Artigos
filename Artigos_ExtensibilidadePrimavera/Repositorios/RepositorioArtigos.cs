using StdBE100;
using ErpBS100;
using StdPlatBS100;
using BasBE100;
using System;
using System.Windows.Forms;

namespace Artigos_ExtensibilidadePrimavera.Repositorios
{
    public class RepositorioArtigos
    {
        private ErpBS _bso = new ErpBS();
        private StdBSInterfPub _pso;
        private BasBEArtigo BasBEArtig;
        public string CodArtigo(ErpBS BSO)
        {
            _bso = BSO;
            StdBELista lista = _bso.Consulta("select Max(Artigo) AS CODIGO FROM Artigo where Artigo NOT like '%A%'");
            if (!lista.Vazia())
            {
                string codigo = lista.Valor(0);

                if (codigo.StartsWith("0"))
                {
                    int CodInt = Convert.ToInt32(codigo.Substring(1));
                    if (CodInt < 9)
                    {
                        CodInt += 1;
                        return "000" + CodInt + "";
                    }
                    else if (CodInt > 8 && CodInt < 99)
                    {
                        CodInt += 1;
                        return "00" + CodInt + "";
                    }
                    if (CodInt > 98 && CodInt < 999)
                    {
                        CodInt += 1;
                        return "0" + CodInt + "";
                    }
                }
                else
                {
                    int CodInt = Convert.ToInt32(codigo);
                    CodInt += 1;
                    //int Total = CodInt;
                    return CodInt.ToString();
                }
            }
            return "0001";
        }
        public void GetArmazens(ErpBS BSO, ComboBox cmbArmazens)
        {
            _bso = BSO;
            StdBELista lista = _bso.Consulta("select Armazem from armazens");
            while (!lista.Vazia() && !lista.NoFim())
            {
                cmbArmazens.Items.Add(lista.Valor(0));
                lista.Seguinte();  
            }
        }
        public string GetLocalizacaoArmazens(ErpBS BSO, string cmbArmazens)
        {
            _bso = BSO;
            StdBELista lista = _bso.Consulta("select Localizacao from ArmazemLocalizacoes where Armazem = '" + cmbArmazens + "'");
            if (!lista.Vazia())
            {
                string txtLocalizacao = lista.Valor(0);
                return txtLocalizacao;
            }
            return string.Empty;
        }

        public string SaveDados(ErpBS BSO, StdBSInterfPub PSO, string SMS, string txtArtigo, string txtDescricao, string cmbMoeda, string cmbArmazens, string txtLocalizacaoArm, string cmbTipo, string txtUnidade)
        {
            _bso = BSO; _pso = PSO;
            string SMS2 = "Erro ao gravar o Artigo: '" + txtArtigo + "'!";
            StdBELista lista = _bso.Consulta("select Artigo from artigo where Artigo = '" + txtArtigo + "'");
            if (!lista.Vazia())
            {
                string artigo = "Artigo '" + txtArtigo + "' já existe!";
                return artigo;
            }
            else
            {
                try
                {
                    BasBEArtig = new BasBEArtigo();
                    BasBEArtig.Artigo = txtArtigo;
                    BasBEArtig.Descricao = txtDescricao;
                    BasBEArtig.ArmazemSugestao = cmbArmazens;
                    BasBEArtig.UnidadeBase = txtUnidade;
                    BasBEArtig.UnidadeVenda = txtUnidade;
                    BasBEArtig.UnidadeEntrada = txtUnidade;
                    BasBEArtig.UnidadeSaida = txtUnidade;
                    BasBEArtig.UnidadeCompra = txtUnidade;
                    BasBEArtig.TipoArtigo = cmbTipo;
                    BasBEArtig.FactorUnidadeSup = 1;
                    BasBEArtig.IVA = "14";
                    BasBEArtig.DeduzIVA = true;
                    BasBEArtig.PercIncidenciaIVA = 100;
                    BasBEArtig.PercIvaDedutivel = 100;

                    _bso.Base.Artigos.Actualiza(BasBEArtig);
                    return SMS;
                }
                catch (Exception ex)
                {
                    _pso.MensagensDialogos.MostraAviso(SMS2, StdBSTipos.IconId.PRI_Informativo);
                    return SMS2;
                }
            }
        }
    }
}
