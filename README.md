# 🎮 Narrative Scenario Editor & Test Engine

> Projet C# orienté objet – Éditeur et moteur de scénarios narratifs (type RPG / livre dont vous êtes le héros)

---

## 📌 Description

Ce projet consiste à développer une application permettant de :

- ✏️ **Créer des scénarios narratifs** (éditeur)
- ▶️ **Tester ces scénarios en les jouant** (moteur de test)

Un scénario est composé de **scènes** reliées par des **choix**.  
Chaque choix peut être soumis à des **conditions** et produire des **effets** sur l’état du joueur.

Certaines scènes permettent également :
- ⚔️ des **combats**
- 🛒 des **interactions avec une boutique**

👉 Le projet est conçu comme un **éditeur + moteur de test**, et non comme un jeu complet.

---

## 🎯 Objectifs pédagogiques

- Appliquer les principes de la **programmation orientée objet**
- Mettre en place :
  - encapsulation (champs privés + propriétés)
  - validation des données
  - séparation des responsabilités
- Concevoir un modèle :
  - cohérent
  - extensible
  - réutilisable
- Préparer une base pour une future **persistance des données**

---

⚙️ Configuration locale requise

Ce projet utilise un fichier de configuration local pour accéder aux données.

Après avoir cloné le dépôt, vous devez créer le fichier suivant :

Configuration/Datas/Config.local.txt

Vous pouvez vous baser sur le fichier Config.txt présent dans le projet, puis adapter le chemin FOLDER en fonction de votre environnement local.

---

## 🧱 Architecture

Le projet est structuré en deux parties principales :

### ✏️ Éditeur (modèle de données)

- `Scenario`
- `Scene`
- `Choice`
- `Condition`
- `Effect`
- `Enemy` (template)
- `Shop`
- `PlayerCharacterTemplate`

👉 Représente la **création du contenu**

---

### ▶️ Runtime (exécution du scénario)

- `GameState`
- `GameEngine`
- `CombatState`
- `EnemyInstance`
- `PlayerCharacterInstance`

👉 Représente **l’exécution du scénario**

---

### 💾 Sauvegarde

- `SaveGame`
- `DataAccess`

---

## ⚙️ Concepts clés

### 🔒 Encapsulation

- Champs privés (backing fields)
- Accès via propriétés
- Validation directement dans les setters

---

### 📦 Collections spécialisées

Le projet utilise des collections héritant de `ObservableCollection<T>` :

- `ScenesCollection`
- `ChoicesCollection`
- `ConditionsCollection`
- `EffectsCollection`
- `EnemiesCollection`
- `ShopsCollection`
- `PlayerCharactersCollection`

Ces collections gèrent :
- la cohérence des relations
- les doublons
- le rattachement au parent

---

### 🔄 Séparation Template / Runtime

Exemples :

- `Enemy` → modèle du scénario  
- `EnemyInstance` → utilisé en combat  

- `PlayerCharacterTemplate` → modèle  
- `PlayerCharacterInstance` → utilisé en jeu  

👉 Permet :
- la rejouabilité
- une meilleure gestion de l’état du jeu
- une séparation claire des responsabilités

---

### ✔ Validation du scénario

Deux niveaux de validation :

- `ValidateSafe()` → scénario en cours d’édition  
- `ValidatePlayable()` → scénario prêt à être testé  

---

## 🎮 Fonctionnement

1. Création d’un scénario
2. Ajout de scènes
3. Ajout de choix entre les scènes
4. Ajout de conditions et effets
5. Ajout d’ennemis, boutiques, personnages
6. Lancement du mode test

---

## 🧪 Test rapide (MainPage)

Un bouton permet d’instancier :

- toutes les classes principales
- avec des données de test cohérentes

👉 Objectifs :
- vérifier les constructeurs
- vérifier les relations entre objets
- démontrer la cohérence du modèle

---

## 📊 État actuel

- ✅ Modèle de données complet
- ✅ Navigation entre scènes
- ✅ Système de choix
- ✅ Conditions et effets
- ✅ Système de combat
- ✅ Inventaire et boutique
- ✅ Séparation template / runtime
- ✅ Sauvegarde simple
- ✅ Collections spécialisées

---

## 🚧 Améliorations prévues

### Court terme
- corrections mineures de cohérence
- amélioration des validations

### Moyen terme
- amélioration du `GameEngine`

### Long terme
- interface utilisateur complète (éditeur)
- persistance (JSON / base de données)
- évolution vers un projet plus complet 

---

## 🛠️ Technologies

- C#
- .NET
- Programmation orientée objet
- ObservableCollection

---

## 👨‍💻 Auteur

Projet réalisé dans le cadre d’un cours de programmation orientée objet.

---

## 💡 Remarque

Le projet est volontairement conçu comme un **outil de création et de test de scénarios narratifs**, avec une architecture simple mais évolutive adaptée à un contexte pédagogique.
