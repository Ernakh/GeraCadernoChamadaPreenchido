using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using System.Data;

namespace CadernoChamadaCompleto
{
    public class GeraCaderno
    {
        BaseFont bf = null;

        public MemoryStream ExecuteResult(string id_turma, string periodo)
        {
            iTextSharp.text.Rectangle psize = new iTextSharp.text.Rectangle(1024, 1500);
            Document documento = new Document(psize.Rotate(), 50, 50, 260, 50);
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(documento, ms);

            StringReader strReader = new StringReader(GetHTMLAvaliacoes(id_turma));
            HTMLWorker objeto = new HTMLWorker(documento);

            documento.Open();

            PdfContentByte cb = writer.DirectContent;
            bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Rectangle pageSize = documento.PageSize;

            Banco bd = new Banco();

            Dados dd = new Dados();
            dd.Carga = 80;
            dd.Cod_curso = 7;
            dd.Cod_disciplina = "1";
            dd.Creditos = 4;
            dd.Docente = "Fabrício Londero";
            dd.Ds_curso = "Administração";
            dd.Ds_disciplina = "Informática aplicada";
            dd.Ds_periodo = "G201601";
            dd.Nr_turma = 1;
            //**********************************************************************
            dd.Situacao = "ENCERRADA";
            dd.Versao = "DEFINITIVA";

            //AVALIAÇÕES
            PdfHelper pdfHelper = new PdfHelper(dd);
            writer.PageEvent = pdfHelper;

            objeto.Parse(strReader);

            //HORARIOS
            documento.NewPage();

            StringReader strReader2 = new StringReader(GetHTMLHorarios(id_turma, "1"));
            objeto.Parse(strReader2);
            
            //PLANO DE ENSINO
            documento.NewPage();

            StringReader strReader3 = new StringReader(GetHTMLPlano(id_turma));
            objeto.Parse(strReader3);

            documento.Close();

            return ms;
        }

