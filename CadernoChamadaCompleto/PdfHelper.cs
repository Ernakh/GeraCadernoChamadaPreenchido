using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Web;

namespace CadernoChamadaCompleto
{
    public class PdfHelper : PdfPageEventHelper
    {
        PdfTemplate template;
        BaseFont bf = null;

        Dados dd;

        public PdfHelper(Dados dados)
        {
            dd = dados;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PegaIP ip = new PegaIP();

            base.OnEndPage(writer, document);
            PdfContentByte cb = writer.DirectContent;
            bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            template = cb.CreateTemplate(50, 50);

            Rectangle pageSize = document.PageSize;
            pageSize.Border = 1;


            /*Paragraph paragrafo = new Paragraph(@"
                    SOCIEDADE CARITATIVA E LITERÁRIA SÃO FRANCISCO DE ASSIS - ZN
                    CENTRO UNIVERSITÁRIO FRANCISCANO 
                    Rua dos Andradas, 1614 - Cep. 97010-491 - Santa Maria - RS - Brasil 
                    Fone: (55)3220-1200 - www.unifra.br 

            ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 12f));
            paragrafo.Alignment = Element.ALIGN_CENTER;
            document.Add(paragrafo);

            Paragraph paragrafox = new Paragraph(@"
                    DIÁRIO DE CLASSE
__________________________________________________________________________________________________________________________________________________________________________________________________
            ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 12f));
            paragrafox.Alignment = Element.ALIGN_RIGHT;
            document.Add(paragrafox);*/


            //cb.SetRGBColorFill(100, 100, 100);

            /*StringBuilder htmlString = new StringBuilder();
            htmlString.AppendLine("<html>");*/

            //CABEçALHO
            /*cb.BeginText();
            cb.SetFontAndSize(bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "SOCIEDADE CARITATIVA E LITERÁRIA SÃO FRANCISCO DE ASSIS - ZN", pageSize.GetLeft(830), pageSize.GetTop(50), 0);
            cb.EndText();*/

            cb.BeginText();
            cb.SetFontAndSize(bf, 18);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "FACULDADE INTEGRADA DE SANTA MARIA", pageSize.GetLeft(830), pageSize.GetTop(65), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 18);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Rua dos xxxxx, 1614 - Cep. 88888-491 - Santa Maria - RS - Brasil", pageSize.GetLeft(830), pageSize.GetTop(85), 0);
            cb.EndText();

            /*cb.BeginText();
            cb.SetFontAndSize(bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Fone: (55)3220-1200 - www.unifra.br", pageSize.GetLeft(830), pageSize.GetTop(100), 0);
            cb.EndText();*/

            cb.BeginText();
            cb.SetFontAndSize(bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "DIÁRIO DE CLASSE", pageSize.GetLeft(1420), pageSize.GetTop(115), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "__________________________________________________________________________________________________________________________________________________________________________________________________", pageSize.GetLeft(1420), pageSize.GetTop(130), 0);
            cb.EndText();

            //IMAGEM
            cb.BeginText();
            iTextSharp.text.Image imghead = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/LOGO.png"));
            imghead.SetAbsolutePosition(100, 920);
            //imghead.ScaleToFit(270, 52);
            cb.AddImage(imghead);
            cb.EndText();

            //DADOS CURSO/DISCIPLINA
            //ESQUERDA
            cb.BeginText();
            cb.SetFontAndSize(bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Curso: (" + dd.Cod_curso + ") " + dd.Ds_curso, pageSize.GetLeft(80), pageSize.GetTop(160), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Disciplina: " + dd.Cod_disciplina + " - " + dd.Ds_disciplina, pageSize.GetLeft(80), pageSize.GetTop(185), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Período Letivo: " + dd.Ds_periodo, pageSize.GetLeft(80), pageSize.GetTop(210), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Turma: " + dd.Nr_turma + "           C. Horária: " + dd.Carga + "           Nº de Créditos: " + dd.Creditos + "           Docente: " + dd.Docente, pageSize.GetLeft(80), pageSize.GetTop(235), 0);
            cb.EndText();

            //DIREITA
            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Emitido em: " + DateTime.Now.Date.ToString().Substring(0, DateTime.Now.Date.ToString().LastIndexOf(' ')), pageSize.GetLeft(1420), pageSize.GetTop(160), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Hora: " + DateTime.Now.TimeOfDay.ToString().Substring(0, DateTime.Now.TimeOfDay.ToString().LastIndexOf('.')), pageSize.GetLeft(1420), pageSize.GetTop(175), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "IP: " + ip.PegarIP(), pageSize.GetLeft(1420), pageSize.GetTop(190), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Situação da Turma: " + dd.Situacao, pageSize.GetLeft(1420), pageSize.GetTop(205), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Versão do Documento: " + dd.Versao, pageSize.GetLeft(1420), pageSize.GetTop(220), 0);
            cb.EndText();

            //RODAPÉ
            cb.BeginText();
            cb.SetFontAndSize(bf, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "__________________________________________________________________________________________________________________________________________________________________________________________________", pageSize.GetLeft(1420), pageSize.GetBottom(50), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Página: " + writer.PageNumber.ToString(), pageSize.GetLeft(1420), pageSize.GetBottom(30), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "AUTENTICAÇÃO", pageSize.GetLeft(80), pageSize.GetBottom(30), 0);
            cb.EndText();

            //FIM

            //cb.AddTemplate(template, pageSize.GetLeft(30), pageSize.GetBottom(20));
        }
    }
}
