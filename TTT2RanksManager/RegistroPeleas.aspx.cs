using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlTypes;

namespace TTT2RanksManager
{
    public partial class RegistroPeleas : System.Web.UI.Page
    {
        int rivalID = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //todo: colocar validaciones de campos

           string rank1Id = string.Empty;
           if (string.IsNullOrEmpty(hfRivalRank1Id.Value))
               rank1Id = buscarIdRankPorNombre(txtRivalRank1.Text);
           else
               rank1Id = hfRivalRank1Id.Value;

           string rank2Id = string.Empty;
           if (string.IsNullOrEmpty(hfRivalRank2Id.Value))
               rank2Id = buscarIdRankPorNombre(txtRivalRank2.Text);
           else
               rank2Id = hfRivalRank2Id.Value;

           generarDatosRival(txtNombreRival.Text, rank1Id, rank2Id);           

           using (SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
               if (oConn.State == ConnectionState.Closed)
                   oConn.Open();

               string strSQL = "dbo.p_ins_pelea";
               SqlCommand oCmd = new SqlCommand(strSQL, oConn);
               oCmd.CommandType = CommandType.StoredProcedure;

               //formateo de la fecha.
               DateTime dt = DateTime.MinValue;
               DateTime.TryParseExact(datepicker.Text, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
               SqlDateTime fecha = new SqlDateTime(dt);
               oCmd.Parameters.AddWithValue("@fecha", fecha);

               ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('dt: ' "  + dt.ToString("MM/dd/yyyy") + ")", true);

               string hfPeleador1Value = Request.Form["hfPeleador1"];

               if (!string.IsNullOrEmpty(hfPeleador1Value) && hfPeleador1Value != "0")
               {
                   oCmd.Parameters.AddWithValue("@char1Id", hfPeleador1Value);
               }
               else
               {
                   string valor = buscarIdPeleadorPorNombre(txtPeleador1.Text);
                   oCmd.Parameters.AddWithValue("@char1Id", valor);
               }


               if (!string.IsNullOrEmpty(hfPeleador2.Value) && hfPeleador2.Value != "0")
               {
                   oCmd.Parameters.AddWithValue("@char2Id", this.hfPeleador2.Value);
               }
               else
               {
                   string valor = buscarIdPeleadorPorNombre(txtPeleador2.Text);
                   oCmd.Parameters.AddWithValue("@char2Id", valor);
               }

               oCmd.Parameters.AddWithValue("@rivalId", rivalID);
               oCmd.Parameters.AddWithValue("@puntos",txtPuntos.Text); 
               oCmd.Parameters.AddWithValue("@promoBattle", rbAscenso1.Checked ? "Y":"N");
               oCmd.Parameters.AddWithValue("@demoBattle", rbDescenso1.Checked ? "Y":"N");

               if (!string.IsNullOrEmpty(hfRivalRank1Id.Value) && hfRivalRank1Id.Value != "0")
               {
                   oCmd.Parameters.AddWithValue("@rivalRank1", hfRivalRank1Id.Value);
               }
               else
               {
                   oCmd.Parameters.AddWithValue("@rivalRank1", buscarIdRankPorNombre(txtRivalRank1.Text));
               }

               if (!string.IsNullOrEmpty(hfRivalRank2Id.Value) && hfRivalRank2Id.Value != "0")
               {
                   oCmd.Parameters.AddWithValue("@rivalRank2", hfRivalRank2Id.Value);
               }
               else
               {
                   oCmd.Parameters.AddWithValue("@rivalRank2", buscarIdRankPorNombre(txtRivalRank2.Text));
               }

               oCmd.Parameters.AddWithValue("@promoBattle2", rbAscenso2.Checked ? "Y":"N");
               oCmd.Parameters.AddWithValue("@demoBattle2", rbDescenso2.Checked ? "Y":"N");


               if (!string.IsNullOrEmpty(hfRivalPeleador1Id.Value) && hfRivalPeleador1Id.Value != "0")
               {
                   oCmd.Parameters.AddWithValue("@rivalChar1Id", hfRivalPeleador1Id.Value);
               }
               else
               {
                   oCmd.Parameters.AddWithValue("@rivalChar1Id", buscarIdPeleadorPorNombre (TxtRivalPeleador1.Text));
               }

               if (!string.IsNullOrEmpty(hfRivalPeleador2Id.Value) && hfRivalPeleador2Id.Value != "0")
               {
                   oCmd.Parameters.AddWithValue("@rivalChar2Id", hfRivalPeleador2Id.Value);
               }
               else
               {
                   oCmd.Parameters.AddWithValue("@rivalChar2Id", buscarIdPeleadorPorNombre (TxtRivalPeleador2.Text));
               }               

               int recs = oCmd.ExecuteNonQuery();

               if (recs != 0)
               {
                   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Se actualizo el registro exitosamente.')", true);
                   txtPuntos.Text = string.Empty;
                   rbNoAplica1.Checked = true;
                   rbNoAplica2.Checked = true;
                   MostrarEstadisticasDia();
               }	                         
            }
        }