        public string GetHTMLAvaliacoes(string turma)
        {
            Banco bd = new Banco();
            DataSet ds = new DataSet();

            ds = bd.executeQuery(@"SELECT CD_CURSO_CURR, ID_MATRICULA, NM_PESSOA, QT_FALTAS, VL_PRI_NOTA, VL_SEG_NOTA,
			                                    ((VL_PRI_NOTA + VL_SEG_NOTA)/2) AS media, VL_EXAME, VL_NOTA, DS_TABELA_ITEM
                                    FROM tabela ");

            StringBuilder htmlString = new StringBuilder();
            htmlString.AppendLine("<html>");
            htmlString.AppendLine("<body>");

            htmlString.AppendLine("<div align=\"center\">");
            htmlString.AppendLine("<table border=\"0\"><tr><td bgcolor=\"#BFBFBF\">");

            htmlString.AppendLine("<b>AVALIAÇÕES</b>");
            htmlString.AppendLine("</td></tr></table>");
            htmlString.AppendLine("</div>");
            htmlString.AppendLine("<br />");

            htmlString.AppendLine("<div align=\"left\">");
            htmlString.AppendLine("<table border=\"0\">");
            htmlString.AppendLine("<tr align=\"center\" bgcolor=\"#BFBFBF\" >");
            htmlString.AppendLine("<th width=\"5%\" >Nº</th>");
            htmlString.AppendLine("<th width=\"5%\" >CURSO</th>");
            htmlString.AppendLine("<th width=\"10%\">MATRÍCULA</th>");
            htmlString.AppendLine("<th width=\"25%\" >ALUNO</th>");
            htmlString.AppendLine("<th>FALTAS</th>");
            htmlString.AppendLine("<th>1ª AVALIAÇÃO</th>");
            htmlString.AppendLine("<th>2ª AVALIAÇÃO</th>");
            htmlString.AppendLine("<th>MÉDIA PARCIAL</th>");
            htmlString.AppendLine("<th>EXAME</th>");
            htmlString.AppendLine("<th>MÉDIA FINAL</th>");
            htmlString.AppendLine("<th width=\"15%\" >SITUAÇÃO</th>");
            htmlString.AppendLine("</tr>");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                htmlString.AppendLine("<font color=\"#000\" face=\"verdana\"  size=\"2\">");

                if(i % 2 == 0)
                    htmlString.AppendLine("<tr>");
                else
                    htmlString.AppendLine("<tr bgcolor=\"#F2F2F2\" >");

                htmlString.AppendLine("<td align=\"center\" >" + (i + 1) + "</td>");
                htmlString.AppendLine("<td align=\"center\" >  " + ds.Tables[0].Rows[i][0].ToString() + "</td>");
                htmlString.AppendLine("<td align=\"center\" >  " + ds.Tables[0].Rows[i][1].ToString() + "</td>");
                htmlString.AppendLine("<td>  " + ds.Tables[0].Rows[i][2].ToString() + "</td>");
                htmlString.AppendLine("<td align=\"center\">" + ds.Tables[0].Rows[i][3].ToString() + "</td>");
                htmlString.AppendLine("<td align=\"center\">" + ds.Tables[0].Rows[i][4].ToString() + "</td>");
                htmlString.AppendLine("<td align=\"center\">" + ds.Tables[0].Rows[i][5].ToString() + "</td>");
                htmlString.AppendLine("<td align=\"center\">" + ds.Tables[0].Rows[i][6].ToString().Substring(0, ds.Tables[0].Rows[i][6].ToString().LastIndexOf(',') + 2) + "</td>");
                htmlString.AppendLine("<td align=\"center\">" + ds.Tables[0].Rows[i][7].ToString() + "</td>");
                htmlString.AppendLine("<td align=\"center\">                " + ds.Tables[0].Rows[i][8].ToString() + "</td>");
                htmlString.AppendLine("<td>  " + ds.Tables[0].Rows[i][9].ToString() + "</td>");

                htmlString.AppendLine("</tr>");
                htmlString.AppendLine("</font>");
            }

            htmlString.AppendLine("</table>");
            htmlString.AppendLine("</div>");
            htmlString.AppendLine("<br />");

            htmlString.AppendLine("</body>");
            htmlString.AppendLine("</html>");

            return htmlString.ToString();

            //ExecuteResult(htmlString.ToString());
        }

