using System;

namespace PIF1006_tp1
{
    /// <summary>
    /// AVANT D'ENTAMER VOTRE TRAVAIL, SVP, VEUILLEZ BIEN LIRE ATTENTIVEMENT LES INSTRUCTIONS ET DIRECTIONS EN COMMENTAIRES DANS LES DIFFÉRENTS
    /// FICHIERS.
    /// 
    /// LES CLASSES ET LEURS MEMBRES PRÉDÉFINIS DOIVENT RESTER TELS QUELS.  VOUS POUVEZ AJOUTER DES MÉTHODES PRIVÉES AU BESOIN, MAIS AU MINIMUM
    /// AJOUTER LE CODE MANQUANT (ET CRÉER LES FICHIERS EN ENTRÉE PERTINENTS) PERMETTANT DE RÉALISER LES FONCTIONNALITÉS DEMANDÉES.
    /// 
    /// VOUS DEVEZ TRAVAILLER EN C# .NET.  LE PROJET EST EN .NET 5.0 AFIN DE S'ASSURER D'UNE COMPATIBILITÉ POUR TOUS ET TOUTES, MAIS VOUS ÊTES
    /// INVITÉ/E/S À UTILISER LA DERNIÈRE VERSION DU FRAMEWORK .NET (8.0).
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========================================================================");
            Console.WriteLine("\t\tAPPLICATION DE GESTION D'AUTOMATES A ETATS FINIS");
            Console.WriteLine("===========================================================================");
            Console.WriteLine("MEMBRES DU GROUPE DE TRAVAIL");
            Console.WriteLine("\t 1- ASONMENE TEWOUNNO ROMUALD \t\t ASOR24338600");
            Console.WriteLine("\t 1- ASONMENE TEWOUNNO ROMUALD \t\t ASOR24338600");

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

                String rep = Console.ReadLine();
                Console.WriteLine(rep);
                //on teste que la valeur sasie est correcte
                while (rep != "0" || rep != "1" || rep != "2" || rep != "3")
                {
                    Console.WriteLine("Entrée incorecte! Saisir le numero de L'operation !");
                    rep = Console.ReadLine();
                }

                switch (rep) 
                {
                    case "1":
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

                    case "2":
                        Console.WriteLine("===========================================================================");
                        Console.WriteLine("Saisir la chaine d'input à tester");
                        String chaine = Console.ReadLine();
                        //on verifie que lq chqine est correcte (contien des 0 ou des 1)
                        while (!IsChaineValide(chaine))
                        {
                            Console.WriteLine("La chaine saisie est incorrecte! saisir une chaine contenant des 0 et 1");
                            chaine = Console.ReadLine();
                        }
                        //test de la chaine 
                        if (automate.Validate(chaine)) 
                        {
                            Console.WriteLine($"La chaine {chaine} est valide pour cet automate");
                        }
                        else
                        {
                            Console.WriteLine($"La chaine {chaine} n'est pas valide pour cet automate");
                        }
                        Console.WriteLine("===========================================================================");
                        break;

                    case "3":
                        Console.WriteLine("===========================================================================");
                        Console.WriteLine(automate);
                        break;

                    case "0":
                        sortie = false;
                        Console.WriteLine("Fermeture de l'application. Appuyer sur \"Enter\" pour quitter");
                        Console.ReadKey();
                        break;
                }
            }
            while (sortie);
            //---------------------------------------------------------------------------------------------------------------------------
            // Vous devez faire une application dont les étapes d'interactions utilisateurs vont exactement comme suit:
            //
            //      (1) Afficher une entête en console comportant:
            //          -> Nom de votre application
            //          -> Liste de vos noms complets et codes permanents
            //
            //      (2) Charger un fichier en spécifiant le chemin (relatif) du fichier.  Vous pouvez charger un fichier par défaut au démarrage;
            //          ->  Pour le format et la façon de charger le fichier, référez-vous aux détails en commentaire dans la méthode LoadFromFile()
            //              de la classe Automate.
            //          ->  Si après chargement du fichier l'automate est invalide (sa propriété IsValid est à faux), l'application se ferme suite à
            //              l'appuie sur ENTER par l'utilisateur.
            //      (3) La représentation de l'automate doit être affichée à la console sous la forme d'une liste des états et la liste des
            //          transitions de chacune d'entre elles, à la manière d'une pseudo table d'action. Si l'état est un état final cela
            //          doit être apparent;
            //              Par exemple:
            //                  [(s0)]
            //                      --0--> s1
            //                      --1--> s0
            //                  s1
            //                      --0--> s1
            //                      --1--> s2
            //                  s2
            //                      --0--> s1
            //                      --1--> s3
            //                  (s3)
            //
            //              Où s0 et s3 sont des états finaux (parenthèses), s0 est l'état initial (square brackets) et
            //              s3 n'a pas de transition vers d'autres états
            //          ->  Vous DEVEZ surdéfinir les méthodes ToString() des différentes classes fournies de sorte à faciliter l'affichage
            //
            //      (4) Soumettre un input en tant que chaîne de 0 ou de 1
            //          ->  Assurez-vous que la chaine passée ne contient QUE ces caractères
            //              avant d'envoyer n'est pas obligatoire, mais cela ne doit pas faire planter de l'autre coté;
            //          ->  Un message doit indiquer si c'est accepté ou rejeté.
            //          ->  Suite à cela, on doit demander à l'utilisateur s'il veut enter un nouvel input plutôt que de quitter
            //              afin de faire des validations en rafales.
            //
            //      (5) Au moment où l'utilisateur choisit de quitter, un message s'affiche lui disant que l'application va se fermer après
            //          avoir appuyé sur ENTER.
        }

        private static bool IsChaineValide(string chaine)
        {
            char[] tabChaine = chaine.ToCharArray();
            bool valid = true;
            foreach (char ch in tabChaine) {
                if (ch != '0' || ch != '1') { 
                    valid = false; 
                    break;
                }
            }
            return valid;
        }


    }
}
