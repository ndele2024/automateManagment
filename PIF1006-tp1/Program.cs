using System;

namespace PIF1006_tp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========================================================================");
            Console.WriteLine("\t\tAPPLICATION DE GESTION D'AUTOMATES A ETATS FINIS");
            Console.WriteLine("===========================================================================");
            Console.WriteLine("MEMBRES DU GROUPE DE TRAVAIL");
            Console.WriteLine("\t 1- ASONMENE TEWOUNNO ROMUALD \t\t\t ASOR24338600");
            Console.WriteLine("\t 2- KANA NGAPGHO MADELEINA WILLIAMYDE  \t\t KANM91280100");
            Console.WriteLine("\t 3- BEKU DJEUJO VALENTIN FRANCK   \t\t BEKV72270200");
            Console.WriteLine("\t 4- SOUMAILA SARÉ  \t\t\t\t SARS93370100");
            Console.WriteLine("\t 5- UDJINE JUNIOR YENCHI GUIMEYA   \t\t YENU65280200");

            Console.WriteLine("===========================================================================");

            //chargement du fichier automate par defaut
            Console.WriteLine("Chargement de l'automate par defaut");
            Automate automate = new Automate("automateDefault.txt");
            bool sortie = true;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Selectionner l'operation a effectuer! (Saisir le numero correspondant)");
                Console.WriteLine("\t0- Quitter l'application");
                Console.WriteLine("\t1- Charger un nouveau fichier d'automate");
                Console.WriteLine("\t2- Tester la validité d'une chaine");
                Console.WriteLine("\t3- Afficher l'automate");
                Console.WriteLine();
                Console.WriteLine("===========================================================================");

                //String rep = Console.ReadLine();
                //Console.WriteLine(rep);
                //on teste que la valeur sasie est correcte
                /*while (rep != "0" && rep != "1" && rep != "2" && rep != "3")
                {
                    Console.WriteLine("Entrée incorecte! Saisir le numero de L'operation !");
                    rep = Console.ReadLine();
                }*/
                int rep = VerifieInt();
                switch (rep) 
                {
                    case 1:
                        Console.WriteLine("===========================================================================");
                        Console.WriteLine("Saisir le nom du fichier automate à charger");
                        String fileName = Console.ReadLine();
                        automate = new Automate(fileName);
                        Console.WriteLine("===========================================================================");
                        if (!automate.IsValid)
                        {
                            sortie = false;
                            Console.WriteLine("Fermeture de l'application. Appuyer sur \"Enter\" pour quitter");
                            Console.ReadKey();
                        }                       
                        break;

                    case 2:
                        Console.WriteLine("===========================================================================");
                        String val = "y";
                        do
                        {
                            Console.WriteLine("Saisir la chaine d'input à tester");
                            String chaine = Console.ReadLine();
                            chaine = chaine.TrimEnd('\r', '\n');
                            //on verifie que lq chqine est correcte (contien des 0 ou des 1)
                            while (!IsChaineValide(chaine))
                            {
                                Console.WriteLine("La chaine saisie est incorrecte! saisir une chaine contenant des 0 et 1");
                                chaine = Console.ReadLine();
                            }
                            //test de la chaine 
                            if (automate.Validate(chaine))
                            {
                                Console.WriteLine($"La chaine {chaine} est reconnue par cet automate");
                            }
                            else
                            {
                                Console.WriteLine($"La chaine {chaine} n'est pas reconnue par cet automate");
                            }
                            Console.WriteLine("===========================================================================");
                            Console.WriteLine("Voulez vous saisir une autre chaine ? Saisir y  ou n");
                            val = Console.ReadLine();
                        }
                        while (val == "y" || val =="Y");
                        break;

                    case 3:
                        Console.WriteLine("===========================================================================");
                        Console.WriteLine(automate);
                        break;

                    case 0:
                        sortie = false;
                        Console.WriteLine("Fermeture de l'application. Appuyer sur \"Enter\" pour quitter");
                        Console.ReadKey();
                        break;
                }
            }
            while (sortie);
            
        }

        private static bool IsChaineValide(string chaine)
        {
            char[] tabChaine = chaine.ToCharArray();
            bool valid = true;
            foreach (char ch in tabChaine) {
                if (ch != '0' && ch != '1') { 
                    valid = false; 
                    break;
                }
            }
            return valid;
        }

        public static int VerifieInt()
        {
            int val;
            while (true)
            {
                try
                {
                    val = int.Parse(Console.ReadLine());
                    break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("valeur incorecte ! Saisir un nombre entier");
                }
            }

            return val;
        }


    }
}