        public string GetHTMLHorarios(string turma, string semestre)
        {
            StringBuilder htmlString = new StringBuilder();
            htmlString.AppendLine("<html>");
            htmlString.AppendLine("<body>");

            htmlString.AppendLine("<div align=\"center\">");
            htmlString.AppendLine("<table border=\"1\"><tr><td bgcolor=\"#BFBFBF\">");

            Banco bd = new Banco();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds = bd.executeQuery(@"
                SELECT TP_SEMANA, cont
                FROM TURMA_ESP_FIS_HOR 
            ");

            dt = BuscaHorarios(turma);

            //HORARIO
            htmlString.AppendLine("<b>HORÁRIOS DA TURMA</b>");
            htmlString.AppendLine("</td></tr></table>");
            htmlString.AppendLine("</div>");
            htmlString.AppendLine("<br />");

            htmlString.AppendLine("<div align=\"left\">");
            htmlString.AppendLine("<table border=\"1\">");
            htmlString.AppendLine("<tr bgcolor=\"#BFBFBF\" >");
            htmlString.AppendLine("<th align=\"center\" >DIA DA SEMANA</th>");
            htmlString.AppendLine("<th align=\"center\" >Nº DE CRÉDITOS*</th>");
            htmlString.AppendLine("<th align=\"center\" >HORÁRIO INICIAL</th>");
            htmlString.AppendLine("<th align=\"center\" >HORÁRIO FINAL</th>");
            htmlString.AppendLine("</tr>");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                htmlString.AppendLine("<font color=\"#000\" face=\"verdana\"  size=\"2\">");

                if (i % 2 == 0)
                    htmlString.AppendLine("<tr align=\"center\" >");
                else
                    htmlString.AppendLine("<tr bgcolor=\"#F2F2F2\" align=\"center\" >");
                               

                if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "SEG")
                {
                    htmlString.AppendLine("<td>SEGUNDA-FEIRA</td>");
                }
                else if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "TER")
                {
                    htmlString.AppendLine("<td>TERÇA-FEIRA</td>");
                }
                else if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "QUA")
                {
                    htmlString.AppendLine("<td>QUARTA-FEIRA</td>");
                }
                else if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "QUI")
                {
                    htmlString.AppendLine("<td>QUINTA-FEIRA</td>");
                }
                else if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "SEX")
                {
                    htmlString.AppendLine("<td>SEXTA-FEIRA</td>");
                }
                else if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "SAB")
                {
                    htmlString.AppendLine("<td>SÁBADO</td>");
                }

                htmlString.AppendLine("<td>" + ds.Tables[0].Rows[i]["cont"].ToString() + "</td>");

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "SEG" && dt.Rows[j]["TP_SEMANA"].ToString() == "Segunda")
                    {
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_INICIAL"].ToString() + "</td>");
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_FINAL"].ToString() + "</td>");
                    }
                    else if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "TER" && dt.Rows[j]["TP_SEMANA"].ToString() == "Terça")
                    {
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_INICIAL"].ToString() + "</td>");
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_FINAL"].ToString() + "</td>");
                    }
                    else if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "QUA" && dt.Rows[j]["TP_SEMANA"].ToString() == "Quarta")
                    {
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_INICIAL"].ToString() + "</td>");
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_FINAL"].ToString() + "</td>");
                    }
                    else if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "QUI" && dt.Rows[j]["TP_SEMANA"].ToString() == "Quinta")
                    {
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_INICIAL"].ToString() + "</td>");
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_FINAL"].ToString() + "</td>");
                    }
                    else if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "SEX" && dt.Rows[j]["TP_SEMANA"].ToString() == "Sexta")
                    {
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_INICIAL"].ToString() + "</td>");
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_FINAL"].ToString() + "</td>");
                    }
                    else if (ds.Tables[0].Rows[i]["TP_SEMANA"].ToString() == "SAB" && dt.Rows[j]["TP_SEMANA"].ToString() == "Sábado")
                    {
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_INICIAL"].ToString() + "</td>");
                        htmlString.AppendLine("<td>" + dt.Rows[j]["HR_FINAL"].ToString() + "</td>");
                    }
                }

                htmlString.AppendLine("</tr>");
                htmlString.AppendLine("</font>");
            }

            htmlString.AppendLine("<font color=\"#000\" face=\"verdana\"  size=\"2\">");
            htmlString.AppendLine("<b>Observação: </b>* Uma unidade de crédito é equivalente a dezessete horas-aula.");
            htmlString.AppendLine("</font>");

            htmlString.AppendLine("</table>");
            htmlString.AppendLine("<br />");
            htmlString.AppendLine("<br />");

            //PRESENÇAS
            htmlString.AppendLine("<table border=\"1\"><tr><td bgcolor=\"#BFBFBF\">");

            htmlString.AppendLine("<b>PRESENÇAS</b>");
            htmlString.AppendLine("</td></tr></table>");
            htmlString.AppendLine("</div>");
            htmlString.AppendLine("<br />");

            htmlString.AppendLine("<div align=\"left\">");
            htmlString.AppendLine("<table border=\"1\">");
            htmlString.AppendLine("<tr bgcolor=\"#BFBFBF\" >");
            htmlString.AppendLine("<th rowspan='2' align=\"center\" >Nº</th>");
            htmlString.AppendLine("<th rowspan='2' align=\"center\" >MATR.</th>");
            htmlString.AppendLine("<th rowspan='2' align=\"center\" >ALUNO</th>");

            DataSet dsA = new DataSet();
            dsA = bd.executeQuery(@"
                SELECT TURMA_ALUNO.ID_MATRICULA, pessoa_fisica.NM_PESSOA  
                FROM TURMA_ALUNO 
                JOIN ALUNO_CURSO ON ALUNO_CURSO.ID_MATRICULA = TURMA_ALUNO.ID_MATRICULA
                JOIN PESSOA_FISICA ON PESSOA_FISICA.ID_PESSOA = ALUNO_CURSO.CD_ALUNO
                WHERE ID_TURMA = " + turma + @"
                ORDER BY 2
            ");

            bool flag = true;

            for (int i = 0; i < dsA.Tables[0].Rows.Count; i++)
            {
                DataSet dsP = new DataSet();
                dsP = bd.executeQuery(@"
                    SELECT * FROM TURMA_CADERNO 
                    WHERE CD_TURMA = " + turma + @" AND CD_MATRICULA = " + dsA.Tables[0].Rows[i]["ID_MATRICULA"].ToString() + @"
                    ORDER BY DT_AULA
                ");

                //dias
                if (flag)
                {
                    int colspan = 0;

                    DateTime data = DateTime.Parse(dsP.Tables[0].Rows[0]["dt_aula"].ToString());

                    string[] meses = { "JAN", "FEV", "MAR", "ABR", "MAI", "JUN", "JUL", "AGO", "SET", "OUT", "NOV", "DEZ" };

                    for (int y = 0; y < dsP.Tables[0].Rows.Count; y++)
                    {
                        if (data.Month == DateTime.Parse(dsP.Tables[0].Rows[y]["dt_aula"].ToString()).Month)
                        {
                            colspan++;
                        }
                        else
                        {
                            htmlString.AppendLine("<th colspan='" + colspan + "' align=\"center\" >" + meses[data.Month - 1] + "</th>");

                            data = DateTime.Parse(dsP.Tables[0].Rows[y]["dt_aula"].ToString());
                            colspan = 1;
                        }
                    }

                    data = DateTime.Parse(dsP.Tables[0].Rows[dsP.Tables[0].Rows.Count - 1]["dt_aula"].ToString());
                    htmlString.AppendLine("<th colspan='" + colspan + "' align=\"center\" >" + meses[data.Month - 1] + "</th>");

                    htmlString.AppendLine("</tr>");
                    htmlString.AppendLine("<tr bgcolor=\"#BFBFBF\" >");
                                        
                    for (int y = 0; y < dsP.Tables[0].Rows.Count; y++)
                    {
                        htmlString.AppendLine("<th align=\"center\" >" + DateTime.Parse(dsP.Tables[0].Rows[y]["dt_aula"].ToString()).Day + "</th>");
                    }

                    htmlString.AppendLine("</tr>");
                                        
                    flag = false;
                }

                //dados
                htmlString.AppendLine("<font color=\"#000\" face=\"verdana\" size=\"2\">");

                if (i % 2 == 0)
                    htmlString.AppendLine("<tr>");
                else
                    htmlString.AppendLine("<tr bgcolor=\"#F2F2F2\" >");

                htmlString.AppendLine("<td align=\"center\" width=\"2%\" >" + (i + 1) + "</td>");
                htmlString.AppendLine("<td width=\"5%\" >" + dsA.Tables[0].Rows[i]["ID_MATRICULA"].ToString() + @"</td>");
                htmlString.AppendLine("<td width=\"10%\" >" + dsA.Tables[0].Rows[i]["nm_pessoa"].ToString() + @"</td>");

                for (int j = 0; j < dsP.Tables[0].Rows.Count; j++)
                {
                    htmlString.AppendLine("<td align=\"center\" >");

                    for (int x = 4; x < 14; x++)
                    {
                        if (dsP.Tables[0].Rows[j][x] != null && dsP.Tables[0].Rows[j][x].ToString() != "")
                        {
                            htmlString.AppendLine(dsP.Tables[0].Rows[j][x].ToString() == "1" ? "F" : ".");
                        }
                        else
                        {
                            continue;
                        }
                    }

                    htmlString.AppendLine("</td>");
                }

                htmlString.AppendLine("</tr>");
                htmlString.AppendLine("</font>");
            }

            htmlString.AppendLine("</table>");

            htmlString.AppendLine("</div>");
            htmlString.AppendLine("<br />");

            htmlString.AppendLine("</body>");
            htmlString.AppendLine("</html>");

            return htmlString.ToString();

            //ExecuteResult(htmlString.ToString());
        }

        public string GetHTMLPlano(string turma)
        {
            StringBuilder htmlString = new StringBuilder();
            htmlString.AppendLine("<html>");
            htmlString.AppendLine("<body>");
            
            htmlString.AppendLine("<div align=\"center\">");
            htmlString.AppendLine("<table border=\"1\"><tr><td bgcolor=\"#BFBFBF\" >");

            htmlString.AppendLine("<b>PLANO DE ENSINO</b>");
            htmlString.AppendLine("</td></tr></table>");
            htmlString.AppendLine("</div>");
            htmlString.AppendLine("<br />");

            htmlString.AppendLine("<div align=\"left\">");
            htmlString.AppendLine("<table border=\"1\">");
            htmlString.AppendLine("<tr bgcolor=\"#BFBFBF\" >");
            htmlString.AppendLine("<th align=\"center\" >Data</th>");
            htmlString.AppendLine("<th align=\"center\" >Conteúdo Lecionado</th>");
            htmlString.AppendLine("</tr>");

            Banco bd = new Banco();
            DataSet ds = new DataSet();
            ds = bd.executeQuery("select * from TURMA_CADERNO_CONTEUDO WHERE CD_TURMA = " + turma);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                htmlString.AppendLine("<font color=\"#000\" face=\"verdana\"  size=\"3\">");

                if (i % 2 == 0)
                    htmlString.AppendLine("<tr>");
                else
                    htmlString.AppendLine("<tr bgcolor=\"#F2F2F2\" >");

                htmlString.AppendLine("<td width=\"6%\" >" + ds.Tables[0].Rows[i]["dt_aula"].ToString().Substring(0, ds.Tables[0].Rows[i]["dt_aula"].ToString().LastIndexOf(' ')) + "</td>");
                htmlString.AppendLine("<td>  " + ds.Tables[0].Rows[i]["ds_conteudo"].ToString() + "</td>");

                htmlString.AppendLine("</tr>");
                htmlString.AppendLine("</font>");
            }

            htmlString.AppendLine("</table>");

            htmlString.AppendLine("</div>");
            htmlString.AppendLine("<br />");

            htmlString.AppendLine("</body>");
            htmlString.AppendLine("</html>");

            return htmlString.ToString();

            //ExecuteResult(htmlString.ToString());
        }

        public DataTable BuscaHorarios(string turma)
        {
            try
            {
                Banco bd = new Banco();
                DataSet ds = new DataSet();
                string sql = @"SELECT CASE WHEN TP_SEMANA = 'SEG' THEN 'Segunda'
                            WHEN TP_SEMANA = 'TER' THEN 'Terça'
                            WHEN TP_SEMANA = 'QUA' THEN 'Quarta'
                            WHEN TP_SEMANA = 'QUI' THEN 'Quinta'
                            WHEN TP_SEMANA = 'SEX' THEN 'Sexta'
                            WHEN TP_SEMANA = 'SAB' THEN 'Sábado'
                    END TP_SEMANA,
                    CONVERT(VARCHAR(5), H.HR_INICIAL, 108) AS HR_INICIAL, 
                    CONVERT(VARCHAR(5), H.HR_FINAL, 108) AS HR_FINAL,
                    T.CD_DISCIPLINA, DIS.DS_DISCIPLINA, T.NR_TURMA,
                    CASE    WHEN TP_SEMANA = 'SEG' THEN 1
                            WHEN TP_SEMANA = 'TER' THEN 2
                            WHEN TP_SEMANA = 'QUA' THEN 3	
                            WHEN TP_SEMANA = 'QUI' THEN 4
                            WHEN TP_SEMANA = 'SEX' THEN 5
                            WHEN TP_SEMANA = 'SAB' THEN 6
                    END AS SEMANA
                    FROM TURMA T
                    INNER JOIN DISCIPLINA DIS ON DIS.ID_DISCIPLINA = T.CD_DISCIPLINA
                    INNER JOIN TURMA_ESP_FIS_HOR TEFH ON TEFH.CD_TURMA = T.ID_TURMA
                    INNER JOIN HORARIO H ON H.ID_HORARIO = TEFH.CD_HORARIO
                    INNER JOIN PESSOA_FISICA PF ON PF.ID_PESSOA = TEFH.CD_PROFESSOR                           
                    INNER JOIN ALUNO_REALIZADO AR ON AR.CD_TURMA = T.ID_TURMA
                    WHERE AR.CD_TURMA = " + turma + @"
                    ORDER BY SEMANA, HR_INICIAL";
                ds = bd.executeQuery(sql);

                DataTable dtHorAluno = new DataTable();
                dtHorAluno.Columns.Add("TP_SEMANA");
                dtHorAluno.Columns.Add("HR_INICIAL");
                dtHorAluno.Columns.Add("HR_FINAL");
                dtHorAluno.Columns.Add("CD_DISCIPLINA");
                dtHorAluno.Columns.Add("DS_DISCIPLINA");
                dtHorAluno.Columns.Add("NR_TURMA");

                string disciplina = "";
                string ds_disciplina = "";
                string nrturma = "";
                string dia = "";
                string hr_inicio = "";
                string hr_fim = "";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (disciplina != ds.Tables[0].Rows[i]["CD_DISCIPLINA"].ToString())
                    {
                        if (i > 0)
                        {
                            dtHorAluno.Rows.Add(dia, hr_inicio, hr_fim, disciplina, ds_disciplina, nrturma);
                        }

                        disciplina = ds.Tables[0].Rows[i]["CD_DISCIPLINA"].ToString();
                        ds_disciplina = ds.Tables[0].Rows[i]["DS_DISCIPLINA"].ToString();
                        nrturma = ds.Tables[0].Rows[i]["NR_TURMA"].ToString();
                        dia = ds.Tables[0].Rows[i]["TP_SEMANA"].ToString();
                        hr_inicio = ds.Tables[0].Rows[i]["HR_INICIAL"].ToString();
                        hr_fim = ds.Tables[0].Rows[i]["HR_FINAL"].ToString();
                        if (i + 1 == ds.Tables[0].Rows.Count)
                        {
                            dtHorAluno.Rows.Add(dia, hr_inicio, hr_fim, disciplina, ds_disciplina, nrturma);
                        }
                    }
                    else
                    {
                        if (dia != ds.Tables[0].Rows[i]["TP_SEMANA"].ToString())
                        {
                            if (i > 0)
                            {
                                dtHorAluno.Rows.Add(dia, hr_inicio, hr_fim, disciplina, ds_disciplina, nrturma);
                            }
                            dia = ds.Tables[0].Rows[i]["TP_SEMANA"].ToString();
                            hr_inicio = ds.Tables[0].Rows[i]["HR_INICIAL"].ToString();
                            hr_fim = ds.Tables[0].Rows[i]["HR_FINAL"].ToString();
                        }
                        else
                        {
                            hr_fim = ds.Tables[0].Rows[i]["HR_FINAL"].ToString();
                        }

                        if (i + 1 == ds.Tables[0].Rows.Count)
                        {
                            dtHorAluno.Rows.Add(dia, hr_inicio, hr_fim, disciplina, ds_disciplina, nrturma);
                        }
                    }
                }

                return dtHorAluno;
            }
            catch (Exception)
            {
                throw new Exception("Erro"); ;
            }
        }
    }
}