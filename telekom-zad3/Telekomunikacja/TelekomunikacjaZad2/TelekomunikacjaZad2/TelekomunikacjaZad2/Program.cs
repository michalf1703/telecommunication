namespace TelekomunikacjaZad2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            GUI form = new GUI();
            form.Text = "M.Ferdzyn | A.Grzybek | TELEKOMUNIKACJA ZADANIE 3";
            Application.Run(form);
        }
    }
}