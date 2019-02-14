using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizador
{
    public static class ListaLocaisOrganizar
    {
        private const string file = "origem.list";
        private static List<string> values = null;

        private static void PreencherLista()
        {
            if (values == null)
                values = new List<string>();

            if (File.Exists(file))
            {
                string linha = "";
                using (StreamReader sr = File.OpenText(file))
                    while ((linha = sr.ReadLine()) != null)
                        if (!string.IsNullOrEmpty(linha))
                            values.Add(linha);
            }
            else
            {
                Add(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads");
                Add(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
                Add(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos));
            }
        }

        public static void Remove(string folder)
        {
            if (values == null) PreencherLista();

            values.Remove(folder);

            Salvar();
        }

        public static void Add(string folder)
        {
            if (values == null) PreencherLista();

            values.Add(folder);

            Salvar();
        }

        private static void Salvar()
        {
            StringBuilder linhas = new StringBuilder();

            foreach (var linha in values)
                linhas.AppendLine(linha);

            File.WriteAllText(file, linhas.ToString());
        }

        public static List<string> List
        {
            get
            {
                if (values == null) PreencherLista();

                return values;
            }
        }

        public static bool Contains(string folder)
        {
            return values.Contains(folder);
        }
    }
}
