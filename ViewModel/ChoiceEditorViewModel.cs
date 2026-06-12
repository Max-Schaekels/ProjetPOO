using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Story;
using ProjetPOO.Utilities.Interfaces;
using ProjetPOO.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Condition = ProjetPOO.Model.Story.Condition;
using Effect = ProjetPOO.Model.Story.Effect;

namespace ProjetPOO.ViewModel
{
    public partial class ChoiceEditorViewModel : BaseViewModel
    {
        private readonly ConditionEditorPage conditionEditorPage;
        private readonly EffectEditorPage effectEditorPage;
        private Scenario? selectedScenario;
        private Scene? selectedScene;
        private Choice? selectedChoice;
        public ChoiceEditorViewModel(IAlertService alertService, IDataAccess dataAccessService, ConditionEditorPage conditionEditorPage, EffectEditorPage effectEditorPage) : base(alertService, dataAccessService)
        {
            this.conditionEditorPage = conditionEditorPage;
            this.effectEditorPage = effectEditorPage;
            PageTitle = "Édition choix";

            canEditChoiceDetails = false;

            choiceLabel = string.Empty;
            availableTargetScenes = new ScenesCollection();
            selectedTargetScene = null;
            conditions = new ConditionsCollection();
            effects = new EffectsCollection();

        }

        [ObservableProperty]
        private string choiceLabel;

        [ObservableProperty]
        private ScenesCollection availableTargetScenes;

        [ObservableProperty]
        private Scene? selectedTargetScene;

        [ObservableProperty]
        private ConditionsCollection conditions;

        [ObservableProperty]
        private EffectsCollection effects;

