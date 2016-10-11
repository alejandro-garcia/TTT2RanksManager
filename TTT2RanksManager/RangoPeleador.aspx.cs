using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TTT2RanksManager
{
    public partial class RangoPeleador : System.Web.UI.Page
    {
        string currentRankId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                llenarCombos();
        }

        protected void ddlPeleador_SelectedIndexChanged(object sender, EventArgs e)
        {
            string rankId = string.Empty;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();


                    DataSet rankDataset = new DataSet();

                    string charId = ddlPeleador.SelectedValue.ToString();

                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT rankId FROM TTTRanks.dbo.RangoXChar WHERE charId = " + charId, conn);
                    adapter.Fill(rankDataset);
                    rankId = rankDataset.Tables[0].Rows[0]["rankId"].ToString();
                    adapter.Dispose();
                    rankDataset.Dispose();
                }
                catch
                {
                }
            }

            if (!string.IsNullOrEmpty(rankId))
            {
                ddlRango.ClearSelection();

                currentRankId = rankId;
                ddlRango.Items.FindByValue(rankId).Selected = true;

                //ddlRango.SelectedIndex = ddlRango.Items.IndexOf(ddlRango.Items.FindByValue(rankId));

                

                //ddlRango.Items.FindByValue(rankId).Selected = true;
                
                //ddlRango.SelectedValue = rankId;
                //ddlRango.Items[ddlRango.Items.IndexOf(ddlRango.Items.FindByValue(rankId))].Selected = true;
                
                
            }
        }

        private void llenarCombos()
        {
            DataTable datosPeleador = new DataTable();
            DataTable rangos = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT charId, name FROM TTTRanks.dbo.Peleadores ORDER BY name", conn);
                    adapter.Fill(datosPeleador);

                    ddlPeleador.DataSource = datosPeleador;
                    ddlPeleador.DataTextField = "name";
                    ddlPeleador.DataValueField = "charId";
                    ddlPeleador.DataBind();

                    adapter.Dispose();

                    adapter = new SqlDataAdapter("SELECT rankId, nombre FROM TTTRanks.dbo.Rangos ORDER BY rankId", conn);
                    adapter.Fill(rangos);

                    //foreach (DataRow row in rangos.Tables[0].Rows)
                    //{
                    //    ddlRango.Items.Add(new ListItem(row["nombre"].ToString(), row["rankId"].ToString()));
                    //}
                    ddlRango.DataSource = rangos;
                    ddlRango.DataTextField = "nombre";
                    ddlRango.DataValueField = "rankId";
                    ddlRango.DataBind();
                }
                catch
                {

                }
                finally
                {
                    datosPeleador.Dispose();
                    rangos.Dispose();
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ddlPeleador.SelectedValue == null || ddlRango.SelectedValue == null)
                return;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string strSQL = "dbo.p_ins_rango_peleador";

                    SqlCommand oCmd = new SqlCommand(strSQL, conn);
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.Parameters.Add("@charId", SqlDbType.Int);
                    oCmd.Parameters.Add("@rankId", SqlDbType.Int);
                    oCmd.Parameters["@charId"].Value = ddlPeleador.SelectedValue;
                    oCmd.Parameters["@rankId"].Value = ddlRango.SelectedValue; 

                    int recs = oCmd.ExecuteNonQuery();

                    if (recs != 0)
                       ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Se actualizo el registro exitosamente.')", true);                   
                }
                catch
                {

                }
            }
        }

        protected void txtPuntos_TextChanged(object sender, EventArgs e)
        {

        }
    }
}