using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class Banco
{
    public SqlConnection conexao(string conec)
    {
        SqlConnection cn = new SqlConnection(conec);
        return cn;
    }

    public SqlConnection abrirConexao()
    {
        string conec = "Data Source=localhost;initial Catalog=testes;User ID=xxxx;password=xxxx;language=Portuguese";

        SqlConnection cn = conexao(conec);

        try
        {
            cn.Open();
            return cn;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public void fecharConexao(SqlConnection cn)
    {
        try
        {
            cn.Close();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public DataSet executeQuery(string sql, SqlConnection conec)
    {
        try
        {
            SqlCommand sqlComm = new SqlCommand(sql, conec);
            sqlComm.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(sqlComm);

            DataSet ds = new DataSet();
            da.Fill(ds);
            fecharConexao(conec);
            return ds;
        }
        catch (Exception e)
        {
            fecharConexao(conec);
            throw e;
        }
    }

    public string executeQuerycomUmDado(SqlCommand sqlComm)
    {
        try
        {
            string dado;

            SqlConnection cn = abrirConexao();
            sqlComm.Connection = cn;
            sqlComm.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(sqlComm);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
                dado = dt.Rows[0][0].ToString();
            else
                dado = null;

            fecharConexao(cn);

            return dado;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    
    public string executeQuerycomUmDado(string sql)
    {
        try
        {
            string dado;

            SqlConnection cn = abrirConexao();
            SqlCommand sqlComm = new SqlCommand(sql, cn);
            sqlComm.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(sqlComm);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dado = dt.Rows[0][0].ToString();

            fecharConexao(cn);

            return dado;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    internal DataSet executeQuery(SqlCommand sqlComm)
    {
        try
        {
            sqlComm.ExecuteNonQuery();

            SqlDataAdapter da = new SqlDataAdapter(sqlComm);

            DataSet ds = new DataSet();
            da.Fill(ds);

            fecharConexao(sqlComm.Connection);
            return ds;
        }
        catch (Exception e)
        {
            fecharConexao(sqlComm.Connection);
            throw e;
        }
    }

    public DataSet executeQuery(string sql)
    {
        try
        {
            SqlCommand sqlComm = new SqlCommand(sql, abrirConexao());
            sqlComm.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(sqlComm);

            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}
