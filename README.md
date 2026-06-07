# 🎮 Narrative Scenario Editor & RPG Test Engine

> Projet C# / .NET MAUI orienté objet – Éditeur et moteur de scénarios narratifs de type RPG / livre dont vous êtes le héros.

---

## 📌 Description

Ce projet est une application .NET MAUI permettant de créer, gérer et tester des scénarios narratifs interactifs.

L’application contient deux grandes parties :

* ✏️ **Une partie édition**, permettant de créer et modifier les éléments d’un scénario.
* ▶️ **Une partie jeu**, permettant de lancer un scénario, choisir un personnage, parcourir les scènes, faire des choix, acheter en boutique et combattre des ennemis.

Un scénario est composé de scènes reliées par des choix.
Chaque choix peut être soumis à des conditions et produire des effets sur l’état du joueur.

Certaines scènes peuvent également contenir :

* ⚔️ un combat
* 🛒 une boutique
* 🏁 une fin de scénario

Le projet est conçu comme un **outil de création et de test de scénarios narratifs**, avec une architecture simple adaptée à un cours de programmation orientée objet.

---

## 🎯 Objectifs pédagogiques

Ce projet a pour objectif de mettre en pratique les principes de la programmation orientée objet :

* Encapsulation
* Validation des données dans les modèles
* Utilisation de propriétés avec backing fields privés
* Séparation entre modèle, vue et logique d’affichage
* Utilisation de collections spécialisées
* Gestion d’un état de jeu runtime
* Persistance des données avec SQL Server
* Navigation entre différentes pages MAUI
* Utilisation du pattern MVVM avec CommunityToolkit.Mvvm

---

## 🛠️ Technologies utilisées

* C#
* .NET 8
* .NET MAUI
* CommunityToolkit.Mvvm
* CommunityToolkit.Maui
* SQL Server
* Microsoft.Data.SqlClient
* Microsoft SQL Server Management Studio 2019
* XAML

---

## 🧱 Architecture du projet

Le projet suit une architecture volontairement simple, adaptée au contexte scolaire.

```text
Model/
    Combat/
    Game/
    Gameplay/
    Story/

ViewModel/

View/

Utilities/
    DataAccess/
    Interfaces/
    Services/
    Randomization/
    EntriesValidation/

Configuration/
    Datas/
```

### Model

Contient les classes métier du projet.

Exemples :

* `Scenario`
* `Scene`
* `Choice`
* `Condition`
* `Effect`
* `Shop`
* `Enemy`
* `EnemyRace`
* `PlayerCharacterTemplate`
* `GameState`
* `GameEngine`
* `SaveGame`

Les modèles contiennent une partie importante de la logique métier ainsi que les validations.

---

### ViewModel

Contient la logique d’affichage, les commandes et la navigation.

Exemples :

* `ScenarioListViewModel`
* `ScenarioEditorViewModel`
* `SceneEditorViewModel`
* `ChoiceEditorViewModel`
* `EnemyEditorViewModel`
* `GameScenarioListViewModel`
* `PlayerCharacterSelectionViewModel`
* `GameViewModel`
* `SaveGameListViewModel`

Les ViewModels héritent de `BaseViewModel`.

---

### View

Contient les pages XAML de l’application.

Exemples :

* `MainPage`
* `ScenarioListPage`
* `ScenarioEditorPage`
* `SceneEditorPage`
* `ChoiceEditorPage`
* `EnemyEditorPage`
* `GameScenarioListPage`
* `PlayerCharacterSelectionPage`
* `GamePage`
* `SaveGameListPage`

---

### Utilities / DataAccess

Contient les classes liées à l’accès aux données, aux services et aux helpers.

La classe principale utilisée actuellement est :

* `DataAccessSqlFile`

Elle permet de gérer les opérations SQL de création, lecture, modification et suppression.

---

## 🗃️ Persistance des données

Le projet utilise actuellement **SQL Server** comme stockage principal.

Les données sont manipulées via `DataAccessSqlFile`.