        [ObservableProperty]
        private bool canEditChoiceDetails;

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand()]
        private async Task Save()
        {
            if (selectedScene == null)
            {
                await alertService.ShowAlert("Scène manquante", "Aucune scène n'est sélectionnée.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ChoiceLabel))
            {
                await alertService.ShowAlert("Texte invalide", "Le texte du choix ne peut pas être vide.");
                return;
            }

            if (ChoiceLabel.Trim().Length < 3)
            {
                await alertService.ShowAlert("Texte invalide", "Le texte du choix doit contenir au moins 3 caractères.");
                return;
            }

            if (ChoiceLabel.Trim().Length > 200)
            {
                await alertService.ShowAlert("Texte invalide", "Le texte du choix ne peut pas dépasser 200 caractères.");
                return;
            }

            if (SelectedTargetScene == null)
            {
                await alertService.ShowAlert("Destination manquante", "Veuillez sélectionner une scène de destination.");
                return;
            }

            if (SelectedTargetScene.Id == selectedScene.Id)
            {
                await alertService.ShowAlert("Destination invalide", "Un choix ne peut pas pointer vers sa propre scène.");
                return;
            }

            try
            {
                if (selectedChoice == null)
                {
                    Choice choice = new Choice(
                        ChoiceLabel.Trim(),
                        SelectedTargetScene.Id,
                        selectedScene.Id
                    );

                    dataAccess.AddChoice(choice);

                    await alertService.ShowAlert("Choix sauvegardé", "Le nouveau choix a bien été créé.");

                    await Shell.Current.Navigation.PopAsync();
                    return;
                }

                selectedChoice.Rename(ChoiceLabel.Trim());
                selectedChoice.AssignToScene(selectedScene.Id);
                selectedChoice.SetTargetScene(SelectedTargetScene.Id);

                dataAccess.UpdateChoice(selectedChoice);

                await alertService.ShowAlert("Choix sauvegardé", "Le choix a bien été mis à jour.");

                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur sauvegarde", exception.Message);
            }
        }

        [RelayCommand()]
        private async Task NewCondition()
        {
            if (selectedChoice == null)
            {
                await alertService.ShowAlert("Choix manquant", "Impossible d'ajouter une condition car aucun choix n'est sélectionné.");
                return;
            }

            if (conditionEditorPage.BindingContext is ConditionEditorViewModel conditionEditorViewModel)
            {
                conditionEditorViewModel.PrepareNewCondition(selectedChoice);
            }

            await Shell.Current.Navigation.PushAsync(conditionEditorPage);
        }

        [RelayCommand()]
        private async Task EditCondition(Condition condition)
        {
            if (condition == null)
            {
                return;
            }

            if (selectedChoice == null)
            {
                await alertService.ShowAlert("Choix manquant", "Impossible de modifier cette condition car aucun choix n'est sélectionné.");
                return;
            }

            if (conditionEditorPage.BindingContext is ConditionEditorViewModel conditionEditorViewModel)
            {
                conditionEditorViewModel.LoadCondition(selectedChoice, condition);
            }

            await Shell.Current.Navigation.PushAsync(conditionEditorPage);
        }

        [RelayCommand()]
        private async Task DeleteCondition(Condition condition)
        {
            if (selectedChoice == null)
            {
                await alertService.ShowAlert("Choix manquant", "Impossible de supprimer cette condition car aucun choix n'est sélectionné.");
                return;
            }

            if (condition == null)
            {
                return;
            }

            bool confirm = await alertService.ShowConfirmation( "Supprimer condition", $"Voulez-vous vraiment supprimer la condition \"{condition.Type}\" ?","Supprimer", "Annuler");

            if (!confirm)
            {
                return;
            }

            try
            {
                dataAccess.DeleteCondition(condition.Id);

                bool removed = selectedChoice.Conditions.RemoveById(condition.Id);

                if (!removed)
                {
                    await alertService.ShowAlert("Suppression impossible", "La condition a été supprimée en base, mais pas trouvée dans le choix chargé.");
                    return;
                }

                await alertService.ShowAlert("Condition supprimée", "La condition a bien été supprimée.");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur suppression", exception.Message);
            }
        }

        [RelayCommand()]
        private async Task NewEffect()
        {
            if (selectedChoice == null)
            {
                await alertService.ShowAlert("Choix manquant", "Impossible d'ajouter un effet car aucun choix n'est sélectionné.");
                return;
            }

            if (effectEditorPage.BindingContext is EffectEditorViewModel effectEditorViewModel)
            {
                effectEditorViewModel.PrepareNewEffect(selectedChoice);
            }

            await Shell.Current.Navigation.PushAsync(effectEditorPage);
        }

        [RelayCommand()]
        private async Task EditEffect(Effect effect)
        {
            if (effect == null)
            {
                return;
            }

            if (selectedChoice == null)
            {
                await alertService.ShowAlert("Choix manquant", "Impossible de modifier cet effet car aucun choix n'est sélectionné.");
                return;
            }

            if (effectEditorPage.BindingContext is EffectEditorViewModel effectEditorViewModel)
            {
                effectEditorViewModel.LoadEffect(selectedChoice, effect);
            }

            await Shell.Current.Navigation.PushAsync(effectEditorPage);
        }

        [RelayCommand()]
        private async Task DeleteEffect(Effect effect)
        {
            if (selectedChoice == null)
            {
                await alertService.ShowAlert("Choix manquant", "Impossible de supprimer cet effet car aucun choix n'est sélectionné.");
                return;
            }

            if (effect == null)
            {
                return;
            }

            bool confirm = await alertService.ShowConfirmation( "Supprimer effet", $"Voulez-vous vraiment supprimer l'effet \"{effect.Type}\" ?", "Supprimer","Annuler");

            if (!confirm)
            {
                return;
            }

            try
            {
                dataAccess.DeleteEffect(effect.Id);

                bool removed = selectedChoice.Effects.RemoveById(effect.Id);

                if (!removed)
                {
                    await alertService.ShowAlert("Suppression impossible", "L'effet a été supprimé en base, mais pas trouvé dans le choix chargé.");
                    return;
                }

                await alertService.ShowAlert("Effet supprimé", "L'effet a bien été supprimé.");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur suppression", exception.Message);
            }
        }

        public void PrepareNewChoice(Scenario scenario, Scene scene)
        {
            if (scenario == null || scene == null)
            {
                return;
            }

            selectedScenario = scenario;
            selectedScene = scene;
            selectedChoice = null;

            PageTitle = "Nouveau choix";

            ChoiceLabel = string.Empty;

            AvailableTargetScenes = BuildAvailableTargetScenes(scene);
            SelectedTargetScene = null;

            Conditions = new ConditionsCollection();
            Effects = new EffectsCollection();

            CanEditChoiceDetails = false;
        }

        public void LoadChoice(Scenario scenario, Scene scene, Choice choice)
        {
            if (scenario == null || scene == null || choice == null)
            {
                return;
            }

            selectedScenario = scenario;
            selectedScene = scene;
            selectedChoice = choice;

            PageTitle = "Édition choix";

            ChoiceLabel = choice.Label;
            AvailableTargetScenes = BuildAvailableTargetScenes(scene);
            SelectedTargetScene = GetSceneById(choice.TargetSceneId);

            Conditions = choice.Conditions;
            Effects = choice.Effects;

            CanEditChoiceDetails = true;
        }

        private ScenesCollection BuildAvailableTargetScenes(Scene currentScene)
        {
            ScenesCollection availableTargetScenes = new ScenesCollection();

            if (selectedScenario == null)
            {
                return availableTargetScenes;
            }

            for (int i = 0; i < selectedScenario.Scenes.Count; i++)
            {
                Scene scene = selectedScenario.Scenes[i];

                if (scene.Id == currentScene.Id)
                {
                    continue;
                }

                availableTargetScenes.Add(scene);
            }

            return availableTargetScenes;
        }

        private Scene? GetSceneById(int sceneId)
        {
            if (selectedScenario == null || sceneId <= 0)
            {
                return null;
            }

            for (int i = 0; i < selectedScenario.Scenes.Count; i++)
            {
                Scene scene = selectedScenario.Scenes[i];

                if (scene.Id == sceneId)
                {
                    return scene;
                }
            }

            return null;
        }

        /// <summary>
        /// Recharge le choix sélectionné depuis la source de données.
        /// Cette méthode permet de rafraîchir les conditions et les effets liés au choix après un retour de navigation,
        /// tout en conservant les valeurs actuellement saisies dans le formulaire d'édition.
        /// </summary>
        /// <remarks>
        /// Le libellé et la scène cible sont sauvegardés temporairement avant le rechargement,
        /// puis réappliqués afin d'éviter d'écraser les modifications non sauvegardées.
        /// </remarks>
        public void RefreshLoadedChoice()
        {
            if (selectedChoice == null)
            {
                return;
            }

            int choiceId = selectedChoice.Id;

            string currentLabel = ChoiceLabel;
            int currentTargetSceneId = SelectedTargetScene == null ? 0 : SelectedTargetScene.Id;

            Choice? refreshedChoice = dataAccess.GetChoiceById(choiceId);

            if (refreshedChoice == null)
            {
                return;
            }

            if (selectedScenario == null || selectedScene == null)
            {
                return;
            }

            selectedChoice = refreshedChoice;

            ChoiceLabel = currentLabel;
            AvailableTargetScenes = BuildAvailableTargetScenes(selectedScene);
            SelectedTargetScene = GetSceneById(currentTargetSceneId);

            Conditions = refreshedChoice.Conditions;
            Effects = refreshedChoice.Effects;

            CanEditChoiceDetails = true;
        }
    }
}
