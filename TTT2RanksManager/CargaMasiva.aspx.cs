using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTT2RanksManager
{
    public partial class CargaMasiva : System.Web.UI.Page
    {
        private int rivalID; 
        

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void txtCombates_TextChanged(object sender, EventArgs e)
        {

        }


        private string NombreReal(string name)
        {
            string result = string.Empty;



            return result; 
        }

        private bool[] EsPeleaRango(int tipo, string datosPelea, int peleador1, int peleador2)
        {
            bool[] result = new bool[2];
            result[0] = false;
            result[1] = false; 

            string tipoRank = (tipo == 1) ? "ascenso" : "descenso";

            if (datosPelea.IndexOf("(") != -1 && datosPelea.ToLower().IndexOf(tipoRank) != -1)
            {
                string[] temp = datosPelea.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                temp = temp[1].TrimStart().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].Split(",".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);                
                //temp = temp[0].Split(")".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                int idPeleador1, idPeleador2;

                idPeleador1 = ObtenerIdPeleador(temp[0]);

                if (idPeleador1 != 0)
                {
                    if (idPeleador1 == peleador1)
                        result[0] = true;
                    else if (idPeleador1 == peleador2)
                        result[1] = true; 
                }

                if (temp.Length == 2 && temp[1].IndexOf(")") != -1)
                {
                    idPeleador2 = ObtenerIdPeleador(temp[1]);

                    if (idPeleador2 == peleador1)
                        result[0] = true;
                    else if (idPeleador2 == peleador2)
                        result[1] = true; 
                }
            }

            return result; 
        }


        protected void BtnGuardar_Click(object sender, EventArgs e)
        {            
            //if (string.IsNullOrEmpty(datepicker.Text) || ddlPeleador1.SelectedItem == null || ddlPeleador2 == null || string.IsNullOrEmpty(txtCombates.Text))
            //return;

            List<string> ListCombates = txtCombates.Text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            if (ListCombates == null || ListCombates.Count == 0)
                return;

            SqlDateTime? fecha = null;
            int peleador1 = -1;
            int peleador2 = -1;

            foreach (string pelea in ListCombates)
            {
                if (pelea.IndexOf("***") != -1)
                {
                    DateTime dt = DateTime.MinValue;
                    DateTime.TryParseExact(pelea.Split(" ".ToCharArray())[1], "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                    fecha = new SqlDateTime(dt);
                    continue;
                }
                else if (pelea.IndexOf("###") != -1)
                {
                     if (pelea.IndexOf("/") != -1)
                     {
                         string peleadores = pelea.Split(" ".ToCharArray())[1];
                         peleador1 = ObtenerIdPeleador(peleadores.Split("/".ToCharArray())[0]);
                         peleador2 = ObtenerIdPeleador(peleadores.Split("/".ToCharArray())[1]);
                     }
                     continue; 
                }
                else if (string.IsNullOrEmpty(pelea))
                {
                    continue;
                }

                string[] datosPelea = pelea.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (datosPelea.Length == 0)
                    continue;

                string rivalNombre = datosPelea[0];

                string rivalRank1 = datosPelea[1];
                int rivalRank1Id = buscarIdRankPorNombre(rivalRank1);
                int rivalRank2Id = -1;

                bool esSoloMode = false;
                string rivalRank2 = "-1";

                if (ValidarRank(datosPelea[2]))
                {
                    rivalRank2 = datosPelea[2];
                    rivalRank2Id = buscarIdRankPorNombre(rivalRank2);
                }
                else
                    esSoloMode = true;



                generarDatosRival(rivalNombre, rivalRank1Id.ToString(), (esSoloMode) ? "-1" : rivalRank2Id.ToString());

                string rivalFighter1 = string.Empty; 
                string rivalFighter2 = string.Empty; 
                int puntos = 0;

                if (!esSoloMode)
                {
                   rivalFighter1 = datosPelea[3];
                   rivalFighter2 = datosPelea[4];
                   puntos = Int32.Parse(datosPelea[5].Split ("(".ToCharArray())[0]);
                   
                }
                else
                {
                    rivalFighter1 = datosPelea[2];
                    rivalFighter2 = "-1";
                    puntos = Int32.Parse(datosPelea[3].Split ("(".ToCharArray())[0]);
                }

                using (SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
                {
                    if (oConn.State == ConnectionState.Closed)
                        oConn.Open();

                    string strSQL = "dbo.p_ins_pelea";
                    SqlCommand oCmd = new SqlCommand(strSQL, oConn);
                    oCmd.CommandType = CommandType.StoredProcedure;

                    //formateo de la fecha.
                    if (fecha == null)
                    {
                        DateTime dt = DateTime.MinValue;
                        DateTime.TryParseExact(datepicker.Text, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dt);
                        fecha = new SqlDateTime(dt);
                    }
                    oCmd.Parameters.AddWithValue("@fecha", fecha);
                       

                    if (peleador1 != -1)
                        oCmd.Parameters.AddWithValue("@char1Id", peleador1);
                    else
                        oCmd.Parameters.AddWithValue("@char1Id", ddlPeleador1.SelectedValue);

                    if (peleador2 != -1)
                        oCmd.Parameters.AddWithValue("@char2Id", peleador2);
                    else
                    {
                        if (ddlPeleador2.SelectedValue == null || ddlPeleador2.SelectedValue == "0")
                            oCmd.Parameters.AddWithValue("@char2Id", "-1");
                        else
                            oCmd.Parameters.AddWithValue("@char2Id", ddlPeleador2.SelectedValue);
                    }


                    oCmd.Parameters.AddWithValue("@rivalId", rivalID);
                    oCmd.Parameters.AddWithValue("@puntos", puntos);
                    
                    bool[] esAscenso = EsPeleaRango(1, pelea, (peleador1 != -1) ? peleador1: Int32.Parse(ddlPeleador1.SelectedValue), (peleador2 != -1) ? peleador2: Int32.Parse(ddlPeleador2.SelectedValue));

                    bool[] esDescenso = EsPeleaRango(2, pelea, (peleador1 != -1) ? peleador1 : Int32.Parse(ddlPeleador1.SelectedValue), (peleador2 != -1) ? peleador2 : Int32.Parse(ddlPeleador2.SelectedValue));

                    oCmd.Parameters.AddWithValue("@promoBattle", esAscenso[0] ? "Y" : "N");
                    oCmd.Parameters.AddWithValue("@demoBattle", esDescenso[0] ? "Y" : "N");
                    oCmd.Parameters.AddWithValue("@rivalRank1", buscarIdRankPorNombre(rivalRank1));

                    if (rivalRank2 != "-1")
                        oCmd.Parameters.AddWithValue("@rivalRank2", buscarIdRankPorNombre(rivalRank2));
                    else
                        oCmd.Parameters.AddWithValue("@rivalRank2", "-1");


                    oCmd.Parameters.AddWithValue("@promoBattle2", esAscenso[1] ? "Y" : "N");
                    oCmd.Parameters.AddWithValue("@demoBattle2", esDescenso[1] ? "Y" : "N");

                    int idRivalPeleador1 = ObtenerIdPeleador(rivalFighter1);
                    int idRivalPeleador2 = (rivalFighter2 != "-1") ? ObtenerIdPeleador(rivalFighter2) : 0;

                    oCmd.Parameters.AddWithValue("@rivalChar1Id", idRivalPeleador1);

                    if (idRivalPeleador2 != 0)
                    {
                        oCmd.Parameters.AddWithValue("@rivalChar2Id", idRivalPeleador2);
                    }
                    else
                    {
                        oCmd.Parameters.AddWithValue("@rivalChar2Id", -1);
                    }

                    int recs = oCmd.ExecuteNonQuery();
                }



            }
        }

        private int ObtenerIdPeleador(string nombre)
        {
            if (nombre.IndexOf(")") != -1)
                nombre = nombre.Split(")".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];

            int result;
            DataSet fighterDataset = new DataSet();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    fighterDataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT charId FROM TTTRanks.dbo.Peleadores where lower(abbreviation) = '" + nombre.Trim().ToLower() + "' OR lower(name) = '" + nombre.Trim().ToLower() + "'", conn);
                    adapter.Fill(fighterDataset);
                    result = Int32.Parse(fighterDataset.Tables[0].Rows[0]["charId"].ToString());
                    adapter.Dispose();
                    fighterDataset.Dispose();

                }
                catch
                {
                    result = 0;
                }
            }
            return result;
        }

        private bool ValidarRank(string rank)
        {
            bool result = false;

            //rank = FormatearRango(rank);            

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string strSQL = string.Format("SELECT * FROM Rangos WHERE lower(nombre) = '{0}' or abbreviation = '{0}'", rank.Trim());

                SqlCommand oCmd = new SqlCommand(strSQL, conn);
                oCmd.CommandType = CommandType.Text;

                SqlDataReader reader = oCmd.ExecuteReader();

                if (reader.Read())
                    result = true;
                
            }

            return result; 
        }

        private static string FormatearRango(string rank)
        {
            if (rank.Trim().ToLower() == "grandmaster")
                rank = "grand master";
            else if (rank.ToLower().Contains("kyu"))
                rank = rank.ToLower().Replace("kyu", " kyu").Trim();
            else if (rank.ToLower().Contains("dan"))
                rank = rank.ToLower().Replace("dan", " dan").Trim();            
                
            return rank;
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
                else
                    oCmd.Parameters["@rank2Id"].Value = "-1";


                oCmd.ExecuteNonQuery();

                
                rivalID = Convert.ToInt32(oCmd.Parameters["@rivalId"].Value);
            }
        }

        private int buscarIdRankPorNombre(string nombre)
        {
            int result = -1;
            DataSet rankDataset = new DataSet();
            
            //nombre = FormatearRango(nombre);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TekkenCnn"].ConnectionString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    rankDataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(string.Format("SELECT rankId FROM TTTRanks.dbo.Rangos where lower(nombre) = '{0}' or lower(abbreviation) = '{0}'",  nombre.Trim().ToLower()), conn);
                    adapter.Fill(rankDataset);
                    result = Int32.Parse(rankDataset.Tables[0].Rows[0]["rankId"].ToString());
                    adapter.Dispose();
                    rankDataset.Dispose();

                }
                catch
                {
                }
            }

            return result;
        }
        protected void datepicker_TextChanged(object sender, EventArgs e)
        {
            //MostrarEstadisticasDia();
        }
    }
}