La persistance SQL permet notamment de gérer :

* les scénarios
* les scènes
* les choix
* les conditions
* les effets
* les boutiques
* les personnages joueurs
* les ennemis
* les races ennemies
* les inventaires runtime
* les personnages runtime
* les états de jeu
* les sauvegardes

Des anciennes classes d’accès aux données CSV / JSON sont encore présentes dans le projet, mais ne sont plus utilisées dans l’application actuelle. Elles représentent les anciennes étapes d’évolution du projet.

---

## ⚙️ Configuration locale

Le projet utilise un fichier de configuration local pour accéder à SQL Server.

Après avoir cloné le dépôt, il faut créer le fichier suivant :

```text
Configuration/Datas/ConfigSql.local.txt
```

Il est possible de se baser sur :

```text
Configuration/Datas/ConfigSql.txt
```

Le fichier local doit contenir les informations nécessaires à la connexion SQL Server.

Exemple de chaîne de connexion :

```text
Server=localhost;Database=ProjetPOO;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;
```

Le nom exact du serveur et de la base de données doit être adapté selon l’environnement local.

---

## ✏️ Fonctionnalités de l’éditeur

La partie édition permet de gérer les éléments nécessaires à la création d’un scénario narratif.

Fonctionnalités principales :

* Création, modification et suppression de scénarios
* Création, modification et suppression de scènes
* Gestion de la scène de départ d’un scénario
* Création, modification et suppression de choix
* Création, modification et suppression de conditions
* Création, modification et suppression d’effets
* Création, modification et suppression de boutiques
* Création, modification et suppression de personnages joueurs
* Création, modification et suppression d’ennemis
* Création et modification des races ennemies via popup
* Suppression des races ennemies bloquée si elles sont utilisées
* Vérification de cohérence d’un scénario
* Affichage des erreurs de validation dans une popup dédiée
* Rafraîchissement des pages lors du retour de navigation
* Boutons permettant de remonter en haut des pages longues

---

## ✔️ Validation des scénarios

Le projet distingue deux types de validation :

### `ValidateSafe()`

Utilisée pour vérifier qu’un scénario ou un élément est globalement cohérent pendant l’édition.

### `ValidatePlayable()`

Utilisée pour vérifier qu’un scénario peut réellement être joué.

Dans l’éditeur, la validation de jouabilité est informative.
Elle permet d’aider l’utilisateur à repérer les problèmes, sans empêcher la sauvegarde.

Dans la partie jeu, la validation de jouabilité est bloquante.
Un scénario non jouable ne peut pas être lancé.

Cette logique permet de respecter l’idée suivante :

* Sauvegarder = enregistrer un état de travail
* Vérifier la cohérence = aider l’utilisateur à corriger le scénario
* Jouer = autoriser uniquement un scénario jouable

---

## ▶️ Fonctionnalités de la partie jeu

La partie jeu permet de tester les scénarios créés dans l’éditeur.

Fonctionnalités principales :

* Affichage de la liste des scénarios disponibles
* Lancement d’une nouvelle partie
* Validation du scénario avant lancement
* Sélection d’un personnage joueur
* Création d’un état de jeu initial
* Affichage de la scène courante
* Affichage de l’image de scène si elle existe
* Affichage des choix disponibles
* Application des conditions et effets
* Gestion de l’inventaire
* Gestion de l’or, des potions et des clés
* Gestion des boutiques
* Achat de potions
* Achat de clés
* Gestion des combats
* Attaque
* Défense
* Utilisation de potion
* Fuite
* Affichage du résultat des rounds
* Passage automatique aux scènes de victoire, défaite ou fuite

---

## 💾 Sauvegarde et chargement de partie

Le projet permet de sauvegarder et de charger une partie.

Une sauvegarde contient notamment :

* l’état de jeu
* le scénario lié
* la scène courante
* l’inventaire du joueur
* le personnage runtime
* les flags actifs
* le nom de la sauvegarde
* la date de création
* la date de dernière sauvegarde