        private void buscarUltimoRankRival(string rivalPsn)
        {
            bool isFound = false;
            DataSet rivalDataSet = new DataSet();
            string rivalId = string.Empty;
            int rivalRank1 = -1;
            int rivalRank2 = -1;
            string rivalRank1Name = string.Empty;
            string rivalRank2Name = string.Empty;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT rivalId FROM TTTRanks.dbo.Rivales WHERE lower(psn) = '" + rivalPsn + "'", conn);
                    adapter.Fill(rivalDataSet);
                    rivalId = rivalDataSet.Tables[0].Rows[0]["rivalId"].ToString();
                    adapter.Dispose();
                    rivalDataSet.Dispose();
                    isFound = true;
                }
                catch (IndexOutOfRangeException)
                { 
                   
                }
                finally
                {
                    rivalDataSet.Dispose();
                }

                if (!isFound)
                    return;

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT TOP 1 p.rivalRank1, r1.nombre rivalRank1Name, p.rivalRank2, r2.nombre rivalRank2Name FROM TTTRanks.dbo.Peleas p LEFT JOIN Rangos r1 ON r1.rankId = rivalRank1 LEFT JOIN Rangos r2 ON r2.rankId = rivalRank2 WHERE rivalId = '" + rivalId + "' ORDER BY peleaId DESC", conn);
                    adapter.Fill(rivalDataSet);
                    int.TryParse(rivalDataSet.Tables[0].Rows[0]["rivalRank1"].ToString(), out rivalRank1);
                    int.TryParse(rivalDataSet.Tables[0].Rows[0]["rivalRank2"].ToString(), out rivalRank2);
                    rivalRank1Name = rivalDataSet.Tables[0].Rows[0]["rivalRank1Name"].ToString();
                    rivalRank1Name = rivalDataSet.Tables[0].Rows[0]["rivalRank2Name"].ToString();

                    adapter.Dispose();

                    if (rivalRank1 != -1)
                        hfRivalRank1Id.Value = rivalRank1.ToString();
                        //ddlRivalRank1.Items.FindByValue(rivalRank1.ToString()).Selected = true;

                    if (rivalRank2 != -1)
                        hfRivalRank2Id.Value = rivalRank2.ToString();
                        //ddlRivalRank2.Items.FindByValue(rivalRank2.ToString()).Selected = true;

                    if (!string.IsNullOrEmpty(rivalRank1Name))
                        this.txtRivalRank1.Text = rivalRank1Name;

