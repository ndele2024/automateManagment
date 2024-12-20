using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIF1006_tp1
{
    public class Automate
    {
        public State InitialState { get; private set; }
        public State CurrentState { get; private set; }
        public List<State> States { get; private set; }
        public bool IsValid { get; private set; }

        public Automate(string filePath)
        {
            States = new List<State>();
            LoadFromFile(filePath);
        }

        private void LoadFromFile(string filePath)
        {
            Dictionary<String, String> dictRejet = new Dictionary<String, String>();
            List<String> listeRejet = new List<String>();
            //initialisation de la props IsValid à true
            IsValid = true;

            // lecture du fichier contenant l'automate
            //string cheminFichier = "automate.txt"; 
            try {
                foreach (string ligne in File.ReadLines(filePath)) {
                    String[] tabLigne = ligne.Trim().Split(' ');
                    if (tabLigne.Length == 4)
                    {
                        if (tabLigne[0] == "state")
                        {
                            //creation d'un state
                            //verifier que les deux derniers caaractères sont soit 1 ou 0--------------------todo
                            bool isfinal = (tabLigne[2] == "1") ? true : false;
                            States.Add(new State(tabLigne[1], isfinal));
                            //initialisation su state initial
                            if(InitialState == null) 
                                InitialState = (tabLigne[3] == "1") ? new State(tabLigne[1], isfinal) : null;
                        }
                        else if(tabLigne[0] == "transition")
                        {
                            //on recherche les etats dans la liste
                            int indexStateSource = RechercherEtat(tabLigne[1]);
                            int indexStateDestination = RechercherEtat(tabLigne[3]);

                            if ((indexStateSource != -1)&&(indexStateDestination != -1))
                            {
                                //les states sont dans la liste
                                char input = tabLigne[2][0];
                                if ((input == '0') || (input == '1'))
                                {
                                    //valeur input correct 0 ou 1
                                    State s = States[indexStateSource];
                                    s.Transitions.Add(new Transition(input, States[indexStateDestination]));
                                    States[indexStateSource] = s;
                                
                                }
                                else
                                {
                                    //IsValid = false;
                                    dictRejet.Add("l'input de cette transition est incorect! saisir 0 ou 1", ligne);
                                }

                            }
                            else
                            {
                                //IsValid = false;
                                dictRejet.Add("cette transition fait reference a un etat inexistant", ligne);
                            }
                        }
                        else
                        {
                            //IsValid = false;
                            dictRejet.Add("vous devez indiquer si la ligne est un state ou une transition", ligne);
                            //continue;
                        }
                    }
                    else
                    {
                        //IsValid = false;
                        dictRejet.Add("le nombre de parametre de la ligne n'est pas correct", ligne);
                        //continue;
                    }

                }
            }
            catch (FileNotFoundException ex) {
                Console.WriteLine($"Erreur lors de la lecture du fichier : {ex.Message}"); 
            }

            //affichage des lignes ignorés et de la raison
            if(dictRejet.Count > 0)
            {
                Console.WriteLine("Lignes rejetés :");
                Console.WriteLine("numero \t\t Raison du rejet");
                foreach (var item in dictRejet.Keys)
                {
                    Console.WriteLine($"{dictRejet[item]} \t {item}");
                }
            }

            //determination si l'automate est valide, deterministe ou pas
            if(States.Count == 0)   //L'automate n'a pas d'etat
            {
                IsValid = false;
                Console.WriteLine("Cet automate ne possède aucun etat : Automate rejeté");
            }
            else
            {
                //L'automate a-t-il un etat initial
                if (InitialState == null)
                {
                    IsValid = false;
                    Console.WriteLine("Cet automate ne possède pas  d'etat initial : Automate rejeté");
                }
                else 
                {
                    //determination si l'automate est deterministe ou pas 
                    if (IsAutomateDeterministe().Count > 0) 
                    {
                        //l'automate est non deterministe
                        IsValid = false;
                        Console.WriteLine("Cet automate n'est pas deterministe : Automate rejeté");
                        AfficheEtatDefaut(IsAutomateDeterministe());
                    }
                    else if (!IsAutomateHasFinalState())
                    {
                        //l'automate n'a pas d'etat final
                        IsValid = false;
                        Console.WriteLine("Cet automate n'a pas d'etat final : Automate rejeté");
                    }
                    else
                    {
                        //l'automate est valide 
                        Console.WriteLine("L'automate chargé est Valide. Liste de ses états et transitions : ");
                        Console.WriteLine(this);
                    }
                }
            }



            // Vous devez pouvoir charger à partir d'un fichier quelconque.  Cela peut être un fichier XML, JSON, texte, binaire, ...
            // P.ex. avec un fichier texte, vous pouvoir balayer ligne par ligne et interprété en séparant chaque ligne en un tableau de strings
            // dont le premier représente l'action, et la suite les arguments. L'équivalent de l'automate décrit manuellement dans la classe
            // Program pourrait être:
            //  state s0 1 1
            //  state s1 0 0
            //  state s2 0 0
            //  state s3 1 0
            //  transition s0 0 s1
            //  transition s0 1 s0
            //  transition s1 0 s1
            //  transition s1 1 s2
            //  transition s2 0 s1
            //  transition s3 1 s3
            //
            // Dans une boucle, on prend les lignes une par une:
            //   - Si le 1er terme est "state", on prend les arguments et on crée un état du même nom
            //     et on l'ajoute à une liste d'état; les 2 et 3e argument représentent alors si c'est un état final, puis si c'est l'état initial
            //   - Si c'est "transition" on cherche dans la liste d'état l'état qui a le nom en 1er argument et on ajoute la transition avec les 2 autres
            //     arguments à sa liste
            // 
            // Considérez que:
            //   - S'il y a d'autres termes, les lignes pourraient être ignorées;
            //   - Si l'état n'est pas trouvé dans la liste (p.ex. l'état est référencé mais n'existe pas (encore)), la transition est ignorée
            //   - Après lecture du fichier:
            //          - si l'automate du fichier n'est pas déterministe (vous devrez penser à comment vérifier cela -> l'état et la transition
            //            en défaut doit être indiquée à l'utilisateur), OU
            //          - si l'automate n'a aucun état, ou aucun état initial
            //     l'automate est considéré comme "invalide" (la propriété IsValid doit alors valoir faux)
            //   - Lorsque des lignes (ou l'automate) sont ignorées ou à la fin l'automate rejeté, cela doit être indiqué à l'utilisateur
            //     à la console avec la ligne/raison du "rejet".
        }

        private void AfficheEtatDefaut(List<State> states)
        {
            foreach (var item in states)
            {
                Console.WriteLine($"L'etat {item.Name} possède les transitions suivantes");
                item.Transitions.ForEach(t => Console.WriteLine(t));
            }
        }

        //determine si l'automate possède un etat final ou pas
        private bool IsAutomateHasFinalState()
        {
            bool isHasFinal = false;

            foreach (var state in States)
            {
                if (state.IsFinal)
                {
                    isHasFinal = true;
                    break;
                }
            }
            return isHasFinal;
        }

        private List<State> IsAutomateDeterministe()
        {
            //initialisation de la liste contanant les etats en defaut
            List<State> states = new List<State>();
            foreach (State state in States)
            {
                List<Transition> transitions = new List<Transition>();
                transitions = state.Transitions;
                //on recupère le premier input et l'etat de destination
                char input = transitions[0].Input;
                State st = transitions[0].TransiteTo;
                for (int i = 1; i < transitions.Count; i++)
                {
                    if ((input == transitions[i].Input) && (st != transitions[i].TransiteTo)) //deux input identiques le state est non deterministe
                    {
                        states.Add(state);
                    }
                }
            }
            return states;
        }

        public bool Validate(string input)
        {
            bool isValid = true;
            Reset();
            //transformer l'input en une liste / un tableau de caractères (char) et les lire un par un;
            Console.WriteLine($"Validation de la chaine {input}");
            Console.WriteLine($"Etat initial {CurrentState.Name}");
            char[] tabchar = input.ToCharArray();
            foreach (var item in tabchar)
            {
                Console.Write($"\tInput lu : {item} ----> ");
                bool trouver = false;
                foreach (var t in CurrentState.Transitions)
                {
                    if (t.Input == item)
                    {
                        //transition trouvé
                        trouver = true;
                        CurrentState = t.TransiteTo;
                        Console.WriteLine($"Transite to {t.TransiteTo}");
                        break;
                    }
                }
                if (!trouver)
                {
                    //aucune transition trouvé
                    isValid = false;
                    Console.WriteLine("Aucune transition pour cet input");
                    break;
                }
            }
            //on teste à la fin si on est sur un etat final
            if (!CurrentState.IsFinal)
            {
                isValid = false;
            }
            // L'automate doit maintenant à jour son "CurrentState" en suivant les transitions et en respectant l'input.
            // Considérez que l'automate est déterministe et que même si dans les faits on aurait pu mettre plusieurs
            // transitions possibles pour un état et un input donné, le 1er trouvé dans la liste est le chemin emprunté.
            // Si aucune transition n'est trouvé pour un état courant et l'input donné, cela doit retourner faux;
            // Si tous les caractères ont été pris en compte, on vérifie si l'état courant est final ou non et on retourne
            // vrai ou faux selon.

            // VOUS DEVEZ OBLIGATOIREMENT AFFICHER la suite des états actuel, input lu, et état transité pour qu'on puisse
            // suivre le déroulement de l'analyse.

            return isValid;
        }

        public void Reset()
        {
            //Le CurrentState est égal au InitialState
            CurrentState = InitialState;
            
        }

        public override string ToString()
        {
            // Vous devez modifier cette partie de sorte à retourner un équivalent string qui décrit tous les états et
            // la table de transitions de l'automate.
            Reset();
            //initialisation d'un Stringbuilder
            StringBuilder builder = new StringBuilder();
            //affichage du state initial
            builder.Append("\t");
            builder.Append($"[({InitialState.Name})]\n");
            foreach (var transition in InitialState.Transitions)
            {
                builder.Append("\t");
                builder.Append(transition.ToString()+"\n");
            }
            //affichage des autres etats de l'automate
            foreach (var state in States)
            {
                if(state.Name != InitialState.Name)
                {
                    builder.Append("\t");
                    builder.Append(state.ToString()+"\n");
                }
            }


            return builder.ToString(); // On ne retourne le stringbuilder
            

        }

        //recherche un state avec son nom dans la liste des states et retourne true ou false
        private int RechercherEtat(string input) {
           int index = -1;
            for (int i = 0; i < States.Count; i++)
            {
                if (States[i].Name == input)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}
