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

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand()]
        private async Task Save()
        {
            await alertService.ShowAlert("Sauvegarder choix", $"La sauvegarde du choix sera ajoutée plus tard.");
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
            if (condition == null)
            {
                return;
            }

            await alertService.ShowAlert("Supprimer condition", $"La suppression de la condition \"{condition.Type}\" sera ajoutée plus tard.");
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
            if (effect == null)
            {
                return;
            }

            await alertService.ShowAlert("Supprimer effet", $"La suppression de l'effet \"{effect.Type}\" sera ajoutée plus tard.");
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
    }
}