                    if (!string.IsNullOrEmpty(rivalRank2Name))
                        this.txtRivalRank2.Text = rivalRank2Name;
                }
                catch (System.Exception ex)
                {
                	
                }
                finally
                {
                    rivalDataSet.Dispose();
                }
         
            }
        }

        protected void txtPeleador2_OnTextChanged(object sender, EventArgs e)
        {
            if (txtPeleador2.Text.TrimEnd().Length < 3)
                return; 

            if (!string.IsNullOrEmpty(txtPeleador2.Text))
                if (!string.IsNullOrEmpty(hfPeleador2.Value))
                    lblRank2.Text = buscarRankPeleador(this.hfPeleador2.Value);
                else
                    lblRank2.Text = buscarRankPeleadorPorNombre(this.txtPeleador2.Text);
        }

        private string buscarRankPeleadorPorNombre(string nombre)
        {
            string result = string.Empty;
            DataSet rankDataset = new DataSet();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    rankDataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT charId FROM TTTRanks.dbo.Peleadores where lower(name) = '" + nombre.TrimEnd().ToLower() + "'", conn);
                    adapter.Fill(rankDataset);
                    string charId = rankDataset.Tables[0].Rows[0]["charId"].ToString();
                    adapter.Dispose();
                    rankDataset.Dispose();


                    adapter = new SqlDataAdapter("SELECT rankId FROM TTTRanks.dbo.RangoXChar WHERE charId = " + charId, conn);
                    rankDataset = new DataSet();
                    adapter.Fill(rankDataset);
                    string rankId = rankDataset.Tables[0].Rows[0]["rankId"].ToString();
                    adapter.Dispose();
                    rankDataset.Dispose();

                    adapter = new SqlDataAdapter("SELECT nombre FROM TTTRanks.dbo.Rangos WHERE rankId = " + rankId, conn);
                    rankDataset = new DataSet();
                    adapter.Fill(rankDataset);
                    result = rankDataset.Tables[0].Rows[0]["nombre"].ToString();
                    //rankDataset.Dispose();
                }
                catch (IndexOutOfRangeException)
                {
                    result = "Actualize el Rango del Peleador";
                }
                catch
                {
                    result = "-1";
                }
                finally
                {
                    rankDataset.Dispose();
                }
            }

            return result;
        }

        private string buscarRankPeleador(string charId)
        {
            string result = string.Empty;
            DataSet rankDataset = new DataSet();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT rankId FROM TTTRanks.dbo.RangoXChar WHERE charId = " + charId, conn);
                    adapter.Fill(rankDataset);
                    string rankId = rankDataset.Tables[0].Rows[0]["rankId"].ToString();
                    adapter.Dispose();
                    rankDataset.Dispose();

                    adapter = new SqlDataAdapter("SELECT nombre FROM TTTRanks.dbo.Rangos WHERE rankId = " + rankId, conn);
                    rankDataset = new DataSet();
                    adapter.Fill(rankDataset);
                    result = rankDataset.Tables[0].Rows[0]["nombre"].ToString();
                    //rankDataset.Dispose();
                }
                catch (IndexOutOfRangeException)
                {
                    result = "Actualize el Rango del Peleador";
                }
                catch
                {
                    result = "-1";
                }
                finally
                {
                    rankDataset.Dispose();
                }
            }

            return result;
        }


        private string buscarIdPeleadorPorNombre(string nombre)
        {
            string result = string.Empty;
            DataSet rankDataset = new DataSet();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    rankDataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT charId FROM TTTRanks.dbo.Peleadores where lower(name) = '" + nombre.TrimEnd().ToLower() + "'", conn);
                    adapter.Fill(rankDataset);
                    result = rankDataset.Tables[0].Rows[0]["charId"].ToString();
                    adapter.Dispose();
                    rankDataset.Dispose();

                }
                catch
                {
                }
            }

            return result;
        }


        private string buscarIdRankPorNombre(string nombre)
        {
            string result = string.Empty;
            DataSet rankDataset = new DataSet();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    rankDataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT rankId FROM TTTRanks.dbo.Rangos where lower(nombre) = '" + nombre.TrimEnd().ToLower() + "'", conn);
                    adapter.Fill(rankDataset);
                    result = rankDataset.Tables[0].Rows[0]["rankId"].ToString();
                    adapter.Dispose();
                    rankDataset.Dispose();

                }
                catch
                {
                }
            }

            return result;
        }

        private void MostrarEstadisticasDia()
        {
            if (string.IsNullOrEmpty(datepicker.Text))
                return; 

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();


                    string strSQL = "dbo.p_cons_estadisticas_dia";

                    SqlCommand oCmd = new SqlCommand(strSQL, conn);
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.Parameters.Add("@fecha", SqlDbType.VarChar);
                    oCmd.Parameters.Add("@victorias", SqlDbType.Int).Direction = ParameterDirection.Output;
                    oCmd.Parameters.Add("@derrotas", SqlDbType.Int).Direction = ParameterDirection.Output;

                    oCmd.Parameters["@fecha"].Value = datepicker.Text;                 

                    int recs = oCmd.ExecuteNonQuery();

                    if (recs != 0)
                    {
                        lblVictorias.Text = oCmd.Parameters["@victorias"].Value.ToString();
                        lblDerrotas.Text = oCmd.Parameters["@derrotas"].Value.ToString();
                    }
                }
                catch
                {

                }
            }

        }

        protected void txtPeleador1_OnTextChanged(object sender, EventArgs e)
        {
            if (txtPeleador1.Text.TrimEnd().Length < 3)
                return;

            string hfPeleador1Value = Request.Form["hfPeleador1"];

            if (!string.IsNullOrEmpty(txtPeleador1.Text))
                if (!string.IsNullOrEmpty(hfPeleador1Value))
                    lblRank1.Text = buscarRankPeleador(hfPeleador1Value);
                else
                    lblRank1.Text = buscarRankPeleadorPorNombre(this.txtPeleador1.Text);
        }

        private void generarDatosRival(string psn, string rivalRank1Id, string rivalRank2Id)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string strSQL = "dbo.p_ins_rival";

                SqlCommand oCmd = new SqlCommand(strSQL, conn);
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.Add("@rivalName", SqlDbType.VarChar, 50);
                oCmd.Parameters.Add("@rivalId", SqlDbType.Int).Direction = ParameterDirection.Output;
                oCmd.Parameters.Add("@rank1Id", SqlDbType.Int).Direction = ParameterDirection.InputOutput;
                oCmd.Parameters.Add("@rank2Id", SqlDbType.Int).Direction = ParameterDirection.InputOutput;


                oCmd.Parameters["@rivalName"].Value = psn;

                if (!string.IsNullOrEmpty(rivalRank1Id))
                    oCmd.Parameters["@rank1Id"].Value = rivalRank1Id;

                if (!string.IsNullOrEmpty(rivalRank2Id))
                    oCmd.Parameters["@rank2Id"].Value = rivalRank2Id;

                oCmd.ExecuteNonQuery();
              
                rivalID = Convert.ToInt32(oCmd.Parameters["@rivalId"].Value);
            }
        }

        protected void rbAscenso1_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void txtNombreRival_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombreRival.Text))
                return;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                try
                {
                    string strSQL = "dbo.p_cons_datos_ult_pelea";
                    SqlCommand oCmd = new SqlCommand(strSQL, conn);
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.Parameters.Add("@psn", SqlDbType.VarChar, 50);
                    oCmd.Parameters.Add("@rivalRank1", SqlDbType.Int).Direction = ParameterDirection.Output;
                    oCmd.Parameters.Add("@rivalRank1Desc", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    oCmd.Parameters.Add("@rivalRank2", SqlDbType.Int).Direction = ParameterDirection.Output;
                    oCmd.Parameters.Add("@rivalRank2Desc", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    oCmd.Parameters.Add("@rivalChar1Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    oCmd.Parameters.Add("@rivalChar1Desc", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    oCmd.Parameters.Add("@rivalChar2Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    oCmd.Parameters.Add("@rivalChar2Desc", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                    oCmd.Parameters["@psn"].Value = txtNombreRival.Text;

                    oCmd.ExecuteNonQuery();

                    txtRivalRank1.Text = oCmd.Parameters["@rivalRank1Desc"].Value.ToString();
                    hfRivalRank1Id.Value = oCmd.Parameters["@rivalRank1"].Value.ToString();

                    txtRivalRank2.Text = oCmd.Parameters["@rivalRank2Desc"].Value.ToString();
                    hfRivalRank2Id.Value = oCmd.Parameters["@rivalRank2"].Value.ToString();

                    TxtRivalPeleador1.Text = oCmd.Parameters["@rivalChar1Desc"].Value.ToString();
                    hfRivalPeleador1Id.Value = oCmd.Parameters["@rivalChar1Id"].Value.ToString();

                    TxtRivalPeleador2.Text = oCmd.Parameters["@rivalChar2Desc"].Value.ToString();
                    hfRivalPeleador2Id.Value = oCmd.Parameters["@rivalChar2Id"].Value.ToString();
                }
                catch
                {
                
                }
            }
        }

        protected void datepicker_TextChanged(object sender, EventArgs e)
        {
            MostrarEstadisticasDia();
        }
    }
}