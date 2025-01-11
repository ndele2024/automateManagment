Pour tester l’application et charger un fichier d’automate, il faut copier le fichier texte de l’automate dans le dossier PIF1006-tp1/bin/Debug/net8.0 à partir du répertoire du projet.
Au démarrage de l’application un fichier d’automate est chargée par défaut (le fichier automateDefault.txt). 
Son contenu est le suivant : 

      state s0 1 1
      state s1 0 0
      state s2 0 0
      state s3 1 0
      transition s0 0 s1
      transition s0 1 s0
      transition s1 0 s1
      transition s1 1 s2
      transition s2 0 s1
      transition s3 1 s3

- structure d'une ligne de state : state nom_state is_final is_initial (state s0 1 1)
- structure d'une ligne transition : transition nom_sate_depart input nom_state_arrivé (transition s0 0 s1)
