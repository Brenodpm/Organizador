using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Collections.Generic.Dictionary<string, string>;

namespace Organizador
{
    public static class ListaLocaisGuardarAquivo
    {
        private const string file = "destino.list";
        public const string NAO_ORGANIZAR = "!";
        private static Dictionary<string, string> values = null;

        private static void PreencherLista()
        {
            if (values == null)
                values = new Dictionary<string, string>();

            if (File.Exists(file))
            {
                string s = "";
                using (StreamReader sr = File.OpenText(file))
                    while ((s = sr.ReadLine()) != null)
                        if (!string.IsNullOrEmpty(s))
                            values.Add(s.Split(';')[0], s.Split(';')[1]);
            }
        }

        public static bool Contains(string tipo)
        {
            if (values == null) PreencherLista();

            return values.ContainsKey(tipo.ToLower());
        }

        public static void Add(string tipo, string folder)
        {
            if (values == null) PreencherLista();

            values.Add(tipo.ToLower(), folder);

            StringBuilder linhas = new StringBuilder();

            foreach (var key in values.Keys)
                linhas.AppendLine($"{ key };{ values[key] }");

            File.WriteAllText(file, linhas.ToString());
        }

        public static string Get(string tipo)
        {
            if (values == null) PreencherLista();

            if (values.ContainsKey(tipo.ToLower()))
                return values[tipo.ToLower()];

            return null;
        }

        public static KeyCollection Keys
        {
            get
            {
                if (values == null) PreencherLista();

                return values.Keys;
            }
        }
    }
}
