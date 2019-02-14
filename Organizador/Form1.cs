using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Organizador
{
    public partial class Organizador : Form
    {
        public Organizador()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            List<string> locais = ListaLocaisOrganizar.List.ToList();
            foreach (var dir in locais)
                LimparLocal(dir);

            LimparLocaisInesistentes();

            Close();
        }

        private void LimparLocaisInesistentes()
        {
            List<string> locais = ListaLocaisOrganizar.List.ToList();

            foreach (string local in locais)
                if (!Directory.Exists(local))
                    ListaLocaisOrganizar.Remove(local);

        }

        private static void LimparLocal(string dir)
        {
            string tipo;
            if (!Directory.Exists(dir))
                return;

            string[] arquivos = Directory.GetFiles(dir);
            foreach (string arquivo in arquivos)
            {
                tipo = GetTipo(arquivo);

                if (!ListaLocaisGuardarAquivo.Contains(tipo))
                {
                    if (PerguntarArquivoPodeSerMovido(tipo, arquivo))
                    {
                        PerguntarOndeGuardarAquivo(tipo);
                    }
                    else
                        ListaLocaisGuardarAquivo.Add(tipo, ListaLocaisGuardarAquivo.NAO_ORGANIZAR);
                }

                if (ArquivoPodeSerMovido(tipo))
                    if (!ArquivoEstaNoLugarCorreto(tipo, arquivo))
                        MoverArquivo(tipo, arquivo);
            }
        }

        private static bool PerguntarArquivoPodeSerMovido(string tipo, string arquivo)
        {
            DialogResult dialogResult = MessageBox.Show(
                $"Olá!\n" +
                $"Encontrei o arquivo {arquivo.Split('\\').Last()} " +
                $"na pasta {arquivo.Replace(arquivo.Split('\\').Last(), "")}.\n\n" +
                $"Gostaria de guardá-lo em outro local?",
                $"Aquivo .{tipo}",
                MessageBoxButtons.YesNo
                );


            return dialogResult == DialogResult.Yes;
        }

        private static bool ArquivoPodeSerMovido(string tipo)
        {
            return ListaLocaisGuardarAquivo.Get(tipo) != ListaLocaisGuardarAquivo.NAO_ORGANIZAR;
        }

        private static bool ArquivoEstaNoLugarCorreto(string tipo, string arquivo)
        {
            return arquivo.StartsWith(ListaLocaisGuardarAquivo.Get(tipo));
        }

        private static void MoverArquivo(string tipo, string arquivo)
        {
            try
            {
                string LocalDestino = ListaLocaisGuardarAquivo.Get(tipo);

                if (!Directory.Exists(LocalDestino))
                    Directory.CreateDirectory(LocalDestino);

                string nome = arquivo.Split('\\').Last().Split('.').First();

                int i = 0;

                while (File.Exists($"{LocalDestino}\\{nome}{(i > 0 ? $"({i})" : "")}.{tipo}"))
                    i++;

                File.Move(arquivo, $"{LocalDestino}\\{nome}{(i > 0 ? $"({i})" : "")}.{tipo}");
            }
            catch
            {
            }
        }

        private static void PerguntarOndeGuardarAquivo(string tipo)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = $"Guardar arquivos .{tipo} em:";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ListaLocaisGuardarAquivo.Add(tipo, fbd.SelectedPath);

                if (!ListaLocaisOrganizar.Contains(fbd.SelectedPath))
                {
                    DialogResult dr = MessageBox.Show(
                       $"Você gostaria de verificar ser os arquivos dentro deste diretório estão organizados também?",
                       $"Adicionar diretório",
                       MessageBoxButtons.YesNo
                       );

                    if (dr == DialogResult.Yes)
                        ListaLocaisOrganizar.Add(fbd.SelectedPath);
                }
            }
        }

        private static string GetTipo(string arquivo)
        {
            return arquivo.Contains(".") ? arquivo.Split('.').Last() : "";
        }
    }
}
