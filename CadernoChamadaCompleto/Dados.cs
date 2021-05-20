using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CadernoChamadaCompleto
{
    public class Dados
    {
        private int cod_curso;

        public int Cod_curso
        {
            get { return cod_curso; }
            set { cod_curso = value; }
        }

        private string ds_curso;

        public string Ds_curso
        {
            get { return ds_curso; }
            set { ds_curso = value; }
        }

        private string cod_disciplina;

        public string Cod_disciplina
        {
            get { return cod_disciplina; }
            set { cod_disciplina = value; }
        }

        private string ds_disciplina;

        public string Ds_disciplina
        {
            get { return ds_disciplina; }
            set { ds_disciplina = value; }
        }

        private string ds_periodo;

        public string Ds_periodo
        {
            get { return ds_periodo; }
            set { ds_periodo = value; }
        }

        private int nr_turma;

        public int Nr_turma
        {
            get { return nr_turma; }
            set { nr_turma = value; }
        }

        private int carga;

        public int Carga
        {
            get { return carga; }
            set { carga = value; }
        }

        private int creditos;

        public int Creditos
        {
            get { return creditos; }
            set { creditos = value; }
        }

        private string docente;

        public string Docente
        {
            get { return docente; }
            set { docente = value; }
        }

        private string situacao;

        public string Situacao
        {
            get { return situacao; }
            set { situacao = value; }
        }

        private string versao;

        public string Versao
        {
            get { return versao; }
            set { versao = value; }
        }
    }
}