Depuis la partie jeu, il est possible de :

* créer une nouvelle sauvegarde
* donner un nom personnalisé à la sauvegarde
* afficher la liste des sauvegardes existantes
* charger une sauvegarde
* supprimer une sauvegarde

La sauvegarde pendant un combat est volontairement bloquée pour le moment, car l’état complet du combat n’est pas encore persisté en SQL.

---

## 🔄 Séparation Template / Runtime

Le projet distingue les objets de création et les objets utilisés pendant l’exécution.

Exemples :

```text
Enemy                → modèle d’ennemi dans le scénario
EnemyInstance        → ennemi utilisé pendant un combat

PlayerCharacterTemplate  → personnage disponible dans l’éditeur
PlayerCharacterInstance  → personnage utilisé pendant une partie
```

Cette séparation permet de ne pas modifier directement les modèles du scénario pendant l’exécution du jeu.

---

## 📦 Collections spécialisées

Le projet utilise plusieurs collections spécialisées héritant de `ObservableCollection<T>`.

Exemples :

* `ScenesCollection`
* `ChoicesCollection`
* `ConditionsCollection`
* `EffectsCollection`
* `EnemiesCollection`
* `EnemyRacesCollection`
* `ShopsCollection`
* `PlayerCharactersCollection`

Ces collections permettent de centraliser certaines règles de cohérence :

* éviter les doublons
* rattacher correctement les éléments à leur parent
* gérer certaines suppressions
* regrouper la logique liée aux listes métier

---

## 🧪 Scénario de test

La base de données contient un scénario de test permettant de vérifier le fonctionnement global de l’application.

Ce scénario permet notamment de tester :

* la navigation entre scènes
* les choix
* les conditions
* les effets
* la boutique
* les combats
* les sauvegardes
* le chargement de partie

---

## 📊 État actuel du projet

Fonctionnalités terminées :

* ✅ Modèle métier principal
* ✅ Éditeur de scénarios
* ✅ Éditeur de scènes
* ✅ Éditeur de choix
* ✅ Éditeur de conditions
* ✅ Éditeur d’effets
* ✅ Éditeur de boutiques
* ✅ Éditeur de personnages joueurs
* ✅ Éditeur d’ennemis
* ✅ Gestion des races ennemies
* ✅ Validation de cohérence
* ✅ Partie jeu
* ✅ Sélection de personnage
* ✅ Navigation narrative
* ✅ Conditions et effets
* ✅ Inventaire
* ✅ Boutique
* ✅ Combat
* ✅ Sauvegarde SQL
* ✅ Chargement SQL
* ✅ Suppression des sauvegardes
* ✅ Interface MAUI fonctionnelle

---

## 🚧 Limites actuelles

Le projet est fonctionnel pour l’objectif prévu, mais certaines limites sont connues :

* La sauvegarde pendant un combat est bloquée.
* L’or de départ est encore défini de manière temporaire pour faciliter les tests.
* Les anciens accès CSV / JSON sont conservés, mais ne sont plus utilisés dans la version actuelle.
* L’interface reste volontairement simple.
* Le projet est un moteur de test et non un jeu complet finalisé.

---

## 🔮 Améliorations possibles

Améliorations envisageables :

* Sauvegarder également l’état exact d’un combat en cours
* Ajouter une gestion plus avancée des objets
* Ajouter d’autres types d’effets
* Ajouter d’autres types de conditions
* Améliorer l’équilibrage des combats
* Ajouter une meilleure gestion des images
* Améliorer le design général de l’interface
* Ajouter un export/import de scénarios
* Ajouter une documentation technique plus détaillée

---

## 👨‍💻 Auteur

Projet réalisé dans le cadre d’un cours de programmation orientée objet.

---

## 💡 Remarque

Le projet est volontairement conçu avec une architecture simple et lisible.

L’objectif principal est de démontrer la maîtrise des concepts de programmation orientée objet, de structuration d’un projet MAUI, de manipulation de données SQL et de gestion d’un moteur narratif interactif.
